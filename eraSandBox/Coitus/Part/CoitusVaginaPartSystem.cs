using System;
using System.Collections.Generic;
using System.Linq;

namespace eraSandBox.Coitus.Part
{
    public class CoitusVaginaPartSystem
    {
        public List<CoitusVaginaRoute> oneDirectionVaginaRoutes;
        public List<CoitusVaginaRoute> totalVaginaRoutes;
        public List<CoitusVaginaRoute> twoDirectionVaginaRoutes;

        /// <summary> 在已生成所有线路的情况下，重新生成包含输入部件的路线 </summary>
        /// <param name="section"> 输入需要整理的部件 </param>
        public void UpdateRoute(List<CoitusVaginaPart> section)
        {
            var needUpdateRoutes = new List<CoitusVaginaRoute>();
            foreach (var part in section)
            foreach (var route in this.totalVaginaRoutes)
                if (route.Contains(part))
                    needUpdateRoutes.Add(route);

            var needUpdateRoutesStartPart = new List<CoitusVaginaPart>();
            foreach (var route in needUpdateRoutes)
            {
                this.totalVaginaRoutes.Remove(route);
                needUpdateRoutesStartPart.Add(route.GetEntrance());
            }

            needUpdateRoutesStartPart = needUpdateRoutesStartPart.Distinct().ToList();

            var newlyRoutes = CoitusVaginaRoute.GetRoutes(needUpdateRoutesStartPart);

            this.totalVaginaRoutes.AddRange(newlyRoutes);

            //UpdateRoutes_ClassificationAndDeduplication();
        }

        /// <summary> 重新生成所有的可用插入路线 </summary>
        /// <param name="total"> 输入未整理的部件 </param>
        public void TotalUpdateRoutes(List<CoitusVaginaPart> total)
        {
            var surfaceParts = new List<CoitusVaginaPart>();

            foreach (var coitusSearchSurface in total)
                if (coitusSearchSurface.coitusLinkType == CoitusVaginaPart.CoitusLinkType.Surface)
                    surfaceParts.Add(coitusSearchSurface);
            //选取起点（这步有一点多余，但是性能消耗很小，所以就无所谓了）

            var totalRoutes = CoitusVaginaRoute.GetRoutes(surfaceParts);

            this.totalVaginaRoutes = totalRoutes;
            //获得路径

            //UpdateRoutes_ClassificationAndDeduplication();
        }


        /// <summary> 对totalVaginaRoutes进行分类和去重，并且直接更新this里面的数值 由于程序的便利问题，本代码被弃用，两端情况会进行重复存储 </summary>
        [Obsolete]
        private void UpdateRoutes_ClassificationAndDeduplication()
        {
            var oneDirectionRoutes = new List<CoitusVaginaRoute>();
            var twoDirectionRoutesWithSameRoutes = new List<CoitusVaginaRoute>();
            foreach (var routeType in this.totalVaginaRoutes)
            {
                if (routeType.HasTwoDirection())
                {
                    twoDirectionRoutesWithSameRoutes.Add(routeType);
                    continue;
                }

                oneDirectionRoutes.Add(routeType);
            }

            this.oneDirectionVaginaRoutes = oneDirectionRoutes;
            var twoDirectionRoutesRemoveSame = new List<CoitusVaginaRoute>();
            foreach (var checkSameRoute0 in twoDirectionRoutesWithSameRoutes)
                if (twoDirectionRoutesRemoveSame.Exists(
                        checkSameRoute1 => checkSameRoute0.IsSame(checkSameRoute1)
                    ))
                    twoDirectionRoutesRemoveSame.Add(checkSameRoute0);

            this.twoDirectionVaginaRoutes = twoDirectionRoutesRemoveSame;
        }
    }
}