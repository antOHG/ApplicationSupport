using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace Concatenator
{
    public class DbHelper
    {
        internal static User CheckUser(string userName)
        {
            User user = null;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["SqlConnUser"]))
            {
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "CheckUser";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@User", userName);
                    conn.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        user = new User();
                        while (rdr.Read())
                        {
                            user.ID = int.Parse(rdr["ID"].ToString());
                            user.UserName = rdr["UserName"].ToString();
                            user.FullName = rdr["FullName"].ToString();
                            user.CreatedDate = DateTime.Parse(rdr["CreatedDate"].ToString());
                        }
                    }
                }
            };
            return user;
        }

        internal static int ExecuteCommand(string sQuery, CommandType commandType)
        {
            int rowEffected = 0;
            SqlConnection _connection = new SqlConnection(ConfigurationManager.AppSettings["SqlConn"]);
            try
            {
                _connection.Open();
                SqlCommand command = new SqlCommand(sQuery, _connection);
                command.CommandType = commandType;
                command.CommandTimeout = 0;
                rowEffected = command.ExecuteNonQuery();
                _connection.Close();
            }
            catch (Exception ex)
            {
                rowEffected = -9;
                _connection.Close();
                throw ex;
            }
            finally
            {
                _connection.Close();
            }
            return rowEffected;
        }

        internal static string RunLoad(User user)
        {
            string result = string.Empty;
            int rows = 0;
            string SPCallSSIS = "[u_sp_saf_concatenator]";
            rows = ExecuteCommand(SPCallSSIS, CommandType.StoredProcedure);
            InsertLog(user.ID);

            if (rows > 0)
            {
                result = "Successfully executed the SAF Concatenator.";
            }
            else
            {
                result = "Error while executing the SAF Concatenator, please contact 'Application Support Team' for assistance.";
            }
            return result;
        }

        internal static DataSet GetDataSet(string query)
        {
            SqlDataAdapter da;
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            SqlConnection con;

            using (con = new SqlConnection(ConfigurationManager.AppSettings["SqlConnUser"]))
            {
                cmd.CommandText = query;
                cmd.Connection = con;
                da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                con.Open();
                cmd.ExecuteNonQuery();
                return ds;
            };
        }

        internal static void InsertLog(int userId)
        {
            var query = "INSERT INTO Logs VALUES (" + userId + ", GETDATE(),'Start Process')";
            int rowEffected = 0;
            using (SqlConnection _connection = new SqlConnection(ConfigurationManager.AppSettings["SqlConnUser"]))
            {
                _connection.Open();
                SqlCommand command = new SqlCommand(query, _connection);
                command.CommandType = CommandType.Text;
                command.CommandTimeout = 0;
                rowEffected = command.ExecuteNonQuery();
                _connection.Close();
            };
        }

        internal static void InsertLog(string content)
        {
            if (!bool.Parse(ConfigurationManager.AppSettings["TextLogging"])) return;

            using (StreamWriter w = File.AppendText(ConfigurationManager.AppSettings["LogFilePath"]))
            {
                w.WriteLine(content);
                w.Flush();
            }
        }
    }
}