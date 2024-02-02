using System;
using System.Collections.Generic;
using System.Linq;

namespace eraSandBox.Coitus.Part;

/// <summary> </summary>
public class CoitusMentulaRoute : ICloneable
{
    public readonly CoitusMentulaRouteDiameterScale diameter;
    public readonly CoitusMentulaRouteLengthScale length;
    public CoitusVaginaRoute insert;
    public List<CoitusMentulaAspect> parts;

    public CoitusMentulaRoute()
    {
        this.length = new CoitusMentulaRouteLengthScale(this);
        this.diameter = new CoitusMentulaRouteDiameterScale(this);
    }

    /// <summary> 复制自身，只复制 <see cref="parts" /> 里的内容 </summary>
    /// <returns> 自身的复制，类型为 <see cref="object" /> </returns>
    public object Clone() =>
        new CoitusMentulaRoute
        {
            parts = this.parts.ToArray().ToList()
        };

    public void Add(CoitusMentulaAspect aspect)
    {
        this.parts.Add(aspect);
    }

    public bool Contains(CoitusMentulaAspect aspect) =>
        this.parts.Contains(aspect);
}

public abstract class CoitusMentulaRouteScale(CoitusMentulaRoute parent)
{
    public readonly CoitusMentulaRoute parent = parent;
}

public class CoitusMentulaRouteLengthScale(CoitusMentulaRoute parent) : CoitusMentulaRouteScale(parent), IScale
{
    public IEnumerable<IScale> SubScales =>
        this.parent.parts.Select(part => part.length);

    public int OriginalMillimeter()
    {
        return this.parent.parts.Sum(part => part.length.OriginalMillimeter());
    }
}

public class CoitusMentulaRouteDiameterScale(CoitusMentulaRoute parent) : CoitusMentulaRouteScale(parent), IScale
{
    public int OriginalMillimeter()
    {
        return this.parent.parts.Select(part => part.diameter.OriginalMillimeter()).Max();
    }
}