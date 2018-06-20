using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using Xfa_Xml_Generator.Helpers;

namespace Xfa_Xml_Generator
{
    public class XfaXml
    {
        public static XmlReader GenerateXML(List<XfaModel> modelList) => new XmlNodeReader(GenerateXMLDocument(modelList));

        public static string GetXML(List<XfaModel> modelList) => GenerateXMLDocument(modelList).OuterXml;

        private static XmlDocument GenerateXMLDocument(List<XfaModel> modelList)
        {
            var xmlDocument = new XmlDocument();

            foreach (var model in modelList.OrderBy(c => c.FieldName))
            {
                var currentNodeStructure = "";
                var keyWithoutSubform = new Regex(XfaRegex.TEXT_WITHOUT_SUBFORM).Replace(model.FieldName, "");
                var nodeLevels = keyWithoutSubform.Split('.');

                for (var i = 0; i < nodeLevels.Length; i++)
                {
                    List<XmlNode> nodeParents = null;
                    List<XmlNode> nodeChilds = null;

                    var innerText = "";
                    var nodeText = GetNodeText(nodeLevels[i]);
                    var nodeIndex = GetNodeIndex(nodeLevels[i]);

                    if (i == nodeLevels.Length - 1)
                    {
                        innerText = model.Value;
                    }

                    if (string.IsNullOrEmpty(currentNodeStructure))
                    {
                        nodeParents = xmlDocument.SelectNodes($"./{ nodeText }").ToList();

                        currentNodeStructure = $"./{ nodeText }";
                    }
                    else
                    {
                        nodeParents = xmlDocument.SelectNodes(currentNodeStructure).ToList();

                        currentNodeStructure = string.Concat(currentNodeStructure, "/", nodeText);
                    }

                    nodeChilds = xmlDocument.SelectNodes(currentNodeStructure).ToList();

                    if (nodeParents.Count > 0)
                    {
                        XmlNode currentNode = null;
                        var haveSameLevel = nodeChilds.Any(prop => prop.Name.Equals(nodeText) &&
                                                        prop.Attributes["index"].Value.Equals(nodeIndex) &&
                                                        keyWithoutSubform.Contains(prop.GetFullPath(true)));

                        if (haveSameLevel && (
                                (nodeParents.Count > 1 && nodeParents.Last().SelectNodes($"./{  nodeText }").Count > 0) ||
                                (nodeParents.Count <= 1)
                            ))
                        {
                            continue;
                        }

                        if (nodeChilds.Count > 0)
                        {
                            currentNode = nodeChilds.FirstOrDefault(n => keyWithoutSubform.Contains(n.GetFullPath(true)) && n.Name.Contains(nodeText));
                        }

                        if (currentNode == null)
                        {
                            currentNode = nodeParents.FirstOrDefault(n => keyWithoutSubform.Contains(n.GetFullPath(true)));
                        }

                        currentNode?.AddNode(xmlDocument, nodeText, nodeIndex, innerText);
                    }
                    else
                    {
                        xmlDocument.AddNode(nodeText, nodeIndex, innerText);
                    }
                }
            }

            return xmlDocument;
        }

        private static string GetNodeText(string key) => new Regex(XfaRegex.TEXT_WITHOUT_BRACKETS).Replace(key, "");

        private static string GetNodeIndex(string key) => new Regex(XfaRegex.TEXT_BETWEEN_BRACKETS).Match(key).Value;
    }
}
