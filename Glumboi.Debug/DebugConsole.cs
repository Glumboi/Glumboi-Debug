using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Glumboi.Debug
{
    public class DebugConsole
    {
        // http://msdn.microsoft.com/en-us/library/ms681944(VS.85).aspx
        /// <summary>
        /// Allocates a new console for the calling process.
        /// </summary>
        /// <returns>nonzero if the function succeeds; otherwise, zero.</returns>
        /// <remarks>
        /// A process can be associated with only one console,
        /// so the function fails if the calling process already has a console.
        /// </remarks>
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern int AllocConsole();

        // http://msdn.microsoft.com/en-us/library/ms683150(VS.85).aspx
        /// <summary>
        /// Detaches the calling process from its console.
        /// </summary>
        /// <returns>nonzero if the function succeeds; otherwise, zero.</returns>
        /// <remarks>
        /// If the calling process is not already attached to a console,
        /// the error code returned is ERROR_INVALID_PARAMETER (87).
        /// </remarks>
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern int FreeConsole();

        int _logLvl = (int)Levels.LogLevelTrace;

        enum Levels
        {
            LogLevelError = 0,
            LogLevelWarning = 1,
            LogLevelTrace = 2
        }

        public DebugConsole(int lvl, string consoleTitle, bool debugMode, bool catchExceptions)
        {
            if (!debugMode) return;
            AllocConsole();
            _logLvl = lvl;
            Console.Title = consoleTitle;
            Init(consoleTitle, catchExceptions);
        }

        public void ChangeLevel(int lvl) => _logLvl = lvl;

        public void Info(string message)
        {
            if (_logLvl >= (int)Levels.LogLevelTrace)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("\n[INFO]: ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(message);
            }
        }

        public void Warn(string message)
        {
            if (_logLvl >= (int)Levels.LogLevelWarning)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("\n[WARNING]: ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(message);
            }
        }

        public void Error(string message)
        {
            if (_logLvl >= (int)Levels.LogLevelError)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("\n[ERROR]: ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(message);
            }
        }

        private void Init(string consoleTitle, bool catchExceptions)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("\n[Initializing console...]: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"Logging level: {_logLvl}");
            if (catchExceptions) CatchAllExceptions();
        }

        private void CatchAllExceptions()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
        }

        private void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            Error(e.Exception.Message);
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Error((e.ExceptionObject as Exception).Message);
        }
    }

}
