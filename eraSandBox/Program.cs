using System;
using System.Diagnostics;
using System.Linq;
using eraSandBox.Coitus;

namespace eraSandBox
{
    internal class Program
    {
        //public static int times = 0;
        public static Stopwatch cost = new Stopwatch();

        public static void Main(string[] args)
        {
            //testC(1000, 1000);
            TestC(10, 100);
            Console.WriteLine($"程序耗时：{cost.ElapsedMilliseconds}ms.");
        }

        public static void TestC(int amount, int times)
        {
            for (int j = 0; j < times; j++)
            {
                var rand = new Random();
                var axisA = new Axis();
                for (int i = 0; i < amount; i++)
                    axisA.Append(rand.Next(0, amount * 10));
                axisA.Sort();
                var axisB = new Axis();
                for (int i = 0; i < amount; i++)
                    axisB.Append(rand.Next(0, amount * 10));
                axisB.Sort();

                MainTest2(axisA, axisB);
            }
        }

        public static void TestB(int amount, int times)
        {
            for (int j = 0; j < times; j++)
            {
                var rand = new Random();
                var axisA = new AxisOld();
                for (int i = 0; i < amount; i++)
                    axisA.Append(rand.Next(0, amount * 10));
                axisA.Sort();
                var axisB = new AxisOld();
                for (int i = 0; i < amount; i++)
                    axisB.Append(rand.Next(0, amount * 10));
                axisB.Sort();

                MainTest(axisA, axisB);
            }
        }

        public static void MainTest(AxisOld axisA, AxisOld axisB)
        {
            cost.Start();
            var a = AxisOld.GetTwoAxisAssignment(axisA, axisB);
            cost.Stop();
        }

        public static void MainTest2(Axis axisA, Axis axisB)
        {
            cost.Start();
            var a = Axis.AssignmentMentulaForVagina(
                Axis.PointsToIntervals(axisA.Points).ToList(),
                Axis.PointsToIntervals(axisB.Points).ToList());
            //Console.WriteLine(a);
            cost.Stop();
        }
    }
}