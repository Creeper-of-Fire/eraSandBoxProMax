using System;
using System.Collections.Generic;
using eraSandBox.Coitus.XmlAssign;
using eraSandBox.Thought;
using eraSandBox.Utility.GameThing;

namespace eraSandBox.Coitus;

/**
 * 每个Pawn维护一个
 */
[Obsolete("Obsolete")]
public class PartSystem : INeedInitialize
{
    private readonly CoitusMentulaSystem coitusMentulaSystem;
    private readonly CoitusVaginaSystem coitusVaginaSystem;
    public IHasParts owner;
    public Dictionary<string, OrganPart> totalParts;

    public PartSystem(IHasParts owner)
    {
        this.coitusVaginaSystem = new CoitusVaginaSystem(this);
        this.coitusMentulaSystem = new CoitusMentulaSystem(this);
        this.owner = owner;
    }

    public Dictionary<string, CoitusVaginaAspect> TotalVaginaAspects { get; }
    public Dictionary<string, CoitusMentulaAspect> TotalMentulaAspects { get; }

    public void Initialize()
    {
        // this.totalParts = PartsBuilder.MakeParts(this, this.partsTemplate);
        // this.TotalVaginaAspects = this.totalParts
        //     .Where(pairs => pairs.Value.vaginaAspects != null)
        //     .ToDictionary(pair => pair.Value.vaginaAspect.baseName, pair => pair.Value.vaginaAspect);
        // this.TotalMentulaAspects = this.totalParts
        //     .Where(pair => pair.Value.mentulaAspect != null)
        //     .ToDictionary(pair => pair.Value.mentulaAspect.baseName, pair => pair.Value.mentulaAspect);
        // this.UpdateRoutesTotally();
    }

    public IEnumerable<Message> MakeMessage()
    {
        return new List<Message>();
    }

    public void UpdateRoutesTotally()
    {
        this.coitusVaginaSystem.UpdateRoutesTotally();
    }

    // public CoitusVaginaRoute ChooseVagina()
    // {
    //     var routes = this.coitusVaginaSystem.TotalVaginaRoutes;
    //     var r = new Random();
    //     return routes.ElementAt(r.Next(routes.Count));
    // }
}