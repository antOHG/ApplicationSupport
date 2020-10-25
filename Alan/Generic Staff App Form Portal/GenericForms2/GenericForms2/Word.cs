using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Office.Interop.Word;
using System.IO;
using System.Drawing;
using Microsoft.Office;

namespace GenericForms2
{
    public class Word : IDisposable
    {
        //private string templateLoc = @"C:\Users\Mark\";
        //private string saveLoc = @"C:\Users\Mark\";

        private string templateLoc = Properties.Settings.Default.TemplateLocation;
        private string saveLoc = Properties.Settings.Default.SaveLocation;

        private static Word _Instance;

        public static Word Instance
        {
            get
            {
                if (_Instance != null)
                {
                    return _Instance;
                }
                else
                {
                    _Instance = new Word();
                    return _Instance;
                }
            }
        }

        public Word()
        {
  
        }

        public struct BlobDoc
        {
            public byte[] Blob;
            public DocumentType DocType;
        }

        private static object zero = 0;
        private static object pdfFormat = WdSaveFormat.wdFormatPDF;
        private static object docxFormat = WdSaveFormat.wdFormatXMLDocument;
        private static object donotsave = WdSaveOptions.wdDoNotSaveChanges;

        [Flags]
        public enum DocumentType
        {
            docx = 1,
            pdf = 2
        }

        private Application _wordApp;

        private Application wordApp
        {
            get 
            {
                if (_wordApp != null && _wordApp.Application != null)
                {
                    return _wordApp;
                }
                else
                {
                    _wordApp = new Application() { Visible = false };
                    return _wordApp;
                }
            }
        }

        public bool CreateDocument(GenericForm frm, DocumentType dt)
        {
            if (frm.WordTemplate != string.Empty)
            {
                foreach (var blobDoc in GetDocs(frm, dt))
                {
                    DataAccess.Instance.SaveFile(frm.FormInstanceId, blobDoc.DocType.ToString(), blobDoc.Blob);
                }
                return true;
            }
            return false;
        }

        private List<BlobDoc> GetDocs(GenericForm frm, DocumentType dt)
        {
            object tfl = templateLoc + frm.WordTemplate;
            var doc = wordApp.Documents.Add(ref tfl);

            foreach (var fld in frm.FormFields)
            {
                if (fld.WordBookmark != string.Empty)
                {
                    if (fld.DataType == GenericForm.FieldDataType.Type_char || fld.DataType == GenericForm.FieldDataType.Type_lookup)
                    {
                        doc.Bookmarks[fld.WordBookmark].Range.Text = fld.Value_char;
                    }
                    if (fld.DataType == GenericForm.FieldDataType.Type_image)
                    {
                        try
                        {
                            string fln = (saveLoc + Guid.NewGuid().ToString() + Utility.GetFileExtension(fld.Value_char));
                            saveFile(fld.Value_binary, fln);
                            var pic = doc.Bookmarks[fld.WordBookmark].Range.InlineShapes.AddPicture(fln);
                            if (fld.Name.ToLower().Contains("sig"))
                            {
                                pic.ScaleHeight = ((float)50.0 / pic.Height);
                                pic.ScaleWidth = ((float)100.0 / pic.Width);
                                pic.Width = 100;
                                pic.Height = 50;
                            }
                            File.Delete(fln);
                        }
                        catch
                        {
                            //frm.Warnings.Add(string.Format("Error adding image for field {0} to bookmark {1}", fld.Name, fld.WordBookmark));
                        }
                    }
                }
            }

            List<BlobDoc> docs = new List<BlobDoc>();

            if ((dt & DocumentType.pdf) == DocumentType.pdf)
            {
                var g = Guid.NewGuid();
                object fln = (saveLoc + g.ToString() + ".pdf");
                doc.SaveAs2(ref fln, ref pdfFormat);
                docs.Add(new BlobDoc() { Blob = getFile((string)fln), DocType = DocumentType.pdf });
                //File.Delete((string)fln);
            }

            if ((dt & DocumentType.docx) == DocumentType.docx)
            {
                var g = Guid.NewGuid();
                object fln = (saveLoc + g.ToString() + ".docx");
                doc.SaveAs2(ref fln, ref docxFormat);
                doc.Close(ref zero);
                docs.Add(new BlobDoc() { Blob = getFile((string)fln), DocType = DocumentType.docx });
                //File.Delete((string)fln);
            }
            else
            {
                doc.Close(ref zero);
            }



            return docs;
        }

        private byte[] getFile(string fileName)
        {
            byte[] file;
            using (var stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                using (var reader = new BinaryReader(stream))
                {
                    file = reader.ReadBytes((int)stream.Length);
                }
            }
            return file;
        }

        private void saveFile(byte[] file, string filename)
        {
            using (var stream = new FileStream(filename, FileMode.CreateNew, FileAccess.Write))
            {
                stream.Write(file, 0, (int)file.Length);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_wordApp != null)
                {
                    _wordApp.Quit(donotsave);
                    _wordApp = null;
                }
            } 
        }
    }
}