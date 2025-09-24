using System;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Puzzled
{

    ////////////////////////////////////////////////////////////////////////////////////
    // Logger
    ////////////////////////////////////////////////////////////////////////////////////
    public class Logger
    {

        ////////////////////////////////////////////////////////////////////////////////////
        // Severity enum
        ////////////////////////////////////////////////////////////////////////////////////
        public enum Severity
        {
            Trace = 0,
            Info,
            Warning,
            Error,
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Static methods
        ////////////////////////////////////////////////////////////////////////////////////
        public static void Trace(string message) { Log(Severity.Trace, message); }
        public static void Info(string message) { Log(Severity.Info, message); }
        public static void Warning(string message) { Log(Severity.Warning, message); }
        public static void Warn(string message) { Warning(message); }
        public static void Error(string message) { Log(Severity.Error, message); }

        ////////////////////////////////////////////////////////////////////////////////////
        // Private static methods
        ////////////////////////////////////////////////////////////////////////////////////
        private static ConsoleColor SeverityToColour(Severity severity)
        {
            switch (severity)
            {
            case Severity.Trace:    return ConsoleColor.White;
            case Severity.Info:     return ConsoleColor.Green;
            case Severity.Warning:  return ConsoleColor.Yellow;
            case Severity.Error:    return ConsoleColor.Red;

            default:
                Debug.Assert(false, "Unreachable.");
                break;
            }

            return ConsoleColor.White;
        }

        private static string SeverityToTag(Severity severity)
        {
            switch (severity)
            {
            case Severity.Trace:    return "TRACE";
            case Severity.Info:     return "INFO";
            case Severity.Warning:  return "WARN";
            case Severity.Error:    return "ERROR";

            default:
                Debug.Assert(false, "Unreachable.");
                break;
            }

            return "<UNKNOWN>";
        }

        private static void Log(Severity severity, string message)
        {
            Console.ForegroundColor = SeverityToColour(severity);
            Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] [{SeverityToTag(severity)}]: {message}");
            Console.ResetColor();
        }

    }

}