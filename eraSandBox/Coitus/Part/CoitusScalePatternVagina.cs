using System;

namespace eraSandBox.Coitus.Part
{
    public abstract class CoitusScale
    {
        /// <summary> 基础数值，即只和外部设定值与高度相关的尺寸，除非高度改变，否则这个尺寸不会变 </summary>
        public int baseValueMillimeter;

        protected CoitusPart parent;

        /// <summary> 尺寸等级，即可以任意改变的尺寸，受多种因素影响 </summary>
        public int scaleLevel;

        /// <param name="baseValueMillimeter">
        /// <see cref="baseValueMillimeter" />
        /// </param>
        /// <param name="scaleLevel">
        /// <see cref="scaleLevel" />
        /// </param>
        public CoitusScale(int baseValueMillimeter, int scaleLevel)
        {
            this.baseValueMillimeter = baseValueMillimeter;
            this.scaleLevel = scaleLevel;
        }

        /// <summary> <see cref="baseValueMillimeter" /> * <see cref="scaleLevel" /> </summary>
        public int valueMillimeter => this.baseValueMillimeter * this.scaleLevel;

        /*
        public static int operator +(CoitusScale scale0, CoitusScale scale1)
        {
            return scale0.valueMillimeter + scale1.valueMillimeter;
        }

        public static int operator -(CoitusScale scale0, CoitusScale scale1)
        {
            return scale0.valueMillimeter - scale1.valueMillimeter;
        }

        public static int operator %(CoitusScale scale0, CoitusScale scale1)
        {
            return scale0.valueMillimeter % scale1.valueMillimeter;
        }

        public static int operator %(CoitusScale scale0, int scale1)
        {
            return scale0.valueMillimeter % scale1;
        }

        public static int operator %(int scale0, CoitusScale scale1)
        {
            return scale0 % scale1.valueMillimeter;
        }

        public static bool operator >(CoitusScale scale0, CoitusScale scale1)
        {
            return scale0.valueMillimeter > scale1.valueMillimeter;
        }

        public static bool operator <(CoitusScale scale0, CoitusScale scale1)
        {
            return scale0.valueMillimeter < scale1.valueMillimeter;
        }

        public static bool operator <=(CoitusScale scale0, CoitusScale scale1)
        {
            return scale0.valueMillimeter <= scale1.valueMillimeter;
        }

        public static bool operator >=(CoitusScale scale0, CoitusScale scale1)
        {
            return scale0.valueMillimeter >= scale1.valueMillimeter;
        }*/
    }

    public class CoitusScalePatternVagina : CoitusScale
    {
        private const float ELASTICITYLevelToScale = 0.5f;

        /// <param name="baseValueMillimeter">
        /// <see cref="CoitusScale.baseValueMillimeter" />
        /// </param>
        /// <param name="scaleLevel">
        /// <see cref="CoitusScale.scaleLevel" />
        /// </param>
        public CoitusScalePatternVagina(int baseValueMillimeter, int scaleLevel) : base(baseValueMillimeter, scaleLevel)
        {
        }

        private CoitusPatternVaginaPart Parent => (CoitusPatternVaginaPart)this.parent;

        public int PerceptMillimeter()
        {
            return (int)(this.valueMillimeter / ELASTICITYLevelToScale / this.Parent.tighticityLevel);
        }

        public int ComfortMillimeter()
        {
            return this.valueMillimeter;
        }

        public int UnComfortMillimeter()
        {
            return (int)(this.valueMillimeter * this.Parent.elasticityLevel / ELASTICITYLevelToScale);
        }

        //小于Perceptible则为ImPerceptible，大于Perceptible则为Comfortable，大于Comfortable则为UnComfortable，大于UnComfortable则为Destructive
        public FuckUtility.ComfortType GetLength_ComfortType(float scale)
        {
            return scale <= PerceptMillimeter() ? FuckUtility.ComfortType.ImPerceptible
                : scale <= ComfortMillimeter() ? FuckUtility.ComfortType.Comfortable
                : scale <= UnComfortMillimeter() ? FuckUtility.ComfortType.UnComfortable
                : FuckUtility.ComfortType.Destructive;
        }

        [Obsolete]
        public bool IsImPerceptible(float scale)
        {
            return scale <= PerceptMillimeter();
        }

        [Obsolete]
        public bool IsComfortable(float scale)
        {
            return scale > PerceptMillimeter() && scale <= ComfortMillimeter();
        }

        [Obsolete]
        public bool IsUnComfortable(float scale)
        {
            return scale > ComfortMillimeter() && scale <= UnComfortMillimeter();
        }

        [Obsolete]
        public bool IsDestructive(float scale)
        {
            return scale < UnComfortMillimeter();
        }
    }
}