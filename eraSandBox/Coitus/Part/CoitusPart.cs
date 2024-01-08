namespace eraSandBox.Coitus.Part
{
    /// <summary> CoitusPart是关于Coitus内容的部件，包括人体也包括物品的可使用部分 </summary>
    public abstract class CoitusPart
    {
        public const int TenThousand = 10000;

        public CoitusScaleLinear diameter;

        /// <value> <see cref="lengthLevel" /> 和 <see cref="diameterLevel" /> 是整个运行过程中唯一需要被外部修改数值 </value>
        protected int diameterLevel;

        public CoitusScaleLinear length;

        /// <value> <see cref="lengthLevel" /> 和 <see cref="diameterLevel" /> 是整个运行过程中唯一需要被外部修改的数值 </value>
        protected int lengthLevel;

        /// <value> 每个CoitusPart都有不同的lengthTenThousandth，其数值由xml决定 </value>
        protected int lengthTenThousandth; //单位：万分之一;大概在10%(即1000)数量级，比较合适

        public TestPawn pawn;


        /// <value> 与另一个CoitusPart相关联，它们两个会一起动。
        /// <para> 一个例子：器官内部和它的表面 </para>
        /// </value>
        public CoitusPart relatedTo;

        public CoitusPart(TestPawn pawn)
        {
            this.pawn = pawn;
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