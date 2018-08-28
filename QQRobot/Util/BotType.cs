using QQRobot.Service.Bot;
using System;
using System.Collections.Generic;
using System.Text;

namespace QQRobot.Util
{
    internal sealed class BotType
    {
        public String Name { get; private set; }
        public String Type { get; private set; }

        public BotType(String name, String type)
        {
            this.Name = name;
            this.Type = type;
        }

        public static explicit operator BotType(String v)
        {
            String n = null;
            switch (v)
            {
                default:
                case "Turing":
                    n = typeof(TuringService).FullName;
                    break;
                case "Repeat":
                    n = typeof(RepeatService).FullName;
                    break;
                case "Test":
                    n = typeof(TestService).FullName;
                    break;
            }
            return new BotType(n,v);
        }
    }
}
