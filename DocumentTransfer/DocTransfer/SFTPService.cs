using Renci.SshNet;
using System;
using System.Configuration;
using System.IO;

namespace DocTransfer
{
    class SFTPService
    {
        public bool UploadFiles(string fileName, string remoteDirectory)
        {
            bool status = false;
            string host = ConfigurationManager.AppSettings["Host"];
            string username = ConfigurationManager.AppSettings["UserName"];
            string password = ConfigurationManager.AppSettings["Password"];
            var fi = new FileInfo(fileName);
            using (SftpClient sftp = new SftpClient(host, username, password))
            {
                try
                {
                    sftp.Connect();
                    using (var uplfileStream = File.OpenRead(fileName))
                    {
                        sftp.ChangeDirectory(remoteDirectory);
                        sftp.UploadFile(uplfileStream, fi.Name, true);
                    }
                    sftp.Disconnect();
                    status = true;
                    LogHelper.LogFile("Successfully Uploaded - #Src: " + fileName + ", #Dest: " + remoteDirectory);
                }
                catch (Exception ex)
                {
                    LogHelper.LogFile("Error while uploading: " + ex.Message);
                }
            }
            return status;
        }
    }
}
