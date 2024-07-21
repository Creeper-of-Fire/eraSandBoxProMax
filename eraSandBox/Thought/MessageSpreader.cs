using System;
using System.Collections.Generic;
using eraSandBox.Coitus.Part;
using eraSandBox.Pawn;
using eraSandBox.World;

namespace eraSandBox.Thought;

/// <summary>
/// MessageSpreader负责生成Message，并且传递它们
/// </summary>
public abstract class MessageSpreader<T>
    where T : Message
{
    public abstract List<T> makeMessage(Cell centerCell, CellThing cellThing);
}

public class VoiceMessageSpreader : MessageSpreader<VoiceMessageSpreader.VoiceMessage>
{
    public static VoiceMessageSpreader Instance { get; } = new();

    public override List<VoiceMessage> makeMessage(Cell centerCell, CellThing cellThing)
    {
    };

    public class VoiceMessage(Cell cell, CellThing sender, float weight = Message.DEFAULT_WEIGHT)
        : Message(cell, sender, weight)
    {
        public override float getCoverRate(OrganPart organPart) =>
            throw new NotImplementedException();
    }
}

public class SmellMessageSpreader : MessageSpreader<SmellMessageSpreader.SmellMessage>
{
    public static SmellMessageSpreader Instance { get; } = new();

    public override List<SmellMessage> makeMessage(Cell centerCell, CellThing cellThing)
    {
        
    };

    public class SmellMessage(Cell cell, CellThing sender, float weight = Message.DEFAULT_WEIGHT)
        : Message(cell, sender, weight)
    {
        public override float getCoverRate(OrganPart organPart) =>
            throw new NotImplementedException();
    }
}