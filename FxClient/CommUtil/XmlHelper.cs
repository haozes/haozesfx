using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Xml;

namespace Haozes.FxClient.CommUtil
{
    public static class XmlHelper
    {
        public static XmlElement AppendChildElement(XmlNode node, string name)
        {
            XmlElement newChild = ((node is XmlDocument) ? ((XmlDocument)node) : node.OwnerDocument).CreateElement(name, node.NamespaceURI);
            node.AppendChild(newChild);
            return newChild;
        }

        public static void CreateXmlReaderForSipMessage(string msgBody, object context, ReadXmlContentDelegate readContentCallback, string rootNode)
        {
            bool flag = (rootNode != null) && (rootNode.Length > 0);
            XmlTextReader reader = new XmlTextReader(msgBody, XmlNodeType.Document, null);
            if (flag)
            {
                reader.ReadStartElement(rootNode);
            }
            if (readContentCallback != null)
            {
                readContentCallback(reader, context);
            }
        }

        public static void CreateXmlReaderForSipResponse(string msgBody, object context, ReadXmlContentDelegate readContentCallback)
        {
            CreateXmlReaderForSipMessage(msgBody, context, readContentCallback, "results");
        }

        public static void CreateXmlWriterForSipMessage(TextWriter tw, object context, WriteXmlContentDelegate writeContentCallback, string rootNode)
        {
            bool flag = (rootNode != null) && (rootNode.Length > 0);
            XmlTextWriter writer = new XmlTextWriter(tw);
            if (flag)
            {
                writer.WriteStartElement(rootNode);
            }
            if (writeContentCallback != null)
            {
                writeContentCallback(writer, context);
            }
            if (flag)
            {
                writer.WriteEndElement();
            }
            writer.Flush();
            writer.Close();
        }

        public static void CreateXmlWriterForSipRequest(TextWriter tw, object context, WriteXmlContentDelegate writeContentCallback)
        {
            CreateXmlWriterForSipMessage(tw, context, writeContentCallback, "args");
        }

        public static XmlNode GetXmlNodeOfReceiveMessage(string msgBody, string nodeName)
        {
            return GetXmlNodeOfReceiveMessage(msgBody, nodeName, null);
        }

        public static XmlNode GetXmlNodeOfReceiveMessage(string msgBody, string nodeName, string rootName)
        {
            XmlDocument document = new XmlDocument();
            document.LoadXml(msgBody);
            XmlNode documentElement = document.DocumentElement;
            if (!string.IsNullOrEmpty(rootName) && (rootName != documentElement.Name))
            {
                throw new ApplicationException("rootName != root.Name");
            }
            return documentElement.SelectSingleNode(nodeName);
        }

        public static bool LoadXmlDocSafely(string filepath, XmlDocument doc, object syncObj)
        {
            lock (syncObj)
            {
                try
                {
                    doc.Load(filepath);
                }
                catch
                {
                    return false;
                }
                return true;
            }
        }

        public static XmlNode MakeSureChildNodeExists(XmlNode node, string name)
        {
            XmlDocument doc = (node is XmlDocument) ? ((XmlDocument)node) : node.OwnerDocument;
            return MakeSureChildNodeExists(node, name, doc);
        }

        private static XmlNode MakeSureChildNodeExists(XmlNode node, string name, XmlDocument doc)
        {
            XmlNode newChild = node.SelectSingleNode(name);
            if (newChild == null)
            {
                if ((node == doc) && doc.HasChildNodes)
                {
                    doc.RemoveAll();
                }
                newChild = doc.CreateElement(name, node.NamespaceURI);
                node.AppendChild(newChild);
            }
            return newChild;
        }

        public static XmlNode MakeSureChildPathExists(XmlNode node, string xpath)
        {
            if (string.IsNullOrEmpty(xpath))
            {
                throw new ArgumentException("Invalid xpath - <empty>");
            }
            XmlDocument doc = (node is XmlDocument) ? ((XmlDocument)node) : node.OwnerDocument;
            XmlNode node2 = null;
            int startIndex = 0;
            while (startIndex < xpath.Length)
            {
                int index = xpath.IndexOf('/', startIndex);
                if (index < 0)
                {
                    index = xpath.Length;
                }
                string name = xpath.Substring(startIndex, index - startIndex);
                if (name.Length <= 0)
                {
                    throw new ArgumentException("Invalid xpath - " + xpath);
                }
                startIndex = index + 1;
                node2 = MakeSureChildNodeExists(node, name, doc);
                node = node2;
            }
            return node2;
        }

        public static XmlNode MakeSureXmlNodeExists(XmlDocument doc, string path)
        {
            XmlNode node = doc;
            while (path.Length > 0)
            {
                int index = path.IndexOf('/');
                if (index == 0)
                {
                    throw new Exception("Invalid path!");
                }
                if (index < 0)
                {
                    return MakeSureChildNodeExists(node, path);
                }
                string name = path.Substring(0, index);
                node = MakeSureChildNodeExists(node, name);
                path = path.Substring(index + 1);
            }
            return null;
        }

        public static bool? ReadXmlAttributeBoolean(XmlNode node, string attrName)
        {
            string str = ReadXmlAttributeString(node, attrName).Trim();
            bool? nullable = null;
            if (str.Length == 1)
            {
                return new bool?(str == "1");
            }
            if (str.Length > 0)
            {
                nullable = new bool?(Convert.ToBoolean(str));
            }
            return nullable;
        }

        public static bool ReadXmlAttributeBoolean(XmlNode node, string attrName, bool defaultVal)
        {
            bool? nullable2 = ReadXmlAttributeBoolean(node, attrName);
            if (!nullable2.HasValue)
            {
                return defaultVal;
            }
            return nullable2.GetValueOrDefault();
        }

        public static DateTime? ReadXmlAttributeDateTime(XmlNode node, string attrName)
        {
            string str = ReadXmlAttributeString(node, attrName).Trim();
            DateTime? nullable = null;
            if (str.Length > 0)
            {
                nullable = new DateTime?(Convert.ToDateTime(str));
            }
            return nullable;
        }

        public static DateTime ReadXmlAttributeDateTime(XmlNode node, string attrName, DateTime defaultVal)
        {
            DateTime? nullable2 = ReadXmlAttributeDateTime(node, attrName);
            if (!nullable2.HasValue)
            {
                return defaultVal;
            }
            return nullable2.GetValueOrDefault();
        }

        public static T ReadXmlAttributeEnum<T>(XmlNode node, string attrName, T defaultVal)
        {
            int? nullable = ReadXmlAttributeInt32(node, attrName);
            if (!nullable.HasValue || !nullable.HasValue)
            {
                return defaultVal;
            }
            try
            {
                return (T)Enum.ToObject(typeof(T), nullable.Value);
            }
            catch
            {
                return defaultVal;
            }
        }

        public static int? ReadXmlAttributeInt32(XmlNode node, string attrName)
        {
            int num;
            if (int.TryParse(ReadXmlAttributeString(node, attrName).Trim(), out num))
            {
                return new int?(num);
            }
            return null;
        }

        public static int ReadXmlAttributeInt32(XmlNode node, string attrName, int defaultVal)
        {
            int? nullable2 = ReadXmlAttributeInt32(node, attrName);
            if (!nullable2.HasValue)
            {
                return defaultVal;
            }
            return nullable2.GetValueOrDefault();
        }

        public static long? ReadXmlAttributeInt64(XmlNode node, string attrName)
        {
            long num;
            if (long.TryParse(ReadXmlAttributeString(node, attrName).Trim(), out num))
            {
                return new long?(num);
            }
            return null;
        }

        public static long ReadXmlAttributeInt64(XmlNode node, string attrName, long defaultVal)
        {
            long? nullable2 = ReadXmlAttributeInt64(node, attrName);
            if (!nullable2.HasValue)
            {
                return defaultVal;
            }
            return nullable2.GetValueOrDefault();
        }

        public static string ReadXmlAttributeString(XmlNode node, string attrName)
        {
            return ReadXmlAttributeString(node, attrName, false);
        }

        public static string ReadXmlAttributeString(XmlNode node, string attrName, bool required)
        {
            XmlAttribute attribute = node.Attributes[attrName];
            if (required)
            {
                if ((attribute == null) || (attribute.Value.Length <= 0))
                {
                    throw new ApplicationException(string.Format("Attribute - {0} is missed!", attrName));
                }
                return attribute.Value;
            }
            if (attribute != null)
            {
                return attribute.Value;
            }
            return string.Empty;
        }

        public static bool SaveXmlDocSafely(XmlDocument doc, string filepath, object syncObj)
        {
            lock (syncObj)
            {
                try
                {
                    doc.Save(filepath);
                }
                catch
                {
                    return false;
                }
                return true;
            }
        }

        public static void SetNodeAttribute(XmlNode node, string attrName, string attrVal)
        {
            XmlDocument document = (node is XmlDocument) ? ((XmlDocument)node) : node.OwnerDocument;
            XmlAttribute attribute = node.Attributes[attrName];
            if (attribute == null)
            {
                attribute = node.Attributes.Append(document.CreateAttribute(attrName));
            }
            attribute.Value = attrVal;
        }

        public static void SetNodeAttributeBool(XmlNode node, string attrName, bool attrVal)
        {
            SetNodeAttribute(node, attrName, attrVal ? "1" : "0");
        }

        public static void SetNodeAttributeInt32(XmlNode node, string attrName, int attrVal)
        {
            SetNodeAttribute(node, attrName, attrVal.ToString());
        }

        public static void WriteAttributeStringNotEmpty(XmlWriter writer, string attrName, string attrVal)
        {
            if (!string.IsNullOrEmpty(attrVal))
            {
                writer.WriteAttributeString(attrName, attrVal);
            }
        }

        public delegate void ReadXmlContentDelegate(XmlReader reader, object context);

        public delegate void WriteXmlContentDelegate(XmlWriter writer, object context);
    }
}

