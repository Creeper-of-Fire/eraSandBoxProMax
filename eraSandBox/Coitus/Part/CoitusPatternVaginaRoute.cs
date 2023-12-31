using System;
using System.Collections.Generic;
using System.Linq;

namespace eraSandBox.Coitus.Part
{
    /// <summary> 一条路线必定以Surface开始，以End或者Surface结束
    /// <para> 实际上这才是和Mentula交互的真正部分：由CoitusVaginaPartSystem将CoitusVaginaPart的链接网络进行分解，最后所生成的路线 </para>
    /// </summary>
    public class CoitusPatternVaginaRoute : ICloneable
    {
        public List<CoitusPatternVaginaPart> parts;

        public TestPawn pawn;

        /// <summary> 复制自身，只复制 <see cref="parts" /> 里的内容 </summary>
        /// <returns> 自身的复制，类型为 <see cref="object" /> </returns>
        public object Clone()
        {
            return new CoitusPatternVaginaRoute
            {
                parts = this.parts.ToArray().ToList()
            };
        }
        
        public void Add(CoitusPatternVaginaPart part)
        {
            this.parts.Add(part);
        }

        /// <summary> 比较两个路线是否一致 </summary>
        /// <param name="route"> 另一条路线 </param>
        public bool IsSame(CoitusPatternVaginaRoute route)
        {
            return !(this.parts.Count == route.parts.Count && this.parts.All(route.parts.Contains));
        }

        public bool Contains(CoitusPatternVaginaPart part)
        {
            return this.parts.Contains(part);
        }

        /// <summary> 不需要使用这个方法 </summary>
        /// <returns> </returns>
        [Obsolete]
        public List<CoitusPatternVaginaPart> GetEntrances()
        {
            //First一定是出口，不需要检测
            if (HasTwoDirection())
                return new List<CoitusPatternVaginaPart> { this.parts.First(), this.parts.Last() };

            return new List<CoitusPatternVaginaPart> { this.parts.First() };
        }

        public CoitusPatternVaginaPart GetEntrance()
        {
            return this.parts.First();
        }

        //可优化：存储得到结果（一个route是否为是固定的，如果要修改则会使用重新生成的方法）
        /// <summary> 是否两头都是入口 <see cref="CoitusPatternVaginaPart.CoitusLinkType.Entrance" /> </summary>
        public bool HasTwoDirection()
        {
            return this.parts.Last().coitusLinkType == CoitusPatternVaginaPart.CoitusLinkType.Entrance;
        }

        //TODO 没包括link
        public int GetDiameter_PerceptMillimeter()
        {
            return this.parts.Select(part => part.diameter.PerceptMillimeter()).Max();
        }

        public int GetDiameter_ComfortMillimeter()
        {
            return this.parts.Select(part => part.diameter.ComfortMillimeter()).Max();
        }

        public int GetDiameter_UnComfortMillimeter()
        {
            return this.parts.Select(part => part.diameter.UnComfortMillimeter()).Max();
        }

        // public void GetLength(Func<CoitusPatternVaginaPart,int> GetValueFunction)
        // {
        //     this.parts.Sum();
        // }

        public int GetLength_PerceptMillimeter()
        {
            return this.parts.Sum(part => part.length.PerceptMillimeter());
        }

        public int GetLength_ComfortMillimeter()
        {
            return this.parts.Sum(part => part.length.ComfortMillimeter());
        }

        public int GetLength_UnComfortMillimeter()
        {
            return this.parts.Sum(part => part.length.UnComfortMillimeter());
        }

        public FuckUtility.ComfortType GetLength_ComfortType(float scale)
        {
            return scale <= GetLength_PerceptMillimeter() ? FuckUtility.ComfortType.ImPerceptible
                : scale <= GetLength_ComfortMillimeter() ? FuckUtility.ComfortType.Comfortable
                : scale <= GetLength_UnComfortMillimeter() ? FuckUtility.ComfortType.UnComfortable
                : FuckUtility.ComfortType.Destructive;
        }


        #region [Obsolete]

        [Obsolete]
        public bool GetLength_IsImPerceptible(float scale)
        {
            return scale <= GetLength_PerceptMillimeter();
        }

        [Obsolete]
        public bool GetLength_IsComfortable(float scale)
        {
            return scale > GetLength_PerceptMillimeter() && scale <= GetLength_ComfortMillimeter();
        }

        [Obsolete]
        public bool GetLength_IsUnComfortable(float scale)
        {
            return scale > GetLength_ComfortMillimeter() && scale <= GetLength_UnComfortMillimeter();
        }

        [Obsolete]
        public bool GetLength_IsDestructive(float scale)
        {
            return scale < GetLength_UnComfortMillimeter();
        }

        #endregion
    }
}