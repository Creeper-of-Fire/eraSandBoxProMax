using System;
using System.Collections.Generic;
using System.Linq;

namespace eraSandBox.Coitus.Part
{
    public class CoitusPatternVaginaPartSystem
    {
        public List<CoitusPatternVaginaRoute> oneDirectionVaginaRoutes;
        public List<CoitusPatternVaginaRoute> totalVaginaRoutes;
        public List<CoitusPatternVaginaRoute> twoDirectionVaginaRoutes;

        /// <summary> 在已生成所有线路的情况下，重新生成包含输入部件的路线 </summary>
        /// <param name="section"> 输入需要整理的部件 </param>
        public void UpdateRoute(List<CoitusPatternVaginaPart> section)
        {
            var needUpdateRoutes = new List<CoitusPatternVaginaRoute>();
            foreach (var part in section)
            foreach (var route in this.totalVaginaRoutes)
                if (route.Contains(part))
                    needUpdateRoutes.Add(route);

            var needUpdateRoutesStartPart = new List<CoitusPatternVaginaPart>();
            foreach (var route in needUpdateRoutes)
            {
                this.totalVaginaRoutes.Remove(route);
                needUpdateRoutesStartPart.Add(route.GetEntrance());
            }

            needUpdateRoutesStartPart = needUpdateRoutesStartPart.Distinct().ToList();

            var newlyRoutes = GetRoutes(needUpdateRoutesStartPart);

            this.totalVaginaRoutes.AddRange(newlyRoutes);

            //UpdateRoutes_ClassificationAndDeduplication();
        }

        /// <summary> 由给予的入口，获得这些入口对应的所有路线（两端皆为入口的会计算两次） </summary>
        /// <param name="entranceParts"> 入口 </param>
        /// <returns> 入口可产生的路线（两端皆为入口的会计算两次） </returns>
        private static List<CoitusPatternVaginaRoute> GetRoutes(List<CoitusPatternVaginaPart> entranceParts)
        {
            var totalRoutes = new List<CoitusPatternVaginaRoute>();
            foreach (var startPart in entranceParts)
            {
                var route = new CoitusPatternVaginaRoute();
                route.Add(startPart);
                checkThisPart(startPart, route);

                void checkThisPart(CoitusPatternVaginaPart nowPart, CoitusPatternVaginaRoute nowRoute)
                {
                    if (nowPart.coitusLinkType == CoitusPatternVaginaPart.CoitusLinkType.End ||
                        nowPart.coitusLinkType == CoitusPatternVaginaPart.CoitusLinkType.Entrance)
                        //如果到了末端，则返回
                    {
                        nowRoute.Add(nowPart);
                        totalRoutes.Add(nowRoute);
                        return;
                    }

                    foreach (var nextPart in nowPart.links)
                    {
                        if (nowPart.links.Contains(nextPart))
                            continue;
                        var nextRoute = (CoitusPatternVaginaRoute)nowRoute.Clone();
                        nextRoute.Add(nextPart);
                        checkThisPart(nextPart, nextRoute);
                    }
                }
            }

            return totalRoutes;
        }

        /// <summary> 重新生成所有的可用插入路线 </summary>
        /// <param name="total"> 输入未整理的部件 </param>
        public void TotalUpdateRoutes(List<CoitusPatternVaginaPart> total)
        {
            var SurfaceParts = new List<CoitusPatternVaginaPart>();

            foreach (var coitus_SearchSurface in total)
                if (coitus_SearchSurface.coitusLinkType == CoitusPatternVaginaPart.CoitusLinkType.Surface)
                    SurfaceParts.Add(coitus_SearchSurface);
            //选取起点（这步有一点多余，但是性能消耗很小，所以就无所谓了）

            var totalRoutes = GetRoutes(SurfaceParts);

            this.totalVaginaRoutes = totalRoutes;
            //获得路径

            //UpdateRoutes_ClassificationAndDeduplication();
        }


        /// <summary> 对totalVaginaRoutes进行分类和去重，并且直接更新this里面的数值 由于程序的便利问题，本代码被弃用，是否为两端会进行重复存储 </summary>
        [Obsolete]
        private void UpdateRoutes_ClassificationAndDeduplication()
        {
            var oneDirectionRoutes = new List<CoitusPatternVaginaRoute>();
            var twoDirectionRoutesWithSameRoutes = new List<CoitusPatternVaginaRoute>();
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
            var twoDirectionRoutesRemoveSame = new List<CoitusPatternVaginaRoute>();
            foreach (var checkSameRoute0 in twoDirectionRoutesWithSameRoutes)
                if (twoDirectionRoutesRemoveSame.Exists(
                        checkSameRoute1 => checkSameRoute0.IsSame(checkSameRoute1)
                    ))
                    twoDirectionRoutesRemoveSame.Add(checkSameRoute0);

            this.twoDirectionVaginaRoutes = twoDirectionRoutesRemoveSame;
        }
    }
}