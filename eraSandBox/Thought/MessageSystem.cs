using System.Collections.Generic;
using eraSandBox.Coitus.Part;
using eraSandBox.Pawn;
using eraSandBox.World;

namespace eraSandBox.Thought;

/// <summary>
/// Message会转化为View，Pawn会对外发射不同种类的Message
/// </summary>
public abstract class Message
{
    public const float DEFAULT_WEIGHT = 100.0f;

    /// <summary>
    /// 发送者
    /// </summary>
    public CellThing sender;

    /// <summary>
    /// 所在的Cell
    /// </summary>
    public Cell cell;

    /// <summary>
    /// 权重，默认值为100.0f
    /// </summary>
    public float weight;

    /// <summary>
    /// Message有不同的Tag
    /// </summary>
    public List<MessageTag> messageTags;

    public string ID;
    public string name;

    public List<OrganPart> containedParts;

    protected Message(Cell cell, CellThing sender, string id, float weight = DEFAULT_WEIGHT)
    {
        this.sender = sender;
        this.ID = id;
        this.cell = cell;
        this.weight = weight;
    }

    // /// <summary>
    // /// 
    // /// </summary>
    // /// <param name="otherMessage">另一个消息</param>
    // /// <returns>覆盖的百分比</returns>
    // public float PartCovered(Message otherMessage)
    // {
    //     var coverRate = 0.0f;
    //     this.containedParts.ForEach(part =>
    //     {
    //         if (otherMessage.containedParts.Contains(part))
    //         {
    //         }
    //     });
    //     //第一步，确认是否有重复
    //     //第二步，
    // }

    public abstract float getCoverRate(OrganPart organPart);

    public float getCoverRate(OrganPart, Message otherMessage)
    {
    }
}

//TODO 字符串比较也许可以优化一下，不过如果性能足够就算了。
public readonly struct MessageTag(string tagId)
{
    public override bool Equals(object obj) =>
        obj is MessageTag other && this.TagID.Equals(other.TagID);

    public override int GetHashCode() =>
        this.TagID != null ? this.TagID.GetHashCode() : 0;

    private readonly string TagID = tagId;

    public static bool operator ==(MessageTag tag1, MessageTag tag2) =>
        tag1.TagID == tag2.TagID;

    public static bool operator !=(MessageTag tag1, MessageTag tag2) =>
        !(tag1 == tag2);
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
/// View会转化为Memory，Human会汲取所在Cell中的所有Message，并且根据其观察力生成View
/// </summary>
public class View<T>(T message)
    where T : Message
{
    public const float CAN_VIEW_RATE = 10.0f;
    public T message = message;
}

/// <summary>
/// Memory会被遗忘，不同的
/// </summary>
public class Memory
{
}