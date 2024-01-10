using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace eraSandBox.Coitus
{
    public static class XmlUtility
    {
        /// <summary> 将xml反序列化为对象 </summary>
        /// <param name="namespaceString"> </param>
        /// <param name="xml"> </param>
        /// <returns> </returns>
        // ReSharper disable once AssignNullToNotNullAttribute
        public static object DeSerializer(string namespaceString, XmlNode xml)
        {
            var xmlNodeReader = new XmlNodeReader(xml);
            xmlNodeReader.LookupNamespace(null);
            var t = Type.GetType(namespaceString + "." + xml.Name);
            var serializer = new XmlSerializer(t);
            return serializer.Deserialize(xmlNodeReader);
        }

        public static object DeSerializer(Type type, XmlNode xml)
        {
            var xmlNodeReader = new XmlNodeReader(xml);
            xmlNodeReader.LookupNamespace(null);
            var serializer = new XmlSerializer(type);
            return serializer.Deserialize(xmlNodeReader);
        }

        /// <summary> 将实体对象转换成XML </summary>
        /// <typeparam name="T"> 实体类型 </typeparam>
        /// <param name="obj"> 实体对象 </param>
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
    }
}