using System.Collections.Generic;
using System.Linq;

namespace eraSandBox.Coitus
{
    public class PartsBuilder
    {
        private PartsBuilder()
        {
        }

        public static PartsBuilder Instance { get; } = new PartsBuilder();

        public void MakePawn()
        {
            var pawn = new TestPawn();
        }

        public void MakeParts(TestPawn owner, string template)
        {
            var (vaginaInfos, mentulaInfos) = LinkXml.AssignPartLink(template);
            var partList = new Dictionary<string, Part>();
            var vaginaList = new List<CoitusVaginaAspect>();
            var mentulaList = new List<CoitusMentulaAspect>();
            foreach (var vaginaInfo in vaginaInfos)
            {
                string baseName = vaginaInfo.Key;
                var part = partList.ContainsKey(baseName)
                    ? new Part(owner, baseName)
                    : partList[baseName];

                var vaginaAspect = new CoitusVaginaAspect(part);
                part.vaginaAspect = vaginaAspect;
                vaginaList.Add(vaginaAspect);
            }

            foreach (var mentulaInfo in mentulaInfos)
            {
                string baseName = mentulaInfo.Key;
                var part = partList.ContainsKey(baseName)
                    ? new Part(owner, baseName)
                    : partList[baseName];

                var mentulaAspect = new CoitusMentulaAspect(part);
                part.mentulaAspect = mentulaAspect;
                mentulaList.Add(mentulaAspect);
            }
        }

        public class LinkInfoBothEnd<T>
        {
            public readonly string name;

            public readonly T represent;

            public readonly Dictionary<LinkInfoBothEnd<T>, LinkPointBothEnd> linkTo =
                new Dictionary<LinkInfoBothEnd<T>, LinkPointBothEnd>();

            public LinkInfoBothEnd(string name, T represent)
            {
                this.name = name;
                this.represent = represent;
            }

            public void Add(LinkInfoBothEnd<T> linkInfo, LinkPointBothEnd point)
            {
                this.linkTo.AddAndCover(linkInfo, point);
            }

            public static IEnumerable<LinkInfoBothEnd<T>> OrganizeConnection(
                IEnumerable<LinkXml.LinkInfoWithStartPoint> linkInfos,
                IReadOnlyDictionary<string, T> representDict)
            {
                Dictionary<LinkXml.LinkInfoWithStartPoint, LinkInfoBothEnd<T>> nodeMap =
                    linkInfos.ToDictionary(linkInfo => linkInfo,
                        linkInfo => new LinkInfoBothEnd<T>(linkInfo.name, representDict[linkInfo.name]));
                foreach (var thisSide in nodeMap.Keys)
                {
                    foreach (var pair in thisSide.linkTo)
                    {
                        var thatSide = pair.Key;
                        var newPoints = new LinkPointBothEnd
                        (
                            pair.Value.percentage,
                            thatSide.linkTo.First(p => p.Key.name == thisSide.name).Value.percentage,
                            nodeMap[thisSide],
                            nodeMap[thatSide]
                        );
                        nodeMap[thisSide].Add(nodeMap[thatSide], newPoints);

                        var reversePoints = new LinkPointBothEnd(
                            newPoints.endPoint,
                            newPoints.startPoint,
                            nodeMap[thatSide],
                            nodeMap[thisSide]
                        );
                        nodeMap[thatSide].Add(nodeMap[thisSide], reversePoints);
                    }
                }

                return nodeMap.Values;
            }

            public class LinkPointBothEnd
            {
                public readonly LinkInfoBothEnd<T> start;
                public readonly LinkInfoBothEnd<T> end;
                public readonly int startPoint;
                public readonly int endPoint;

                public LinkPointBothEnd(int startPoint, int endPoint, LinkInfoBothEnd<T> start, LinkInfoBothEnd<T> end)
                {
                    this.start = start;
                    this.end = end;
                    this.startPoint = startPoint;
                    this.endPoint = endPoint;
                }
            }
        }
    }
}