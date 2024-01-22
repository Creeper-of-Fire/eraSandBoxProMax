using System;
using System.Collections.Generic;
using System.Linq;

namespace eraSandBox.Coitus
{
    public interface IVaginaScale : IScale
    {
        public MinusOneToOneRatio ExpansionOrContractionRatio { get; }
        public int PerceptMillimeter();

        public int ComfortMillimeter();

        public int UnComfortMillimeter();
    }

    // public interface ISummableMentulaRouteScale : IScale
    // {
    //     public IEnumerable<IScale> subScales { get; }
    // }
    //
    // public interface ISummableVaginaRouteScale : IVaginaScale
    // {
    //     public IEnumerable<IVaginaScale> subScales { get; }
    // }


    public interface IScale
    {
        public int OriginalMillimeter();
    }

    public class MinusOneToOneRatio
    {
        private readonly List<float> _ratioList = new List<float>();

        /// <summary> 只会不断增加，大的覆盖小的 </summary>
        private float _ratioMax = -1;

        /// <summary> 只会不断增加，大的覆盖小的 </summary>
        public float RatioMax() =>
            this._ratioMax;

        private void UpdateRatioMax()
        {
            this._ratioMax = this._ratioList.Max();
        }

        public void Add(float value)
        {
            this._ratioList.Add(value);
            UpdateRatioMax();
        }

        public void Remove(float item)
        {
            this._ratioList.Remove(item);
            UpdateRatioMax();
        }

        public void Clear()
        {
            this._ratioList.Clear();
            this._ratioMax = -1;
        }


        public static bool operator >=(MinusOneToOneRatio ratio, float item) =>
            ratio._ratioMax >= item;

        public static bool operator <=(MinusOneToOneRatio ratio, float item) =>
            ratio._ratioMax <= item;

        public static int operator *(int item, MinusOneToOneRatio ratio) =>
            (int)(item * ratio._ratioMax);

        public bool IsZero() =>
            this._ratioMax == 0;
    }

    public static class CoitusUtility
    {
        /// <summary> 结构作为节点的类型 </summary>
        public enum CoitusLinkType
        {
            /// <summary> 暂未赋值 </summary>
            Null = 0,

            /// <summary> 腔道的底部 </summary>
            End = 1,

            /// <summary> 腔道的入口 </summary>
            Entrance = 2,

            /// <summary> 通道部分 </summary>
            Corridor = 3,

            /// <summary> 表面 </summary>
            Surface = 4,

            /// <summary> 标志“未启用”的状态 </summary>
            Hidden = 5
        }

        public const float ImPerceptRatio = -1;
        public const float UnComfortRatio = 1;


        public static int NowScaleMillimeter(this IVaginaScale vaginaScale)
        {
            if (vaginaScale.ExpansionOrContractionRatio <= ImPerceptRatio)
                return vaginaScale.PerceptMillimeter();
            if (vaginaScale.ExpansionOrContractionRatio >= UnComfortRatio)
                return vaginaScale.UnComfortMillimeter();
            if (vaginaScale.ExpansionOrContractionRatio.IsZero())
                return vaginaScale.OriginalMillimeter();
            if (vaginaScale.ExpansionOrContractionRatio <= 0)
                return vaginaScale.OriginalMillimeter() +
                       vaginaScale.ToPerceptMillimeter() *
                       vaginaScale.ExpansionOrContractionRatio;
            if (vaginaScale.ExpansionOrContractionRatio >= 0)
                return vaginaScale.OriginalMillimeter() +
                       vaginaScale.ToUnComfortMillimeter() *
                       vaginaScale.ExpansionOrContractionRatio;
            throw new InvalidOperationException();
        }

        //小于Perceptible则为ImPerceptible，大于Perceptible则为Comfortable，大于Comfortable则为UnComfortable，大于UnComfortable则为Destructive
        public static FuckUtility.ComfortType ComfortType(this IVaginaScale vaginaScale, float scale) =>
            scale <= vaginaScale.PerceptMillimeter() ? FuckUtility.ComfortType.ImPerceptible
            : scale <= vaginaScale.ComfortMillimeter() ? FuckUtility.ComfortType.Comfortable
            : scale <= vaginaScale.UnComfortMillimeter() ? FuckUtility.ComfortType.UnComfortable
            : FuckUtility.ComfortType.Destructive;

        public static int ToPerceptMillimeter(this IVaginaScale vaginaScale) =>
            vaginaScale.ComfortMillimeter() - vaginaScale.PerceptMillimeter();

        public static int ToUnComfortMillimeter(this IVaginaScale vaginaScale) =>
            vaginaScale.UnComfortMillimeter() - vaginaScale.ComfortMillimeter();
    }
}