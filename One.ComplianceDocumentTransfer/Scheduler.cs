using System;
using System.Configuration;
using System.IO;
using System.ServiceProcess;
using NLog;
using System.Collections.Generic;
using System.Timers;
using System.Linq;

namespace OneComplianceDocumentTransfer
{
    public partial class Scheduler : ServiceBase
    {
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private Dictionary<string, string> _sourceDestinationPairs;
        private static Timer _timer;
        public Scheduler()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            StartService();
        }

        public void StartService()
        {
            try
            {
                _logger.Info("FILEWATCHER SERVICE STARTED");

                _timer = new Timer(1000);

                // Get the Interval in Minutes
                _timer.Interval = 1000 * 60 * Convert.ToInt32(ConfigurationManager.AppSettings["IntervalMinutes"]);

                // Hook up the Elapsed event for the timer.
                _timer.Elapsed += new ElapsedEventHandler(ProcessFilesInDirectories);
                _timer.Start();
                _sourceDestinationPairs = new Dictionary<string, string>();
            
                var countOfSourceDestinationPairs = int.Parse(ConfigurationManager.AppSettings["CountOfSourceDestinationPairs"].ToString());
                for (var i = 1; i <= countOfSourceDestinationPairs; i++)
                {
                    _logger.Info(string.Format("Started processing number {0} source destination pair", i));
                    try
                    {
                        var sourceFolderToProcess = ConfigurationManager.AppSettings[string.Format("SourceFolder{0}", i)];
                        var destinationFolderToProcess = ConfigurationManager.AppSettings[string.Format("DestinationFolder{0}", i)];
                        _sourceDestinationPairs.Add(sourceFolderToProcess, destinationFolderToProcess);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(ex, "Error in reading source/destination folder");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in in starting service");
            }
        }

        public void StopService()
        {
            _logger.Info("FILEWATCHER SERVICE STOPPED");
            try
            {
                if(_timer!=null)
                {
                    _timer.Close();
                    _timer.Dispose();
                }
            }
            catch(Exception ex)
            {
                _logger.Error(ex, "Error stopping service");
            }
        }

        protected override void OnStop()
        {
            StopService();
        }

        // Process files om each of the source directories when the Elapsed event is 
        // raised.
        private void ProcessFilesInDirectories(object source, ElapsedEventArgs e)
        {
            _timer.Stop();
            foreach (var sourceDestinationPair in _sourceDestinationPairs)
            {
                try
                {
                    TransferAndRenameDocuments(sourceDestinationPair.Key, sourceDestinationPair.Value);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, $"Error in transfering and renaming documents from source directory {sourceDestinationPair.Key}");
                }
            }
            _timer.Start();
        }

        private void TransferAndRenameDocuments(string sourceFilepath, string destinationFilePaths)
        {
            var files = Directory.GetFiles(sourceFilepath);
            var archiveDirectory = ConfigurationManager.AppSettings["ArchiveDirectory"];

            foreach (var file in files)
            {
                try
                {
                    var currentFile = new FileInfo(file);

                    foreach(var destinationFilePath in destinationFilePaths?.Split(','))
                    {
                        try
                        {
                            var destinationFile = Path.Combine(destinationFilePath, currentFile.Name.Replace("_", "-"));
                            File.Copy(file, destinationFile, true);
                            _logger.Info($"Copied the file {currentFile.FullName} and moved to {destinationFile}");
                        }
                        catch(Exception ex)
                        {
                            _logger.Error(ex, $"Error copying file {file} to destination {destinationFilePath}");
                        }
                    }

                    var archivedDirectoryPath = Path.Combine(sourceFilepath, archiveDirectory);

                    if (!Directory.Exists(archivedDirectoryPath))
                    {
                        Directory.CreateDirectory(archivedDirectoryPath);
                    }

                    var achiveFileName = $"{DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss")}--{currentFile.Name}";
                    var archiveFilePath = Path.Combine(archivedDirectoryPath, achiveFileName);

                    File.Move(file, archiveFilePath);
                    _logger.Info($"Renamed and moved file to {archiveFilePath} for archiving");
                }
                catch(Exception ex)
                {
                    _logger.Error(ex, $"Error processing file {file} with source {sourceFilepath} and destination(s) {destinationFilePaths}");
                }
            }
        }
    }
}
