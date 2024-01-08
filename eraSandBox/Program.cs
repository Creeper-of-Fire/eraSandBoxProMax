using System;
using eraSandBox.Coitus;

namespace eraSandBox
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var rand = new Random();
            var axisA = new Axis();
            for (int i = 0; i < 10; i++)
                axisA.Append(rand.Next(1, 10000));

            var axisB = new Axis();
            for (int i = 0; i < 10; i++)
                axisB.Append(rand.Next(1, 10000));

            Console.WriteLine(axisA);
            Console.WriteLine(axisB);
            Console.WriteLine(Axis.GetTwoAxisAssignment(axisA,axisB));
        }
    }
}