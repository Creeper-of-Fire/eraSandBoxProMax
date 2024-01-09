using System.Collections.Generic;

namespace eraSandBox.Coitus
{
    public class PartTracker
    {
        public CoitusVaginaPartSystem coitusVaginaSystem = new CoitusVaginaPartSystem();
        public List<Part> allPart = new List<Part>();
        public TestPawn owner;

        public PartTracker(TestPawn owner)
        {
            this.owner = owner;
        }
    }
}