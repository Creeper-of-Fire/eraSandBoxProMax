using System;
using System.Collections.Generic;
using eraSandBox.Pawn;
using eraSandBox.Thought;

namespace eraSandBox.CoitusSimple;

/**
 * WearThing的特点是其可以根据先后顺序来覆盖其下的衣物。由于不想做物理引擎，所以出此下策。
 * 它依附在Animal上
 */
public class WearThing : ThingOnBody
{
    public List<CoverAbilityOfMessage> coverAbility = new();
    public Animal owner;

    public WearThing(int scaleMillimeter, string partsTemplate, Animal owner)
    {
        this.owner = owner;
        this.ScaleMillimeter = scaleMillimeter;
        this.parts = new PartManager(this);
        this.partsTemplate = partsTemplate;
    }


    public void takeTurn()
    {
        throw new NotImplementedException();
    }

    public PartManager parts { get; }
    public string partsTemplate { get; }

    public void Initialize()
    {
        throw new NotImplementedException();
    }

    public int ScaleMillimeter { get; }


    public void processMessage()
    {
    }
}

public class WearThingManager
{
}