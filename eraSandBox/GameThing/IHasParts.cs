using eraSandBox.Coitus;
using eraSandBox.Coitus.Part;

namespace eraSandBox.GameThing;

public interface IHasParts : INeedInitialize
{
    public PartSystem parts { get; }
    public string partsTemplate { get; }
}