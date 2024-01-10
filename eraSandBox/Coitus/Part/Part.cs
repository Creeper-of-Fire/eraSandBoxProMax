namespace eraSandBox.Coitus
{
    public class Part
    {
        [NeedInitialize]
        public readonly string baseName;

        public readonly TestPawn owner;

        [NeedInitialize]
        public CoitusMentulaAspect mentulaAspect;

        [NeedInitialize]
        public CoitusVaginaAspect vaginaAspect;

        public Part(TestPawn owner, string baseName)
        {
            this.owner = owner;
            this.baseName = baseName;
        }
    }
}