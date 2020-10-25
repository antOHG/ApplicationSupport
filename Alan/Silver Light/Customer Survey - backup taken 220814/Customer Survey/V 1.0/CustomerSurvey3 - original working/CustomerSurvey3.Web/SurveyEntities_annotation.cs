using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.ComponentModel.DataAnnotations;

namespace CustomerSurvey3.Web
{
    [MetadataType(typeof(FindUHTenant_ResultMetadata))]
    public partial class FindUHTenant_Result : ComplexObject
    {
        internal sealed class FindUHTenant_ResultMetadata
        {
            public FindUHTenant_ResultMetadata() { }

            [Key]
            public string UHTenancyRef { get; set; }
            [Key]
            public int PersNo { get; set; }

        }
    }
}


