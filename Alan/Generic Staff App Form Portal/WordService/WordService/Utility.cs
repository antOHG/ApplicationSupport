using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace WordService
{
    public static class Utility
    {
        /// <summary>
        /// Extract file extension from a filename
        /// </summary>
        /// <param name="filename">The file name</param>
        /// <param name="includeDot">Include the dot in the extension</param>
        /// <returns>THE EXTENSION</returns>
        public static string GetFileExtension(string filename, bool includeDot = true)
        {

            int pos = filename.LastIndexOf('.');
            if (pos != -1)
            {
                return filename.Substring(pos + (includeDot ? 0 : 1));
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Prepend date and time so as to format an error message
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static string FormatErrMsg(string msg)
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " " + msg;
        }
    }
}