using eraSandBox.Thought;

namespace eraSandBox.Utility.GameThing;

public interface ICanMakeMessage
{
    public IEnumerable<MessageSpreader> MakeMessageSpreader();
    public View ProcessView(View view);
}