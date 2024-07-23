using System.Collections.Generic;
using System.Linq;

namespace eraSandBox.Coitus.Fuck;

public static partial class FuckUtility
{
    public static void FuckIn(CoitusMentulaRoute mentulaRoute, CoitusVaginaRoute vaginaRoute)
    {
        mentulaRoute.insert = vaginaRoute;
        FuckBetweenRoute(vaginaRoute.length, mentulaRoute.length);
    }

    public static void FuckBetweenRoute<T0, T1>(
        T0 vaginaScale,
        T1 mentulaScale,
        int mentulaInsertLength,
        FuckAttitude fuckAttitude = FuckAttitude.ShouldComfort)
        where T0 : CoitusVaginaRouteScale, IVaginaScale
        where T1 : CoitusMentulaRouteScale, IScale
    {
        var vaginaParts = vaginaScale.parent.PartLink;
        var mentulaParts = mentulaScale.parent.parts;

        var fuckMode = GetFuckModeAndExpansionOrContractionRatio(
            vaginaScale, mentulaInsertLength, fuckAttitude);

        var fuckerPart = new List<CoitusMentulaAspect>();

        AssignVaginaForMentula(vaginaScale.parent, mentulaScale.parent,
            fuckMode == FuckModeType.NotFullEntryComfortable || fuckMode == FuckModeType.NotFullEntryUnComfortable,
            in fuckerPart);

        foreach (var part in fuckerPart)
            Fuck(part);

        if (fuckMode == FuckModeType.Destructive)
            Destructive(vaginaParts.Last().value);
    }

    private static void Fuck(CoitusMentulaAspect aspect)
    {
    }

    private static void Destructive(CoitusVaginaAspect aspect)
    {
    }

    public static void FuckBetweenRoute<T0, T1>(
        T0 vaginaRoute,
        T1 mentulaRoute,
        FuckAttitude fuckAttitude = FuckAttitude.ShouldComfort)
        where T0 : CoitusVaginaRouteScale, IVaginaScale
        where T1 : CoitusMentulaRouteScale, IScale
    {
        FuckBetweenRoute(vaginaRoute, mentulaRoute, mentulaRoute.OriginalMillimeter(), fuckAttitude);
    }

    public static void RemoveFuckRoute(
        CoitusMentulaRoute mentulaRemoveRoute)
    {
        var mentulaParts = mentulaRemoveRoute.parts;

        foreach (var part in mentulaParts)
            part.insert.Clear();
    }
}