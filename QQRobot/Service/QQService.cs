using DumbQQ.Client;
using DumbQQ.Models;
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
        private class UserInfo
        {
            public Int64 UserID { get; set; }
            public String NickName { get; set; }
            public String Gender { get; set; }
        }

        private static QQBotLogger logger = new QQBotLogger(nameof(QQService));
        private static DumbQQClient client;
        private static UserInfo userInfo = new UserInfo();

        public static void StartQQBot(DumbQQClient dclient)
        {
            client = dclient;
            // 初始化QQ机器人
            InitQQClient();

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
            // 获取用户信息
            userInfo = new UserInfo()
            {
                UserID = client.Id,
                NickName = client.Nickname,
                Gender = client.Gender
            };
        }

        public static void InitQQClient()
        {
            logger.Info("初始化QQ机器人");
            // 机器人名字
            BotConfig.BotName = IniParse.INIGetValue(SharedInfo.ConfigFile, "config", "qqbotName");
            // 机器人类型
            BotConfig.BotType = Int32.Parse(IniParse.INIGetValue(SharedInfo.ConfigFile, "config", "qqbotType"));
            logger.Debug($"机器人类型为:{BotConfig.BotType}");
            // 获取工作群组
            BotConfig.WorkGroups = GetWorkGroups();
            // 获取图灵机器人设置
            BotConfig.TuringApi = IniParse.INIGetValue(SharedInfo.ConfigFile, "config", "turing.api");
            BotConfig.TuringKey = IniParse.INIGetValue(SharedInfo.ConfigFile, "config", "turing.key");
            // 好友消息回调
            client.FriendMessageReceived += OnFriendMessage;
            // 群消息回调
            client.GroupMessageReceived += OnGroupMessage;
            //// 消息回显
            //client.MessageEcho += (sender, e) =>
            //{
            //    Console.WriteLine($"{DateTime.Now}>{e.Target.Name}>{e.Content}");
            //};
        }

        public static void CloseQQClient()
        {
            client.Close();
        }

        private static void OnFriendMessage(Object sender, FriendMessage message)
        {
            var alias = message.Sender.Alias;
            var nickName = message.Sender.Nickname;
            var content = message.Content;

            logger.Info($"{nameof(OnFriendMessage)}>>{alias ?? nickName}:{content}");
        }

        private static void OnGroupMessage(Object sender, GroupMessage message)
        {
            var groupName = message.Group.Name;
            var groupId = message.Group.Id;
            var senderId = message.Sender.Id;
            var senderAlias = message.Sender.Alias;
            var senderName = message.Sender.Nickname;
            var content = message.Content;

            foreach(var g in BotConfig.WorkGroups)
            {
                if (groupName == g)
                {
                    if (senderId != userInfo.UserID)
                    {
                        logger.Info($"{nameof(OnGroupMessage)}>>{groupName}>>{senderAlias ?? senderName}:{content}");
                        var msg = Answer(content, groupName);
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
            }
            logger.Debug($"{nameof(OnGroupMessage)}>>{groupName}>>{senderAlias ?? senderName}:{content}");
            logger.Debug($"群组 [{groupName}] 不在工作群组内");
        }

        private static String Answer(String content, String groupName)
        {
            String msg = String.Empty;

            switch (BotConfig.BotType)
            {
                case 1:
                    msg = TuringService.Chat(content, groupName);
                    break;
                case 2:
                    msg = RepeatService.Chat(content, groupName);
                    break;
                case 3:
                    msg = RepeatMarkIIService.Chat(content, groupName);
                    break;
            }

            return msg;
        }

        private static void SendMessageToGroup(Int64 groupID, String content)
        {
            client.Message(DumbQQClient.TargetType.Group, groupID, content);
        }

        private static List<String> GetWorkGroups()
        {
            var groups = IniParse.INIGetValue(SharedInfo.ConfigFile, "config", "workGroup");
            logger.Debug($"工作群组为: {groups}");
            return new List<string>(groups.Split(','));
        }
    }
}
