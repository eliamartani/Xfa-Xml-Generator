using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;

namespace Xfa_Xml_Generator.Helpers
{
    public static class XfaXmlHelper
    {
        public static void AddNode(this XmlDocument node, string name, string index, string value)
        {
            AddNode(node, node, name, index, value);
        }

        public static void AddNode(this XmlNode node, XmlDocument xmlDocument, string name, string index, string value)
        {
            XmlNode newNode = xmlDocument.CreateNode(XmlNodeType.Element, name, "");
            XmlAttribute newAttribute = xmlDocument.CreateAttribute("index");

            newAttribute.Value = Convert.ToString(index);

            newNode.Attributes.Append(newAttribute);

            if (!string.IsNullOrEmpty(value))
            {
                newNode.InnerText = value;
            }

            node.AppendChild(newNode);
        }

        public static string GetFullPath(this XmlNode node, bool addIndex = false)
        {
            List<string> stringList = new List<string>();
            XmlNode currentNode = node;
            Action AddString = () =>
            {
                if (addIndex)
                {
                    stringList.Add($"{ currentNode.Name }[{ currentNode.Attributes["index"]?.Value ?? "0" }]");
                }
                else
                {
                    stringList.Add($"{ currentNode.Name }");
                }
            };

            while (currentNode != null && currentNode.Name != "form1")
            {
                AddString();

                currentNode = currentNode.ParentNode;
            }

            if (currentNode != null)
            {
                AddString();
            }

            stringList.Reverse();

            if (addIndex)
            {
                return string.Join(".", stringList.ToArray());
            }
            else
            {
                return string.Concat("//", string.Join("//", stringList.ToArray()));
            }
        }

        public static List<XmlNode> ToList(this XmlNodeList node)
        {
            List<XmlNode> nodeList = new List<XmlNode>();

            foreach (XmlNode item in node)
            {
                nodeList.Add(item);
            }

            return nodeList;
        }
    }
}
