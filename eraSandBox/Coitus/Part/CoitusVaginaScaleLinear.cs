namespace eraSandBox.Coitus.Part
{
    public class CoitusVaginaScaleLinear : CoitusScaleLinear, IVaginaScale
    {
        private const float LEVELToSCALE = 0.5f;

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

        public MinusOneToOneRatio expansionOrContractionRatio { get; } = new MinusOneToOneRatio();

        public int PerceptMillimeter() =>
            (int)(this.valueMillimeter / LEVELToSCALE / this.Parent.tighticityLevel);

        /// <summary> 用于“结点”类型的计算 </summary>
        public int ComfortMillimeter() =>
            OriginalMillimeter();

        public int UnComfortMillimeter() =>
            (int)(this.valueMillimeter * this.Parent.elasticityLevel / LEVELToSCALE);

        /// <summary> 存储原始的数据值 </summary>
        public int OriginalMillimeter() =>
            this.valueMillimeter;
    }
}