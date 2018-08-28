using DumbQQ.Models;
using QQRobot.Util;
using System;
using System.Threading;

namespace QQRobot.Service.Bot
{
    internal sealed class RepeatService : BotService
    {
        private Int32 probability;

        public override String Introduction => $"复读机，{probability}% 几率复读群友说的话";

        public RepeatService() : base(nameof(RepeatService))
        {
            this.probability = Int32.Parse(IniParse.INIGetValue(SharedInfo.ConfigFile, "Repeat", "Probability"));
        }

        public override String Chat(GroupMessage message)
        {
            var msg = message.Content;
            var groupName = message.Group.Name;
            if (IsLucky())
            {
                Thread.Sleep(new Random(Guid.NewGuid().GetHashCode()).Next(0, 2000));
                logger.Info($"Chat To Group:[{groupName}]>>{msg}");
                return msg;
            }
            return String.Empty;
        }
        
        private Boolean IsLucky()
        {
            Int32 num = new Random(Guid.NewGuid().GetHashCode()).Next(1, 100);
            if (num >= 1 && num <= probability)
                return true;
            else
                return false;
        }
    }
}
