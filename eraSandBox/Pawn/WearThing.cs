using System;
using System.Collections.Generic;
using eraSandBox.Coitus.Part;
using eraSandBox.GameThing;
using eraSandBox.Thought;

namespace eraSandBox.Pawn;

/**
 * WearThing的特点是其可以根据先后顺序来覆盖其下的衣物。由于不想做物理引擎，所以出此下策。
 * 它依附在Animal上
 */
public class WearThing : IGameObject, IHasScale, IHasParts
{
    public Animal owner;
    public List<CoverAbilityOfMessage> coverAbility = new();
    public List<OrganPart> coverParts = new();

    public WearThing(int scaleMillimeter, string partsTemplate, Animal owner)
    {
        this.owner = owner;
        this.ScaleMillimeter = scaleMillimeter;
        this.parts = new PartSystem(this);
        this.partsTemplate = partsTemplate;
    }


    public void processMessage()
    {
        
    }


    public void takeTurn()
    {
        throw new NotImplementedException();
    }

    public int ScaleMillimeter { get; }
    public PartSystem parts { get; }
    public string partsTemplate { get; }

    public void Initialize() =>
        throw new NotImplementedException();
}

public class WearThingManager
{
}