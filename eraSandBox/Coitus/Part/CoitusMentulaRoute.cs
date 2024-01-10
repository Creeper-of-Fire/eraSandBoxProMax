using System;
using System.Collections.Generic;
using System.Linq;

namespace eraSandBox.Coitus
{
    /// <summary> </summary>
    public class CoitusMentulaRoute : ICloneable
    {
        public readonly CoitusMentulaRouteDiameterScale diameter;
        public readonly CoitusMentulaRouteLengthScale length;
        public CoitusVaginaRoute insert;
        public List<CoitusMentulaAspect> parts;
        public PartTracker partTracker;

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

        public CoitusVaginaRoute ChooseVagina()
        {
            var routes = this.partTracker.coitusVaginaSystem.totalVaginaRoutes;
            var r = new Random();
            return routes.ElementAt(r.Next(routes.Count));
        }

        public void FuckIn(CoitusVaginaRoute vaginaRoute)
        {
            this.insert = vaginaRoute;
            FuckUtility.FuckBetweenRoute(vaginaRoute.length, this.length);
        }

        public bool Contains(CoitusMentulaAspect aspect) =>
            this.parts.Contains(aspect);
    }

    public abstract class CoitusMentulaRouteScale
    {
        public readonly CoitusMentulaRoute parent;

        protected CoitusMentulaRouteScale(CoitusMentulaRoute parent)
        {
            this.parent = parent;
        }
    }

    public class CoitusMentulaRouteLengthScale : CoitusMentulaRouteScale, IScale
    {
        public CoitusMentulaRouteLengthScale(CoitusMentulaRoute parent) : base(parent)
        {
        }

        public IEnumerable<IScale> SubScales =>
            this.parent.parts.Select(part => part.length);

        public int OriginalMillimeter() =>
            this.parent.parts.Sum(part => part.length.OriginalMillimeter());
    }

    public class CoitusMentulaRouteDiameterScale : CoitusMentulaRouteScale, IScale
    {
        public CoitusMentulaRouteDiameterScale(CoitusMentulaRoute parent) : base(parent)
        {
        }

        public int OriginalMillimeter() =>
            this.parent.parts.Select(part => part.diameter.OriginalMillimeter()).Max();
    }
}