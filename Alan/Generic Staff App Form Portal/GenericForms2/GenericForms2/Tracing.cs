using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
using System.IO;

namespace GenericForms2
{
    /// <summary>
    /// Class to deal with logging errors
    /// </summary>
    public static class Tracing
    {
        /// <summary>
        /// Enum of type of events to trace
        /// </summary>
        public enum TracingEventType
        {
            AvailableForms,
            Word,
            ReviewForm,
            FormInstance,
            SaveForm,
            CompleteForm,
            Startup,
            Closing
        }

        private static TextWriterTraceListener _listener;
        private static TextWriterTraceListener listener
        {
            get
            {
                if (_listener == null)
                {
                    _listener = new TextWriterTraceListener(Properties.Settings.Default.LogFile);
                   // _listener.TraceOutputOptions = TraceOptions.Callstack;
                }
                return _listener;
            }
            set
            {
                _listener = value;
            }
        }
        private static TraceSource _Source;

        public static TraceSource Source
        {
            get
            {
                if (_Source == null)
                {
                    Trace.AutoFlush = true;
                    _Source = new TraceSource("GenericForms", SourceLevels.Information);
                    _Source.Listeners.Add(listener);
                }
                return _Source;
            }
        }
        /// <summary>
        /// Write an error to the log
        /// </summary>
        /// <param name="ex">The exception to log</param>
        /// <param name="evt">Event type of this error</param>
        public static void HandleError(System.Exception ex, TracingEventType evt)
        {
            string msg = ex.Message;
            Exception e = ex;
            while (e.InnerException != null)
            {
                e = e.InnerException;
                msg += "[Inner exception: " + e.Message + "]";
            }
            msg += "\nStack trace: " + ex.StackTrace;
            Source.TraceEvent(TraceEventType.Error, (int)evt, Utility.FormatErrMsg(msg));
        }
        /// <summary>
        /// Write a message to the error log.  The date and time will be added to the message
        /// </summary>
        /// <param name="message">The message to write</param>
        public static void WriteLine(string message)
        {
            listener.WriteLine(Utility.FormatErrMsg(message));
            listener.Flush();
        }
    }
}