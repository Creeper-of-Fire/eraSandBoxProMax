using System.Collections.Generic;
using System.Xml;

namespace eraSandBox.Coitus
{
    public class DefXml : Xml
    {
        private readonly List<object> _defs = new List<object>();

        /// <summary>
        /// 全自动化调用xml
        /// </summary>
        public DefXml() : base("Def")
        {
            LoadDef();
        }

        private void LoadDef()
        {
            foreach (XmlNode defNode in this.rootNode.ChildNodes)
            {
                //this._defs.Add(XmlUtility.DeSerializer(GetType().Namespace, defNode));
                this._defs.Add(XmlUtility.DeSerializer(GetType().Namespace, defNode));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="defNeeder">被注入def的对象</param>
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
}