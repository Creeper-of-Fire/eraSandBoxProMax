using System;
using System.Collections.Generic;
using System.Linq;
using eraSandBox.Pawn;
using eraSandBox.Thought;
using eraSandBox.Utility.GameThing;

namespace eraSandBox.World;

/**
 * 构成世界的最基本单元
 */
public class Cell : IGameObject
{
    public SubWorld canMoveTo;
    public SubWorld owner;
    public List<CellThing> pawnsInThisCell { get; } = new();
    public List<Message> messages { get; } = new();
    private List<Cell> neighbors { get; } = new();

    public void takeTurn()
    {
        this.pawnsInThisCell.ForEach(pawn => pawn.takeTurn());
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
                var depth = maxDepth;
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