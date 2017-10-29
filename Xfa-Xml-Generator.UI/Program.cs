using iTextSharp.text.pdf;
using System.IO;

namespace Xfa_Xml_Generator.UI
{
    class Program
    {
        static void Main(string[] args)
        {
            ProcessPdf(@"C:\Users\Eliamar\Desktop\BRADESCO\Files\4008-174E.pdf");
        }

        static void ProcessPdf(string filename)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            using (PdfReader pdfReader = new PdfReader(filename))
            using (PdfStamper pdfStamper = new PdfStamper(pdfReader, memoryStream))
            using (Xfa xfa = new Xfa(pdfStamper))
            {
                foreach (XfaModel field in xfa.GetFields())
                {
                    field.SetValue("1");
                }
                
                xfa.AddJavascript("var numericValue = 1;");

                xfa.Javascript.SetVisibility(xfa.Fields[0].FieldName, XfaVisibility.Presence);

                xfa.Javascript.SetValue(xfa.Fields[0].FieldName, "Testing Javascript RawValue");

                xfa.SaveChanges();

                pdfStamper.Close();
            }
        }
    }
}
