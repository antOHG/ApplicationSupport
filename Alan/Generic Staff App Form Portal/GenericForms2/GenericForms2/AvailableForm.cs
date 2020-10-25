using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GenericForms2
{
    public class AvailableForm
    {
        public int CompletedFormId { get; set; }

        public DateTime? TSCreated { get; set; }

        public string UserCreated { get; set; }

        public string FormName { get; set; }

        public DateTime? LockedTS { get; set; }

        public string LockedUser { get; set; }

        public string ReferenceGroup { get; set; }

        public string RecipientReference { get; set; }

        public string RecipientAddress { get; set; }

    }
}