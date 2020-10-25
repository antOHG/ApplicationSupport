using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace WordService
{
    public partial class WordService : ServiceBase
    {
        // if thread is running.  Volatile so that the compiler knows not to optimise in a manner that is not threadsafe
        private volatile bool isExecuting;

        private Timer timer;
        public WordService()
        {
            InitializeComponent();
        }

        public void Start()
        {
            // code that runs when service starts
            Tracing.WriteLine("Starting service");
            timer = new Timer(Properties.Settings.Default.PollIntervalSeconds * 1000);
            timer.Elapsed += this.timer_Elapsed;
            timer.Start();
        }

        public void DoStop()
        {
            // code runs when service stops
            timer.Stop();
            timer.Dispose();
            Tracing.WriteLine("Ending service");
            Tracing.Source.Close();
            Word.Instance.Dispose();
            DataAccess.Instance.Dispose();
 
        }

        protected override void OnStart(string[] args)
        {
            this.Start();
        }

        protected override void OnStop()
        {
            this.DoStop();
        }

        private void timer_Elapsed(object sender, EventArgs e)
        {
            // iseExecuting flag a safety measure; should never be true at this point
            if(isExecuting)
            {
                return;
            }
            timer.Stop();
            isExecuting = true;

            try
            {
                int completedFormId = DataAccess.Instance.GetNextDocToProcess();
                GenericForm frm;
                while(completedFormId != -1) // -1 means no doc to process
                {
                    try
                    {
                        Tracing.WriteLine("Processing CompletedFormId=" + completedFormId.ToString());

                        //get form instance from the database
                        frm = new GenericForm(completedFormId);
                        try
                        {
                            if (!Word.Instance.CreateDocument(frm, Word.DocumentType.docx | Word.DocumentType.pdf))
                            {
                                Tracing.WriteLine("No word template listed; unable to create document.");
                            }
                        }
                        catch
                        {
                            DataAccess.Instance.MarkDocCreationError(frm.FormInstanceId);
                            throw;
                        }
                    }
                    catch (Exception ex)
                    {
                        Tracing.HandleError(ex, Tracing.TracingEventType.Word);
                    }
                    finally
                    {
                        frm = null;
                        completedFormId = DataAccess.Instance.GetNextDocToProcess();
                    }
                }
            }
            catch(Exception ex)
            {
                Tracing.HandleError(ex, Tracing.TracingEventType.Word);
            }
            finally
            {
                isExecuting = false;
                timer.Start();
            }
        }
    }
}
