using System;
using System.Linq;
using eraSandBox.Coitus;

namespace eraSandBox
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var infoObjectsA = LinkXml.AssignPartLink("人类");
            var infoObjects = LinkXml.LinkInfoBothEnd.OrganizeConnection(infoObjectsA.Item1.Values);
            // 打印生成的info对象
            foreach (var obj in infoObjects)
                Console.WriteLine(
                    $"Name: {obj.name}, LinkTo: {string.Join(",", obj.linkTo.Select(pair => pair.Key.name.ToString() + ":" + pair.Value.startPoint + ',' + pair.Value.endPoint))}");
        }
    }
}