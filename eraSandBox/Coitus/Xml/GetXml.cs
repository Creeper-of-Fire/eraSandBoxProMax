using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace eraSandBox.Coitus
{
    /// <summary>
    /// 需要在Def的帮助下才能实现初始化，也就是说这个类/字段并不是在程序运行过程中为了特定目的生成的，而就是主要被操作的对象
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Class)]
    public class NeedDefInitialize : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class NeedDef : Attribute
    {
    }

    /// <summary>
    /// 需要调用初始化函数的字段
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class NeedInitialize : Attribute
    {
    }

    public interface INeedInitialize
    {
        public void Initialize();
    }

    public abstract class Xml
    {
        //TODO
        private const string Path =
            @"D:\creeper10\Desktop\Project\eraSandBox\eraSandBox\eraSandBox\Coitus\XmlData\";

        public readonly XmlElement rootNode;

        protected Xml(string filename)
        {
            var document = new XmlDocument();
            document.Load(Path + filename + ".xml");
            this.rootNode = document.DocumentElement;
        }
    }


    public class DefXml : Xml
    {
        private readonly List<object> _defs = new List<object>();

        public DefXml() : base("Def")
        {
            LoadDef();
        }

        private void LoadDef()
        {
            foreach (XmlNode defNode in this.rootNode.ChildNodes)
            {
                this._defs.Add(XmlExtend.DeSerializer(this.GetType().Namespace,defNode));
            }
        }

        public void AssignDef(object defNeeder)
        {
            var defOfDefNeeder = defNeeder.GetType().GetField("def");
            string defType = defOfDefNeeder.FieldType.Name + "Def";
            object newDef = MakeDef(defType);
            defOfDefNeeder.SetValue(defNeeder, newDef);
        }

        private object MakeDef(string defType)
        {
            return this._defs.Find(d => d.GetType().Name == defType);
        }
    }

    public class LinkXml : Xml
    {
        public LinkXml() : base("LinkSetup")
        {
        }

        public void AssignRoute(string species)
        {
            XmlElement speciesNode;
            foreach (XmlElement node in this.rootNode.ChildNodes)
            {
                if (node.GetAttribute("name") != species)
                    continue;
                speciesNode = node;
                break;
            }

            var routeList = new List<RouteInfo>();
        }

        public class RouteInfo
        {
            public string name;
            public RouteInfo link;
        }
    }

    public static class XmlExtend
    {
        // public static IEnumerable<XmlNode> ChildNodes(this XmlNode)
        // {
        // }

        // public static void AddFieldByReflection(this object thisObject, string fieldName, object value)
        // {
        //     var defOfDefNeeder = thisObject.GetType().GetField(fieldName);
        //     string defName = defOfDefNeeder.FieldType.Name;
        //     defOfDefNeeder.SetValue(thisObject, value);
        // }

        // ReSharper disable once AssignNullToNotNullAttribute
        public static object DeSerializer(string namespaceString, XmlNode xml)
        {
            var xmlNodeReader = new XmlNodeReader(xml);
            xmlNodeReader.LookupNamespace(null);
            var t = Type.GetType(namespaceString + "." + xml.Name);
            var serializer = new XmlSerializer(t);
            return serializer.Deserialize(xmlNodeReader);
        }


        // public static object DeSerializer(string typeName, string strXml)
        // {
        //     var a= new StringReader(strXml);
        //     var t = Type.GetType(typeName);
        //     var serializer = new XmlSerializer(t);
        //     return serializer.Deserialize(a);
        // }

        /// <summary>
        /// 将实体对象转换成XML
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="obj">实体对象</param>
        public static string XmlSerialize<T>(T obj)
        {
            try
            {
                using var sw = new StringWriter();
                var t = obj.GetType();
                var serializer = new XmlSerializer(obj.GetType());
                serializer.Serialize(sw, obj);
                sw.Close();
                return sw.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("将实体对象转换成XML异常", ex);
            }
        }

        public class NamespaceIgnorantXmlTextReader : XmlNodeReader
        {
            public override string NamespaceURI
            {
                get { return ""; }
            }

            public NamespaceIgnorantXmlTextReader(XmlNode node) : base(node)
            {
            }
        }
    }
}