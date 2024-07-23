using System.Collections.Generic;
using eraSandBox.Coitus.XmlAssign;
using eraSandBox.Pawn;

namespace eraSandBox.Coitus;

/// <summary> CoitusPart是关于Coitus内容的部件，包括人体也包括物品的可使用部分 </summary>
[NeedDefInitialize]
public abstract class CoitusAspect(OrganPart owner) : INeedInitialize, ILinkTo<CoitusAspect>
{
    public const int TenThousand = 10000;

    /// <value> 与一个Part相关联 </value>
    [NeedInitialize] public readonly OrganPart owner = owner;

    [NeedDef]
    // ReSharper disable once UnassignedField.Global
    public CoitusAspectDef def;

    [NeedDefInitialize] public CoitusScaleLinear diameter;

    /// <value> <see cref="lengthLevel" /> 和 <see cref="diameterLevel" /> 是整个运行过程中唯一需要被外部修改数值 </value>
    [NeedDefInitialize] protected int diameterLevel;

    [NeedDefInitialize] public CoitusScaleLinear length;

    /// <value> <see cref="lengthLevel" /> 和 <see cref="diameterLevel" /> 是整个运行过程中唯一需要被外部修改的数值 </value>
    [NeedDefInitialize] protected int lengthLevel;

    public List<LinkPoint<CoitusAspect>> linkTo { get; } = [];

    public string baseName =>
        this.def.defName;

    public virtual void Initialize()
    {
        this.diameterLevel = this.def.diameterLevel;
        this.lengthLevel = this.def.lengthLevel;
        this.length =
            new CoitusScaleLinear(CalculateBaseLength(this.owner.owner, this.def.lengthTenThousandth),
                this.lengthLevel, this);
        this.diameter =
            new CoitusScaleLinear(CalculateBaseDiameter(this.owner.owner, this.def.lengthTenThousandth),
                this.diameterLevel, this);
    }

    protected static int CalculateBaseLength(CellThing cellThing, int lengthTenThousandth)
    {
        return cellThing.ScaleMillimeter * lengthTenThousandth / TenThousand;
    }

    protected static int CalculateBaseDiameter(CellThing cellThing, int lengthTenThousandth)
    {
        return CalculateBaseLength(cellThing, lengthTenThousandth);
    }

    public void Agere() //启动效果，包括Vagina和Mentula效果
    {
    }
}