using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace board_games.src.Log
{
    /// <summary>
    /// Interface for a simple logger. Method "shutDown" should always be called when the logger will no longer be used.
    /// Otherwise, log messages might be lost
    /// </summary>
    public interface IFileLogger
    {
        void Log(string message, LogLevel logLevel);
        void Shutdown();
        string GetFileName();
        void ChangeFileName(string newFileName);
    }
}
