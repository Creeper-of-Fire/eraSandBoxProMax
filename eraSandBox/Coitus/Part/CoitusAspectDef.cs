// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnassignedField.Global

using System;

namespace eraSandBox.Coitus;

[Serializable]
public class CoitusAspectDef : Def
{
    public bool ableToTransform;

    public int diameterLevel;

    public int elasticityLevel;

    public bool isSurface;

    public int lengthLevel;

    /// <value> 每个CoitusPart都有不同的lengthTenThousandth，其数值由xml决定 </value>
    public int lengthTenThousandth; //单位：万分之一;大概在10%(即1000)数量级，比较合适

    public int plasticityLevel;

    public int tighticityLevel;
}

[Serializable]
public class Def
{
    public string defName;
}