using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Reflection.Emit;

namespace board_games.src.Log
{
    public class SimpleFileLogger : IFileLogger
    {
        private const string _defaultFileName = "logs.txt";

        private string _fileName;
        private int _infoLogsBufferCapacity;
        private List<Tuple<DateTime, string>> _infoLogsBuffer;
        private readonly object generalUsageLock = new object(); // TODO more locks?

        public SimpleFileLogger(string fileName, int infoLogsBufferCapacity)
        {
            _fileName = fileName;
            _infoLogsBufferCapacity = infoLogsBufferCapacity;
            _infoLogsBuffer = new List<Tuple<DateTime, string>>();
        }
        public SimpleFileLogger(string fileName) : this(fileName, 20) { }

        public SimpleFileLogger() : this(_defaultFileName) { }


        public void Log(string message, LogLevel level)
        {
            if (level == LogLevel.Info)
            {
                HandleInfoMessage(message);
            }
            else
            {
                FlushInfoMessages();

                lock (generalUsageLock)
                {
                    using (StreamWriter fileWriter = new StreamWriter(_fileName, true))
                    {
                        string logLevelPreffix = (level == LogLevel.Warn ? "[WARN]" : "[ERROR]");

                        fileWriter.WriteLine(GetPrintableLogMessage(DateTime.Now, logLevelPreffix, message));
                    }
                }
            }
        }

        public void Shutdown()
        {
            FlushInfoMessages();
            _infoLogsBuffer.Clear();
        }

        public string GetFileName()
        {
            return _fileName;
        }

        public void ChangeFileName(string newFileName)
        {
            FlushInfoMessages();
            _fileName = newFileName;
        }

        private void HandleInfoMessage(string message)
        {
            lock (generalUsageLock)
            {
                _infoLogsBuffer.Add(new Tuple<DateTime, string>(DateTime.Now, message));
                if (_infoLogsBuffer.Count > _infoLogsBufferCapacity)
                {
                    FlushInfoMessages();
                }
            }
        }

        private void FlushInfoMessages()
        {
            StringBuilder allMessages = new StringBuilder();
            lock (generalUsageLock)
            {
                foreach (Tuple<DateTime, string> timestampedMessage in _infoLogsBuffer)
                {
                    DateTime timestamp = timestampedMessage.Item1;
                    string message = timestampedMessage.Item2;
                    allMessages.AppendLine(GetPrintableLogMessage(timestamp, "[INFO]", message));
                }

                using (StreamWriter fileWriter = new StreamWriter(_fileName, true))
                {
                    fileWriter.Write(allMessages);
                }
                _infoLogsBuffer.Clear();
            }
        }

        private string GetPrintableLogMessage(DateTime timestamp, string logLevelPreffix, string message)
        {
            return timestamp + " " + logLevelPreffix + " " + message;
        }
    }
}
