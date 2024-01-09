namespace eraSandBox.Coitus.Part
{
    public class CoitusVaginaScaleLinear : CoitusScaleLinear, IVaginaScale
    {
        private const float LevelToScale = 0.5f;

        /// <param name="baseValueMillimeter">
        /// <see cref="CoitusScaleLinear.baseValueMillimeter" />
        /// </param>
        /// <param name="scaleLevel">
        /// <see cref="CoitusScaleLinear.scaleLevel" />
        /// </param>
        public CoitusVaginaScaleLinear(int baseValueMillimeter, int scaleLevel) : base(baseValueMillimeter,
            scaleLevel)
        {
        }

        private CoitusVaginaPart Parent =>
            (CoitusVaginaPart)this.parent;

        public MinusOneToOneRatio ExpansionOrContractionRatio { get; } = new MinusOneToOneRatio();

        public int PerceptMillimeter() =>
            (int)(OriginalMillimeter() / LevelToScale / this.Parent.tighticityLevel);

        /// <summary> 用于“结点”类型的计算 </summary>
        public int ComfortMillimeter() =>
            OriginalMillimeter();

        public int UnComfortMillimeter() =>
            (int)(OriginalMillimeter() * this.Parent.elasticityLevel / LevelToScale);
    }
}