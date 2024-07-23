using System.Collections.Generic;
using eraSandBox.Coitus;
using eraSandBox.Utility.GameThing;

namespace eraSandBox.World;

/**
 * 所有的地图都是SubWorld，它们之间可以嵌套，并且每个SubWorld都有Cell，某些Cell会是通往其他SubWorld的出入口
 */
public class SubWorld : IGameObject
{
    public List<Cell> cells = new();
    public SubWorldDef def;

    public void takeTurn()
    {
        this.cells.ForEach(cell => cell.takeTurn());
    }
}

public class SubWorldDef : Def
{
}