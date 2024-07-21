using System.Collections.Generic;
using eraSandBox.GameThing;
using eraSandBox.Pawn;
using eraSandBox.Thought;

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

    public void takeTurn()
    {
        this.pawnsInThisCell.ForEach(pawn => pawn.takeTurn());
    }
}