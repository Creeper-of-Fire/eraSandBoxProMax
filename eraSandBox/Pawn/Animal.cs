using eraSandBox.Thought;
using eraSandBox.World;

namespace eraSandBox.Pawn;

/**
 * Animal是所有具有意识，并且可以主动行动的事物的基类
 */
public class Animal(Cell position, int scaleMillimeter = 1700, string partsTemplate = "人类")
    : CellThing(position, scaleMillimeter, partsTemplate)
{
    protected List<InterestOfMessage> interests { get; } = [];
    protected List<Memory> memories { get; } = [];
    protected List<View> views { get; } = [];

    public override void ReceiveMessageFromCell()
    {
        this.views.Clear();
        base.ReceiveMessageFromCell();
        var tempViews = this.position.messages.Select(this.ViewMessage);
        foreach (var tempView in tempViews)
        {
            if (tempView.CanBeMemory())
                this.memories.Add(new Memory(tempView));
            if (tempView.CanBeView())
                this.views.Add(tempView);
        }
    }

    public bool ContainMemory(string ID)
    {
        return this.memories.Any(memory => memory.ID == ID);
    }

    public bool ContainView(string ID)
    {
        return this.views.Any(view => view.message.ID == ID);
    }

    protected virtual View ViewMessage(Message message)
    {
        return InterestOfMessage.ProcessView(this.interests, this.parts.ReceiveMessageFromCell(message));
    }
}