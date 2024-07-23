using System.Collections.Generic;
using System.Linq;
using eraSandBox.Coitus.XmlAssign;
using eraSandBox.Pawn;

namespace eraSandBox.Coitus;

public class PartsBuilder
{
    public static PartsBuilder Instance { get; } = new();

    public static Dictionary<string, OrganPart> MakeParts(CellThing owner, string template)
    {
        var (vaginaInfos, mentulaInfos) = LinkXml.AssignPartLink(template);
        var partList = new Dictionary<string, OrganPart>();
        var vaginaList = new List<CoitusVaginaAspect>();
        var mentulaList = new List<CoitusMentulaAspect>();
        foreach (string baseName in vaginaInfos.Select(vaginaInfo => vaginaInfo.Key))
        {
            var part = partList.TryGetValue(baseName, out var value)
                ? value
                : new OrganPart(owner, baseName);

            var vaginaAspect = new CoitusVaginaAspect(part);
            DefXml.AssignDef(vaginaAspect, baseName);
            part.vaginaAspects.Add(vaginaAspect);
            vaginaList.Add(vaginaAspect);
            partList.AddAndSkip(part.baseName, part);
        }

        foreach (string baseName in mentulaInfos.Select(mentulaInfo => mentulaInfo.Key))
        {
            var part = partList.TryGetValue(baseName, out var value)
                ? value
                : new OrganPart(owner, baseName);

            var mentulaAspect = new CoitusMentulaAspect(part);
            DefXml.AssignDef(mentulaAspect, baseName);
            part.mentulaAspects.Add(mentulaAspect);
            mentulaList.Add(mentulaAspect);
            partList.AddAndSkip(part.baseName, part);
        }

        //此时，vagina和mentula已经连接完毕，但是还只是linkInfo状态
        OrganizeConnection<CoitusAspect, LinkPoint<CoitusAspect>>(vaginaInfos.Values, vaginaList);
        OrganizeConnection<CoitusAspect, LinkPoint<CoitusAspect>>(mentulaInfos.Values, mentulaList);

        foreach (var aspect in vaginaList) aspect.Initialize();

        foreach (var aspect in mentulaList) aspect.Initialize();

        return partList;
    }

    private static void OrganizeConnection<L2, P2>(
        IEnumerable<LinkXml.LinkInfoWithStartPoint> linkInfos,
        IEnumerable<L2> nodesNeedLink)
        where L2 : ILinkTo<L2>
        where P2 : LinkPoint<L2>
    {
        var nodeMap =
            linkInfos.ToDictionary(
                linkInfo => linkInfo,
                linkInfo => nodesNeedLink.First(nullLink => nullLink.baseName == linkInfo.baseName));
        foreach (var thisSide in nodeMap.Keys)
        foreach (var pair in thisSide.linkTo)
        {
            var thatSide = pair.Key;
            var newPoints = new LinkPoint<L2>
            (
                pair.Value.percentage,
                thatSide.linkTo.First(p => p.Key.baseName == thisSide.baseName).Value.percentage,
                nodeMap[thatSide],
                nodeMap[thisSide]
            );
            nodeMap[thisSide].linkTo.Add((P2)newPoints);
            // var reversePoints = new LinkPoint<L2>
            // (
            //     newPoints.endPoint,
            //     newPoints.startPoint,
            //     newPoints.end,
            //     newPoints.start
            // );
            //nodeMap[thatSide].linkTo.Add((P2)newPoints);
        }
    }
}