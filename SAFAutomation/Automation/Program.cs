using System;
using System.Configuration;
using System.IO;
using System.Text;

namespace Automation
{
    public class Program
    {
        public static int MaxLen = int.Parse(ConfigurationManager.AppSettings["MaxLength"]);

        public static void Main(string[] args)
        {
            string[] files = Directory.GetFiles(ConfigurationManager.AppSettings["SourcePath"], "*.*", SearchOption.TopDirectoryOnly);
            foreach (var file in files)
            {
                ProcessFile(file);
            }
        }

        public static void ProcessFile(string fileName)
        {
            var sb = new StringBuilder();
            using (StreamReader sr = File.OpenText(fileName))
            {
                string s = string.Empty;
                while ((s = sr.ReadLine()) != null)
                {
                    if (s.Length > 0)
                    {
                        var temp = MakeCorrection(s);

                        sb.AppendLine(temp);
                    }
                }
            }

            var newFileName = string.Empty;
            if (sb.ToString().Length > 0)
            {
                var fi = new FileInfo(fileName);
                newFileName = fi.Name;
                var newFile = Path.Combine(ConfigurationManager.AppSettings["DestinationPath"], newFileName);
                if (File.Exists(newFile))
                {
                    File.Delete(newFile);
                }
                WriteFile(sb.ToString().TrimEnd('\r', '\n', '\n', '\r', '\n'), newFile);
            }
            Console.WriteLine("Successfully created the file: " + newFileName);
        }

        public static void WriteFile(string content, string fileName)
        {
            using (var sw = File.AppendText(fileName))
            {
                sw.WriteLine(content);
            }
        }

        public static string MakeCorrection(string content)
        {
            if (content.Length < MaxLen)
            {
                var contentLength = content.Length;
                for (int i = contentLength; i < MaxLen; i++)
                {
                    content += " ";
                }
            }
            return content;
        }
    }
}
