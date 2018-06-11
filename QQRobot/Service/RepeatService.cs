using QQRobot.Util;
using System;
using System.Threading;

namespace QQRobot.Service
{
    internal sealed class RepeatService
    {
        private static QQBotLogger logger = new QQBotLogger(nameof(RepeatService));

        public static String Chat(String msg, String groupName)
        {
            var num = GetRandNum(1, 100);
            if (num >= 40 && num <= 45)
            {
                logger.Info($"Chat To Group:[{groupName}]>>{msg}");
                Thread.Sleep(GetRandNum(0, 2000));
                return msg;
            }
            return String.Empty;
        }

        private static Int32 GetRandNum(int min, int max)
        {
            Random r = new Random(Guid.NewGuid().GetHashCode());
            return r.Next(min, max);
        }
    }
}
