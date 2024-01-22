using eraSandBox.Coitus;

namespace eraSandBox
{
    public class TestPawn
    {
        /// <summary> 单位：毫米 </summary>
        public int heightMillimeter;

        public PartTracker parts;
        public string template;

        public TestPawn(int heightMillimeter = 170, string template = "人类")
        {
            this.heightMillimeter = heightMillimeter;
            this.parts = new PartTracker(this);
            this.template = template;
        }
    }
}