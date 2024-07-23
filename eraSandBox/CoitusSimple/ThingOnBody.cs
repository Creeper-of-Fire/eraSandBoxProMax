using System;
using eraSandBox.Utility.GameThing;

namespace eraSandBox.CoitusSimple;

/// <summary>
///     应该有多种情况，比如插入、穿戴、融合……
///     融合时，两个部件变为一个大的部件
/// </summary>
public class ThingOnBody : IGameObject, IHasScale, IHasParts
{
    public virtual void takeTurn()
    {
        throw new NotImplementedException();
    }

    public virtual int ScaleMillimeter { get; }

    public virtual void Initialize()
    {
        throw new NotImplementedException();
    }

    public virtual PartManager parts { get; }
    public virtual string partsTemplate { get; }
}