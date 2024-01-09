using System;
using System.Diagnostics;
using eraSandBox.Coitus;

namespace eraSandBox
{
    internal class Program
    {
        //public static int times = 0;
        public static Stopwatch cost = new Stopwatch();

        public static void Main(string[] args)
        {
            // var b = new CoitusAspectDef();
            // var a = XmlExtend.XmlSerialize<CoitusAspectDef>(b);
            //
            var a = new DefXml();
            Console.WriteLine(a);
            //Console.WriteLine($"程序耗时：{cost.ElapsedMilliseconds}ms.");
        }
    }
}