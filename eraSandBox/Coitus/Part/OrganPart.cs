using System.Collections.Generic;
using eraSandBox.Coitus.XmlAssign;
using eraSandBox.Pawn;

namespace eraSandBox.Coitus;

public class OrganPart(CellThing owner, string baseName)
{
    [NeedInitialize] public readonly string baseName = baseName;

    public readonly CellThing owner = owner;

    [NeedInitialize] public List<CoitusMentulaAspect> mentulaAspects;

    [NeedInitialize] public List<CoitusVaginaAspect> vaginaAspects;
}