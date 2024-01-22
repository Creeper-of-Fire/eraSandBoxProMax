using System.Collections.Generic;
using System.Linq;

namespace eraSandBox.Coitus
{
    public class PartTracker : INeedInitialize
    {
        public Dictionary<string, CoitusVaginaAspect> totalVaginaAspects;
        public Dictionary<string, Part> totalParts;
        public readonly CoitusVaginaPartSystem coitusVaginaSystem;
        public TestPawn owner;

        public PartTracker(TestPawn owner)
        {
            this.coitusVaginaSystem = new CoitusVaginaPartSystem(this);
            this.owner = owner;
        }
        public void Initialize()
        {
            this.totalVaginaAspects = this.totalParts
                .Where(pair => pair.Value.vaginaAspect != null)
                .ToDictionary(pair => pair.Key, pair => pair.Value.vaginaAspect);
        }
    }
}