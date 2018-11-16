using System.Text.RegularExpressions;

namespace System.Xml.Linq
{
    public static class XmlExtensions
    {
        public static string GetValue(this XElement element)
        {
            if (element == null)
            {
                return null;
            }
            return element.Value;
        }

        public static XDeclaration SetDeclaration(this XDocument doc, string version = "1.0", string encoding = "utf-8", string standalone = null)
        {
            if (doc.Declaration != null)
            {
                doc.Declaration.Version = version;
                doc.Declaration.Encoding = encoding;
                doc.Declaration.Standalone = standalone;
            }
            else
            {
                XDeclaration declaration = new XDeclaration(version, encoding, standalone);
                doc.Declaration = declaration;
            }
            return doc.Declaration;
        }

        public static XElement AddNode(this XContainer parentNode, string nodeName)
        {
            XElement xElement = new XElement(nodeName);
            parentNode.Add(xElement);
            return xElement;
        }

        public static XElement AddNode(this XContainer parentNode, string nodeName, object content)
        {
            XElement xElement = new XElement(nodeName, content);
            parentNode.Add(xElement);
            return xElement;
        }

        public static XElement AddNode(this XContainer parentNode, string nodeName, params object[] content)
        {
            XElement xElement = new XElement(nodeName, content);
            parentNode.Add(xElement);
            return xElement;
        }

        public static XElement AddTextNode(this XContainer parentNode, string nodeName, object value)
        {
            string value2 = string.Empty;
            if (value != null)
            {
                value2 = value.ToString();
            }
            return parentNode.AddTextNode(nodeName, value2);
        }

        public static XElement AddTextNode(this XContainer parentNode, string nodeName, string value)
        {
            string value2 = XmlExtensions.ReplaceInvalidChar(value);
            XElement xElement = new XElement(nodeName);
            xElement.Value = value2;
            parentNode.Add(xElement);
            return xElement;
        }

        public static XElement AddTextNode(this XContainer parentNode, string nodeName, int value)
        {
            return parentNode.AddTextNode(nodeName, value.ToString());
        }

        public static XElement AddCDataNode(this XContainer parentNode, string nodeName, object value)
        {
            string value2 = string.Empty;
            if (value != null)
            {
                value2 = value.ToString();
            }
            return parentNode.AddCDataNode(nodeName, value2);
        }

        public static XElement AddCDataNode(this XContainer parentNode, string nodeName, string value)
        {
            XElement xElement = new XElement(nodeName);
            XCData content = new XCData(XmlExtensions.ReplaceInvalidChar(value));
            xElement.Add(content);
            parentNode.Add(xElement);
            return xElement;
        }

        private static string ReplaceInvalidChar(string s)
        {
            s = (s ?? "");
            string pattern = "[\0-\b]|[\v-\f]|[\u000e-\u001f]";
            s = s.Replace("￿", "");
            s = Regex.Replace(s, pattern, "");
            return s;
        }
    }
}