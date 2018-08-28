using DumbQQ.Client;
using DumbQQ.Models;
using QQRobot.Service.Bot;
using QQRobot.Util;
using System;
using System.Collections.Generic;

namespace QQRobot.Service
{
    /**
     * 
     * QQ机器人服务
     * 使用到Nuget库DumpQQ-Core
     * https://github.com/rmbadmin/DumbQQ-Core
     * 
     **/
    internal sealed class QQService
    {
        private QQBotLogger logger;
        private DumbQQClient client;
        
        private String botName { get; set; }
        private BotType botType { get; set; }
        private List<String> workGroups { get; set; }

        private BotService botService;

        public QQService()
        {
            this.logger = new QQBotLogger(nameof(QQService));
            this.botName = 
                IniParse.INIGetValue(SharedInfo.ConfigFile, "GlobalConfig", "BotName");
            this.botType =
                (BotType)IniParse.INIGetValue(SharedInfo.ConfigFile, "BotTypes", 
                    IniParse.INIGetValue(SharedInfo.ConfigFile, "GlobalConfig", "BotType"));
            this.workGroups = 
                new List<string>(IniParse.INIGetValue(SharedInfo.ConfigFile, "GlobalConfig", "WorkGroup").Split(','));
        }

        public void StartQQBot()
        {
            // 初始化QQ机器人
            logger.Info("初始化QQ机器人");
            logger.Debug($"机器人类型为:{botType.Type}");
            client = new DumbQQClient();
            botService = BotService.GetServiceInstance(botType.Name);

            // 好友消息回调
            client.FriendMessageReceived += OnFriendMessage;
            // 群消息回调
            client.GroupMessageReceived += OnGroupMessage;

            // 二维码登录
            var result = QrCode.Login(client);
            switch (result)
            {
                case 0:
                    logger.Info($"登录成功，{client.Nickname}!");
                    break;
                case 1:
                    logger.Info($"登录失败，正在退出，{client.Nickname}!");
                    CloseQQClient();
                    Environment.Exit(1);
                    return;
            }

            // 上线提醒
            String hello = $"机器人{botName}开始工作，当前工作模式：{botService.Introduction}";
            foreach(var group in client.Groups)
            {
                if (workGroups.Contains(group.MyAlias ?? group.Name))
                {
                    logger.Debug($"{group.Name}>>上线提醒:{hello}");
                    SendMessageToGroup(group.Id, hello);
                }
            }
        }

        public void CloseQQClient()
        {
            if (client.Status == DumbQQClient.ClientStatus.Active)
            {
                String goodbye = $"机器人{botName}已下线";
                foreach (var group in client.Groups)
                {
                    if (workGroups.Contains(group.MyAlias ?? group.Name))
                    {
                        logger.Debug($"{group.Name}>>下线提醒:{goodbye}");
                        SendMessageToGroup(group.Id, goodbye);
                    }
                }
                client.Close();
            }
        }

        private void OnFriendMessage(Object sender, FriendMessage message)
        {
            var alias = message.Sender.Alias;
            var nickName = message.Sender.Nickname;
            var content = message.Content;

            logger.Info($"{nameof(OnFriendMessage)}>>{alias ?? nickName}:{content}");
        }

        private void OnGroupMessage(Object sender, GroupMessage message)
        {
            var groupName = message.Group.Name;
            var groupId = message.Group.Id;
            var senderId = message.Sender.Id;
            var senderAlias = message.Sender.Alias;
            var senderName = message.Sender.Nickname;
            var content = message.Content;

            if (workGroups.Contains(groupName))
            {
                if (senderId != client.Id)
                {
                    logger.Info($"{nameof(OnGroupMessage)}>>{groupName}>>{senderAlias ?? senderName}:{content}");
                    var msg = Answer(message);
                    if (String.IsNullOrEmpty(msg))
                    {
                        return;
                    }
                    SendMessageToGroup(groupId, msg);
                }
                else
                {
                    logger.Debug("不回复自己说的话");
                }
                return;
            }

            logger.Debug($"群组 [{groupName}] 不在工作群组内");
        }

        private String Answer(GroupMessage message)
        {
            return botService.Chat(message);
        }

        private void SendMessageToGroup(Int64 groupID, String content)
        {
            client.Message(DumbQQClient.TargetType.Group, groupID, content);
        }
    }
}
