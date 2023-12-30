using System;
using System.Collections.Generic;
using System.Linq;

namespace eraSandBox.Coitus
{
    /// <summary>
    /// 一条路线必定以Surface开始，以End或者Surface结束
    /// <para>实际上这才是和Mentula交互的真正部分：由CoitusVaginaPart链接网络所生成的路线</para>
    /// </summary>
    public class CoitusPatternVaginaRoute : ICloneable
    {
        ///只有part的开头和结尾有可能作为入口，其中First一定是出口
        private List<CoitusPatternVaginaPart> parts;
        

        public void Add(CoitusPatternVaginaPart part)
        {
            parts.Add(part);
        }

        /// <summary>比较两个路线是否一致</summary>
        /// <param name="route">另一条路线</param>
        public bool IsSame(CoitusPatternVaginaRoute route)
        {
            return !(this.parts.Count == route.parts.Count && this.parts.All(route.parts.Contains));
        }

        public bool Contains(CoitusPatternVaginaPart part)
        {
            return parts.Contains(part);
        }

        public List<CoitusPatternVaginaPart> GetEntrance()
        {
            //First一定是出口，不需要检测
            if (HasTwoDirection())
            {
                return new List<CoitusPatternVaginaPart> { parts.First(), parts.Last() };
            }

            return new List<CoitusPatternVaginaPart> { parts.First() };
        }

        //可优化：存储得到结果（一个route是否为是固定的，如果要修改则会使用重新生成的方法）
        /// <summary>是否两头都是入口<see cref="CoitusPatternVaginaPart.CoitusLinkType.Entrance"/></summary>
        public bool HasTwoDirection()
        {
            return parts.Last().coitusLinkType == CoitusPatternVaginaPart.CoitusLinkType.Entrance;
        }

        //TODO 没包括link
        public float GetComfortDiameter() =>
            parts.Select(part => part.diameter.Comfort()).Max();

        public float GetUnComfortDiameter() =>
            parts.Select(part => part.diameter.UnComfort()).Max();

        public float GetComfortLength() =>
            parts.Sum(part => part.length.Comfort());

        public float GetUnComfortLength() =>
            parts.Sum(part => part.length.UnComfort());

        /// <summary>
        /// 复制自身，只复制<see cref="parts"/>里的内容
        /// </summary>
        /// <returns>自身的复制，类型为<see cref="object"/></returns>
        public object Clone()
        {
            return new CoitusPatternVaginaRoute
            {
                parts = parts.ToArray().ToList()
            };
        }
    }
}