using DumbQQ.Client;
using System;
using System.IO;

namespace QQRobot.Util
{
    internal sealed class QrCode
    {
        private static QQBotLogger logger = new QQBotLogger(nameof(QrCode));

        public static Int32 Login(DumbQQClient client)
        {
            while (true)
            {
                switch (client.Start(array =>
                {
                    File.WriteAllBytes(SharedInfo.QrCodeFile, array);
                    logger.Info($"二维码已获取，请打开文件 {SharedInfo.QrCodeFile} 扫描二维码");
                }))
                {
                    case DumbQQClient.LoginResult.Succeeded:
                        return 0;

                    case DumbQQClient.LoginResult.QrCodeExpired:
                        logger.Warn("二维码失效，请重新扫码");
                        continue;
                    default:
                        logger.Warn("登录失败，需要重试吗？(y / n)");
                        var response = Console.ReadLine();
                        if (response.Contains("y"))
                        {
                            continue;
                        }
                        return 1;
                }
            }
        }
    }
}
