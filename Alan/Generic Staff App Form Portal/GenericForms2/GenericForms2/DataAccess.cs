using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace GenericForms2
{
    /// <summary>
    /// Singleton class that handles all data access for the application
    /// </summary>
    public class DataAccess : IDisposable
    {
        private static DataAccess _Instance;

        public static DataAccess Instance
        {
            get 
            {
                if (_Instance != null)
                {
                    return _Instance;
                }
                else
                {
                    _Instance = new DataAccess();
                    return _Instance;
                }
            }
        }
        
        public DataAccess()
        {
            _dbCon = new SqlConnection(connectionString);
        }

        //private const string connectionString = "Server=.;Database=GenericForms;Trusted_Connection=True;Connection Timeout=30;";
        private string connectionString = Properties.Settings.Default.ConnectionString;

        private SqlConnection _dbCon;

        /// <summary>
        /// The connection property's getter handles a number of scenarios, opening the connection as necessary
        /// </summary>
        private SqlConnection dbCon
        {
            get 
            {
                if (_dbCon != null)
                {
                    if (_dbCon.State == ConnectionState.Closed)
                    {
                        if (_dbCon.ConnectionString == null || _dbCon.ConnectionString == string.Empty)
                        {
                            _dbCon.ConnectionString = connectionString;
                        }
                        _dbCon.Open();
                    } return _dbCon;
                }
                else
                {
                    _dbCon = new SqlConnection(connectionString);
                    _dbCon.Open();
                    return _dbCon;
                }
            }
        }

        /// <summary>
        /// Gets details of a single form instance
        /// </summary>
        /// <param name="completedFormId">The completed form ID, which is the ID in the Documotive database</param>
        /// <returns></returns>
        public IEnumerable<IDataRecord> GetFormInstance(int completedFormId)
        {
            var prm = new List<SqlParameter>() { new SqlParameter("@CompletedFormId", completedFormId) };
            return GetData("dbo.usp_GetFormInstance", prm);
        }

        /// <summary>
        /// Gets all fields for a particular form instance
        /// </summary>
        /// <param name="formInstanceId">The form instance ID internal to the application</param>
        /// <returns></returns>
        public IEnumerable<IDataRecord> GetFormFieldInstances(int formInstanceId)
        {
            var prm = new List<SqlParameter>() { new SqlParameter("@FormInstanceId", formInstanceId) };
            return GetData("dbo.usp_GetFormFieldInstances", prm);
        }

        /// <summary>
        /// Sets the character value for a field instance
        /// </summary>
        /// <param name="formField">The field to update</param>
        /// <returns>true if successful</returns>
        public bool UpdateFormFieldInstance(GenericForm.FormField formField)
        {
            return WriteData("usp_UpdateFormFieldInstance", new List<SqlParameter>()
            {
                new SqlParameter("@FormFieldInstanceId", formField.FormFieldInstanceId),
                new SqlParameter("@Value_char", formField.Value_char)
            });
        }

        /// <summary>
        /// Get the next document to process from the queue
        /// </summary>
        /// <returns></returns>
        public int GetNextDocToProcess()
        {
            return GetScalar<int>("dbo.usp_GetNextDocToProcess", new List<SqlParameter>());
        }

        /// <summary>
        /// Mark a document as processed
        /// </summary>
        /// <param name="formInstanceId"></param>
        /// <returns></returns>
        public bool MarkDocProcessed(int formInstanceId)
        {
            return WriteData("dbo.MarkDocProcessed", new List<SqlParameter>()
            {
                new SqlParameter("@FormInstanceId", formInstanceId)
            });
        }

        /// <summary>
        /// Mark a document as having errored during document creation
        /// </summary>
        /// <param name="formInstanceId"></param>
        /// <returns></returns>
        public bool MarkDocCreationError(int formInstanceId)
        {
            return WriteData("dbo.MarkDocCreationError", new List<SqlParameter>()
            {
                new SqlParameter("@FormInstanceId", formInstanceId)
            });
        }

        /// <summary>
        /// Determne if a user has permission to view a form based on their SID
        /// </summary>
        /// <param name="completedFormId"></param>
        /// <param name="sid">User's sid</param>
        /// <returns>true if permitted, false if not</returns>
        public bool IsPermitted(int completedFormId, string sid)
        {
            return GetScalar<bool>("usp_IsPermitted", new List<SqlParameter>()
            {
                new SqlParameter("@CompletedFormId_foreign",completedFormId),
                new SqlParameter("@SID",sid)
            });
        }

        /// <summary>
        /// Save a file to the database
        /// </summary>
        /// <param name="formInstanceId">Form instance ID associated with the file</param>
        /// <param name="dataType">The file's type (e.g. docx, pdf)</param>
        /// <param name="blob">File binary</param>
        /// <returns></returns>
        public bool SaveFile(int formInstanceId, string dataType, byte[] blob)
        {
            return WriteData("dbo.usp_AddFile",new List<SqlParameter>() 
            {
                new SqlParameter("@FormInstanceId",formInstanceId),
                new SqlParameter("@DataType",dataType),
                new SqlParameter("@Blob",blob)
            });
        }


        /// <summary>
        /// Add a form istance to the queue to create documents
        /// </summary>
        /// <param name="formInstanceId">Form instance ID</param>
        /// <returns></returns>
        public bool MarkReadyToBlob(int formInstanceId)
        {
            return WriteData("dbo.usp_MarkReadyToBlob", new List<SqlParameter>() 
            {
                new SqlParameter("@FormInstanceId",formInstanceId)
            });
        }


        /// <summary>
        /// Lock a form so that it cannot be edited by another user
        /// </summary>
        /// <param name="formInstanceId">Form instance ID</param>
        /// <param name="user">The user's name (not SID)</param>
        /// <param name="lockTimeoutMinutes">The time after which a lock is no longer valid</param>
        /// <returns>Empty string if successful, error message if not</returns>
        public string LockFormInstance(int formInstanceId, string user, int lockTimeoutMinutes)
        {
            return GetScalar<string>("dbo.usp_LockFormInstance", new List<SqlParameter>()
            {
                new SqlParameter("@FormInstanceId", formInstanceId),
                new SqlParameter("@User", user),
                new SqlParameter("@LockTimeoutMinutes", lockTimeoutMinutes)
            });
        }

        /// <summary>
        /// Mark a form as complete
        /// </summary>
        /// <param name="formInstanceId">Form instance ID</param>
        /// <param name="user">The user's name (not SID)</param>
        /// <param name="lockTimeoutMinutes">The time after which a lock is no longer valid</param>
        /// <returns>Empty string if successful, error message if not</returns>
        public string CompleteFormInstance(int formInstanceId, string user, int lockTimeoutMinutes)
        {
            return GetScalar<string>("dbo.usp_CompleteFormInstance", new List<SqlParameter>()
            {
                new SqlParameter("@FormInstanceId", formInstanceId),
                new SqlParameter("@User", user),
                new SqlParameter("@LockTimeoutMinutes", lockTimeoutMinutes)
            });
        }

        /// <summary>
        /// Get a list of forms to review that a user has permission to edit 
        /// </summary>
        /// <param name="sid">User's SID</param>
        /// <param name="referenceGroup">The reference group, "*" for all</param>
        /// <param name="formName">The form name, "*" for all</param>
        /// <returns></returns>
        public IEnumerable<IDataRecord> GetAvailableFormInstances(string sid, string referenceGroup, string formName)
        {
            return GetData("dbo.usp_GetAvailableFormInstances", new List<SqlParameter>()
            {
                new SqlParameter("@SID", sid),
                new SqlParameter("@ReferenceGroup", referenceGroup),
                new SqlParameter("@FormName", formName)
            });
        }

        /// <summary>
        /// Get a list of completed forms  on a user's SID
        /// </summary>
        /// <param name="sid">User's SID</param>
        /// <param name="referenceGroup">The reference group, "*" for all</param>
        /// <param name="formName">The form name, "*" for all</param>
        public IEnumerable<IDataRecord> GetCompletedFormInstances(string sid, string referenceGroup, string formName)
        {
            return GetData("dbo.usp_GetCompletedFormInstances", new List<SqlParameter>()
            {
                new SqlParameter("@SID", sid),
                new SqlParameter("@ReferenceGroup", referenceGroup),
                new SqlParameter("@FormName", formName)
            });
        }

        /// <summary>
        /// Get a PDF file
        /// </summary>
        /// <param name="formInstanceId">The form instance ID</param>
        /// <returns></returns>
        public byte[] GetPdf(int formInstanceId)
        {
            return GetScalar<byte[]>("dbo.usp_GetPDF", new List<SqlParameter>()
            {
                new SqlParameter("@FormInstanceId",formInstanceId)
            });
        }

        /// <summary>
        /// Read multiple records from the database
        /// </summary>
        /// <param name="storedProc">Name of stored procedure to execute</param>
        /// <param name="parameters">Collection of parameters</param>
        /// <returns>Collection of data records</returns>
        private IEnumerable<IDataRecord> GetData(string storedProc, IEnumerable<SqlParameter> parameters)
        {
            using (var cmd = new SqlCommand(storedProc, this.dbCon))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                foreach (var prm in parameters)
                {
                    cmd.Parameters.Add(prm);
                }

                using (var rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                        yield return rdr;
                    rdr.Close();
                }
                dbCon.Close();
            }
        }

        /// <summary>
        /// Read a single value from the database
        /// </summary>
        /// <typeparam name="T">The type to cast the value to</typeparam>
        /// <param name="StoredProc">Name of stored procedure to execute</param>
        /// <param name="parameters">Collection of parameters</param>
        /// <returns></returns>
        private T GetScalar<T>(string StoredProc, IEnumerable<SqlParameter> parameters)
        {
            using (var cmd = new SqlCommand(StoredProc, this.dbCon))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                foreach (var prm in parameters)
                {
                    cmd.Parameters.Add(prm);
                }

                T val = (T)Convert.ChangeType(cmd.ExecuteScalar(), typeof(T));
                dbCon.Close();
                return val;
            }
        }

        /// <summary>
        /// Execute a stored procedure without returning a value
        /// </summary>
        /// <param name="storedProc">Name of stored procedure to execute</param>
        /// <param name="parameters">Collection of parameters</param>
        /// <returns></returns>
        private bool WriteData(string storedProc, IEnumerable<SqlParameter> parameters)
        {
            using (var cmd = new SqlCommand(storedProc, this.dbCon))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                foreach (var prm in parameters)
                {
                    cmd.Parameters.Add(prm);
                }

                cmd.ExecuteNonQuery();
            }
            dbCon.Close();
            return true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_dbCon != null)
                {
                    _dbCon.Close();
                    _dbCon.Dispose();
                }
            }
        }
    }
}