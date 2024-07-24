namespace eraSandBox.CoitusSimple;

/**
 * WearThing的特点是其可以根据先后顺序来覆盖其下的衣物。由于不想做物理引擎，所以出此下策。
 * 它依附在Animal上
 */
public class WearThing(PartManager owner, int scaleMillimeter, string partsTemplate)
    : ThingOnBody(owner, scaleMillimeter, partsTemplate)
{
    public override void TakeTurn()
    {
        base.TakeTurn();
    }
}

public class WearThingManager
{
}