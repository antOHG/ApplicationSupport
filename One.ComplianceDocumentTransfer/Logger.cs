using System;
using System.IO;

namespace OneComplianceDocumentTransfer
{
    public static class Logger
    {
        public static void WriteErrorLog(Exception ex)
        {
            WriteLog(ex.Source.ToString().Trim() + ": " + ex.Message.ToString().Trim());
        }
        public static void WriteLog(string ex)
        {
            var logFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
            if (Directory.Exists(logFolder) == false)
            {
                Directory.CreateDirectory(logFolder);
            }
            var fileName = "FileWatcher_" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
            var filePath = Path.Combine(logFolder, fileName);
            using (var sw = new StreamWriter(filePath, true))
            {
                sw.WriteLine(DateTime.Now.ToString() + ": " + ex);
            }
        }
    }
}
