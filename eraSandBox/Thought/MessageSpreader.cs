using System;
using eraSandBox.Pawn;

namespace eraSandBox.Thought;

// /// <summary>
// /// MessageSpreader负责生成Message，并且传递它们
// /// </summary>
// public abstract class MessageSpreader<T>
//     where T : Message
// {
//     public abstract List<T> makeMessage(Cell centerCell, CellThing cellThing);
// }

/// <summary>
///     对于声音来说，其理论上无限传播，实际上其weight每传播一格就会被衰减至1/128，若weight为1.0f，则不会继续传播
/// </summary>
/// <param name="sender"></param>
/// <param name="id"></param>
/// <param name="weight"></param>
public class VoiceMessageSpreader(CellThing sender, string id, float weight = MessageSpreader.DEFAULT_WEIGHT)
    : MessageSpreader(sender, id, weight)
{
    public override void Spread()
    {
        var maxDepth = (int)Math.Log(this.startWeight, 128);

        this.senderCell.ForNeighbors(
            (cell, depth) =>
            {
                var weight = (float)(this.startWeight / Math.Pow(128, depth));
                if (weight >= 1.0f)
                    cell.messages.Add(this.MakeNewMessage(cell, weight));
            },
            maxDepth);
    }
}

/// <summary>
/// 
/// </summary>
/// <param name="sender"></param>
/// <param name="id"></param>
/// <param name="weight"></param>
public class SmellMessageSpreader(CellThing sender, string id, float weight = MessageSpreader.DEFAULT_WEIGHT)
    : MessageSpreader(sender, id, weight)
{
    public override void Spread()
    {
        throw new NotImplementedException();
    }
}