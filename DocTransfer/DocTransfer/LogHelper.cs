using System;
using System.Configuration;
using System.IO;

namespace DocTransfer
{
    class LogHelper
    {
        internal static void LogFile(string content)
        {
            var path = ConfigurationManager.AppSettings["LogFilePath"]; ;
            using (var txtFile = File.AppendText(path))
            {
                txtFile.WriteLine(DateTime.Now.ToString() + " - " + content);
            }
        }
    }
}
