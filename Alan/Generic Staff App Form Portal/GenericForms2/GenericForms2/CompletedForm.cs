
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GenericForms2
{
    public class CompletedForm
    {
        public int FormInstanceId { get; set; }

        public DateTime? TSCreated { get; set; }

        public string UserCreated { get; set; }

        public string FormName { get; set; }

        public bool PdfAvailble { get; set; }

        public string ReferenceGroup { get; set; }

        public string RecipientReference { get; set; }

        public string RecipientAddress { get; set; }
    }
}