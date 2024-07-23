using System.Collections.Generic;

namespace eraSandBox.Coitus;

public class CoitusMentulaSystem(PartSystem owner)
{
    private Dictionary<string, CoitusMentulaAspect> TotalAspects =>
        owner.TotalMentulaAspects;
}