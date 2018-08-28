using DumbQQ.Models;
using QQRobot.Util;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace QQRobot.Service.Bot
{
    abstract class BotService
    {
        public QQBotLogger logger;

        public abstract String Introduction { get; }

        public BotService(String name)
        {
            logger = new QQBotLogger(name);
        }

        public static BotService GetServiceInstance(String name)
        {
            BotService service = null;
            if (name != "")
            {
                //service = (BotService)Activator.CreateInstance(Assembly.GetExecutingAssembly().GetType(name));
                service = (BotService)Assembly.GetExecutingAssembly().CreateInstance(name);
            }
            return service;
        }

        public abstract String Chat(GroupMessage message);

    }
}
