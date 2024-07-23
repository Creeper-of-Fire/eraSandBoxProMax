using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace eraSandBox.Coitus.XmlAssign;

public class DefXml : Xml
{
    private readonly Dictionary<(string, string), object> _defs = new();

    /// <summary> 全自动化调用xml </summary>
    private DefXml(string defFileName) : base(defFileName)
    {
        if (defFileName == "")
            return;
        this.LoadDef();
    }

    private DefXml(IEnumerable<string> defFileNames)
    {
        this._defs = defFileNames
            .SelectMany(fileName => new DefXml(fileName)._defs)
            .ToDictionary(pair => pair.Key, pair => pair.Value);
    }

    private static DefXml Instance { get; } = new(GetAllDefFile());

    private static IEnumerable<string> GetAllDefFile()
    {
        string[] fileDirs = Directory.GetFiles(XmlPath, "*Def.xml", SearchOption.AllDirectories);
        return fileDirs.Select(Path.GetFileNameWithoutExtension);
    }

    private void LoadDef()
    {
        foreach (XmlNode defNode in this.rootNode.ChildNodes)
        {
            object nodeDeSerialized = XmlUtility.DeSerializer(this.GetType().Namespace, defNode);
            this._defs.Add(
                (nodeDeSerialized.GetType().Name
                    , (string)nodeDeSerialized.GetType().GetField("defName").GetValue(nodeDeSerialized)),
                nodeDeSerialized);
        }
    }

    /// <summary> </summary>
    /// <param name="defNeeder"> 被注入def的对象 </param>
    /// <param name="defName"></param>
    public static void AssignDef(object defNeeder, string defName)
    {
        var defOfDefNeeder = defNeeder.GetType().GetField("def");
        string defType = defOfDefNeeder.FieldType.Name;
        object newDef = MakeDef(defType, defName);
        defOfDefNeeder.SetValue(defNeeder, newDef);
    }

    private static object MakeDef(string defType, string defName)
    {
        if (Instance._defs.ContainsKey((defType, defName)))
            return Instance._defs[(defType, defName)];
        object def = DataUtility.Copy(Instance._defs[(defType, "Default")]);
        def.GetType().GetField("defName").SetValue(def, defName);
        return def;
    }
}