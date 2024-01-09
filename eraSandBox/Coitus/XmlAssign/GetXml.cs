using System;
using System.Collections.Generic;
using System.Xml;

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

    /// <summary>
    /// 需要使用Initialize进行二次初始化
    /// </summary>
    public interface INeedInitialize
    {
        public void Initialize();
    }

    public abstract class Xml
    {
        //TODO 可替换地址
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
}