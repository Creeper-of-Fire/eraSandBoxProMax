using eraSandBox.CoitusSimple;
using eraSandBox.Utility.GameThing;
using eraSandBox.World;

// using eraSandBox.Coitus.Part;

namespace eraSandBox.Pawn;

/// <summary>
///     所有的在地图上的东西都是CellThing，包括衣服、地上的物品……等等
/// </summary>
public abstract class CellThing : IGameObject, IInCell, IHasScale, IHasParts
{
    public string uuid = string.Empty;

    protected CellThing(Cell position, int scaleMillimeter, string partsTemplate)
    {
        this.ScaleMillimeter = scaleMillimeter;
        this.parts = new PartManager(this);
        this.position = position;
        this.partsTemplate = partsTemplate;
    }

    public virtual void TakeTurn()
    {
        // AddToTop(this.SendMessageToCell);
        // AddToBot(this.ReceiveMessageFromCell);
    }

    public PartManager parts { get; }
    public string partsTemplate { get; }

    public void Initialize()
    {
        this.parts.Initialize();
    }

    public int ScaleMillimeter { get; }
    public virtual Cell position { get; }


    public virtual void SendMessageToCell()
    {
        foreach (var message in this.parts.MakeMessageSpreader())
            message.Spread();
    }

    public virtual void ReceiveMessageFromCell()
    {
    }

    protected static void AddToTop(params Action[] actions)
    {
        TotalWorld.TotalWorldUtility.AddToTop(actions);
    }

    protected static void AddToBot(params Action[] actions)
    {
        TotalWorld.TotalWorldUtility.AddToBot(actions);
    }

    protected static void AddToMid(params Action[] actions)
    {
        TotalWorld.TotalWorldUtility.AddToMid(actions);
    }
}