using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xfa_Xml_Generator
{
    public partial class Xfa : IDisposable
    {
        private StringBuilder javascriptBuilder = new StringBuilder();
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

            Javascript = new XfaJavascript(ref javascriptBuilder);
        }

        public void Dispose()
        {
            Fields.Clear();

            javascriptBuilder.Clear();

            GC.SuppressFinalize(this);
        }

        public XfaModel this[string fieldName] => Fields?.Find(model => model.FieldName.Equals(fieldName));

        public void AddField(string fieldName, string value) => Fields.Add(new XfaModel(fieldName: fieldName, value: value));

        public void AddField(string fieldName, string fieldType, string value) => Fields.Add(new XfaModel(fieldName, fieldType, value));

        public void AddJavascript(string javascriptContent) => javascriptBuilder?.AppendLine(javascriptContent);

        public IEnumerable<XfaModel> GetAllFields(bool includeButtons = true) => (
                from prop in PdfStamper.AcroFields.Xfa.TemplateSom.Order
                let propType = PdfStamper.AcroFields.Xfa.TemplateSom.GetFieldType(prop)
                where includeButtons || !propType.Equals("button", StringComparison.InvariantCultureIgnoreCase)
                select new XfaModel(prop, propType)
            );

        public IEnumerable<XfaModel> GetFields() => GetAllFields(includeButtons: false);

        public string GetXML() => XfaXml.GetXML(Fields);

        public void SaveChanges()
        {
            var xmlDocument = XfaXml.GenerateXML(Fields);

            if (javascriptBuilder.Length > 0)
            {
                var jsPdf = PdfAction.JavaScript(javascriptBuilder.ToString(), PdfStamper.Writer, true);

                PdfStamper.Writer.AddJavaScript("XfaCustomJavascriptCall", jsPdf);
            }

            PdfStamper.AcroFields.Xfa.FillXfaForm(xmlDocument);
        }

        public void SetValue(string fieldName, string value) => this[fieldName]?.SetValue(value);
    }
}
