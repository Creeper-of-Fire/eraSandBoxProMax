using System.Collections.Generic;
using System.Linq;
using eraSandBox.Pawn;
using eraSandBox.World;

namespace eraSandBox.Thought;

/// <summary>
///     Message会转化为View，Pawn会对外发射不同种类的Message
/// </summary>
public class Message(
    MessageSpreader messageSpreader,
    Cell cell,
    float weight
)
{
    // public MessageSpreader messageSpreader => this.messageSpreader;

    /// <summary>
    /// 发送者
    /// </summary>
    public readonly CellThing sender = messageSpreader.sender;

    /// <summary>
    /// 权重，默认值为100.0f
    /// </summary>
    public readonly float weight = weight;

    /// <summary>
    /// 所在的Cell
    /// </summary>
    public Cell cell = cell;

    /// <summary>
    ///     Message有不同的Tag
    /// </summary>
    public List<MessageTag> messageTags => this.messageSpreader.messageTags;

    public string name => this.messageSpreader.name;
    private readonly MessageSpreader messageSpreader = messageSpreader;

    // public Message MakeCopy()
    // {
    //     return new Message(this.messageSpreader, this.cell, this.weight);
    // }
}

public abstract class MessageSpreader(
    CellThing sender,
    string id,
    float startWeight = MessageSpreader.DEFAULT_WEIGHT)
{
    protected Message MakeNewMessage(Cell cell, float weight)
    {
        return new Message(this, cell, weight);
    }

    /// <summary>
    /// 发送者
    /// </summary>
    public readonly CellThing sender = sender;

    /// <summary>
    /// 名字
    /// </summary>
    public readonly string name;

    /// <summary>
    /// 所在的Cell
    /// </summary>
    public Cell senderCell => this.sender.position;

    protected const float DEFAULT_WEIGHT = 100.0f;

    /// <summary>
    /// 权重，默认值为100.0f
    /// </summary>
    public readonly float startWeight = startWeight;

    /// <summary>
    ///     Message有不同的Tag
    /// </summary>
    public List<MessageTag> messageTags { get; }

    /// <summary>
    /// ID
    /// </summary>
    public readonly string ID = id;

    public abstract void Spread();
}

//TODO 字符串比较也许可以优化一下，不过如果性能足够就算了。
public readonly struct MessageTag(string tagId)
{
    public override bool Equals(object obj)
    {
        return obj is MessageTag other && this.TagID.Equals(other.TagID);
    }

    public override int GetHashCode()
    {
        return this.TagID != null ? this.TagID.GetHashCode() : 0;
    }

    private readonly string TagID = tagId;

    public static bool operator ==(MessageTag tag1, MessageTag tag2)
    {
        return tag1.TagID == tag2.TagID;
    }

    public static bool operator !=(MessageTag tag1, MessageTag tag2)
    {
        return !(tag1 == tag2);
    }
}

/// <summary>
/// </summary>
/// <param name="tagId">会进行处理的tag</param>
/// <param name="add">先计算</param>
/// <param name="mul">后计算</param>
public struct InterestOfMessage(string tagId, float add = 0.0f, float mul = 1.0f)
{
    public float add = add;
    public float mul = mul;
    public MessageTag messageTag = new(tagId);

    /// <summary>
    ///     处理View
    /// </summary>
    /// <param name="interestList">请输入自身的所有interests</param>
    /// <param name="view">原来的View</param>
    /// <returns>返回的是原来的View而非复制</returns>
    /// TODO 如果遇到性能问题，可以考虑用矩阵来处理
    public static View ProcessView(IEnumerable<InterestOfMessage> interestList, View view)
    {
        var needProcess = interestList.Where(message => view.messageTags.Contains(message.messageTag)).ToList();
        foreach (var interestOfMessage in needProcess)
            view.weight += interestOfMessage.add;
        foreach (var interestOfMessage in needProcess)
            view.weight *= interestOfMessage.mul;
        return view;
    }
}

/// <summary>
/// </summary>
/// <param name="tagId">会进行处理的tag</param>
/// <param name="sub">先计算</param>
/// <param name="mul">后计算</param>
public struct CoverAbilityOfMessage(string tagId, float sub = 0.0f, float mul = 1.0f)
{
    public float sub = sub;
    public float mul = mul;
    public MessageTag messageTag = new(tagId);
}

/// <summary>
///     View会转化为Memory，Human会汲取所在Cell中的所有Message，并且根据其观察力生成View
/// </summary>
public struct View(Message message)
{
    public const float CAN_VIEW_WEIGHT = 10.0f;

    public const float CAN_KNOW_OWNER_WEIGHT = 80.0f;

    private readonly Message message = message;
    public float weight = message.weight;
    public List<MessageTag> messageTags => this.message.messageTags;
}

/// <summary>
///     Memory会被遗忘，不同的
/// </summary>
public class Memory
{
}