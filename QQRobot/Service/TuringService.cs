using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QQRobot.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace QQRobot.Service
{
    internal sealed class TuringService
    {
        private static QQBotLogger logger = new QQBotLogger(nameof(TuringService));

        public static String Chat(String msg, String groupName)
        {
            if (null == BotConfig.TuringApi || null == BotConfig.TuringKey)
            {
                logger.Warn("图灵机器人没有正确配置，无法工作");
                return null;
            }
            if (String.IsNullOrEmpty(msg) || !msg.Contains(BotConfig.BotName))
            {
                return null;
            }

            msg = ReplaceBotName(msg);
            msg =  ChatWithTuringBot(msg);
            logger.Info($"Chat To Group:[{groupName}]>>{msg}");
            return msg;
        }

        private static String ReplaceBotName(String msg)
        {
            if (String.IsNullOrEmpty(msg))
            {
                return null;
            }
            if (msg.StartsWith(BotConfig.BotName + " "))
            {
                msg = msg.Replace(BotConfig.BotName + " ", "");
            }
            if (msg.StartsWith(BotConfig.BotName + "，"))
            {
                msg = msg.Replace(BotConfig.BotName + "，", "");
            }
            if (msg.StartsWith(BotConfig.BotName + ","))
            {
                msg = msg.Replace(BotConfig.BotName + ",", "");
            }
            if (msg.StartsWith(BotConfig.BotName))
            {
                msg = msg.Replace(BotConfig.BotName, "");
            }
            return msg;
        }

        public static String ChatWithTuringBot(String msg)
        {
            String result = String.Empty;

            using (WebClient client = new WebClient())
            {
                client.QueryString.Add("key", BotConfig.TuringKey);
                client.QueryString.Add("info", msg);
                try
                {
                    var responsebytes = client.DownloadData(BotConfig.TuringApi);
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
