namespace eraSandBox.Coitus.Part
{
    public abstract class CoitusScale
    {
        ///<summary>基础数值，即只和外部设定值与高度相关的尺寸，除非高度改变，否则这个尺寸不会变</summary>
        public float baseValue;

        protected CoitusPart parent;

        ///<summary>尺寸等级，即可以任意改变的尺寸，受多种因素影响</summary>
        public int scaleLevel;

        /// <param name="baseValue">
        /// <see cref="baseValue" />
        /// </param>
        /// <param name="scaleLevel">
        /// <see cref="scaleLevel" />
        /// </param>
        public CoitusScale(float baseValue, int scaleLevel)
        {
            this.baseValue = baseValue;
            this.scaleLevel = scaleLevel;
        }

        /// <summary><see cref="baseValue" /> * <see cref="scaleLevel" /></summary>
        public float value => this.baseValue * this.scaleLevel;
    }

    public class CoitusScalePatternVagina : CoitusScale
    {
        private const float ELASTICITYLevelToScale = 0.5f;

        /// <param name="baseValue">
        /// <see cref="CoitusScale.baseValue" />
        /// </param>
        /// <param name="scaleLevel">
        /// <see cref="CoitusScale.scaleLevel" />
        /// </param>
        public CoitusScalePatternVagina(float baseValue, int scaleLevel) : base(baseValue, scaleLevel)
        {
        }

        private CoitusPatternVaginaPart Parent => (CoitusPatternVaginaPart)this.parent;

        public float Percept()
        {
            return this.value / this.Parent.elasticityLevel / ELASTICITYLevelToScale;
        }

        public float Comfort()
        {
            return this.value;
        }

        public float UnComfort()
        {
            return this.value * this.Parent.elasticityLevel / ELASTICITYLevelToScale;
        }

        //小于Perceptible则为ImPerceptible，大于Perceptible则为Comfortable，大于Comfortable则为UnComfortable，大于UnComfortable则为Destructive
        public bool IsImPerceptible(float scale)
        {
            return scale <= Percept();
        }

        public bool IsComfortable(float scale)
        {
            return scale > Percept() && scale <= Comfort();
        }

        public bool IsUnComfortable(float scale)
        {
            return scale > Comfort() && scale <= UnComfort();
        }

        public bool IsDestructive(float scale)
        {
            return scale < UnComfort();
        }

        // public CoitusScalePatternVagina(CoitusScale coitusScale) : this(coitusScale.baseValue,coitusScale.scaleLevel)
        // {
        // }
    }
}