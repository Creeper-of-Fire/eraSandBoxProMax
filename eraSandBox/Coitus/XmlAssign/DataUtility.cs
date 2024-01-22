using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace eraSandBox.Coitus
{
    public static class DataUtility
    {
        /// <summary>
        ///     如果Key重复就覆盖
        /// </summary>
        public static void AddAndCover<K, V>(this IDictionary<K, V> dictionary, K key, V value)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key] = value;
                return;
            }
            dictionary.Add(key, value);
        }
        /// <summary>
        ///     如果Key重复就跳过
        /// </summary>
        public static void AddAndSkip<K, V>(this IDictionary<K, V> dictionary, K key, V value)
        {
            if (dictionary.ContainsKey(key))
            {
                return;
            }
            dictionary.Add(key, value);
        }

        // /// <summary>
        // /// </summary>
        // /// <param name="nodesNeedLink">linkTo内部没有注册</param>
        // /// <param name="linkInfos">linkTo内部已经注册</param>
        // /// <returns></returns>
        // /// <exception cref="DataException"></exception>
        // public static void AssignLinkBy<L1, L2, P1, P2>(
        //     this IEnumerable<L2> nodesNeedLink,
        //     IEnumerable<L1> linkInfos)
        //     where L1 : ILinkTo<L1>
        //     where L2 : ILinkTo<L2>
        //     where P1 : LinkPoint<L1>
        //     where P2 : LinkPoint<L1>
        // {
        //     var nodeMap =
        //         linkInfos.ToDictionary(
        //             linkInfo => linkInfo,
        //             linkInfo => nodesNeedLink.First(nullLink => nullLink.baseName == linkInfo.baseName)
        //         );
        //     foreach (var point in nodeMap.Keys.SelectMany(thisSide => thisSide.linkTo))
        //     {
        //         nodeMap[point].linkTo.Add(nodeMap[point.Key], (P2)new LinkPoint<L1>(point.Value));
        //     }
        // }

        public static T Copy<T>(T obj)
        {
            if (obj == null)
            {
                return default;
            }
            using var ms = new MemoryStream();
            var bf = new BinaryFormatter();
            bf.Serialize(ms, obj); //序列化
            ms.Seek(0, SeekOrigin.Begin);
            var res = (T)bf.Deserialize(ms); //反序列化
            return res;
        }

        //TODO 应该改成IGrouping,因为两个器官之间可能会有多种连接点
    }

    public interface ILinkTo<TSelf>
        where TSelf : ILinkTo<TSelf>
    {
        public string baseName { get; }

        /// <summary>
        /// T:连向哪个，POINT：从哪里连的
        /// </summary>
        public IList<LinkPoint<TSelf>> linkTo { get; }
    }

    /// <summary>
    /// 从物品A上的点A连接到物品B上的点B
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LinkPoint<T>
    {
        /// <summary>
        /// <see cref="pointA"/>位于<see cref="objectA"/>上
        /// </summary>
        public readonly T objectA;

        /// <summary>
        /// <see cref="pointB"/>位于<see cref="objectB"/>上
        /// </summary>
        public readonly T objectB;

        public readonly int pointAPercentage;
        public readonly int pointBPercentage;

        public int GetPoint(T objectAorB)
        {
            return Equals(this.objectA, objectAorB)
                ? this.pointAPercentage
                : this.pointBPercentage;
        }

        public T GetOppositeObject(T objectAorB)
        {
            return Equals(this.objectA, objectAorB)
                ? this.objectB
                : this.objectA;
        }

        public int GetOppositePoint(T objectAorB)
        {
            return Equals(this.objectA, objectAorB)
                ? this.pointBPercentage
                : this.pointAPercentage;
        }


        public LinkPoint(int pointA, int pointB, T objectA, T objectB)
        {
            this.objectA = objectA;
            this.objectB = objectB;
            this.pointAPercentage = pointA;
            this.pointBPercentage = pointB;
        }

        public LinkPoint(Tuple<T, T, int, int> point)
        {
            this.objectA = point.Item1;
            this.objectB = point.Item2;
            this.pointAPercentage = point.Item3;
            this.pointBPercentage = point.Item4;
        }

        public static (T, T, int, int) ToPoint<T2>(LinkPoint<T2> point)
        {
            return ((T)(object)point.objectA, (T)(object)point.objectB, point.pointAPercentage, point.pointBPercentage);
        }
    }
}