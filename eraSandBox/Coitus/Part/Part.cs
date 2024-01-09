using eraSandBox.Coitus;

namespace eraSandBox.Coitus
{
    [NeedDefInitialize]
    public class Part
    {
        [NeedInitialize]
        public readonly string baseName;
        [NeedDefInitialize]
        public CoitusMentulaAspect mentulaAspect;
        [NeedDefInitialize]
        public CoitusVaginaAspect vaginaAspect;

        public TestPawn owner;

        public Part(CoitusMentulaAspect mentulaAspect, CoitusVaginaAspect vaginaAspect)
        {
            this.mentulaAspect = mentulaAspect;
            this.vaginaAspect = vaginaAspect;
        }
    }
}