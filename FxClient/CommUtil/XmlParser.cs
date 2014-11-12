using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Data;
using System.IO;

namespace Haozes.FxClient.CommUtil
{
    public class XmlParser
    {
        private XmlDocument doc;

        public XmlParser(string dataStr)
        {
            this.doc = new XmlDocument();
            try
            {
                this.doc.LoadXml(dataStr);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public XmlDocument Doc
        {
            get { return this.doc; }
        }

        /// <summary>
        /// 读取指定结点信息
        /// </summary>
        /// <param name="sectionName">父节点</param>
        /// <param name="nodename">子节点名称</param>
        /// <returns>节点值</returns>
        public string GetAppsetting(string sectionName, string nodename)
        {
            string result = string.Empty;
            XmlNodeList nodelist = null;
            try
            {
                nodelist = this.doc.SelectSingleNode(sectionName).ChildNodes;
            }
            catch
            {
                result = "Cannot find the value of" + sectionName + "in ConfigFile!";
            }
            foreach (XmlNode node in nodelist)
            {
                if (node.Name == nodename)
                {
                    result = node.InnerText;
                    break;
                }
                else
                {
                    result = "Cannot find the value of" + nodename + "in ConfigFile!";
                }
            }
            return result;
        }

        /// <summary>
        /// 返回指定结点的值
        /// </summary>
        /// <param name="xmlPathNode">路径（xpath）</param>
        /// <returns>如果XPATH找不到，返回NULL</returns>
        public string GetNodeValue(string xmlPathNode)
        {
            string nodeValue = string.Empty;
            if (this.doc.SelectSingleNode(xmlPathNode) != null)
            {
                nodeValue = this.doc.SelectSingleNode(xmlPathNode).InnerText;
            }
            else
            {
                nodeValue = string.Empty;
            }
            return nodeValue;
        }

        public string GetNodeAttribute(string xmlPath, string attribName)
        {
            XmlNode node = null;
            if (this.doc.SelectSingleNode(xmlPath) != null)
            {
                node = this.doc.SelectSingleNode(xmlPath);
                XmlAttribute attrib = node.Attributes[attribName];
                if (attrib != null)
                    return attrib.Value;
                else
                    return string.Empty;
            }
            return string.Empty;
        }

        // 查找指定节点的数据 返回DataView
        public DataView GetDataView(string xmlPathNode)
        {
            DataSet dst = new DataSet();
            StringReader read = new StringReader(this.doc.SelectSingleNode(xmlPathNode).OuterXml);
            dst.ReadXml(read);
            return dst.Tables[0].DefaultView;
        }

        public DataTable GetDataTable(string xmlPathNode)
        {
            if (this.doc.SelectSingleNode(xmlPathNode) == null)
                return null;
            DataSet dst = new DataSet();
            StringReader read = new StringReader(this.doc.SelectSingleNode(xmlPathNode).OuterXml);
            dst.ReadXml(read);
            if (dst.Tables.Count > 0)
                return dst.Tables[0];
            else
                return null;
        }

        public DataSet GetDataSet(string xmlPathNode)
        {
            DataSet dst = new DataSet();
            StringReader read = new StringReader(this.doc.SelectSingleNode(xmlPathNode).OuterXml);
            dst.ReadXml(read);
            return dst;
        }

        /// <summary>
        /// 更新指定节点的内容
        /// </summary>
        /// <param name="xmlPathNode">节点</param>
        /// <param name="content">内容</param>
        public void EditNode(string xmlPathNode, string content)
        {
            this.doc.SelectSingleNode(xmlPathNode).InnerText = content;
        }

        /// <summary>
        /// 删除指定节点
        /// </summary>
        /// <param name="node">节点</param>
        public void Delete(string node)
        {
            string mainNode = node.Substring(0, node.LastIndexOf("/"));
            this.doc.SelectSingleNode(mainNode).RemoveChild(this.doc.SelectSingleNode(node));
        }

        /// <summary>
        /// 插入一个节点和此节点的一子节点。
        /// </summary>
        /// <param name="mainNode">根节点</param>
        /// <param name="childNode">子节点</param>
        /// <param name="element">元素名</param>
        /// <param name="content">值</param>
        public void InsertNode(string mainNode, string childNode, string element, string content)
        {
            XmlNode objRootNode = this.doc.SelectSingleNode(mainNode);
            XmlElement objChildNode = this.doc.CreateElement(childNode);
            objRootNode.AppendChild(objChildNode);
            XmlElement objElement = this.doc.CreateElement(element);
            objElement.InnerText = content;
            objChildNode.AppendChild(objElement);
        }

        /// <summary>
        /// 插入一节点 包括一个属性
        /// </summary>
        /// <param name="mainNode">根节点</param>
        /// <param name="element">子节点</param>
        /// <param name="attrib">属性名</param>
        /// <param name="attribContent">属性内容</param>
        /// <param name="content">子节点内容</param>
        public void InsertElement(string mainNode, string element, string attrib, string attribContent, string content)
        {
            XmlNode objNode = this.doc.SelectSingleNode(mainNode);
            XmlElement objElement = this.doc.CreateElement(element);
            objElement.SetAttribute(attrib, attribContent);
            objElement.InnerText = content;
            objNode.AppendChild(objElement);
        }

        // 插入一个节点，包括两个属性
        public void InsertElement(string mainNode, string element, string attrib1, string attrib2, string attribContent1, string attribContent2, string content)
        {
            XmlNode objNode = this.doc.SelectSingleNode(mainNode);
            XmlElement objElement = this.doc.CreateElement(element);
            objElement.SetAttribute(attrib1, attribContent1);
            objElement.SetAttribute(attrib2, attribContent2);
            objElement.InnerText = content;
            objNode.AppendChild(objElement);
        }

        // 插入一节点不带属性
        public void InsertElement(string mainNode, string element, string content)
        {
            XmlNode objNode = this.doc.SelectSingleNode(mainNode);
            XmlElement objElement = this.doc.CreateElement(element);
            objElement.InnerText = content;
            objNode.AppendChild(objElement);
        }

        public void Dispose()
        {
            this.doc = null;
        }
    }
}
