using System;
using System.Collections.Generic;
using System.Linq;

namespace eraSandBox.Coitus.Part
{
    /// <summary> 一条路线必定以Surface开始，以End或者Surface结束
    /// <para> 实际上这才是和Mentula交互的真正部分：由CoitusVaginaPartSystem将CoitusVaginaPart的链接网络进行分解，最后所生成的路线 </para>
    /// </summary>
    public class CoitusVaginaRoute : ICloneable
    {
        public readonly CoitusVaginaRouteLengthScale length;
        public IVaginaScale diameter;
        public List<CoitusVaginaPart> parts;

        public TestPawn pawn;

        /// <summary> 构造函数是private，因为真正的创造一个Route需要使用<see cref="GetRoutes" /> </summary>
        private CoitusVaginaRoute()
        {
            this.length = new CoitusVaginaRouteLengthScale(this);
            this.diameter = new CoitusVaginaRouteDiameterScale(this);
        }

        /// <summary> 复制自身，只复制 <see cref="parts" /> 里的内容 </summary>
        /// <returns> 自身的复制，类型为 <see cref="object" /> </returns>
        public object Clone() =>
            new CoitusVaginaRoute
            {
                parts = this.parts.ToArray().ToList()
            };

        private void Initialize()
        {
            this.length.MakeAxis();
        }

        public void GetCoitusVaginaPartInRange(CoitusMentulaRoute mentulaRoute, int pointFirst, int pointLast)
        {
        }

        private void Add(CoitusVaginaPart part)
        {
            this.parts.Add(part);
        }

        /// <summary> 比较两个路线是否一致 </summary>
        /// <param name="vaginaRoute"> 另一条路线 </param>
        public bool IsSame(CoitusVaginaRoute vaginaRoute) =>
            !(this.parts.Count == vaginaRoute.parts.Count &&
              this.parts.All(vaginaRoute.parts.Contains));

        public bool Contains(CoitusVaginaPart part) =>
            this.parts.Contains(part);

        [Obsolete]
        public List<CoitusVaginaPart> GetEntrances()
        {
            //First一定是出口，不需要检测
            if (HasTwoDirection())
                return new List<CoitusVaginaPart> { this.parts.First(), this.parts.Last() };

            return new List<CoitusVaginaPart> { this.parts.First() };
        }

        public CoitusVaginaPart GetEntrance() =>
            this.parts.First();

        //可优化：存储得到结果（一个route是否为是固定的，如果要修改则会使用重新生成的方法）
        /// <summary> 是否两头都是入口 <see cref="CoitusVaginaPart.CoitusLinkType.Entrance" /> </summary>
        public bool HasTwoDirection() =>
            this.parts.Last().coitusLinkType == CoitusVaginaPart.CoitusLinkType.Entrance;

        /// <summary> 由给予的入口，获得这些入口对应的所有路线（两端皆为入口的会计算两次） </summary>
        /// <param name="entranceParts"> 入口 </param>
        /// <returns> 入口可产生的路线（两端皆为入口的会计算两次） </returns>
        public static List<CoitusVaginaRoute> GetRoutes(List<CoitusVaginaPart> entranceParts)
        {
            var totalRoutes = new List<CoitusVaginaRoute>();
            foreach (var startPart in entranceParts)
            {
                var route = new CoitusVaginaRoute();
                route.Add(startPart);
                checkThisPart(startPart, route);
                continue;

                void checkThisPart(CoitusVaginaPart nowPart, CoitusVaginaRoute nowRoute)
                {
                    if (nowPart.coitusLinkType == CoitusVaginaPart.CoitusLinkType.End ||
                        nowPart.coitusLinkType == CoitusVaginaPart.CoitusLinkType.Entrance)
                        //如果到了末端，则返回
                    {
                        nowRoute.Add(nowPart);
                        nowRoute.Initialize();
                        totalRoutes.Add(nowRoute);
                        return;
                    }

                    foreach (var nextPart in nowPart.links)
                    {
                        if (nowPart.links.Contains(nextPart))
                            continue;
                        var nextRoute = (CoitusVaginaRoute)nowRoute.Clone();
                        nextRoute.Add(nextPart);
                        checkThisPart(nextPart, nextRoute);
                    }
                }
            }

            return totalRoutes;
        }
    }

    public class CoitusVaginaRouteLengthScale : IVaginaScale, IWithAxis
    {
        private readonly CoitusVaginaRoute parent;

        public CoitusVaginaRouteLengthScale(CoitusVaginaRoute parent)
        {
            this.parent = parent;
        }

        public MinusOneToOneRatio expansionOrContractionRatio { get; } = new MinusOneToOneRatio();

        /// <summary> 存储原始的数据值 </summary>
        public int OriginalMillimeter()
        {
            return this.parent.parts.Sum(part => part.length.OriginalMillimeter());
        }

        public int PerceptMillimeter()
        {
            return this.parent.parts.Sum(part => part.length.PerceptMillimeter());
        }

        public int ComfortMillimeter()
        {
            return this.parent.parts.Sum(part => part.length.ComfortMillimeter());
        }

        public int UnComfortMillimeter()
        {
            return this.parent.parts.Sum(part => part.length.UnComfortMillimeter());
        }

        public Axis baseAxis { get; } = new Axis();

        public void MakeAxis()
        {
            this.baseAxis.MakeAxis(this.parent.parts.Select(
                distance => distance.length.OriginalMillimeter()
            ).ToList());
        }
    }

    public class CoitusVaginaRouteDiameterScale : IVaginaScale
    {
        private readonly CoitusVaginaRoute parent;
        //TODO 没包括link

        public CoitusVaginaRouteDiameterScale(CoitusVaginaRoute parent)
        {
            this.parent = parent;
        }

        public MinusOneToOneRatio expansionOrContractionRatio { get; } = new MinusOneToOneRatio();

        public int OriginalMillimeter()
        {
            return this.parent.parts.Select(part => part.length.OriginalMillimeter()).Max();
        }

        public int PerceptMillimeter()
        {
            return this.parent.parts.Select(part => part.diameter.PerceptMillimeter()).Max();
        }

        public int ComfortMillimeter()
        {
            return this.parent.parts.Select(part => part.diameter.ComfortMillimeter()).Max();
        }

        public int UnComfortMillimeter()
        {
            return this.parent.parts.Select(part => part.diameter.UnComfortMillimeter()).Max();
        }
    }
}