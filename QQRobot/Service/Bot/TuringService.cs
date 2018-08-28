using DumbQQ.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QQRobot.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace QQRobot.Service.Bot
{
    internal sealed class TuringService : BotService
    {
        private String turingApi;
        private String turingKey;
        private String botName;

        public override String Introduction => $"图灵机器人，关键字{botName}触发";

        public TuringService() : base(nameof(TuringService))
        {
            this.turingApi = IniParse.INIGetValue(SharedInfo.ConfigFile, "Turing", "Api");
            this.turingKey = IniParse.INIGetValue(SharedInfo.ConfigFile, "Turing", "Key");
            this.botName = IniParse.INIGetValue(SharedInfo.ConfigFile, "GlobalConfig", "BotName");
        }

        public override String Chat(GroupMessage message)
        {
            var msg = message.Content;
            var groupName = message.Group.Name;
            if (null == turingApi || null == turingKey)
            {
                logger.Warn("图灵机器人没有正确配置，无法工作");
                return null;
            }
            if (String.IsNullOrEmpty(msg) || !msg.Contains(botName))
            {
                return null;
            }

            msg = ReplaceBotName(msg);
            msg =  ChatWithTuringBot(msg);
            logger.Info($"Chat To Group:[{groupName}]>>{msg}");
            return msg;
        }

        private String ReplaceBotName(String msg)
        {
            if (String.IsNullOrEmpty(msg))
            {
                return null;
            }
            if (msg.StartsWith(botName + " "))
            {
                msg = msg.Replace(botName + " ", "");
            }
            if (msg.StartsWith(botName + "，"))
            {
                msg = msg.Replace(botName + "，", "");
            }
            if (msg.StartsWith(botName + ","))
            {
                msg = msg.Replace(botName + ",", "");
            }
            if (msg.StartsWith(botName))
            {
                msg = msg.Replace(botName, "");
            }
            return msg;
        }

        private String ChatWithTuringBot(String msg)
        {
            String result = String.Empty;

            using (WebClient client = new WebClient())
            {
                client.QueryString.Add("key", turingKey);
                client.QueryString.Add("info", msg);
                try
                {
                    var responsebytes = client.DownloadData(turingApi);
                    result = Encoding.UTF8.GetString(responsebytes);

                    dynamic json = JToken.Parse(result);
                    Int32 code = json.code;
                    String text = json.text;
                    String url = json.url;

                    switch (code)
                    {
                        case 40001:
                            result = "参数 key 错误";
                            break;
                        case 40002:
                        case 40007:
                            result = String.Empty;
                            break;
                        case 40004:
                            result = "当天请求次数已使用完";
                            break;
                        case 100000:
                            result = text;
                            break;
                        case 200000:
                            result = text + "  " + url;
                            break;
                    }
                }
                catch
                {
                    logger.Error("Chat with Turing Robot failed");
                    result = String.Empty;
                }
            }

            return result;
        }
    }
}
