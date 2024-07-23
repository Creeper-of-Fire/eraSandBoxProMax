using System.Collections.Generic;
using eraSandBox.Coitus.XmlAssign;

namespace eraSandBox.Coitus;

/// <summary> </summary>
[NeedDefInitialize]
public class CoitusMentulaAspect(OrganPart owner) : CoitusAspect(owner)
{
    /// <summary> 有多个，但是一定同属于一条CoitusVaginaRoute,int为插入长度 </summary>
    public readonly Dictionary<CoitusVaginaAspect, int> insert = new();

    [NeedInitialize] public readonly List<CoitusMentulaAspect> linksTo = [];


    public void Agere() //启动效果，包括Vagina和Mentula效果
    {
    }
}