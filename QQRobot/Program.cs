using DumbQQ.Client;
using QQRobot.Service;
using QQRobot.Util;
using System;

namespace QQRobot
{
    public class Program
    {
        public static readonly DumbQQClient client = new DumbQQClient { CacheTimeout = TimeSpan.FromDays(1) };
        private static QQBotLogger logger = new QQBotLogger(SharedInfo.AppName);

        public static void Main(string[] args)
        {
            // 程序退出时关闭客户端
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(CurrentDomain_ProcessExit);
            Console.CancelKeyPress += CurrentDomain_ProcessExit;
            // 启动服务
            QQService.StartQQBot(client);
            // 防止程序终止
            logger.Info("程序正在运行，输入exit退出");
            while (true)
            {
                var line = Console.ReadLine();
                if ("exit" == line)
                {
                    Environment.Exit(1);
                }
            }
        }

        private static void CurrentDomain_ProcessExit(Object sender, EventArgs e)
        {
            logger.Debug("Exiting");
            if (client.Status == DumbQQClient.ClientStatus.Active)
            {
                QQService.CloseQQClient();
            }
        }
    }
}
