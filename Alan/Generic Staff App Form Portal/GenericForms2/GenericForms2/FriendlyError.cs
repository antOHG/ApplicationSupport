using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace GenericForms2
{
    public class FriendlyError

    {
        private List<string> messages;

        private string _firstLine;

        private bool _ErrorsExist;

        /// <summary>
        /// Have any errors been added to the collection?
        /// </summary>
        public bool ErrorsExist { get { return _ErrorsExist; } }

        /// <summary>
        /// The error message to report to the user
        /// </summary>
        public string ErrorMessage
        {
            get
            {
                if (!_ErrorsExist) { return string.Empty; }

                StringBuilder sb = new StringBuilder();
                sb.AppendLine(_firstLine);
                foreach (string m in messages)
                {
                    sb.AppendLine(m);
                }
                _ErrorsExist = false;
                messages.Clear();
                return sb.ToString();
            }
        }

        public FriendlyError()
            : this("The following error(s) occurred:")
        {

        }

        public FriendlyError(string firstLine)
        {
            messages = new List<string>();
            _ErrorsExist = false;
            _firstLine = firstLine;
        }

        public void Add(string message)
        {
            messages.Add(message);
            _ErrorsExist = true;
        }

        /// <summary>
        /// Generate an HTML form to post to an error page
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <param name="postbackUrl"></param>
        /// <returns></returns>
        public static string ErrorPost(string errorMessage, string postbackUrl)
        {

            StringBuilder sb = new StringBuilder();
            sb.Append("<html>");
            sb.AppendFormat(@"<body onload='document.forms[""form""].submit()'>");
            sb.AppendFormat("<form name='form' action='{0}' method='post'>", postbackUrl);
            sb.AppendFormat("<input type='hidden' name='ErrMsg' value='{0}'>", errorMessage);
            // Other params go here
            sb.Append("</form>");
            sb.Append("</body>");
            sb.Append("</html>");

            return sb.ToString();
        }
    }
}