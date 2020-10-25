using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace WordService
{
    public static class DataFactory
    {
        public static AvailableForm AvailableFormFactory(IDataRecord rec)
        {
            AvailableForm a = new AvailableForm()
            {
                CompletedFormId = Convert.ToInt16(rec[0]),
                TSCreated = (rec[1] == DBNull.Value) ? null : (DateTime?)Convert.ToDateTime(rec[1]),
                UserCreated = rec[2] == DBNull.Value ? null : (string)rec[2],
                FormName = rec[3] == DBNull.Value ? null : (string)rec[3],
                LockedTS = (rec[4] == DBNull.Value) ? null : (DateTime?)Convert.ToDateTime(rec[4]),
                LockedUser = rec[5] == DBNull.Value ? null : (string)rec[5]
            };

            return a;
        }

        public static GenericForm FormFactory(IDataRecord rec, List<GenericForm.FormField> fields)
        {
            throw new System.NotImplementedException();
        }
        
        public static GenericForm.FormField FormFieldFactory(IDataRecord rec)
        {
            var dt = new GenericForm.FieldDataType();
            string dtString = "Type_" + (string)rec[6];

            foreach (GenericForm.FieldDataType val in Enum.GetValues(typeof(GenericForm.FieldDataType)))
            {
                if (val.ToString() == dtString)
                {
                    dt = val;
                    break;
                }
            }

            int formfieldinstaneid = Convert.ToInt32(rec[0]);
            string formfieldname = (string)rec[1];
            string formfielddesc = (string)rec[2];
            bool displayonwebform = (bool)rec[3];
            bool canbemodifiedonwebform = (bool)rec[4];
            string wordbookmark = (string)rec[5];
            //string fieldtypename = (string)rec[6];
            int fieldminlen = Convert.ToInt32(rec[7]);
            int fieldmaxlen = Convert.ToInt32(rec[8]);
            bool isrequired = (bool)rec[9];
            string val_char = Convert.ToString(rec[10]);
            byte[] val_binary = (rec[11] is DBNull) ? null : (byte[])rec[11];
            string x = Convert.ToString(DBNull.Value);

            return new GenericForm.FormField(formfieldinstaneid, formfieldname, formfielddesc, displayonwebform, canbemodifiedonwebform, wordbookmark, dt, fieldminlen, fieldmaxlen, 
                isrequired, val_char, val_binary);

            //return new GenericForm.FormField(formfieldid, (string)rec[0], (string)rec[1], (decimal)rec[2], (bool)rec[3], (bool)rec[4], (string)rec[5], (string)rec[6],
            //    dt, (string)rec[8], (int)rec[9], (int)rec[10], (int)rec[11], (int)rec[12], Convert.ToSingle(rec[13]), Convert.ToSingle(rec[14]), Convert.ToDateTime(rec[15]), 
            //    Convert.ToDateTime(rec[16]), (string)rec[17], Convert.ToDateTime(rec[18]), (int)rec[19], Convert.ToSingle(rec[20]));
        }
    }
}