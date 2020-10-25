using System;
using System.IO;

namespace DocTransfer
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
             * Parameters:  "SOURCE TYPE" "SOURCE LOCATION" "DESTINATION LOCATION" "DELETE SOURCE"
             * Example # 1: "f" "src" "dest" "false"
             * Example # 2: "p" "src" "dest" "true"
             */

            try
            {
                var type = args[0];
                var fileName = args[1];
                var remotePath = args[2];
                var isDelete = args[3];

                var sftp = new SFTPService();
                var status = false;
                if (type.ToLower() == "f")
                {
                    status = sftp.UploadFiles(fileName, remotePath);
                    if (bool.Parse(isDelete) && status)
                    {
                        File.Delete(fileName);
                    }
                }
                else if (type.ToLower() == "p")
                {
                    var fileEntries = Directory.GetFiles(fileName);
                    foreach (string file in fileEntries)
                    {
                        status = sftp.UploadFiles(file, remotePath);
                        if (bool.Parse(isDelete) && status)
                        {
                            File.Delete(file);
                        }
                        status = false;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogFile("General error:" + ex.Message);
            }
        }
    }
}
