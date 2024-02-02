using eraSandBox.Coitus;
using eraSandBox.Coitus.Part;

namespace eraSandBox;

public class TestPawn
{
    /// <summary> 单位：毫米 </summary>
    public int heightMillimeter;

    public PartSystem parts;
    public string template;

    public TestPawn(int heightMillimeter = 170, string template = "人类")
    {
        this.heightMillimeter = heightMillimeter;
        this.parts = new PartSystem(this);
        this.template = template;
    }
}