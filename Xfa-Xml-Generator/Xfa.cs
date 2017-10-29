using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xfa_Xml_Generator
{
    public partial class Xfa : IDisposable
    {
        private StringBuilder JavascriptBuilder = new StringBuilder();

        public XfaJavascript Javascript { get; private set; }
        public PdfStamper PdfStamper { get; private set; }
        public List<XfaModel> Fields { get; } = new List<XfaModel>();

        public Xfa(PdfStamper pdfStamper)
        {
            if (!pdfStamper.AcroFields.Xfa.XfaPresent)
            {
                throw new Exception("There is no Xfa in this document. This component is not necessary.");
            }

            PdfStamper = pdfStamper;

            Javascript = new XfaJavascript(ref JavascriptBuilder);
        }

        public void Dispose()
        {
            Fields.Clear();

            JavascriptBuilder.Clear();
        }


        public XfaModel this[string fieldName]
        {
            get
            {
                return Fields?.Find(model => model.FieldName.Equals(fieldName));
            }
        }


        public void AddJavascript(string javascriptContent)
        {
            JavascriptBuilder?.AppendLine(javascriptContent);
        }

        public void SetValue(string fieldName, string value)
        {
            this[fieldName]?.SetValue(value);
        }

        public List<XfaModel> GetFields()
        {
            return (
                from prop in PdfStamper.AcroFields.Xfa.TemplateSom.Order
                let propType = PdfStamper.AcroFields.Xfa.TemplateSom.GetFieldType(prop)
                where !propType.Equals("button")
                select new XfaModel(prop, propType)
            ).ToList();
        }

        public List<XfaModel> GetAllFields()
        {
            return (
                from prop in PdfStamper.AcroFields.Xfa.TemplateSom.Order
                let propType = PdfStamper.AcroFields.Xfa.TemplateSom.GetFieldType(prop)
                select new XfaModel(prop, propType)
            ).ToList();
        }

        public void SaveChanges()
        {
            var xml = new XfaXml();
            var xmlDocument = xml.GenerateXML(Fields);

            if (JavascriptBuilder.Length > 0)
            {
                PdfAction jsPdf = PdfAction.JavaScript(JavascriptBuilder.ToString(), PdfStamper.Writer, true);

                PdfStamper.Writer.AddJavaScript("XfaCustomJavascriptCall", jsPdf);
            }
            
            PdfStamper.AcroFields.Xfa.FillXfaForm(xmlDocument);
        }
    }
}
