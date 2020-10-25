using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Office.Interop.Word;
using System.IO;
using System.Drawing;
using Microsoft.Office;

namespace WordService
{
    public class Word : IDisposable
    {

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


        /// <summary>
        /// A blob and its filetype
        /// </summary>
        public struct BlobDoc
        {
            public byte[] Blob;
            public DocumentType DocType;
        }
        // Objects for COM interop
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

        /// <summary>
        /// Create and save the files of a form to the database
        /// </summary>
        /// <param name="frm">The form for which the file(s) should be created</param>
        /// <param name="dt">The document type(s) to create</param>
        /// <returns>true if docs were created; false if no word template exists</returns>
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

        /// <summary>
        /// Create a list of binary documents for the given form
        /// </summary>
        /// <param name="frm">The form from which to create the document</param>
        /// <param name="dt">The document type(s) to create</param>
        /// <returns>The created documents</returns>
        private List<BlobDoc> GetDocs(GenericForm frm, DocumentType dt)
        {
            object tfl = templateLoc + frm.WordTemplate;
            Microsoft.Office.Interop.Word.Document doc;
            try
            {
                // Attempt to add a document using the template
                doc = wordApp.Documents.Add(ref tfl);
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating word template from " + (string)tfl, ex);
            }
            // Cycle through the fields and add to the word document at the bookmarks specified
            foreach (var fld in frm.FormFields)
            {
                if (fld.WordBookmark != string.Empty)
                {
                    // Is the field a text field?  If so add the text...
                    if (fld.DataType == GenericForm.FieldDataType.Type_char || fld.DataType == GenericForm.FieldDataType.Type_lookup)
                    {
                        try
                        {
                            doc.Bookmarks[fld.WordBookmark].Range.Text = fld.Value_char;
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Error setting text for bookmark " + fld.WordBookmark + ". Check it exists", ex);
                        }
                    }
                    // Is the field type an image?  If so, we can only add images from a file so create a temporary file to add the image
                    // use GUID as file name for simplicity since it's a temporary file
                    if (fld.DataType == GenericForm.FieldDataType.Type_image)
                    {
                        try
                        {
                            string fln = (saveLoc + Guid.NewGuid().ToString() + Utility.GetFileExtension(fld.Value_char));
                            saveFile(fld.Value_binary, fln);
                            var pic = doc.Bookmarks[fld.WordBookmark].Range.InlineShapes.AddPicture(fln);

                            // If the field name contains the letters 'sig' it's likely to be a signature, hence scale the image down
                            if (fld.Name.ToLower().Contains("sig"))
                            {
                                pic.ScaleHeight = ((float)50.0 / pic.Height);
                                pic.ScaleWidth = ((float)100.0 / pic.Width);
                                pic.Width = 100;
                                pic.Height = 50;
                            }
                            File.Delete(fln);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Error adding image to bookmark " + fld.WordBookmark + ". Error could be that the bookmark doesn't exist, a problem saving the temporary file or a problem with the image.", ex);
                        }
                    }
                }
            }

            List<BlobDoc> docs = new List<BlobDoc>();

            // Create a PDF if required.  This needs to be saved as a temporary file first
            if ((dt & DocumentType.pdf) == DocumentType.pdf)
            {
                var g = Guid.NewGuid();
                object fln = (saveLoc + g.ToString() + ".pdf");
                try
                {
                    doc.SaveAs2(ref fln, ref pdfFormat);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error saving temporary word doc " + (string)fln, ex);
                }
                docs.Add(new BlobDoc() { Blob = getFile((string)fln), DocType = DocumentType.pdf });
                try
                {
                    File.Delete((string)fln);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error deleting temporary file " + (string)fln, ex);
                }
            }

            // Create a Word docx if required.  This needs to be saved as a temporary file first
            if ((dt & DocumentType.docx) == DocumentType.docx)
            {
                var g = Guid.NewGuid();
                object fln = (saveLoc + g.ToString() + ".docx");
                doc.SaveAs2(ref fln, ref docxFormat);
                doc.Close(ref zero);
                try
                {
                    docs.Add(new BlobDoc() { Blob = getFile((string)fln), DocType = DocumentType.docx });
                }
                catch (Exception ex)
                {
                    throw new Exception("Error saving temporary file " + (string)fln, ex);
                }
                try
                {
                    File.Delete((string)fln);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error deleting temporary file " + (string)fln, ex);
                }
            }
            else
            {
                doc.Close(ref zero);
            }

            return docs;
        }

        /// <summary>
        /// Load file into byte array
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Save file from byte array
        /// </summary>
        /// <param name="file"></param>
        /// <param name="filename"></param>
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