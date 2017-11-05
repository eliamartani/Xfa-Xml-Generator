using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using Xfa_Xml_Generator.Helpers;

namespace Xfa_Xml_Generator
{
    public class XfaXml
    {
        public XmlReader GenerateXML(List<XfaModel> modelList)
        {
            return new XmlNodeReader(GenerateXMLDocument(modelList));
        }

        public string GetXML(List<XfaModel> modelList)
        {
            return GenerateXMLDocument(modelList).OuterXml;
        }

        private XmlDocument GenerateXMLDocument(List<XfaModel> modelList)
        {
            XmlDocument xmlDocument = new XmlDocument();

            foreach (XfaModel model in modelList.OrderBy(c => c.FieldName))
            {
                string currentNodeStructure = "";
                string keyWithoutSubform = new Regex(XfaRegex.TEXT_WITHOUT_SUBFORM).Replace(model.FieldName, "");
                string[] nodeLevels = keyWithoutSubform.Split('.');

                for (int i = 0; i < nodeLevels.Length; i++)
                {
                    List<XmlNode> nodeParents = null;
                    List<XmlNode> nodeChilds = null;

                    string innerText = "";
                    string nodeText = GetNodeText(nodeLevels[i]);
                    string nodeIndex = GetNodeIndex(nodeLevels[i]);

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
                        bool haveSameLevel = nodeChilds.Any(prop => prop.Name.Equals(nodeText) &&
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
                            currentNode = nodeChilds.Where(n => keyWithoutSubform.Contains(n.GetFullPath(true)) && n.Name.Contains(nodeText)).FirstOrDefault();
                        }

                        if (currentNode == null)
                        {
                            currentNode = nodeParents.Where(n => keyWithoutSubform.Contains(n.GetFullPath(true))).FirstOrDefault();
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

        private string GetNodeText(string key)
        {
            return new Regex(XfaRegex.TEXT_WITHOUT_BRACKETS).Replace(key, "");
        }

        private string GetNodeIndex(string key)
        {
            return new Regex(XfaRegex.TEXT_BETWEEN_BRACKETS).Match(key).Value;
        }
    }
}
