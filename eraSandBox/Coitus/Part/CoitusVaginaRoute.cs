using System;
using System.Collections.Generic;
using System.Linq;

namespace eraSandBox.Coitus.Part;

/// <summary>
///     一条路线必定以Surface开始，以End或者Surface结束
///     <para> 实际上这才是和Mentula交互的真正部分：由CoitusVaginaPartSystem将CoitusVaginaPart的链接网络进行分解，最后所生成的路线 </para>
/// </summary>
public class CoitusVaginaRoute : ICloneable
{
    public readonly CoitusVaginaRouteDiameterScale diameter;
    public readonly CoitusVaginaRouteLengthScale length;
    public TestPawn owner;

    /// <summary> 真正的创造一个Route需要使用<see cref="GetRoutes" /> </summary>
    private CoitusVaginaRoute()
    {
        this.length = new CoitusVaginaRouteLengthScale(this);
        this.diameter = new CoitusVaginaRouteDiameterScale(this);
    }

    public List<CoitusVaginaRoutePiece> PartLink { get; private set; } = new();

    public HashSet<CoitusVaginaAspect> AllParts =>
        this.PartLink.Select(piece => piece.value).ToHashSet();

    /// <summary> 复制自身，只复制 <see cref="PartLink" /> 里的内容 </summary>
    /// <returns> 自身的复制，类型为 <see cref="object" /> </returns>
    public object Clone() =>
        new CoitusVaginaRoute
        {
            PartLink = this.PartLink.ToArray().ToList()
        };

    public void GetCoitusVaginaPartInRange(CoitusMentulaRoute mentulaRoute, int pointFirst, int pointLast)
    {
    }

    private void Add(CoitusVaginaAspect aspect, LinkPoint<CoitusVaginaAspect> linkFrom,
        LinkPoint<CoitusVaginaAspect> linkTo)
    {
        this.PartLink.Add(new CoitusVaginaRoutePiece(aspect, linkFrom, linkTo));
    }

    /// <summary> 比较两个路线是否一致 </summary>
    /// <param name="vaginaRoute"> 另一条路线 </param>
    public bool IsSame(CoitusVaginaRoute vaginaRoute) =>
        !(this.PartLink.Count == vaginaRoute.PartLink.Count &&
          this.PartLink.All(vaginaRoute.PartLink.Contains));

    private bool Contains(CoitusVaginaAspect aspect, LinkPoint<CoitusVaginaAspect> linkFrom)
    {
        return this.PartLink.Any(piece => piece.linkFrom == linkFrom && piece.value == aspect);
    }

    public bool Contains(CoitusVaginaAspect aspect) =>
        this.AllParts.Contains(aspect);

    public CoitusVaginaAspect GetEntrance() =>
        this.PartLink.First().value;

    //可优化：存储得到结果（一个route是否为是固定的，如果要修改则会使用重新生成的方法）
    /// <summary> 是否两头都是入口 <see cref="CoitusVaginaAspect.CoitusLinkType.Entrance" /> </summary>
    public bool HasTwoDirection() =>
        this.PartLink.Last().value.coitusLinkType == CoitusVaginaAspect.CoitusLinkType.Entrance;

    /// <summary> 由给予的入口，获得这些入口对应的所有路线（两端皆为入口的会计算两次） </summary>
    /// <param name="entranceParts"> 入口 </param>
    /// <returns> 入口可产生的路线（两端皆为入口的会计算两次） </returns>
    public static List<CoitusVaginaRoute> GetRoutes(List<CoitusVaginaAspect> entranceParts)
    {
        var totalRoutes = new List<CoitusVaginaRoute>();
        foreach (var startPart in entranceParts)
        {
            var route = new CoitusVaginaRoute();
            CheckThisPart(startPart, null, route);
        }

        return totalRoutes;


        //第n层递归：添加n层的piece，根据n层来生成n+1层
        void CheckThisPart(
            CoitusVaginaAspect nowPart,
            LinkPoint<CoitusVaginaAspect> nowLinkFrom,
            CoitusVaginaRoute nowRoute)
        {
            if (nowLinkFrom != null && (nowPart.coitusLinkType == CoitusVaginaAspect.CoitusLinkType.End ||
                                        nowPart.coitusLinkType == CoitusVaginaAspect.CoitusLinkType.Entrance))
            {
                //如果到了末端，则返回
                nowRoute.Add(nowPart, nowLinkFrom, null);
                totalRoutes.Add(nowRoute);
                return;
            }

            foreach (var nextPoint in nowPart.linkTo)
            {
                var a = nowRoute.PartLink.Select(piece => piece.linkFrom)
                    .Concat(nowRoute.PartLink.Select(piece => piece.linkTo));
                if (a.Any(p => p != null && IsSame(p, nextPoint)))
                    continue;
                var nextPart = nextPoint.GetOppositeObject(nowPart);
                if (nextPart.coitusLinkType == CoitusVaginaAspect.CoitusLinkType.Surface)
                    continue;
                var nextRoute = (CoitusVaginaRoute)nowRoute.Clone();
                nextRoute.Add(nowPart, nowLinkFrom, nextPoint);
                CheckThisPart(nextPart, nextPoint, nextRoute);
            }
        }

        bool IsSame(LinkPoint<CoitusVaginaAspect> a, LinkPoint<CoitusVaginaAspect> b)
        {
            return (a.objectA.baseName, a.objectB.baseName, a.pointAPercentage, a.pointBPercentage) ==
                   (b.objectA.baseName, b.objectB.baseName, b.pointAPercentage, b.pointBPercentage) ||
                   (a.objectA.baseName, a.objectB.baseName, a.pointAPercentage, a.pointBPercentage) ==
                   (b.objectB.baseName, b.objectA.baseName, b.pointBPercentage, b.pointAPercentage);
        }
    }

    public readonly struct CoitusVaginaRoutePiece(
        CoitusVaginaAspect value,
        LinkPoint<CoitusVaginaAspect> linkFrom,
        LinkPoint<CoitusVaginaAspect> linkTo)
    {
        private const int DePercentage = 100;
        public readonly CoitusVaginaAspect value = value;
        public readonly LinkPoint<CoitusVaginaAspect> linkFrom = linkFrom;
        public readonly LinkPoint<CoitusVaginaAspect> linkTo = linkTo;

        private string Name =>
            this.value.baseName;

        private int LengthPercentage =>
            Math.Abs(this.linkFrom.GetPoint(this.value) - this.linkTo.GetPoint(this.value));

        public float LengthRatio =>
            (float)this.LengthPercentage / DePercentage;
    }
}

public abstract class CoitusVaginaRouteScale(CoitusVaginaRoute parent)
{
    public readonly CoitusVaginaRoute parent = parent;
}

public class CoitusVaginaRouteLengthScale(CoitusVaginaRoute parent) : CoitusVaginaRouteScale(parent), IVaginaScale
{
    public MinusOneToOneRatio ExpansionOrContractionRatio { get; } = new();

    /// <summary> 存储原始的数据值 </summary>
    public int OriginalMillimeter()
    {
        return (int)this.parent.PartLink.Sum(part => part.value.length.OriginalMillimeter() * part.LengthRatio);
    }

    public int PerceptMillimeter()
    {
        return (int)this.parent.PartLink.Sum(part => part.value.length.PerceptMillimeter() * part.LengthRatio);
    }

    public int ComfortMillimeter()
    {
        return (int)this.parent.PartLink.Sum(part => part.value.length.ComfortMillimeter() * part.LengthRatio);
    }

    public int UnComfortMillimeter()
    {
        return (int)this.parent.PartLink.Sum(part => part.value.length.UnComfortMillimeter() * part.LengthRatio);
    }
}

public class CoitusVaginaRouteDiameterScale(CoitusVaginaRoute parent) : CoitusVaginaRouteScale(parent), IVaginaScale
{
    //TODO 没包括link的Diameter

    public MinusOneToOneRatio ExpansionOrContractionRatio { get; } = new();

    public int OriginalMillimeter()
    {
        return this.parent.PartLink.Select(part => part.value.length.OriginalMillimeter()).Max();
    }

    public int PerceptMillimeter()
    {
        return this.parent.PartLink.Select(part => part.value.diameter.PerceptMillimeter()).Max();
    }

    public int ComfortMillimeter()
    {
        return this.parent.PartLink.Select(part => part.value.diameter.ComfortMillimeter()).Max();
    }

    public int UnComfortMillimeter()
    {
        return this.parent.PartLink.Select(part => part.value.diameter.UnComfortMillimeter()).Max();
    }
}