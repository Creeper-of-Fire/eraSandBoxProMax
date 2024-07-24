using eraSandBox.Pawn;

namespace eraSandBox.Thought;

//TODO 字符串比较也许可以优化一下，不过如果性能足够就算了。
public readonly struct MessageTag(string tagId)
{
    public override bool Equals(object? obj)
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
/// <param name="mul">乘算 (不进行加算，因为很显然兴趣只能乘)</param>
public struct InterestOfMessage(
    string tagId,
    // float add =1.0f, 
    float mul = 1.0f)
{
    // public float add = add;
    public float mul = mul;
    public MessageTag messageTag = new(tagId);

    /// <summary>
    ///     处理View
    /// </summary>
    /// <param name="interestList">请输入自身的所有interests</param>
    /// <param name="view">原来的View</param>
    /// <returns>返回的是原来的View而非复制</returns>
    /// TODO 如果遇到性能问题，可以考虑用矩阵来处理
    public static View ProcessView(List<InterestOfMessage> interestList, View view)
    {
        // foreach (var interestOfMessage in needProcess)
        //     view.weight += interestOfMessage.add;
        foreach (var interestOfMessage in interestList.Where(message => view.messageTags.Contains(message.messageTag)))
            view.weight *= interestOfMessage.mul;
        return view;
    }
}

/// <summary>
/// View会转化为Memory，Human会汲取所在Cell中的所有Message，并且根据其观察力生成View
/// </summary>
public class View(Message message)
{
    /// <summary>
    /// 达到这个值时，会被观察，但是不知道发出者
    /// </summary>
    public const float CAN_VIEW_WEIGHT = 10.0f;

    /// <summary>
    /// 超过这个值后，会被记忆
    /// </summary>
    public const float CAN_KNOW_OWNER_WEIGHT = 80.0f;

    public readonly Message message = message;
    public float weight = message.weight;
    public List<MessageTag> messageTags => this.message.messageTags;

    public bool CanBeMemory()
    {
        return this.weight >= CAN_KNOW_OWNER_WEIGHT;
    }

    public bool CanBeView()
    {
        return this.weight >= CAN_VIEW_WEIGHT;
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
/// Memory会被遗忘(WIP)，其保存的只有message/messageSpreader的id，其目的为，这个id的message被产生以后，下次这个id的message就会直接被读取。
/// </summary>
public readonly struct Memory(View view)
{
    public readonly string ID = view.message.ID;
    public readonly CellThing sender = view.message.sender;
}