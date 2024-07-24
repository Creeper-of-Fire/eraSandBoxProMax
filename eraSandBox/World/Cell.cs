using eraSandBox.Pawn;
using eraSandBox.Thought;
using eraSandBox.Utility.GameThing;

namespace eraSandBox.World;

/**
 * 构成世界的最基本单元
 */
public class Cell(SubWorld owner, string id, SubWorld? canMoveTo = null) : IGameObject
{
    public SubWorld owner = owner;
    public string ID = id;
    public SubWorld? canMoveTo = canMoveTo;

    public List<Message> messages { get; } = [];
    public List<Cell> neighbors { get; } = [];
    public List<CellThing> pawnsInThisCell { get; } = [];

    public Cell AddPawn(CellThing cellThing)
    {
        this.pawnsInThisCell.Add(cellThing);
        return this;
    }

    public void TakeTurn()
    {
        this.pawnsInThisCell.ForEach(pawn => pawn.SendMessageToCell());
        this.pawnsInThisCell.ForEach(pawn => pawn.ReceiveMessageFromCell());
        this.pawnsInThisCell.ForEach(pawn => pawn.TakeTurn());
    }

    /// <summary>
    /// 获取Cell的邻居，并且执行动作
    /// </summary>
    /// <param name="action">动作</param>
    /// <param name="maxDepth">深度，深度小于1时只会包含自己</param>
    public void ForNeighbors(Action<Cell, int> action, int maxDepth = 1)
    {
        switch (maxDepth)
        {
            case <= 1:
                action(this, 1);
                return;
            case 2:
                action(this, 1);
                this.neighbors.ForEach(cell => action(cell, 2));
                return;
            default:
                var queue = new Queue<Cell>();
                var visited = new HashSet<Cell>();

                queue.Enqueue(this);
                visited.Add(this);
                int depth = maxDepth;
                while (queue.Count > 0 && depth > 0)
                {
                    var currentCell = queue.Dequeue();
                    action(currentCell, depth);

                    foreach (var neighbor in currentCell.neighbors.Where(neighbor => !visited.Contains(neighbor)))
                    {
                        queue.Enqueue(neighbor);
                        visited.Add(neighbor);
                    }

                    depth--;
                }

                return;
        }
    }
}