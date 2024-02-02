using System;
using System.Collections.Generic;
using System.Linq;

namespace eraSandBox.Coitus.Part;

public class PartSystem : INeedInitialize
{
    private readonly CoitusVaginaSystem coitusVaginaSystem;
    private readonly CoitusMentulaSystem coitusMentulaSystem;
    public TestPawn owner;
    public Dictionary<string, OrganPart> totalParts;

    public Dictionary<string, CoitusVaginaAspect> TotalVaginaAspects { get; private set; }
    public Dictionary<string, CoitusMentulaAspect> TotalMentulaAspects { get; private set; }

    public PartSystem(TestPawn owner)
    {
        this.coitusVaginaSystem = new CoitusVaginaSystem(this);
        this.coitusMentulaSystem = new CoitusMentulaSystem(this);
        this.owner = owner;
    }

    public void UpdateRoutesTotally() =>
        this.coitusVaginaSystem.UpdateRoutesTotally();

    public void Initialize()
    {
        this.TotalVaginaAspects = this.totalParts
            .Where(pairs => pairs.Value.vaginaAspect != null)
            .ToDictionary(pair => pair.Value.vaginaAspect.baseName, pair => pair.Value.vaginaAspect);
        this.TotalMentulaAspects = this.totalParts
            .Where(pair => pair.Value.mentulaAspect != null)
            .ToDictionary(pair => pair.Value.mentulaAspect.baseName, pair => pair.Value.mentulaAspect);
    }

    // public CoitusVaginaRoute ChooseVagina()
    // {
    //     var routes = this.coitusVaginaSystem.TotalVaginaRoutes;
    //     var r = new Random();
    //     return routes.ElementAt(r.Next(routes.Count));
    // }
}