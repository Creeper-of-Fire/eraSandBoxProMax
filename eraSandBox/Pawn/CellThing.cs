using System;
using eraSandBox.Coitus;
using eraSandBox.Coitus.Part;
using eraSandBox.GameThing;
using eraSandBox.World;

namespace eraSandBox.Pawn;

/// <summary>
/// 所有的在地图上的东西都是CellThing，包括衣服、地上的物品……等等
/// </summary>
public abstract class CellThing : IGameObject, IInCell, IHasScale, IHasParts
{
    public PartSystem parts { get; }
    public string partsTemplate { get; }
    public int ScaleMillimeter { get; }
    public virtual Cell position { get; }
    public string uuid;

    protected CellThing(Cell position, int scaleMillimeter, string partsTemplate)
    {
        this.ScaleMillimeter = scaleMillimeter;
        this.parts = new PartSystem(this);
        this.position = position;
        this.partsTemplate = partsTemplate;
    }

    public void Initialize()
    {
        this.parts.totalParts = PartsBuilder.MakeParts(this, this.partsTemplate);
        this.parts.Initialize();
        this.parts.UpdateRoutesTotally();
    }
    

    protected virtual void SendMessageToCell()
    {
        foreach (var message in this.parts.MakeMessage())
            this.position.messages.Add(message);
    }

    protected virtual void ReceiveMessageFromCell()
    {
    }

    public virtual void takeTurn()
    {
        AddToTop(this.SendMessageToCell);
        AddToBot(this.ReceiveMessageFromCell);
    }

    protected static void AddToTop(params Action[] actions) =>
        TotalWorldUtility.AddToTop(actions);

    protected static void AddToBot(params Action[] actions) =>
        TotalWorldUtility.AddToBot(actions);

    protected static void AddToMid(params Action[] actions) =>
        TotalWorldUtility.AddToMid(actions);
}