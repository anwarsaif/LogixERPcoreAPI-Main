using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logix.Application.Interfaces.IRepositories;
using System.IO;

namespace Logix.Application.Common
{
    /// <summary>
    /// Simple logging abstraction used to persist error messages to disk.
    /// This is intentionally lightweight; for production scenarios consider using a structured logging
    /// framework (e.g., Serilog, NLog, Microsoft.Extensions.Logging) with proper sinks and retention policies.
    /// </summary>
    public interface ILogError
    {
        /// <summary>
        /// Adds an error entry to the configured log sink (file in the current implementation).
        /// </summary>
        /// <param name="errorMessage">The textual error message or exception message.</param>
        /// <param name="functionName">The name of the function where the error occurred.</param>
        /// <param name="className">The class name where the error occurred.</param>
        /// <returns>True when the write succeeded; otherwise false.</returns>
        bool Add(string errorMessage, string functionName, string className);
    }

    /// <summary>
    /// File-based logger that appends error lines to a per-month log file under Files/Logs/{year}.
    /// </summary>
    public class LogError : ILogError
    {
        /// <summary>
        /// Writes an error entry to the current log file.
        /// </summary>
        /// <remarks>
        /// This method swallows exceptions and returns false on failure to keep callers simple.
        /// Consider allowing the exception to bubble or logging the failure via a secondary mechanism.
        /// </remarks>
        public bool Add(string errorMessage, string functionName, string className)
        {
            try
            {
                // Build the directory and file path based on current year/month to keep logs organized.
                string currentYear = DateTime.Now.Year.ToString();
                string logsDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files", "Logs", currentYear);
                string logFileName = $"log_{DateTime.Now:yy_MM}.txt";
                string logFilePath = Path.Combine(logsDirectory, logFileName);

                // Ensure the target directory exists.
                if (!Directory.Exists(logsDirectory))
                {
                    Directory.CreateDirectory(logsDirectory);
                }

                // Compose a single-line timestamped log entry.
                string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - Error in class '{className}', function '{functionName}': {errorMessage}";

                // Append the log entry to the file. Using StreamWriter with append=true opens/locks briefly.
                using (StreamWriter writer = new StreamWriter(logFilePath, true))
                {
                    writer.WriteLine(logEntry);
                }

                return true;
            }
            catch
            {
                // Swallow exceptions to avoid cascading failures from logging itself.
                return false;
            }
        }

        /// <summary>
        /// Static convenience method to quickly add an error to the log without creating an instance.
        /// Mirrors the instance behaviour but does not return success/failure to the caller.
        /// </summary>
        public static void AddToFile(string errorMessage, string functionName, string className)
        {
            try
            {
                string currentYear = DateTime.Now.Year.ToString();
                string logsDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files", "Logs", currentYear);
                string logFileName = $"log_{DateTime.Now:yy_MM}.txt";
                string logFilePath = Path.Combine(logsDirectory, logFileName);

                if (!Directory.Exists(logsDirectory))
                {
                    Directory.CreateDirectory(logsDirectory);
                }

                string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - Error in class '{className}', function '{functionName}': {errorMessage}";

                using (StreamWriter writer = new StreamWriter(logFilePath, true))
                {
                    writer.WriteLine(logEntry);
                }
            }
            catch
            {
                // Intentionally ignore logging failures in this static helper.
            }
        }
    }
}
