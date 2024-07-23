using System;
using System.Collections.Generic;
using eraSandBox.Coitus.XmlAssign;
using eraSandBox.Pawn;
using eraSandBox.Thought;
using eraSandBox.Utility;
using eraSandBox.Utility.GameThing;

namespace eraSandBox.CoitusSimple;

public class PartManager(IHasParts owner) : INeedInitialize
{
    public readonly IHasParts owner = owner;
    private List<ThingOnBody> allThings { get; } = [];
    private List<MessageSpreader> messageSpreaders { get; set; } = [];

    public void Initialize()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<MessageSpreader> MakeMessageSpreader()
    {
        this.messageSpreaders = this.allThings.SelectMany(wear => wear.MakeMessageSpreader()).ToList();
        return this.messageSpreaders;
    }

    public View ReceiveMessageFromCell(Message message)
    {
    }

    public View ProcessView(View view) =>
        ThingOnBody.ProcessView(this.allThings, view);

    public void Render()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<MessageSpreader> CanSeeMessage(Animal others)
    {
        if (this.owner == others)
            return this.messageSpreaders;
        return this.messageSpreaders.Where(messageSpreader => others.ContainMemory(messageSpreader.ID));
    }
}

public static class MessageData
{
    private static Dictionary<string, string> IDToName;

    public static string GetName(string id) =>
        IDToName.GetValueOrDefault(id, string.Empty);

    private static Dictionary<string, List<MessageTag>> IDToTag = new()
    {
        { "T-shit", [new MessageTag("衣服")] }
    };

    /// <summary>
    /// 只是给出了初始的Tag，之后还可以添加其他的Tag
    /// </summary>
    public static List<MessageTag> GetStartMessageTag(string id) =>
        IDToTag.GetValueOrDefault(id, []);

    private static Dictionary<string, string> IDToDescription = new()
    {
        { "T-shit", "衬衫" }
    };

    public static string GetDescription(string id) =>
        IDToDescription.GetValueOrDefault(id, string.Empty);
}