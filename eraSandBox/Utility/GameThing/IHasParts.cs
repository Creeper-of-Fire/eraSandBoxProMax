using eraSandBox.Coitus.XmlAssign;
using eraSandBox.CoitusSimple;

namespace eraSandBox.Utility.GameThing;

public interface IHasParts : INeedInitialize
{
    public PartManager parts { get; }
    public string partsTemplate { get; }
}