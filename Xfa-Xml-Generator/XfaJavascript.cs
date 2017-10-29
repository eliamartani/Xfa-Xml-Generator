using System.Text;

namespace Xfa_Xml_Generator
{
    public class XfaJavascript
    {
        private StringBuilder JavascriptBuilder = null;

        public XfaJavascript(ref StringBuilder javascriptBuilder)
        {
            JavascriptBuilder = javascriptBuilder;
        }

        public void SetVisibility(string key, string visibility)
        {
            JavascriptBuilder?.AppendLine($"xfa.resolveNode('xfa.form.{ key }').presence = '{ visibility }';");
        }

        public void SetValue(string key, string value)
        {
            JavascriptBuilder?.AppendLine($"xfa.resolveNode('xfa.form.{ key }').rawValue = '{ value }';");
        }
    }
}
