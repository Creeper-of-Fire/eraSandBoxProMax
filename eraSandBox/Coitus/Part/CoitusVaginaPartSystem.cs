using System;
using System.Collections.Generic;
using System.Linq;

namespace eraSandBox.Coitus
{
    public class CoitusVaginaPartSystem
    {
        public List<CoitusVaginaRoute> TotalVaginaRoutes { get; private set; }
        private Dictionary<string, CoitusVaginaAspect> totalAspects => this.owner.totalVaginaAspects;
        private readonly PartTracker owner;
        public CoitusVaginaPartSystem(PartTracker owner)
        {
            this.owner = owner;
        }

        /// <summary> 在已生成所有线路的情况下，重新生成包含输入部件的路线 </summary>
        /// <param name="section"> 输入需要整理的部件 </param>
        public void UpdateRoute(List<CoitusVaginaAspect> section)
        {
            var needUpdateRoutes = new List<CoitusVaginaRoute>();
            foreach (var part in section)
            foreach (var route in this.TotalVaginaRoutes)
                if (route.Contains(part))
                    needUpdateRoutes.Add(route);

            var needUpdateRoutesStartPart = new List<CoitusVaginaAspect>();
            foreach (var route in needUpdateRoutes)
            {
                this.TotalVaginaRoutes.Remove(route);
                needUpdateRoutesStartPart.Add(route.GetEntrance());
            }

            needUpdateRoutesStartPart = needUpdateRoutesStartPart.Distinct().ToList();

            var newlyRoutes = CoitusVaginaRoute.GetRoutes(needUpdateRoutesStartPart);

            this.TotalVaginaRoutes.AddRange(newlyRoutes);

            //UpdateRoutes_ClassificationAndDeduplication();
        }

        public void UpdateRoutesTotally()
        {
            this.TotalVaginaRoutes = GetTotalUpdateRoutes(this.totalAspects.Values);
        }

        /// <summary> 重新生成所有的可用插入路线 </summary>
        /// <param name="total"> 输入未整理的部件 </param>
        private static List<CoitusVaginaRoute> GetTotalUpdateRoutes(IEnumerable<CoitusVaginaAspect> total)
        {
            var entranceParts = new List<CoitusVaginaAspect>();

            foreach (var coitusSearchSurface in total)
                if (coitusSearchSurface.coitusLinkType == CoitusVaginaAspect.CoitusLinkType.Entrance)
                    entranceParts.Add(coitusSearchSurface);
            //选取起点（这步有一点多余，但是性能消耗很小，所以就无所谓了）

            var totalRoutes = CoitusVaginaRoute.GetRoutes(entranceParts);

            return totalRoutes;
            //获得路径

            //UpdateRoutes_ClassificationAndDeduplication();
        }
    }
}