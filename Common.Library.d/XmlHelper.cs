﻿using System.Data;
using System.Xml;

namespace System
{
    /// <summary>
    /// Xml的操作公共类
    /// </summary>
    public class XmlHelper
    {
        #region 字段定义

        /// <summary>
        /// XML文件的物理路径
        /// </summary>
        private string _filePath = string.Empty;

        /// <summary>
        /// Xml文档
        /// </summary>
        private XmlDocument _xml;

        /// <summary>
        /// XML的根节点
        /// </summary>
        private XmlElement _element;

        #endregion 字段定义

        #region 构造方法

        /// <summary>
        /// 实例化XmlHelper对象
        /// </summary>
        /// <param name="xmlFilePath">Xml文件的相对路径</param>
        public XmlHelper(string xmlFilePath)
        {
            //获取XML文件的绝对路径
            this._filePath = SysHelper.GetPath(xmlFilePath);
        }

        #endregion 构造方法

        #region 创建XML的根节点

        /// <summary>
        /// 创建XML的根节点
        /// </summary>
        private void CreateXMLElement()
        {
            //创建一个XML对象
            this._xml = new XmlDocument();

            if (DirFileHelper.IsExistFile(this._filePath))
            {
                //加载XML文件
                this._xml.Load(this._filePath);
            }

            //为XML的根节点赋值
            this._element = this._xml.DocumentElement;
        }

        #endregion 创建XML的根节点

        #region 获取指定XPath表达式的节点对象

        /// <summary>
        /// 获取指定XPath表达式的节点对象
        /// </summary>
        /// <param name="xPath">XPath表达式,
        /// 范例1: @"Skill/First/SkillItem", 等效于 @"//Skill/First/SkillItem"
        /// 范例2: @"Table[USERNAME='a']" , []表示筛选,USERNAME是Table下的一个子节点.
        /// 范例3: @"ApplyPost/Item[@itemName='岗位编号']",@itemName是Item节点的属性.
        /// </param>
        public XmlNode GetNode(string xPath)
        {
            //创建XML的根节点
            this.CreateXMLElement();

            //返回XPath节点
            return this._element.SelectSingleNode(xPath);
        }

        #endregion 获取指定XPath表达式的节点对象

        #region 获取指定XPath表达式节点的值

        /// <summary>
        /// 获取指定XPath表达式节点的值
        /// </summary>
        /// <param name="xPath">XPath表达式,
        /// 范例1: @"Skill/First/SkillItem", 等效于 @"//Skill/First/SkillItem"
        /// 范例2: @"Table[USERNAME='a']" , []表示筛选,USERNAME是Table下的一个子节点.
        /// 范例3: @"ApplyPost/Item[@itemName='岗位编号']",@itemName是Item节点的属性.
        /// </param>
        public string GetValue(string xPath)
        {
            //创建XML的根节点
            this.CreateXMLElement();

            //返回XPath节点的值
            return this._element.SelectSingleNode(xPath).InnerText;
        }

        #endregion 获取指定XPath表达式节点的值

        #region 获取指定XPath表达式节点的属性值

        /// <summary>
        /// 获取指定XPath表达式节点的属性值
        /// </summary>
        /// <param name="xPath">XPath表达式,
        /// 范例1: @"Skill/First/SkillItem", 等效于 @"//Skill/First/SkillItem"
        /// 范例2: @"Table[USERNAME='a']" , []表示筛选,USERNAME是Table下的一个子节点.
        /// 范例3: @"ApplyPost/Item[@itemName='岗位编号']",@itemName是Item节点的属性.
        /// </param>
        /// <param name="attributeName">属性名</param>
        public string GetAttributeValue(string xPath, string attributeName)
        {
            //创建XML的根节点
            this.CreateXMLElement();

            //返回XPath节点的属性值
            return this._element.SelectSingleNode(xPath).Attributes[attributeName].Value;
        }

        #endregion 获取指定XPath表达式节点的属性值

        #region 新增节点

        /// <summary>
        /// 1. 功能：新增节点。
        /// 2. 使用条件：将任意节点插入到当前Xml文件中。
        /// </summary>
        /// <param name="xmlNode">要插入的Xml节点</param>
        public void AppendNode(XmlNode xmlNode)
        {
            //创建XML的根节点
            this.CreateXMLElement();

            //导入节点
            XmlNode node = this._xml.ImportNode(xmlNode, true);

            //将节点插入到根节点下
            this._element.AppendChild(node);
        }

        /// <summary>
        /// 1. 功能：新增节点。
        /// 2. 使用条件：将DataSet中的第一条记录插入Xml文件中。
        /// </summary>
        /// <param name="ds">DataSet的实例，该DataSet中应该只有一条记录</param>
        public void AppendNode(DataSet ds)
        {
            //创建XmlDataDocument对象
            XmlDataDocument xmlDataDocument = new XmlDataDocument(ds);

            //导入节点
            XmlNode node = xmlDataDocument.DocumentElement.FirstChild;

            //将节点插入到根节点下
            this.AppendNode(node);
        }

        #endregion 新增节点

        #region 删除节点

        /// <summary>
        /// 删除指定XPath表达式的节点
        /// </summary>
        /// <param name="xPath">XPath表达式,
        /// 范例1: @"Skill/First/SkillItem", 等效于 @"//Skill/First/SkillItem"
        /// 范例2: @"Table[USERNAME='a']" , []表示筛选,USERNAME是Table下的一个子节点.
        /// 范例3: @"ApplyPost/Item[@itemName='岗位编号']",@itemName是Item节点的属性.
        /// </param>
        public void RemoveNode(string xPath)
        {
            //创建XML的根节点
            this.CreateXMLElement();

            //获取要删除的节点
            XmlNode node = this._xml.SelectSingleNode(xPath);

            //删除节点
            this._element.RemoveChild(node);
        }

        #endregion 删除节点

        #region 保存XML文件

        /// <summary>
        /// 保存XML文件
        /// </summary>
        public void Save()
        {
            //创建XML的根节点
            this.CreateXMLElement();

            //保存XML文件
            this._xml.Save(this._filePath);
        }

        #endregion 保存XML文件

        #region 静态方法

        #region 创建根节点对象

        /// <summary>
        /// 创建根节点对象
        /// </summary>
        /// <param name="xmlFilePath">Xml文件的相对路径</param>
        private static XmlElement CreateRootElement(string xmlFilePath)
        {
            //定义变量，表示XML文件的绝对路径
            string filePath = "";

            //获取XML文件的绝对路径
            filePath = SysHelper.GetPath(xmlFilePath);

            //创建XmlDocument对象
            XmlDocument xmlDocument = new XmlDocument();
            //加载XML文件
            xmlDocument.Load(filePath);

            //返回根节点
            return xmlDocument.DocumentElement;
        }

        #endregion 创建根节点对象

        #region 获取指定XPath表达式节点的值

        /// <summary>
        /// 获取指定XPath表达式节点的值
        /// </summary>
        /// <param name="xmlFilePath">Xml文件的相对路径</param>
        /// <param name="xPath">XPath表达式,
        /// 范例1: @"Skill/First/SkillItem", 等效于 @"//Skill/First/SkillItem"
        /// 范例2: @"Table[USERNAME='a']" , []表示筛选,USERNAME是Table下的一个子节点.
        /// 范例3: @"ApplyPost/Item[@itemName='岗位编号']",@itemName是Item节点的属性.
        /// </param>
        public static string GetValue(string xmlFilePath, string xPath)
        {
            //创建根对象
            XmlElement rootElement = CreateRootElement(xmlFilePath);

            //返回XPath节点的值
            return rootElement.SelectSingleNode(xPath).InnerText;
        }

        #endregion 获取指定XPath表达式节点的值

        #region 获取指定XPath表达式节点的属性值

        /// <summary>
        /// 获取指定XPath表达式节点的属性值
        /// </summary>
        /// <param name="xmlFilePath">Xml文件的相对路径</param>
        /// <param name="xPath">XPath表达式,
        /// 范例1: @"Skill/First/SkillItem", 等效于 @"//Skill/First/SkillItem"
        /// 范例2: @"Table[USERNAME='a']" , []表示筛选,USERNAME是Table下的一个子节点.
        /// 范例3: @"ApplyPost/Item[@itemName='岗位编号']",@itemName是Item节点的属性.
        /// </param>
        /// <param name="attributeName">属性名</param>
        public static string GetAttributeValue(string xmlFilePath, string xPath, string attributeName)
        {
            //创建根对象
            XmlElement rootElement = CreateRootElement(xmlFilePath);

            //返回XPath节点的属性值
            return rootElement.SelectSingleNode(xPath).Attributes[attributeName].Value;
        }

        #endregion 获取指定XPath表达式节点的属性值

        #endregion 静态方法

        public static void SetValue(string xmlFilePath, string xPath, string newtext)
        {
            //string path = SysHelper.GetPath(xmlFilePath);
            //var queryXML = from xmlLog in xelem.Descendants("msg_log")
            //               //所有名字为Bin的记录
            //               where xmlLog.Element("user").Value == "Bin"
            //               select xmlLog;

            //foreach (XElement el in queryXML)
            //{
            //    el.Element("user").Value = "LiuBin";//开始修改
            //}
            //xelem.Save(path);
        }
    }
}