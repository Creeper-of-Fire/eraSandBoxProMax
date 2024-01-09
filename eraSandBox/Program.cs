using System;
using System.Collections.Generic;
using System.Diagnostics;
using eraSandBox.Coitus;

namespace eraSandBox
{
    class Program
    {
        static void Main(string[] args)
        {
            var a = new PawnBuilder.LinkXml();
            var infoObjects = a.AssignRoute("人类") ?? throw new ArgumentNullException("a.AssignRoute(\"人类\")");
            // 打印生成的info对象
            foreach (PawnBuilder.LinkXml.PartInfo obj in infoObjects.Item1.Values)
            {
                Console.WriteLine($"Name: {obj.name}, LinkTo: {string.Join(",", obj.linkTo)}");
            }
        }
    }
}