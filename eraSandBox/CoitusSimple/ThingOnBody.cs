using System;
using eraSandBox.Thought;
using eraSandBox.Utility;
using eraSandBox.Utility.GameThing;

namespace eraSandBox.CoitusSimple;

/// <summary>
///     应该有多种情况，比如插入、穿戴、融合……
///     融合时，两个部件变为一个大的部件
/// </summary>
public class ThingOnBody : IGameObject, IHasScale, IHasParts
{
    public virtual void TakeTurn()
    {
    }

    public virtual int ScaleMillimeter { get; }

    public virtual void Initialize()
    {
    }

    public readonly PartManager owner;

    /// <summary>
    ///     应该有多种情况，比如插入、穿戴、融合……
    ///     融合时，两个部件变为一个大的部件
    /// </summary>
    public ThingOnBody(PartManager owner, int scaleMillimeter, string partsTemplate)
    {
        this.ScaleMillimeter = scaleMillimeter;
        this.owner = owner;
        this.parts = new PartManager(this);
        this.partsTemplate = partsTemplate;
    }

    public PartManager parts { get; }
    public virtual string partsTemplate { get; }

    public IEnumerable<MessageSpreader> MakeMessageSpreader()
    {
        throw new NotImplementedException();
    }

    public static View ProcessView(List<ThingOnBody> list, View view)
    {
        throw new NotImplementedException();
    }
}