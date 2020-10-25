namespace HRAutomation
{
    class Program
    {
        static void Main(string[] args)
        {
            //SftpUploader.UploadToSftp(@"C:\Projects_Temp\Basheer\HRAutomation\var_pay_20171204_152405.csv");

            AgencyDb.ReadAndSaveAccessData();

            //var adList = Utilities.GetActiveDirectoryData();
            
            //Utilities.SaveToDb(adList);
        }
    }
}
