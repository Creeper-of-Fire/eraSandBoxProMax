using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace eraSandBox.Coitus
{
    public class PawnBuilder
    {
        public class LinkXml : Xml
        {
            //private readonly List<object> _data = new List<object>();

            public LinkXml() : base("LinkSetup")
            {
                // foreach (XmlNode defNode in this.rootNode.ChildNodes)
                // {
                //     this._data.Add(XmlUtility.DeSerializer(GetType().Namespace, defNode));
                // }
            }

            public (Dictionary<string, PartInfo>, Dictionary<string, PartInfo>)? AssignRoute(string species)
            {
                XmlElement speciesNode = null;
                foreach (XmlElement node in this.rootNode.ChildNodes)
                {
                    if (node.GetAttribute("name") != species)
                        continue;
                    speciesNode = node;
                    break;
                }

                if (speciesNode == null)
                    return null;

                var vaginaRoutes = new List<XmlElement>();
                var mentulaRoutes = new List<XmlElement>();
                //speciesNode位于<species name="xxx">

                foreach (XmlElement node in speciesNode.ChildNodes)
                {
                    switch (node.Name)
                    {
                        case "vaginaRoute":
                            vaginaRoutes.Add(node);
                            break;
                        case "mentulaRoute":
                            mentulaRoutes.Add(node);
                            break;
                    }
                }

                //此时两个list之中为<vaginaRoute>、<mentulaRoute>
                var vaginaInfoSet = new Dictionary<string, PartInfo>();
                var mentulaInfoSet = new Dictionary<string, PartInfo>();
                getParts(in vaginaInfoSet, vaginaRoutes);
                getParts(in mentulaInfoSet, mentulaRoutes);


                var vaginaDict = AssignLinkInfo(vaginaRoutes, vaginaInfoSet);
                var mentulaDict = AssignLinkInfo(mentulaRoutes, mentulaInfoSet);


                return (vaginaDict, mentulaDict);

                void getParts(in Dictionary<string, PartInfo> infoSet, IEnumerable<XmlElement> routes)
                {
                    var hashSet = new HashSet<string>();
                    foreach (var part in routes.SelectMany(route => route.ChildNodes.Cast<XmlElement>()))
                        hashSet.Add(part.GetAttribute("name"));
                    foreach (string name in hashSet)
                    {
                        infoSet.Add(name, new PartInfo(name));
                    }
                }

                #region 无用代码

                // void For(List<XmlElement> routes, List<PartInfo> list, IDictionary<string, PartInfo> dict)
                // {
                //     foreach (var route in routes)
                //     {
                //         PartInfo lastPart = null;
                //         //route是<part name="">的列表
                //         foreach (var nowPart in from XmlElement part in route
                //                  select new PartInfo(part.GetAttribute("name")))
                //         {
                //             list.Add(nowPart);
                //             if (lastPart != null)
                //             {
                //                 lastPart.linkTo.Add(nowPart);
                //                 nowPart.linkTo.Add(lastPart);
                //             }
                //
                //             lastPart = nowPart;
                //         }
                //
                //         foreach (var partInfo in list)
                //         {
                //             Add(dict, partInfo);
                //         }
                //     }
                // }
                // void Add(IDictionary<string, PartInfo> dict, PartInfo p)
                // {
                //     if (dict.ContainsKey(p.name))
                //     {
                //         dict[p.name].linkTo.AddRange(p.linkTo);
                //         dict[p.name].linkTo = RemoveRepetitive(dict[p.name].linkTo);
                //     }
                //     else
                //     {
                //         dict.Add(p.name, p);
                //     }
                // }
                // static List<PartInfo> RemoveRepetitive(IEnumerable<PartInfo> partInfos)
                // {
                //     var result = partInfos
                //         .GroupBy(p => p.name)
                //         .Select(g => g.OrderBy(p => p.linkTo.Count).First())
                //         .ToList();
                //
                //     return result;
                // }

                #endregion
            }

            public static Dictionary<string, PartInfo> AssignLinkInfo(
                IEnumerable<XmlElement> routes,
                Dictionary<string, PartInfo> infoSet)
            {
                // 生成不重复的info对象
                var infoObjects = new Dictionary<string, PartInfo>();
                foreach (var newRoute in routes.Select(route => route.Cast<XmlElement>().ToList()))
                {
                    for (int i = 0; i < newRoute.Count; i++)
                    {
                        if (!infoObjects.ContainsKey(getName(i)))
                        {
                            infoObjects[getName(i)] = infoSet[getName(i)];
                        }

                        if (i > 0)
                        {
                            infoObjects[getName(i)].linkTo.Add(infoSet[getName(i - 1)]);
                        }

                        if (i < newRoute.Count - 1)
                        {
                            infoObjects[getName(i)].linkTo.Add(infoSet[getName(i + 1)]);
                        }

                        continue;

                        string getName(int j)
                        {
                            return newRoute[j].GetAttribute("name");
                        }
                    }
                }

                return infoObjects;
            }

            public class PartInfo
            {
                public string name;
                public List<PartInfo> linkTo = new List<PartInfo>();

                public PartInfo(string name)
                {
                    this.name = name;
                }
            }

            public class PartInfoWithString
            {
                public string name;
                public List<string> linkTo = new List<string>();

                public PartInfoWithString(string name)
                {
                    this.name = name;
                }
            }
        }

        public void MakePawn()
        {
            var pawn = new TestPawn();
        }
    }
}