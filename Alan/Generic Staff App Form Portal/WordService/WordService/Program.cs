using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace WordService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            var ServiceToRun = new WordService();
            if (Environment.UserInteractive)
            {
                // This used to run the service as a console app (development phase only)

                ServiceToRun.Start();

                Console.WriteLine("Press Enter to terminate ...");
                Console.ReadLine();

                ServiceToRun.DoStop();
            }
            else
            {
                ServiceBase.Run(ServiceToRun);
            }

            //ServiceBase[] ServicesToRun;
            //ServicesToRun = new ServiceBase[]
            //{
            //    new WordService()
            //};
            //ServiceBase.Run(ServicesToRun);
        }
    }
}
