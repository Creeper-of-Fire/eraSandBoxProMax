using eraSandBox.Pawn;
using eraSandBox.Utility;
using eraSandBox.World;

namespace eraSandBox.Thought;

/// <summary>
///     Message会转化为View，Pawn会对外发射不同种类的Message
/// </summary>
public readonly struct Message(
    MessageSpreader messageSpreader,
    Cell cell,
    float weight
)
{
    /// <summary>
    /// 所在的Cell
    /// </summary>
    public readonly Cell cell = cell;

    /// <summary>
    /// 固定的MessageSpreader
    /// </summary>
    private readonly MessageSpreader messageSpreader = messageSpreader;

    /// <summary>
    /// 权重，默认值为100.0f
    /// </summary>
    public readonly float weight = weight;

    /// <summary>
    /// 发送者
    /// </summary>
    public CellThing sender => this.messageSpreader.sender;

    /// <summary>
    /// Message有不同的Tag
    /// </summary>
    public List<MessageTag> messageTags => this.messageSpreader.messageTags;

    public string ID => this.messageSpreader.ID;
}

public abstract class MessageSpreader(
    CellThing sender,
    string id,
    float startWeight = MessageSpreader.DEFAULT_WEIGHT,
    params MessageTag[] messageTags)
{
    protected Message MakeNewMessage(Cell cell, float weight)
    {
        return new Message(this, cell, weight);
    }

    protected const float DEFAULT_WEIGHT = 100.0f;

    public readonly string ID = id;

    /// <summary>
    /// 发送者
    /// </summary>
    public readonly CellThing sender = sender;

    /// <summary>
    /// 权重，默认值为100.0f
    /// </summary>
    protected readonly float startWeight = startWeight;

    /// <summary>
    /// 发出者所在的Cell
    /// </summary>
    protected Cell senderCell => this.sender.position;

    /// <summary>
    /// Message有不同的Tag
    /// </summary>
    public readonly List<MessageTag> messageTags =
        new List<MessageTag>().AddAll(messageTags);

    /// <summary>
    /// 传播函数，调用本函数才能添加message到Cell
    /// </summary>
    public abstract void Spread();
}