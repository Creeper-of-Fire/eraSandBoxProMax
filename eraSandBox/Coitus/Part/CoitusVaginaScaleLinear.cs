namespace eraSandBox.Coitus
{
    public class CoitusVaginaScaleLinear : CoitusScaleLinear, IVaginaScale
    {
        private const float LevelToScale = 0.5f;
        public CoitusVaginaScaleLinear(int baseValueMillimeter, int scaleLevel, CoitusAspect parent) : base(baseValueMillimeter, scaleLevel, parent)
        {
        }


        private CoitusVaginaAspect Parent =>
            (CoitusVaginaAspect)this.parent;

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