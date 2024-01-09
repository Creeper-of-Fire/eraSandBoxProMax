using eraSandBox.Coitus;

namespace eraSandBox.Coitus
{
    /// <summary> CoitusPart是关于Coitus内容的部件，包括人体也包括物品的可使用部分 </summary>
    [NeedDefInitialize]
    public abstract class CoitusAspect : INeedInitialize
    {
        [NeedDef]
        public CoitusAspectDef def;

        public const int TenThousand = 10000;

        [NeedDefInitialize]
        public CoitusScaleLinear diameter;

        /// <value> <see cref="lengthLevel" /> 和 <see cref="diameterLevel" /> 是整个运行过程中唯一需要被外部修改数值 </value>
        [NeedDefInitialize]
        protected int diameterLevel;

        [NeedDefInitialize]
        public CoitusScaleLinear length;

        /// <value> <see cref="lengthLevel" /> 和 <see cref="diameterLevel" /> 是整个运行过程中唯一需要被外部修改的数值 </value>
        [NeedDefInitialize]
        protected int lengthLevel;

        /// <value> 与另一个Part相关联 </value>
        [NeedInitialize]
        public Part owner;

        public CoitusAspect(Part owner)
        {
            this.owner = owner;
        }

        public virtual void Initialize()
        {
            this.diameterLevel = this.def.diameterLevel;
            this.lengthLevel = this.def.lengthLevel;
            this.length =
                new CoitusVaginaScaleLinear(CalculateBaseLength(this.owner.owner, this.def.lengthTenThousandth),
                    this.lengthLevel);
            this.diameter =
                new CoitusVaginaScaleLinear(CalculateBaseDiameter(this.owner.owner, this.def.lengthTenThousandth),
                    this.diameterLevel);
        }

        protected static int CalculateBaseLength(TestPawn pawn, int lengthTenThousandth) =>
            pawn.heightMillimeter * lengthTenThousandth / TenThousand;

        protected static int CalculateBaseDiameter(TestPawn pawn, int lengthTenThousandth) =>
            CalculateBaseLength(pawn, lengthTenThousandth);

        public void Agere() //启动效果，包括Vagina和Mentula效果
        {
        }
    }
}