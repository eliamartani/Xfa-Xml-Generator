using System;
using System.Collections.Generic;
using System.Xml;
using System.Linq;
using System.Xml.Linq;

namespace Xfa_Xml_Generator.Helpers
{
    public static class XfaXmlHelper
    {
        public static void AddNode(this XmlDocument node, string name, string index, string value) => AddNode(node, node, name, index, value);

        public static void AddNode(this XmlNode node, XmlDocument xmlDocument, string name, string index, string value)
        {
            var newNode = xmlDocument.CreateNode(XmlNodeType.Element, name, "");
            var newAttribute = xmlDocument.CreateAttribute("index");

            newAttribute.Value = index;

            newNode.Attributes.Append(newAttribute);

            newNode.InnerText = value;

            node.AppendChild(newNode);
        }

        public static string GetFullPath(this XmlNode node, bool addIndex = false)
        {
            var stringList = new List<string>();
            var currentNode = node;

            while (currentNode != null && currentNode.Name != "form1")
            {
                stringList.Add(addIndex ?
                    $"{ currentNode.Name }[{ currentNode.Attributes["index"]?.Value ?? "0" }]" :
                    $"{ currentNode.Name }");

                currentNode = currentNode.ParentNode;
            }

            if (currentNode != null)
            {
                stringList.Add(addIndex ?
                    $"{ currentNode.Name }[{ currentNode.Attributes["index"]?.Value ?? "0" }]" :
                    $"{ currentNode.Name }");
            }

            stringList.Reverse();

            if (addIndex)
            {
                return string.Join(".", stringList.ToArray());
            }

            return string.Concat("//", string.Join("//", stringList.ToArray()));
        }

        public static IEnumerable<XmlNode> ToEnumerable(this XmlNodeList node)
        {
            foreach (XmlNode item in node)
            {
                yield return item;
            }
        }
    }
}
