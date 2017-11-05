namespace Xfa_Xml_Generator
{
    public struct XfaRegex
    {
        public const string TEXT_WITHOUT_BRACKETS = "\\[(.*?)\\]";
        public const string TEXT_BETWEEN_BRACKETS = "(?<=\\[)(.*?)(?=\\])";
        public const string TEXT_WITHOUT_SUBFORM = "#subform\\[(.*?)].";
    }
}
