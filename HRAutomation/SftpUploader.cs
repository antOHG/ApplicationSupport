using System;
using System.Configuration;
using WinSCP;

namespace HRAutomation
{
    class SftpUploader
    {
        public static void UploadToSftp(string filename)
        {
            try
            {
                var so = new SessionOptions
                {
                    Protocol = Protocol.Sftp,
                    HostName = ConfigurationManager.AppSettings["HostName"],
                    UserName = ConfigurationManager.AppSettings["UserName"],
                    Password = ConfigurationManager.AppSettings["Password"],
                    SshPrivateKeyPath = "",
                    GiveUpSecurityAndAcceptAnySshHostKey = true,
                    PortNumber = int.Parse(ConfigurationManager.AppSettings["Port"])
                };

                using (var sess = new Session())
                {
                    sess.SessionLogPath = "your log path";
                    sess.Open(so);
                    var to = new TransferOptions();
                    to.TransferMode = TransferMode.Binary;
                    to.FilePermissions = null;
                    to.PreserveTimestamp = false;
                    to.ResumeSupport.State = TransferResumeSupportState.Off;
                    TransferOperationResult tor;
                    tor = sess.PutFiles(filename, ConfigurationManager.AppSettings["RemotePath"], false, to);
                    tor.Check();
                    Console.WriteLine("Successfully uploaded");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error message: " + ex.Message);
            }
        }
    }
}
