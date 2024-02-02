using System.Collections;
using System.Collections.Generic;

namespace eraSandBox.Coitus.Part;

public class OrganPart(TestPawn owner, string baseName)
{
    [NeedInitialize]
    public readonly string baseName = baseName;

    public readonly TestPawn owner = owner;

    [NeedInitialize]
    public IList<CoitusMentulaAspect> mentulaAspects;

    [NeedInitialize]
    public IList<CoitusVaginaAspect> vaginaAspects;
}