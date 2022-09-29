using System;
using System.Collections.Generic;
using System.IO;
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
        private static extern int AllocConsole();

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

        private static readonly StringBuilder LogString = new StringBuilder();

        private int _logLvl = (int)Levels.LogLevelTrace;

        private enum Levels
        {
            LogLevelError = 0,
            LogLevelWarning = 1,
            LogLevelTrace = 2
        }

        public DebugConsole(int lvl = 2, string consoleTitle = "Console", bool debugMode = true, bool catchExceptions = false)
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
            if (_logLvl < (int)Levels.LogLevelTrace) return;
            Console.ForegroundColor = ConsoleColor.Blue;
            Write("\n[INFO]: ");
            Console.ForegroundColor = ConsoleColor.White;
            Write(message);
        }

        public void Warn(string message)
        {
            if (_logLvl < (int)Levels.LogLevelWarning) return;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Write("\n[WARNING]: ");
            Console.ForegroundColor = ConsoleColor.White;
            Write(message);
        }

        public void Error(string message)
        {
            if (_logLvl < (int)Levels.LogLevelError) return;
            Console.ForegroundColor = ConsoleColor.Red;
            Write("\n[ERROR]: ");
            Console.ForegroundColor = ConsoleColor.White;
            Write(message);
        }

        private void Init(string consoleTitle, bool catchExceptions)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Write("\n[Initializing console...]: ");
            Console.ForegroundColor = ConsoleColor.White;
            Write($"Logging level: {_logLvl}\n");
            if (catchExceptions) CatchAllExceptions();
        }

        private void CatchAllExceptions()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
        }

        public static void WriteLine(string str)
        {
            Console.WriteLine(str);
            LogString.Append(str).Append(Environment.NewLine);
        }

        public static void Write(string str)
        {
            Console.Write(str);
            LogString.Append(str);
        }

        public void SaveLog(bool Append = false, string Path = "./Log.txt")
        {
            if (LogString == null || LogString.Length <= 0) return;
            if (Append)
            {
                using (StreamWriter file = System.IO.File.AppendText(Path))
                {
                    file.Write(LogString.ToString());
                    file.Close();
                    file.Dispose();
                }
            }
            else
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(Path))
                {
                    file.Write(LogString.ToString());
                    file.Close();
                    file.Dispose();
                }
            }
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
