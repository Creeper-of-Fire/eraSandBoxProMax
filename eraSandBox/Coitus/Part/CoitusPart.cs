namespace eraSandBox.Coitus.Part
{
    /// <summary>CoitusPart是关于Coitus内容的部件，包括人体也包括物品的可使用部分</summary>
    public abstract class CoitusPart
    {
        public CoitusScale diameter;

        /// <value><see cref="lengthLevel" /> 和 <see cref="diameterLevel" /> 是整个运行过程中唯一需要被外部修改数值</value>
        protected int diameterLevel;

        public CoitusScale length;

        /// <value><see cref="lengthLevel" /> 和 <see cref="diameterLevel" /> 是整个运行过程中唯一需要被外部修改的数值</value>
        protected int lengthLevel;

        /// <value>每个CoitusPart都有不同的lengthPercentage，其数值由xml决定</value>
        protected float lengthPercentage; //大概在0.1左右比较合适

        public TestPawn pawn;


        /// <value>与另一个CoitusPart相关联，它们两个会一起动。
        /// <para>一个例子：器官内部和它的表面</para>
        /// </value>
        public CoitusPart relatedTo;

        public CoitusPart(TestPawn pawn)
        {
            this.pawn = pawn;
        }

        protected static float CalculateBaseLength(TestPawn pawn, float lengthPercentage)
        {
            return pawn.height * lengthPercentage;
        }

        protected static float CalculateBaseDiameter(TestPawn pawn, float lengthPercentage)
        {
            return CalculateBaseLength(pawn, lengthPercentage);
        }

        public void Agere() //启动效果，包括Vagina和Mentula效果
        {
        }
    }
}