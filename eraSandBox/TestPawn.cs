using eraSandBox.Coitus;

namespace eraSandBox
{
    public class TestPawn
    {
        /// <summary> 单位：毫米 </summary>
        public int heightMillimeter;

        public PartTracker parts;
        public string species;

        public TestPawn(int heightMillimeter = 170, string species = "人类")
        {
            this.heightMillimeter = heightMillimeter;
            this.species = species;
        }
    }
}