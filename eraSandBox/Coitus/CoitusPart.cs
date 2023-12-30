namespace eraSandBox.Coitus
{
    /// <summary>
    /// CoitusPart是关于Coitus内容的部件，包括人体也包括物品的可使用部分
    /// ///本来设计了diameter和length放在这里，但是太麻烦了，所以它的子类自己实现这两个
    /// </summary>
    public abstract class CoitusPart
    {
        public TestPawn pawn;

        /// <summary>
        /// 每个CoitusPart都有不同的lengthPercentage，其数值由xml决定
        /// </summary>
        protected float lengthPercentage; //大概在0.1左右比较合适

        protected int lengthLevel;
        protected int diameterLevel;



        ///<summary>与另一个CoitusPart相关联，它们两个会一起动。<para>一个例子：器官内部和它的表面</para></summary>
        public CoitusPart relatedTo; 

        public CoitusPart(TestPawn pawn)
        {
            this.pawn = pawn;
        }

        public void Agere() //启动效果，包括Vagina和Mentula效果
        {
        }
    }
    
}