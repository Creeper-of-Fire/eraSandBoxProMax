using System;
using System.Collections.Generic;
using System.Linq;

namespace eraSandBox.Coitus.Part
{
    /// <summary> </summary>
    public class CoitusMentulaRoute : ICloneable
    {
        public List<CoitusMentulaPart> parts;
        public TestPawn pawn;

        /// <summary> 复制自身，只复制 <see cref="parts" /> 里的内容 </summary>
        /// <returns> 自身的复制，类型为 <see cref="object" /> </returns>
        public object Clone() =>
            new CoitusMentulaRoute
            {
                parts = this.parts.ToArray().ToList()
            };

        public void Add(CoitusMentulaPart part)
        {
            this.parts.Add(part);
        }

        public CoitusVaginaRoute ChooseVagina()
        {
            var routes = this.pawn.System.totalVaginaRoutes;
            var r = new Random();
            return routes.ElementAt(r.Next(routes.Count));
        }

        public int GetLengthMillimeter()
        {
            return this.parts.Sum(part => part.length.valueMillimeter);
        }

        public int GetDiameterMillimeter()
        {
            return this.parts.Select(part => part.diameter.valueMillimeter).Max();
        }

        public void FuckIn(CoitusVaginaRoute vaginaRoute)
        {
            FuckUtility.FuckBetweenRoute(vaginaRoute, this);
        }

        public bool Contains(CoitusMentulaPart part) =>
            this.parts.Contains(part);
    }
}