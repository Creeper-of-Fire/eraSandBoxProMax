using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;

namespace eraSandBox.Coitus
{
    public class LinkXml : Xml
    {
        private LinkXml() : base("LinkSetup")
        {
        }

        private static LinkXml Instance { get; } = new LinkXml();

        /// <summary> 根据给予的模板的名字，返回两个字典，字典的键值对为(名字,名字对应的连接信息) </summary>
        /// <param name="template"> 模板的名字 </param>
        /// <returns> (vaginaDict, mentulaDict) </returns>
        public static (Dictionary<string, LinkInfoWithStartPoint>, Dictionary<string, LinkInfoWithStartPoint>)
            AssignPartLink(string template)
        {
            var nameGetter = new Func<XmlElement, string>(part => part.GetAttribute("name"));

            var linkByPointGetter = new Func<XmlElement, (int?, int?)?>(part => LinkByPointGetter(part, "linkBy"));
            var templateNode =
                Instance.rootNode.ChildNodes.Cast<XmlElement>()
                    .FirstOrDefault(node => nameGetter(node) == template);

            if (templateNode == null)
                return (null, null);

            var vaginaLinks = new List<XmlElement>();
            var mentulaLinks = new List<XmlElement>();
            //speciesNode位于<template name="xxx">

            foreach (XmlElement node in templateNode.ChildNodes)
                switch (node.Name)
                {
                    case "vaginaLink":
                        vaginaLinks.Add(node);
                        break;
                    case "mentulaLink":
                        mentulaLinks.Add(node);
                        break;
                }
            //此时两个list之中为<vaginaRoute>、<mentulaRoute>

            var vaginaDict =
                AssignLinkInfo(vaginaLinks, nameGetter, linkByPointGetter);
            var mentulaDict =
                AssignLinkInfo(mentulaLinks, nameGetter, linkByPointGetter);


            return (vaginaDict, mentulaDict);

            //TODO(优先级很低) 改成可以表示范围的那种
            (int?, int?)? LinkByPointGetter(XmlElement part, string attributeName)
            {
                if (!part.HasAttribute(attributeName))
                    return null;
                string input = part.GetAttribute(attributeName);
                const string pattern = @"(\d*),(\d*)";
                var match = Regex.Match(input, pattern);
                if (!match.Success)
                    return null;
                int? num1;
                if (string.IsNullOrEmpty(match.Groups[1].Value))
                    num1 = null;
                else
                    num1 = int.Parse(match.Groups[1].Value);
                int? num2;
                if (string.IsNullOrEmpty(match.Groups[2].Value))
                    num2 = null;
                else
                    num2 = int.Parse(match.Groups[2].Value);
                return (num1, num2);
            }
        }

        /// <summary>
        ///     处理这样的连接情况：
        ///     <para> 一个列表有一系列带名字的对象，且它们以列表顺序互相前后连接 </para>
        ///     有许多这样的列表
        /// </summary>
        /// <returns> </returns>
        private static Dictionary<string, LinkInfoWithStartPoint> AssignLinkInfo<T>(
            IReadOnlyCollection<IEnumerable> routes,
            Func<T, string> NameGetter,
            Func<T, (int?, int?)?> LinkByPointGetter) =>
            AssignLinkInfo(routes, GetParts(routes, NameGetter), NameGetter, LinkByPointGetter);

        private static Dictionary<string, LinkInfoWithStartPoint> GetParts<T>(IEnumerable<IEnumerable> routes,
            Func<T, string> NameGetter)
        {
            var hashSet = new HashSet<string>();
            foreach (var route in routes)
            foreach (var part in route.Cast<T>())
                hashSet.Add(NameGetter(part));
            return hashSet.ToDictionary(name => name, name => new LinkInfoWithStartPoint(name));
        }

        /// <summary> 因为所有的对象都是从一个Set里面提上来的，所以各种引用关系得以保留 </summary>
        /// <param name="routes"> 要处理的原始数据 </param>
        /// <param name="infoSet"> 一个集合，相当于建立了一个“名字-指针”的关系 </param>
        /// <param name="NameGetter"> </param>
        /// <param name="LinkByPointGetter"> </param>
        /// <returns> 实际上和<paramref name="infoSet" /> 完全一样，理论上可以把它重新返回回去，但是反正这里不需要考虑性能。 </returns>
        private static Dictionary<string, LinkInfoWithStartPoint> AssignLinkInfo<T>(
            IEnumerable<IEnumerable> routes,
            IReadOnlyDictionary<string, LinkInfoWithStartPoint> infoSet,
            Func<T, string> NameGetter,
            Func<T, (int?, int?)?> LinkByPointGetter)
        {
            // 生成不重复的info对象
            var infoDict = new Dictionary<string, LinkInfoWithStartPoint>();
            foreach (var newRoute in routes.Select(route => route.Cast<T>().ToList()))
                for (int i = 0; i < newRoute.Count; i++)
                {
                    var nowObject = newRoute[i];
                    if (!infoDict.ContainsKey(GetName(nowObject)))
                        infoDict[GetName(nowObject)] = SearchLinkInfo(GetName(nowObject));

                    if (i > 0)
                    {
                        var prevInfo = SearchLinkInfo(GetName(newRoute[i - 1]));
                        var points = LinkByPointGetter(nowObject);
                        var linkPoint = GetLinkPoint(points, true);
                        infoDict[GetName(nowObject)].AssignLinkToInfo(prevInfo, linkPoint);
                    }

                    if (i < newRoute.Count - 1)
                    {
                        var nextInfo = SearchLinkInfo(GetName(newRoute[i + 1]));
                        var points = LinkByPointGetter(nowObject);
                        var linkPoint = GetLinkPoint(points, false);
                        infoDict[GetName(nowObject)].AssignLinkToInfo(nextInfo, linkPoint);
                    }

                    continue;

                    LinkInfoWithStartPoint.LinkStartPoint GetLinkPoint((int?, int?)? points, bool isLinkToPrev)
                    {
                        if (points == null)
                            return new LinkInfoWithStartPoint.LinkStartPoint(null, isLinkToPrev);
                        (int? pointToPrev, int? pointToNext) = ((int?, int?))points;
                        return isLinkToPrev
                            ? new LinkInfoWithStartPoint.LinkStartPoint(pointToPrev, true)
                            : new LinkInfoWithStartPoint.LinkStartPoint(pointToNext, false);
                    }

                    LinkInfoWithStartPoint SearchLinkInfo(string name)
                    {
                        return infoSet[name];
                    }

                    string GetName(T otherObject)
                    {
                        return NameGetter(otherObject);
                    }
                }

            return infoDict;
        }


        public class LinkInfoWithStartPoint
        {
            public readonly string baseName;

            /// <summary> </summary>
            /// <typeparam name="LinkInfoWithStartPoint"> 连接到的其他节点 </typeparam>
            /// <typeparam name="LinkStartPoint"> 本次连接的出发点 </typeparam>
            public readonly Dictionary<LinkInfoWithStartPoint, LinkStartPoint> linkTo;

            public LinkInfoWithStartPoint(string baseName)
            {
                this.linkTo = new Dictionary<LinkInfoWithStartPoint, LinkStartPoint>();
                this.baseName = baseName;
            }

            public void AssignLinkToInfo(LinkInfoWithStartPoint linkInfoOfOther, LinkStartPoint newStartPoint)
            {
                AddLinkDict(linkInfoOfOther, newStartPoint);
            }

            private void AddLinkDict(LinkInfoWithStartPoint linkInfoOfOther, LinkStartPoint newStartPoint)
            {
                if (!this.linkTo.ContainsKey(linkInfoOfOther))
                {
                    this.linkTo.Add(linkInfoOfOther, newStartPoint);
                    return;
                }

                var oldStartPoint = this.linkTo[linkInfoOfOther];

                switch (oldStartPoint.isBlank, newStartPoint.isBlank)
                {
                    case (true, true) when oldStartPoint.percentage != newStartPoint.percentage:
                        throw new SystemException(string.Join("", this.baseName, "与", linkInfoOfOther.baseName, "之间的存在冲突：",
                            newStartPoint, "与", oldStartPoint, "均为空白，但",
                            newStartPoint.percentage, "不等于", oldStartPoint.percentage));
                    case (false, false) when oldStartPoint.percentage != newStartPoint.percentage:
                        throw new SystemException(string.Join("", this.baseName, "与", linkInfoOfOther.baseName, "之间的存在冲突：",
                            newStartPoint, "与", oldStartPoint, "均不为空白，但",
                            newStartPoint.percentage, "不等于", oldStartPoint.percentage));
                    case (true, false):
                        this.linkTo[linkInfoOfOther] = newStartPoint;
                        break;
                    case (false, true):
                        break;
                }
            }

            public readonly struct LinkStartPoint
            {
                /// <summary> 这里指的是连接到上一个节点时，从本节点的开头开始进行连接 </summary>
                private const int Start = 0;

                private const int End = 100;

                /// <summary> 连接出发点位置的百分比信息（例如，从A出发连接到B，这个点是位于A上的），0在头，100在尾 </summary>
                public readonly int percentage;

                public readonly bool isBlank;

                public LinkStartPoint(int? percentage, bool isLinkToPrev)
                {
                    if (percentage == null)
                    {
                        if (isLinkToPrev)
                            this.percentage = Start;
                        else
                            this.percentage = End;
                        this.isBlank = true;
                    }
                    else
                    {
                        this.percentage = (int)percentage;
                        this.isBlank = false;
                    }
                }
            }
        }
    }
}