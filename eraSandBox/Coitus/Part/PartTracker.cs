using System.Collections.Generic;

namespace eraSandBox.Coitus
{
    public class PartTracker
    {
        public List<Part> allPart = new List<Part>();
        public CoitusVaginaPartSystem coitusVaginaSystem = new CoitusVaginaPartSystem();
        public TestPawn owner;

        public PartTracker(TestPawn owner)
        {
            this.owner = owner;
        }
    }
}