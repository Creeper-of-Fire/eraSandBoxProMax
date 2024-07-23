using System.Collections.Generic;
using eraSandBox.Coitus;
using eraSandBox.Pawn;
using eraSandBox.Utility.GameThing;

namespace eraSandBox.World;

/**
 * 所有的地图都是SubWorld，它们之间可以嵌套，并且每个SubWorld都有Cell，某些Cell会是通往其他SubWorld的出入口
 */
public class SubWorld(string id) : IGameObject
{
    public readonly List<Cell> cells = [];
    public string ID = id;

    public SubWorld AddCell(Cell cell)
    {
        this.cells.Add(cell);
        return this;
    }

    public void TakeTurn()
    {
        this.cells.ForEach(cell => cell.TakeTurn());
    }
}

public static class SubWorldUtility
{
    // public static Dictionary<string,>
    public static SubWorld makeTestSubWorld()
    {
        var subWorld = new SubWorld("test");
        var cell = new Cell(subWorld, "test");
        return subWorld.AddCell(cell.AddPawn(new Animal(cell)));
    }
}