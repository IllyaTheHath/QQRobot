using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using DumbQQ.Models;

namespace QQRobot.Service.Bot
{
    internal sealed class TestService : BotService
    {
        public override String Introduction => "测试机器人，会复读群友所有消息";

        public TestService() : base(nameof(TestService)) { }

        public override String Chat(GroupMessage message)
        {
            var msg = message.Content;
            var groupName = message.Group.Name;
            Thread.Sleep(100);
            logger.Info($"Chat To Group:[{groupName}]>>{msg}");
            return msg;
        }
    }
}
