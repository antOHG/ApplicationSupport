using System;
using System.Configuration;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.IO;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;


namespace ActiveDirectoryLookup
{
    public class Program
    {
        static StringBuilder builder = new StringBuilder();

        public static void Main(string[] args)
        {
            var startTime = DateTime.Now;
            var domainName = ConfigurationManager.AppSettings["DomainName"];

            //GetAllFields(domainName); //GET ALL FIELDS FROM ACTIVE DIRECTORY

            GetAllUsersAndSaveToDb(domainName);

            var endTime = DateTime.Now;
            WriteLog("Application started at: " + startTime + Environment.NewLine + "Application ended at: " + endTime);
            Console.WriteLine("Successfully retrieved Users from Active Directory");
            Console.WriteLine("\nPress ENTER to exit");
            Console.ReadLine();
        }

        private static void GetAllUsersAndSaveToDb(string domainName)
        {
            using (var context = new PrincipalContext(ContextType.Domain, domainName))
            {
                using (var searcher = new PrincipalSearcher(new UserPrincipal(context)))
                {
                    foreach (var result in searcher.FindAll())
                    {
                        DirectoryEntry de = result.GetUnderlyingObject() as DirectoryEntry;

                        var sb = new StringBuilder();
                        foreach (var prop in de.Properties.PropertyNames)
                        {
                            if (prop.ToString() == "comment" || prop.ToString() == "msExchSenderHintTranslations") { continue; }
                            sb.Append(prop + "=" + de.Properties[prop.ToString()].Value + "|");
                        }
                        var temp = sb.ToString();

                        var query = MakeInsertQuery(temp);
                        if (query != "")
                        {
                            SaveToDatabase(query);
                        }
                    }
                }
            }
        }

        private static string GetAllFields(string domainName)
        {
            List<string> fieldNames = new List<string>();
            using (var context = new PrincipalContext(ContextType.Domain, domainName))
            {
                using (var searcher = new PrincipalSearcher(new UserPrincipal(context)))
                {
                    foreach (var result in searcher.FindAll())
                    {
                        DirectoryEntry de = result.GetUnderlyingObject() as DirectoryEntry;
                        foreach (var prop in de.Properties.PropertyNames)
                        {
                            if (prop.ToString() == "comment" || prop.ToString() == "msExchSenderHintTranslations") { continue; }
                            if (fieldNames.Contains(prop.ToString()) == false)
                            {
                                fieldNames.Add(prop.ToString());
                            }
                        }
                    }
                }
            }
            var fields = "";
            foreach (var item in fieldNames)
            {
                fields += item + "|";
            }
            return fields;
        }

        private static void WriteLog(string content)
        {
            if (!bool.Parse(ConfigurationManager.AppSettings["EnableLogFile"])) return;
            using (var writetext = new StreamWriter(ConfigurationManager.AppSettings["LogFilePath"], true))
            {
                writetext.WriteLine(content);
            }
        }

        private static void GetDomains()
        {
            //CURRENTLY NOT USING THIS METHOD
            DirectoryEntry de = new DirectoryEntry();
            DirectorySearcher ds = new DirectorySearcher(de, "objectCategory=Domain");
            SearchResultCollection srcoll = ds.FindAll();
            foreach (SearchResult rs in srcoll)
            {
                ResultPropertyCollection resultPropColl = rs.Properties;
                foreach (object domainName in resultPropColl["name"])
                {
                    Console.WriteLine(domainName.ToString());
                }
            }
        }

        private static void SaveToDatabase(string query)
        {
            var connetionString = ConfigurationManager.AppSettings["ConnStr"];
            using (var cnn = new SqlConnection(connetionString))
            {
                cnn.Open();
                using (var cmd = new SqlCommand(query, cnn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private static string MakeInsertQuery(string input)
        {
            input = input.Trim();
            if (input.Contains("=") == false || input == "") return "";

            var query = string.Empty;
            var columns = "";
            var columnValue = "";
            var data = input.Split('|');

            foreach (var item in data)
            {
                if (item == "") continue;
                int separator = item.IndexOf('=');
                columns += "[" + item.Substring(0, separator).Trim() + "],";
                columnValue += "'" + item.Substring(separator + 1).Trim().Replace("'", "''") + "',";
            }

            if (columns.LastIndexOf(',') == columns.Length - 1)
            {
                columns = columns.Remove(columns.Length - 1);
            }
            if (columnValue.LastIndexOf(',') == columnValue.Length - 1)
            {
                columnValue = columnValue.Remove(columnValue.Length - 1);
            }

            query = "INSERT INTO AD (" + columns + ") VALUES(" + columnValue + ")";

            return query;
        }
    }
}
