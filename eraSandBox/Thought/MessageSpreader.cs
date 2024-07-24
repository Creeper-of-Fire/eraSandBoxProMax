using eraSandBox.Pawn;

namespace eraSandBox.Thought;

/// <summary>
/// 对于声音来说，其理论上无限传播，实际上其的weight每传播一格就会被衰减至之前的1/ATTENUATION)，若weight为LOWEST_WEIGHT，则不会继续传播。
/// <para>声音的权重会在游戏中体现为“音量”</para>
/// </summary>
public class VoiceMessageSpreader(CellThing sender, string id, float weight = MessageSpreader.DEFAULT_WEIGHT)
    : MessageSpreader(sender, id, weight)
{
    private const int ATTENUATION = 32; //衰减
    private const int LOWEST_WEIGHT = 1; //最小可听见的weight

    public override void Spread()
    {
        int maxDepth = 0;
        float currentWeight = this.startWeight;
        var weights = new List<float>();
        // 使用循环来计算最大深度
        while (currentWeight >= LOWEST_WEIGHT)
        {
            currentWeight /= ATTENUATION;
            maxDepth++;
            weights.Add(currentWeight);
        }

        this.senderCell.ForNeighbors(
            (cell, depth) =>
            {
                if (depth < 0 || depth >= weights.Count) return;
                float weight = weights[depth]; // 直接从预计算列表中获取权重
                if (weight >= LOWEST_WEIGHT)
                    cell.messages.Add(this.MakeNewMessage(cell, weight));
            },
            maxDepth);
    }
}

/// <summary>
/// 对于气味，其比起声音更不容易传播，所以ATTENUATION更大。其的weight每传播一格就会被衰减至之前的1/ATTENUATION，若weight为LOWEST_WEIGHT，则不会继续传播。
/// </summary>
public class SmellMessageSpreader(CellThing sender, string id, float weight = MessageSpreader.DEFAULT_WEIGHT)
    : MessageSpreader(sender, id, weight)
{
    private const int ATTENUATION = 128; //衰减
    private const int LOWEST_WEIGHT = 1; //最小可闻到的weight

    public override void Spread()
    {
        int maxDepth = 0;
        float currentWeight = this.startWeight;
        var weights = new List<float>();
        // 使用循环来计算最大深度
        while (currentWeight >= LOWEST_WEIGHT)
        {
            currentWeight /= ATTENUATION;
            maxDepth++;
            weights.Add(currentWeight);
        }

        this.senderCell.ForNeighbors(
            (cell, depth) =>
            {
                if (depth < 0 || depth >= weights.Count) return;
                float weight = weights[depth]; // 直接从预计算列表中获取权重
                if (weight >= LOWEST_WEIGHT)
                    cell.messages.Add(this.MakeNewMessage(cell, weight));
            },
            maxDepth);
    }
}