using NLog;
using System;

namespace QQRobot.Util
{
    internal sealed class QQBotLogger
    {
        private readonly Logger logger;

        internal QQBotLogger(String name)
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            logger = LogManager.GetLogger(name);
        }

        /// <summary>
        /// 调试日志
        /// </summary>
        /// <param name="msg">日志内容</param>
        public void Debug(String msg)
        {
            logger.Debug(msg);
        }

        /// <summary>
        /// 信息日志
        /// </summary>
        /// <param name="msg">日志内容</param>
        public void Info(String msg)
        {
            logger.Info(msg);
        }

        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="msg">日志内容</param>
        public void Error(String msg)
        {
            logger.Error(msg);
        }

        /// <summary>
        /// 严重致命错误日志
        /// </summary>
        /// <param name="msg">日志内容</param>
        public void Fatal(String msg)
        {
            logger.Fatal(msg);
        }

        /// <summary>
        /// 警告日志
        /// </summary>
        /// <param name="msg">日志内容</param>
        public void Warn(String msg)
        {
            logger.Warn(msg);
        }

    }
}
