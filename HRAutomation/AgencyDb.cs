using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.OleDb;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace HRAutomation
{
    public class AgencyDb
    {
        public static void ReadAndSaveAccessData()
        {
            var csvFile = @"C:\Projects_Temp\Basheer\HRAutomation\Output.csv";
            /*
            "J" Drive referring to the below path should be available from the system where this program executes:
            \\Ohg.local\shares\HCS
            */
            var sqlQuery = "SELECT * FROM qryHoursForNorthgate";
            var param1 = "05/02/2018";
            var param2 = "25/01/2018";

            try
            {
                using (var conn = new OleDbConnection())
                {
                    conn.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data source=" + ConfigurationManager.AppSettings["AccessFile"];
                    conn.Open();
                    var cmd = new OleDbCommand(sqlQuery, conn);

                    DateTime dt1 = DateTime.ParseExact(param1, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    DateTime dt2 = DateTime.ParseExact(param2, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                    cmd.Parameters.Add("Exclude from week beginning date", OleDbType.Date).Value = dt1;
                    cmd.Parameters.Add("Enter most recent date entered", OleDbType.Date).Value = dt2;
                    var reader = cmd.ExecuteReader();

                    var hrsList = ConvertDataReaderToList(reader);

                    var content = ConvertHrsListToString(hrsList);

                    content = "<DEFN>" + Environment.NewLine + content + "<EOD>";
                    SaveFile(csvFile, content);

                    Console.WriteLine("Successfully extracted the data to " + csvFile);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.ReadLine();
        }

        public static void ToCsv(OleDbDataReader dataReader, string fileName, bool includeHeaderAsFirstRow)
        {
            const string Separator = ",";
            var streamWriter = new StreamWriter(fileName);
            var sb = new StringBuilder();

            if (includeHeaderAsFirstRow)
            {
                //sb = new StringBuilder();
                for (int index = 0; index < dataReader.FieldCount; index++)
                {
                    if (dataReader.GetName(index) != null)
                        sb.Append(dataReader.GetName(index));

                    if (index < dataReader.FieldCount - 1)
                        sb.Append(Separator);
                }
                //streamWriter.WriteLine(sb.ToString());
            }

            while (dataReader.Read())
            {
                sb.AppendLine();
                //sb = new StringBuilder();
                for (int index = 0; index < dataReader.FieldCount; index++)
                {
                    if (!dataReader.IsDBNull(index))
                    {
                        string value = dataReader.GetValue(index).ToString();
                        if (dataReader.GetFieldType(index) == typeof(String))
                        {
                            if (value.IndexOf("\"") >= 0)
                                value = value.Replace("\"", "\"\"");

                            if (value.IndexOf(Separator) >= 0)
                                value = "\"" + value + "\"";
                        }
                        sb.Append(value);
                    }

                    if (index < dataReader.FieldCount - 1)
                        sb.Append(Separator);
                }

                //if (!dataReader.IsDBNull(dataReader.FieldCount - 1))
                //{
                //    //sb.Append(dataReader.GetValue(dataReader.FieldCount - 1).ToString().Replace(Separator, " "));
                //    sb.Append(sb.ToString().Replace(Separator, " "));
                //}

                //streamWriter.WriteLine(sb.ToString());
            }

            streamWriter.WriteLine(sb.ToString());

            dataReader.Close();
            streamWriter.Close();
        }

        private static string ConvertDataReaderToString(OleDbDataReader dataReader)
        {
            const string Separator = ",";
            var sb = new StringBuilder();

            for (int index = 0; index < dataReader.FieldCount; index++)
            {
                if (dataReader.GetName(index) != null)
                    sb.Append(EncloseContent(dataReader.GetName(index)));

                if (index < dataReader.FieldCount - 1)
                    sb.Append(Separator);
            }
            sb.AppendLine();
            sb.Append("<DATA>");

            while (dataReader.Read())
            {
                sb.AppendLine();
                for (int index = 0; index < dataReader.FieldCount; index++)
                {
                    var columnName = dataReader.GetName(index);
                    if (!dataReader.IsDBNull(index))
                    {
                        string value = dataReader.GetValue(index).ToString();
                        //if (string.IsNullOrEmpty(value) && columnName == "Rate")
                        //{
                        //    continue;
                        //}
                        if (dataReader.GetFieldType(index) == typeof(string))
                        {
                            if (value.IndexOf("\"") >= 0)
                                value = value.Replace("\"", "\"\"");

                            if (value.IndexOf(Separator) >= 0)
                                value = "\"" + value + "\"";
                        }
                        sb.Append(value);
                    }
                    if (index < dataReader.FieldCount - 1)
                    {
                        sb.Append(Separator);
                    }

                }
            }
            dataReader.Close();
            return sb.ToString();
        }

        private static List<Hours> ConvertDataReaderToList(OleDbDataReader dataReader)
        {
            var hrsList = new List<Hours>();
            while (dataReader.Read())
            {
                var hrs = new Hours();
                for (int index = 0; index < dataReader.FieldCount; index++)
                {
                    var columnName = dataReader.GetName(index);
                    if (!dataReader.IsDBNull(index))
                    {
                        var value = dataReader.GetValue(index).ToString();
                        switch (columnName.ToLower())
                        {
                            case "rate":
                                hrs.Rate = value;
                                break;
                            case "tempname":
                                hrs.TempName = value;
                                break;
                            case "costcentrecode":
                                hrs.CostCenterCode = value;
                                break;
                            case "sumofhrsworked":
                                hrs.SumOfHrsWorked = value;
                                break;
                            case "us_empno":
                                hrs.US_EmpNo = value;
                                break;
                            case "element code":
                                hrs.ElementCode = value;
                                break;
                        }
                    }
                }
                hrsList.Add(hrs);
            }
            dataReader.Close();
            return hrsList;
        }

        private static string ConvertHrsListToString(List<Hours> hrsList)
        {
            const string Separator = ",";
            var sb = new StringBuilder();

            var tempList = hrsList.Select(o => o.ElementCode).Distinct().ToList();
            tempList.Sort();
            var distList = tempList.Where(r => r != null).ToList();

            sb.AppendLine("<EMPID>,<POSTID>,<COSTCEN>," + ElementCodeColumns(distList));
            sb.AppendLine("<DATA>");

            foreach (var item in hrsList)
            {
                if (string.IsNullOrEmpty(item.ElementCode) || string.IsNullOrEmpty(item.US_EmpNo)) continue; //IGNORE IF ELEMENTCODE IS NULL OR EMPID IS NULL

                var content = item.US_EmpNo + Separator + "MAIN" + Separator + item.CostCenterCode + Separator + EvaluateElementCode(distList, item.ElementCode, item.SumOfHrsWorked);
                sb.AppendLine(content);
            }

            return sb.ToString();
        }

        private static string EncloseContent(string content)
        {
            if (string.IsNullOrEmpty(content)) return content;
            else return "<" + content + ">";
        }

        private static void SaveFile(string filePath, string content)
        {
            using (var streamWriter = new StreamWriter(filePath))
            {
                streamWriter.WriteLine(content);
            }
        }

        private static string EvaluateElementCode(List<string> elemCodeList, string elemCode, string hoursWorked)
        {
            var content = string.Empty;
            foreach (var item in elemCodeList)
            {
                if (item == elemCode)
                {
                    content += hoursWorked + ",";
                }
                else
                {
                    content += ",";
                }
            }

            return (content.Contains(",")) ? content.Substring(0, content.Length - 1) : "";
        }

        private static string ElementCodeColumns(List<string> elemCodeList)
        {
            var content = string.Empty;
            foreach (var item in elemCodeList)
            {
                content += EncloseContent(item) + ",";
            }

            return (content.Contains(",")) ? content.Substring(0, content.Length - 1) : "";
        }
    }

    public class Hours
    {
        public string Rate { get; set; }
        public string TempName { get; set; }
        public string CostCenterCode { get; set; }
        public string SumOfHrsWorked { get; set; }
        public string US_EmpNo { get; set; }
        public string ElementCode { get; set; }
    }
}

