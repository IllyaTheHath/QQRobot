using Parsini;
using System;

namespace QQRobot.Util
{
    /**
     * 
     * INI文件解析
     * 使用到Nuget库Parsini
     * https://github.com/mathdeziel/Parsini
     * 
     **/
    internal sealed class IniParse
    {
        public static String INIGetValue(String iniFIle, String section, String key)
        {
            IniParser parser = new IniParser(iniFIle);
            return parser.Result.Sections[section].Keys[key].Value;
        }
    }
}
