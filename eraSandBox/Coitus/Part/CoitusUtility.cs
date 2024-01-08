using System;
using System.Collections.Generic;
using System.Linq;

namespace eraSandBox.Coitus.Part
{
    public interface IVaginaScale
    {
        public MinusOneToOneRatio expansionOrContractionRatio { get; }
        public int OriginalMillimeter();
        public int PerceptMillimeter();

        public int ComfortMillimeter();

        public int UnComfortMillimeter();
    }

    public class MinusOneToOneRatio
    {
        private readonly List<float> ratioList = new List<float>();

        /// <summary> 只会不断增加，大的覆盖小的 </summary>
        private float ratioMax = -1;

        /// <summary> 只会不断增加，大的覆盖小的 </summary>
        public float RatioMax() =>
            this.ratioMax;

        private void UpdateRatioMax()
        {
            this.ratioMax = this.ratioList.Max();
        }

        public void Add(float value)
        {
            this.ratioList.Add(value);
            UpdateRatioMax();
        }

        public void Remove(float item)
        {
            this.ratioList.Remove(item);
            UpdateRatioMax();
        }

        public void Clear()
        {
            this.ratioList.Clear();
            this.ratioMax = -1;
        }


        public static bool operator >=(MinusOneToOneRatio ratio, float item) =>
            ratio.ratioMax >= item;

        public static bool operator <=(MinusOneToOneRatio ratio, float item) =>
            ratio.ratioMax <= item;

        public static int operator *(int item, MinusOneToOneRatio ratio) =>
            (int)(item * ratio.ratioMax);

        public bool IsZero() =>
            this.ratioMax == 0;
    }

    public static class CoitusUtility
    {
        public const float ImPerceptRATIO = -1;
        public const float UnComfortRATIO = 1;


        public static int NowScaleMillimeter(this IVaginaScale vaginaScale)
        {
            if (vaginaScale.expansionOrContractionRatio <= ImPerceptRATIO)
                return vaginaScale.PerceptMillimeter();
            if (vaginaScale.expansionOrContractionRatio >= UnComfortRATIO)
                return vaginaScale.UnComfortMillimeter();
            if (vaginaScale.expansionOrContractionRatio.IsZero())
                return vaginaScale.OriginalMillimeter();
            if (vaginaScale.expansionOrContractionRatio <= 0)
                return vaginaScale.OriginalMillimeter() +
                       vaginaScale.ToPerceptMillimeter() *
                       vaginaScale.expansionOrContractionRatio;
            if (vaginaScale.expansionOrContractionRatio >= 0)
                return vaginaScale.OriginalMillimeter() +
                       vaginaScale.ToUnComfortMillimeter() *
                       vaginaScale.expansionOrContractionRatio;
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