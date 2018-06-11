using System;
using System.Collections.Generic;

namespace QQRobot.Util
{
    internal sealed class BotConfig
    {
        public static String BotName { get; set; }

        public static Int32 BotType { get; set; }

        public static List<String> WorkGroups { get; set; }

        public static String TuringApi { get; set; }

        public static String TuringKey { get; set; }
    }
}
