using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Office.Interop.Word;


namespace WordService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    /// <summary>
    /// Represents a form instance
    /// </summary>
    public class GenericForm
    {
        public enum FieldDataType
        {
            Type_char,
            Type_image,
            Type_lookup,
            Type_divider,
            Type_label
        }


        /// <summary>
        /// Dictionary holding the list of fields that have changed.  Holding this list reduces data access.
        /// </summary>
        public Dictionary<int, string> ChangedFields;

        #region Properties

        private int _FormInstanceId;

        public int FormInstanceId
        {
            get { return _FormInstanceId; }
            private set { _FormInstanceId = value; }
        }

        private int _CompletedFormId;

        private string _WordTemplate;

        public string WordTemplate
        {
            get { return _WordTemplate; }
            private set { _WordTemplate = value; }
        }

        private List<FormField> _FormFields;

        public List<FormField> FormFields
        {
            get { return _FormFields; }
            private set { _FormFields = value; }
        }

        #endregion Properties

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="CompletedFormId">The completed form ID (from the Documotive database)</param>
        public GenericForm(int CompletedFormId)
        {
            var frmrecs = DataAccess.Instance.GetFormInstance(CompletedFormId);
            foreach (var frmrec in frmrecs)
            {
                this.FormInstanceId = Convert.ToInt32(frmrec[0]);
                this.WordTemplate = Convert.ToString(frmrec[1]);
            }

            var recs = DataAccess.Instance.GetFormFieldInstances(this.FormInstanceId);
            List<FormField> flds = new List<FormField>();

            foreach (var rec in recs)
            {
                flds.Add(DataFactory.FormFieldFactory(rec));
            }

            this.FormFields = flds;
            //this.Warnings = new List<string>();
            ChangedFields = new Dictionary<int, string>();
        }

        /// <summary>
        /// Add form to the queue for document creation
        /// </summary>
        /// <returns>true if this has been done, false if there is no word template for the form</returns>
        public bool MarkReadyToBlob()
        {
            if (this.WordTemplate != string.Empty)
            {
                return DataAccess.Instance.MarkReadyToBlob(this.FormInstanceId);
            }
            return false;
        }

        /// <summary>
        /// Mark the form as complete.  If the form is locked by a different user and the lock has not expired, it is not marked
        /// </summary>
        /// <param name="user">User's name</param>
        /// <param name="lockTimeoutMinutes">lock timeout</param>
        /// <returns>Empty string if successful, error message if not</returns>
        public string CompleteForm(string user, int lockTimeoutMinutes)
        {
            return DataAccess.Instance.CompleteFormInstance(this.FormInstanceId, user, lockTimeoutMinutes);
        }

        /// <summary>
        /// Save changes to the form
        /// </summary>
        /// <returns></returns>
        public bool SaveChanges()
        {
            foreach (FormField fld in this.FormFields)
            {
                string changedVal;

                if (ChangedFields.TryGetValue(fld.FormFieldInstanceId, out changedVal))
                {
                    fld.Value_char = changedVal;
                    fld.UpdateValueInDb();
                }
            }
            return true;
        }
        /// <summary>
        /// Class representing a form field instance: a question and answer
        /// </summary>
        public class FormField
        {

            #region Properties

            private int _FormFieldInstanceId;

            public int FormFieldInstanceId
            {
                get { return _FormFieldInstanceId; }
                private set { _FormFieldInstanceId = value; }
            }

            private string _Name;

            public string Name
            {
                get { return _Name; }
                private set { _Name = value; }
            }

            private string _Description;

            public string Description
            {
                get { return _Description; }
                private set { _Description = value; }
            }

            //private decimal _DisplayOrder;

            //public decimal DisplayOrder
            //{
            //    get { return _DisplayOrder; }
            //    private set { _DisplayOrder = value; }
            //}

            private bool _DisplayOnWebForm;

            public bool DisplayOnWebForm
            {
                get { return _DisplayOnWebForm; }
                private set { _DisplayOnWebForm = value; }
            }

            private bool _CanBeModifiedOnWebForm;

            public bool CanBeModifiedOnWebForm
            {
                get { return _CanBeModifiedOnWebForm; }
                private set { _CanBeModifiedOnWebForm = value; }
            }

            private string _WordBookmark;

            public string WordBookmark
            {
                get { return _WordBookmark; }
                private set { _WordBookmark = value; }
            }

            private string _FieldTypeName;

            private FieldDataType _DataType;

            public FieldDataType DataType
            {
                get { return _DataType; }
                private set { _DataType = value; }
            }

            //private string _DataTypeFormat;

            //public string DataTypeFormat
            //{
            //    get { return _DataTypeFormat; }
            //    private set { _DataTypeFormat = value; }
            //}

            private int _MinLen;

            public int MinLen
            {
                get { return _MinLen; }
                private set { _MinLen = value; }
            }

            private int _MaxLen;

            public int MaxLen
            {
                get { return _MaxLen; }
                private set { _MaxLen = value; }
            }

            private bool _IsRequired;

            public bool IsRequired
            {
                get { return _IsRequired; }
                private set { _IsRequired = value; }
            }

            private string _Value_char;

            public string Value_char
            {
                get { return _Value_char; }
                set
                {
                    if (this.DataType == FieldDataType.Type_char || this.DataType == FieldDataType.Type_lookup)
                    {
                        if (value.Length >= this.MinLen && value.Length <= this.MaxLen)
                        {
                            _Value_char = value;
                        }
                        else
                        {
                            throw new SystemException("String length outside permitted range");
                        }
                    }
                    else
                    {
                        throw new SystemException("Cannot set char property for datatype " + this.DataType.ToString());
                    }

                }
            }

            private byte[] _Value_binary;

            public byte[] Value_binary
            {
                get { return _Value_binary; }
                set
                {
                    if (this.DataType == FieldDataType.Type_image)
                    {
                        _Value_binary = value;
                    }
                    else
                    {
                        throw new SystemException("Cannot set binary property for datatype " + this.DataType.ToString());
                    }

                }
            }

            #endregion Properties

            #region Constructor


            public FormField(int formFieldInstanceId, string name, string description, bool displayOnWebForm, bool canBeModifiedOnWebForm, string wordBookmark,
                    FieldDataType dataType, int minLen, int maxLen, bool isRequired, string value_char, byte[] value_binary)
            {
                FormFieldInstanceId = formFieldInstanceId;
                Name = name;
                Description = description;
                //DisplayOrder = displayOrder;
                DisplayOnWebForm = displayOnWebForm;
                CanBeModifiedOnWebForm = canBeModifiedOnWebForm;
                WordBookmark = wordBookmark;
                DataType = dataType;
                MinLen = minLen;
                MaxLen = maxLen;
                IsRequired = isRequired;
                _Value_char = value_char;
                _Value_binary = value_binary;
            }
            #endregion Constructor

            /// <summary>
            /// Update the value of the field in the database
            /// </summary>
            /// <returns></returns>
            public bool UpdateValueInDb()
            {
                return DataAccess.Instance.UpdateFormFieldInstance(this);
            }
        }

    }
}