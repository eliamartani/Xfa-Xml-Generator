using iTextSharp.text.pdf;
using System.Collections.Generic;

namespace Xfa_Xml_Generator
{
    public static class Core
    {
        public static List<string> GetAllXfaFields(this PdfStamper pdfStamper)
        {
            return pdfStamper.AcroFields.Xfa.TemplateSom.Order;
        }
    }
}
