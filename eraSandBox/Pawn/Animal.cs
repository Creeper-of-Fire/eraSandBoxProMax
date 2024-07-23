using System.Collections.Generic;
using System.Linq;
using eraSandBox.Coitus;
using eraSandBox.Coitus.XmlAssign;
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
    private List<InterestOfMessage> interests { get; } = new();

    public override void takeTurn()
    {
        base.takeTurn();
    }

    protected override void ReceiveMessageFromCell()
    {
        base.ReceiveMessageFromCell();
        var tempView = this.position.messages.Select(this.ViewMessage);
    }

    protected virtual View ViewMessage<T>(T message) where T : Message
    {
        return InterestOfMessage.ProcessView(this.interests, this.parts.ProcessMessage(message));
    }

    public class AnimalDef : Def
    {
    }
}