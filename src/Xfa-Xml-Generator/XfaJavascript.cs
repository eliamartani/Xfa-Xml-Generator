using System.Text;

namespace Xfa_Xml_Generator
{
    public class XfaJavascript
    {
        private readonly StringBuilder _javascriptBuilder;

        public XfaJavascript(ref StringBuilder javascriptBuilder) => _javascriptBuilder = javascriptBuilder;

        public void SetVisibility(string key, string visibility) => _javascriptBuilder?.AppendLine($"xfa.resolveNode('xfa.form.{ key }').presence = '{ visibility }';");

        public void SetValue(string key, string value) => _javascriptBuilder?.AppendLine($"xfa.resolveNode('xfa.form.{ key }').rawValue = '{ value }';");
    }
}
