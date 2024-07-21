using System.Collections.Generic;
using eraSandBox.Coitus;
using eraSandBox.Coitus.Part;
using eraSandBox.Thought;
using eraSandBox.World;

namespace eraSandBox.Pawn;

/**
 * Animal是所有具有意识，并且可以主动行动的事物的基类
 */
[NeedDefInitialize]
public class Animal(Cell position, int scaleMillimeter = 1700, string partsTemplate = "人类")
    : CellThing(position, scaleMillimeter, partsTemplate)
{
    public List<InterestOfMessage> interests { get; } = new();
    public List<WearThing> wearThings { get; } = new();

    public class AnimalDef : Def
    {
        
    }

    public override void takeTurn()
    {
        base.takeTurn();
    }

    protected override void ReceiveMessageFromCell()
    {
        base.ReceiveMessageFromCell();
        this.position.messages
    }

    protected virtual void MessageToView(Message)
    {
    }
}