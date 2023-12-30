namespace eraSandBox.Coitus
{
    public abstract class CoitusScale
    {
        protected CoitusPart parent;

        ///<summary>基础数值，即只和外部设定值与高度相关的尺寸，除非高度改变，否则这个尺寸不会变</summary>
        public float baseValue;

        /// <summary><see cref="baseValue"/> * <see cref="scaleLevel"/></summary>
        public float value => baseValue * scaleLevel;

        ///<summary>尺寸等级，即可以任意改变的尺寸，受多种因素影响</summary>
        public int scaleLevel;

        /// <param name="baseValue"><see cref="baseValue"/></param>
        /// <param name="scaleLevel"><see cref="scaleLevel"/></param>
        public CoitusScale(float baseValue, int scaleLevel)
        {
            this.baseValue = baseValue;
            this.scaleLevel = scaleLevel;
        }
    }

    public class CoitusScalePatternVagina : CoitusScale
    {
        private const float ELASTICITYLevelToScale = 0.5f;
        private CoitusPatternVaginaPart Parent => (CoitusPatternVaginaPart)parent;

        public float Percept() =>
            value / Parent.elasticityLevel / ELASTICITYLevelToScale;

        public float Comfort() =>
            value;

        public float UnComfort() =>
            value * Parent.elasticityLevel / ELASTICITYLevelToScale;

        //小于Perceptible则为ImPerceptible，大于Perceptible则为Comfortable，大于Comfortable则为UnComfortable，大于UnComfortable则为Destructive
        public bool IsImPerceptible(float scale) =>
            scale <= Percept();

        public bool IsComfortable(float scale) =>
            scale > Percept() && scale <= Comfort();

        public bool IsUnComfortable(float scale) =>
            scale > Comfort() && scale <= UnComfort();

        public bool IsDestructive(float scale) =>
            scale < UnComfort();

        /// <param name="baseValue"><see cref="CoitusScale.baseValue"/></param>
        /// <param name="scaleLevel"><see cref="CoitusScale.scaleLevel"/></param>
        public CoitusScalePatternVagina(float baseValue, int scaleLevel) : base(baseValue, scaleLevel)
        {
        }

        // public CoitusScalePatternVagina(CoitusScale coitusScale) : this(coitusScale.baseValue,coitusScale.scaleLevel)
        // {
        // }
    }
}