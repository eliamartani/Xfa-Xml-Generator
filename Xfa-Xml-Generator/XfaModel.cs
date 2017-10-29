namespace Xfa_Xml_Generator
{
    public class XfaModel
    {
        public string FieldName { get; private set; }
        public string FieldType { get; private set; }
        public string Value { get; private set; }
        
        public XfaModel(string fieldName = null, string fieldType = null, string value = null)
        {
            FieldName = fieldName;
            FieldType = fieldType;
            Value = value;
        }

        public void SetValue(string value)
        {
            Value = value;
        }
    }
}
