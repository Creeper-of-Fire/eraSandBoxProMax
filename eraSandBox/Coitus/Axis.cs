using System;
using System.Collections.Generic;
using System.Linq;

namespace eraSandBox.Coitus
{
    public class Axis
    {
        protected List<Point> points = new List<Point>();

        public int size =>
            this.endPoint.x - this.startPoint.x;

        public Point endPoint =>
            this.points.Last();

        public Point startPoint =>
            this.points.First();

        public void Append(int x)
        {
            this.points.Add(new Point(x, this));
        }

        public void MakeAxis(List<int> distanceList)
        {
            int x = 0;
            Append(x);
            foreach (int distance in distanceList)
            {
                x += distance;
                Append(x);
            }

            Sort();
        }

        public void Sort()
        {
            this.points.Sort();
            for (int index = 0; index < this.points.Count; index++)
                this.points[index].index = index;
        }

        public Axis Clone() =>
            new Axis
            {
                points = this.points.ToArray().ToList()
            };

        public static Axis operator +(Axis axis, int value)
        {
            return new Axis
            {
                points = axis.points.Select(point => point + value).ToList()
            };
        }

        public static Axis operator *(Axis axis, int value)
        {
            return new Axis
            {
                points = axis.points.Select(point => point * value).ToList()
            };
        }

        /// <summary> 一个点落在区间上，获得这个区间的左端点 </summary>
        /// <returns> 序号：左端点
        /// <para> 如果点在端点上，则点对应的区间为端点左边 </para>
        /// 如果从左边落出Axis，则为-1；如果从右边落出Axis，则为<see cref="points" />的最大index </returns>
        public int GetEndPoint(Point a, int index)
        {
            return this.points.FindLastIndex(index, point => point <= a);
        }

        /// <summary> 获得一个点所在的区间的序号 </summary>
        /// <param name="point"> </param>
        /// <returns> </returns>
        public int GetIntervalIndex(Point point, int index = 0)
        {
            int startIndex = GetEndPoint(point, index);
            if (this.points[startIndex] == point)
                startIndex -= 1;

            return startIndex;
        }

        /// <summery> A、B都处于各自的区间中，AB之间包住了几个端点 </summery>
        /// <returns> (A所在区间序号，B所在区间序号，A到最近的包住的端点的距离，B到最近的包住的端点的距离) </returns>
        public (int, int, int, int) GetIntervalInfo(Point pointA, Point pointB, int index = 0)
        {
            MakeSureASmallerThanB(ref pointA, ref pointB);
            int startIndex = GetEndPoint(pointA, index);
            int endIndex = GetEndPoint(pointB, index);
            int distanceFirst = this.points[startIndex + 1] - pointA;
            int distanceLast = pointB - this.points[endIndex];
            return (startIndex, endIndex, distanceFirst, distanceLast);
        }

        public static int IntervalOverlapScale(Point pointA0, Point pointA1, Point pointB0, Point pointB1)
        {
            int scale = pointA0 > pointB0
                ? pointB1 - pointA0
                : pointA1 - pointB0;
            return scale < 0 ? 0 : scale;
        }

        public int IntervalOverlapScale(int intervalIndexA, int intervalIndexB) =>
            IntervalOverlapScale(
                this.points[intervalIndexA],
                this.points[intervalIndexA + 1],
                this.points[intervalIndexB],
                this.points[intervalIndexB + 1]);

        private static void MakeSureASmallerThanB(ref Point a, ref Point b)
        {
            if (a > b)
                (a, b) = (b, a);
        }


        /// <summary> 得到的结果不会被存储，所以需要直接使用 </summary>
        /// <param name="axisFocus"> 初始点坐标可能不为0 </param>
        /// <param name="axisBackground"> 初始点坐标必定为0 </param>
        /// <returns> 结果是以<paramref name="axisBackground" />中的点为基础的 </returns>
        public static Dictionary<Interval, Dictionary<IntervalInfo, int>> GetTwoAxisAssignment(
            Axis axisFocus,
            Axis axisBackground)
        {
            var intervals = AssignTwoAxis(axisFocus, axisBackground);
            return intervals.ToDictionary(
                interval => interval,
                interval => interval.GetContainInfo());
        }

        /// <summary> 得到的结果不会被存储，所以需要直接使用 </summary>
        /// <param name="axisFocus"> 初始点坐标可能不为0 </param>
        /// <param name="axisBackground"> 初始点坐标必定为0 </param>
        /// <returns> 结果是以<paramref name="axisBackground" />中的点为基础的 </returns>
        protected static IEnumerable<Interval> AssignTwoAxis(Axis axisFocus, Axis axisBackground)
        {
            var points = new List<Point>();
            points.AddRange(axisBackground.points);
            points.AddRange(axisFocus.points);
            points.Sort();
            var intervals = new List<Interval>();
            Interval nowInterval = null;
            foreach (var nowPoint in points)
            {
                if (nowPoint.owner == axisBackground)
                {
                    var nextPoint = nowPoint.NextPoint();
                    nowInterval = new Interval(nowPoint.index, nowPoint, nextPoint);
                    intervals.Add(nowInterval);
                }

                if (nowInterval != null && nowPoint.owner != axisBackground)
                    nowInterval.AddPoint(nowPoint);
            }

            intervals = intervals.Where(interval
                => interval.length != 0 && interval.IsEmpty()
            ).ToList();
            return intervals;
        }

        // /// <summary> </summary>
        // /// <param name="axisFocus"> 初始点坐标可能不为0 </param>
        // /// <param name="axisBackground"> 初始点坐标必定为0 </param>
        // public static void CompareAssignment(Axis axisFocusA, Axis axisFocusB, Axis axisBackground)
        // {
        //     int index = 0;
        //     foreach (int point in axisFocus.points)
        //     {
        //         index++;
        //         axisBackground.GetIntervalIndex(point, index);
        //     }
        // }

        public class Point : IComparable<Point>
        {
            public readonly Axis owner;
            public int index;
            public int x;

            public Point(int x, Axis owner)
            {
                this.x = x;
                this.owner = owner;
            }

            public int CompareTo(Point other) =>
                this.x.CompareTo(other.x);

            /// <returns> 返回下一个；当为最后一个时，返回自身 </returns>
            public Point NextPoint() =>
                IsLastInItsAxis() ? this : this.owner.points[this.index + 1];

            /// <returns> 返回上一个；当为第一个时，返回自身 </returns>
            public Point PrevPoint() =>
                IsFirstInItsAxis() ? this : this.owner.points[this.index - 1];

            public bool IsLastInItsAxis() =>
                this.index == this.owner.points.Count;

            public bool IsFirstInItsAxis() =>
                0 == this.index;

            public static Point operator +(Point point, int value) =>
                new Point(point.x + value, point.owner);

            public static Point operator *(Point point, int value) =>
                new Point(point.x * value, point.owner);

            public static int operator -(Point pointA, Point pointB) =>
                pointA.x - pointB.x;

            public static bool operator <=(Point pointA, Point pointB) =>
                pointA.x <= pointB.x;

            public static bool operator >=(Point pointA, Point pointB) =>
                pointA.x >= pointB.x;

            public static bool operator <(Point pointA, Point pointB) =>
                pointA.x < pointB.x;

            public static bool operator >(Point pointA, Point pointB) =>
                pointA.x > pointB.x;
        }

        /// <summary>
        /// 这是<see cref="Interval"/>的简洁版本，只包含关键信息
        /// </summary>
        public readonly struct IntervalInfo
        {
            public readonly int index;

            public IntervalInfo(int index)
            {
                this.index = index;
            }

            public override int GetHashCode() =>
                this.index;
        }

        /// <summary> 临时生成，用于保存区间的信息 </summary>
        public class Interval
        {
            protected readonly List<Point> contain = new List<Point>();
            protected readonly int index;
            protected readonly Point start;
            protected readonly Point end;

            public Tuple<int, Point, Point> GetBuildInfo()
            {
                return new Tuple<int, Point, Point>(this.index, this.start, this.end);
            }

            protected Interval(Tuple<int, Point, Point> buildInfo) : this(
                buildInfo.Item1,
                buildInfo.Item2,
                buildInfo.Item3)
            {
            }

            public Interval(int index, Point start, Point end)
            {
                this.index = index;
                this.start = start;
                this.end = end;
            }

            public int length =>
                this.end - this.start;

            public bool IsEmpty() =>
                this.contain.Count == 0;

            public void AddPoint(Point point)
            {
                this.contain.Add(point);
            }

            public override int GetHashCode() =>
                this.index;

            public Dictionary<IntervalInfo, int> GetContainInfo()
            {
                var containInfo = new Dictionary<IntervalInfo, int>();
                int i = 0;
                while (true)
                {
                    var nowPoint = this.contain[i];
                    if (nowPoint.IsFirstInItsAxis())
                        continue;
                    var prevPoint = i == 0
                        ? this.start
                        : this.contain[i - 1];

                    int intervalIndex = nowPoint.index;

                    AppendInfo(intervalIndex, nowPoint - prevPoint);

                    if (nowPoint.IsLastInItsAxis())
                        break;

                    if (i == this.contain.Count)
                    {
                        AppendInfo(intervalIndex, this.end - nowPoint);
                        break;
                    }

                    i++;
                }

                return containInfo;

                void AppendInfo(int intervalIndex, int lengthValue)
                {
                    containInfo.Add(new IntervalInfo(intervalIndex), lengthValue);
                }
            }
        }
    }

    /// <summary> <see cref="Axis.points" />是端点，<see cref="list" />则与区间一一对应
    /// <para> <see cref="list" />的元素比<see cref="Axis.points" />少一个 </para>
    /// </summary>
    /// <typeparam name="T"> </typeparam>
    public class AxisWithList<T> : Axis
    {
        private readonly List<T> list;

        public AxisWithList(List<T> list)
        {
            this.list = list;
        }

        public void Reserve()
        {
            this.points = this.points.Select(point => this.startPoint + (this.endPoint - point)).ToList();
            this.list.Reverse();
        }

        public static Dictionary<Interval<T>, Dictionary<IntervalInfo<T>, int>> GetTwoAxisAssignment(
            AxisWithList<T> axisFocus, AxisWithList<T> axisBackground)
        {
            var baseIntervals = AssignTwoAxis(axisFocus, axisBackground);
            var intervals = baseIntervals.Select(interval => new Interval<T>(interval, axisBackground.list));
            return intervals.ToDictionary(
                interval => interval,
                interval => interval.GetContainInfo(axisBackground.list));
        }


        public class Interval<TRep> : Interval
        {
            public TRep represent;

            public Dictionary<IntervalInfo<TRep>, int> GetContainInfo(IReadOnlyList<TRep> represents)
            {
                var BaseContainInfo = base.GetContainInfo();
                var containInfo = BaseContainInfo.ToDictionary(
                    pair => new IntervalInfo<TRep>(pair.Key, represents),
                    pair => pair.Value);

                return containInfo;
            }

            public Interval(Interval interval, TRep represent) : base(interval.GetBuildInfo())
            {
                this.represent = represent;
            }

            public Interval(Interval interval, IReadOnlyList<TRep> list) : base(interval.GetBuildInfo())
            {
                this.represent = list[this.index];
            }
        }

        public readonly struct IntervalInfo<TRep>
        {
            public readonly int index;
            public readonly TRep represent;

            public IntervalInfo(IntervalInfo intervalInfo, TRep represent)
            {
                this.index = intervalInfo.index;
                this.represent = represent;
            }

            public IntervalInfo(IntervalInfo intervalInfo, IReadOnlyList<TRep> list)
            {
                this.index = intervalInfo.index;
                this.represent = list[this.index];
            }

            public override int GetHashCode() =>
                this.index;
        }
    }

    public interface IWithAxis
    {
        public Axis baseAxis { get; }
        public void MakeAxis();
    }
}