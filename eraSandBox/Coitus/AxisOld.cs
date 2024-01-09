using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace eraSandBox.Coitus
{
    /// <summary> 核心代码为：<see cref="AssignTwoAxis" />和<see cref="" /> </summary>
    [Obsolete]
    public class AxisOld
    {
        protected List<Point> points = new List<Point>();

        public List<Point> Points =>
            this.points;

        public int Size =>
            this.EndPoint.x - this.StartPoint.x;

        public Point EndPoint =>
            this.points.Last();

        public Point StartPoint =>
            this.points.First();

        public static IEnumerable<Interval> PointsToIntervals(List<Point> points)
        {
            foreach (var point in points)
            {
                var nowIntervalBackground = new Interval(point.index, point, point.NextPoint());
                yield return nowIntervalBackground;
            }
        }

        public static Dictionary<Interval, Dictionary<int, int>> AssignmentMentulaForVagina(
            List<Interval> vaginaIntervals,
            List<Interval> mentulaIntervals
        )
        {
            var assignment = new Dictionary<Interval, Dictionary<int, int>>();
            foreach (var interval in vaginaIntervals)
                assignment.Add(interval, new Dictionary<int, int>());

            using var nowVaginaEnumerator = vaginaIntervals.GetEnumerator();
            using var nowMentulaEnumerator = mentulaIntervals.GetEnumerator();
            nowVaginaEnumerator.MoveNext();
            nowMentulaEnumerator.MoveNext();
            var nowVagina = nowVaginaEnumerator.Current;
            var nowMentula = nowMentulaEnumerator.Current;
            int oldVaginaLengthRemain = 0;
            int oldMentulaLengthRemain = 0;
            Debug.Assert(nowVagina != null, nameof(nowVagina) + " != null");
            Debug.Assert(nowMentula != null, nameof(nowMentula) + " != null");
            Check();

            return assignment;

            void Check()
            {
                while (true)
                {
                    int newVaginaLength = nowVagina.Length;
                    int newMentulaLength = nowMentula.Length;
                    int newVaginaLengthRemain = newVaginaLength - oldVaginaLengthRemain;
                    int newMentulaLengthRemain = newMentulaLength - oldMentulaLengthRemain;
                    switch ((MathUtility.Comparing)newMentulaLengthRemain.CompareTo(newVaginaLengthRemain))
                    {
                        case MathUtility.Comparing.Equal:
                            //nowMentulaLengthRemain == nowVaginaLengthRemain，刚刚好
                            assignment[nowVagina].Add(nowMentula.index, newMentulaLengthRemain);
                            oldVaginaLengthRemain = 0;
                            oldMentulaLengthRemain = 0;
                            if (nowVaginaEnumerator.MoveNext())
                                return;
                            if (nowMentulaEnumerator.MoveNext())
                                return;
                            break;
                        case MathUtility.Comparing.LastBigger:
                            //nowMentulaLengthRemain < nowVaginaLengthRemain，Vagina多出一大截
                            assignment[nowVagina].Add(nowMentula.index, newMentulaLengthRemain);
                            oldVaginaLengthRemain = newVaginaLengthRemain - newMentulaLength;
                            //之前减去了newMentulaLengthRemain，所以实际上是：
                            //“newVaginaLengthRemain - newMentulaLengthRemain + oldVaginaLengthRemain”
                            oldMentulaLengthRemain = 0;
                            if (nowMentulaEnumerator.MoveNext())
                                return;
                            break;
                        case MathUtility.Comparing.FirstBigger:
                            //nowMentulaLengthRemain > nowVaginaLengthRemain，Mentula多出一大截
                            assignment[nowVagina].Add(nowMentula.index, newVaginaLengthRemain);
                            oldVaginaLengthRemain = 0;
                            oldMentulaLengthRemain = newMentulaLengthRemain - newVaginaLength;
                            //之前减去了newVaginaLengthRemain，所以实际上是：
                            //“newMentulaLengthRemain - newVaginaLengthRemain + oldMentulaLengthRemain”
                            if (nowVaginaEnumerator.MoveNext())
                                return;
                            break;
                    }
                }
            }
        }

        public void Append(int x)
        {
            this.points.Add(new Point(x, this));
        }

        public void MakeAxis(IEnumerable<int> distanceList)
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

        public AxisOld Clone() =>
            new AxisOld
            {
                points = this.points.ToArray().ToList()
            };


        public static AxisOld operator +(AxisOld axis, int value)
        {
            return new AxisOld
            {
                points = axis.points.Select(point => point + value).ToList()
            };
        }

        public static AxisOld operator *(AxisOld axis, float value)
        {
            return new AxisOld
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
        [Obsolete]
        public static Dictionary<Interval, Dictionary<int, int>> GetTwoAxisAssignment(
            AxisOld axisBackground,
            AxisOld axisFocus)
        {
            var intervals = AssignTwoAxis(axisBackground, axisFocus);
            var dictionary = new Dictionary<Interval, Dictionary<int, int>>();
            foreach (var interval in intervals)
                dictionary.Add(interval, interval.GetContainInfo());
            return dictionary;
        }

        /// <summary> 得到的结果不会被存储，所以需要直接使用 </summary>
        /// <param name="axisFocus"> 初始点坐标可能不为0 </param>
        /// <param name="axisBackground"> 初始点坐标必定为0 </param>
        /// <returns> 结果是以<paramref name="axisBackground" />中的点为基础的 </returns>
        [Obsolete]
        protected static IEnumerable<Interval> AssignTwoAxis(AxisOld axisBackground, AxisOld axisFocus)
        {
            var points = new List<Point>();
            for (int index = 0; index < Math.Max(axisBackground.points.Count, axisFocus.points.Count); index++)
            {
                var p1 = axisBackground.points[index];
                var p2 = axisFocus.points[index];
                points.Add(p1);
                points.Add(p2);
            }

            points.Sort();
            var intervals = new List<Interval>();
            Interval nowIntervalBackground = default;
            Point prevPoint = default;
            Point prevFocusPoint = default;
            foreach (var nowPoint in points)
            {
                if (prevFocusPoint != null && prevPoint.owner == axisBackground && nowPoint.owner == axisBackground)
                    nowIntervalBackground.AddPoint(prevFocusPoint.Clone(prevPoint.x));
                //如果上一个区间一无所获，则在开头加一个左端点用来标记这是第几个区间；
                //此时还没有更新nowIntervalBackground，所以nowIntervalBackground属于上一个区间；

                if (nowPoint.owner == axisBackground)
                {
                    nowIntervalBackground = new Interval(nowPoint.index, nowPoint, nowPoint.NextPoint());
                    intervals.Add(nowIntervalBackground);
                }
                //更新nowIntervalBackground, 步进

                if (nowIntervalBackground != null && nowPoint.owner == axisFocus)
                {
                    nowIntervalBackground.AddPoint(nowPoint);
                    prevFocusPoint = nowPoint;
                }

                prevPoint = nowPoint;

                if (nowPoint.IsLastInItsAxis())
                    break;
            }

            foreach (var interval in intervals)
                if (interval.contain.Count != 0)
                    yield return interval;
        }

        public class Point : IComparable<Point>, ICloneable
        {
            public readonly AxisOld owner;
            public readonly int x;
            public int index;

            public Point(int x, AxisOld owner)
            {
                this.x = x;
                this.owner = owner;
            }

            public object Clone() =>
                new Point(this.x, this.owner)
                {
                    index = this.index
                };

            public int CompareTo(Point other)
            {
                switch (this.x.CompareTo(other.x))
                {
                    case -1:
                        return -1;
                    case 1:
                        return 1;
                    default:
                        if (this.owner.GetHashCode() != other.owner.GetHashCode())
                            return 0;
                        return this.index.CompareTo(other.index);
                }
            }

            /// <returns> 返回下一个；当为最后一个时，返回自身 </returns>
            public Point NextPoint() =>
                IsLastInItsAxis() ? this : this.owner.points[this.index + 1];

            /// <returns> 返回上一个；当为第一个时，返回自身 </returns>
            public Point PrevPoint() =>
                IsFirstInItsAxis() ? this : this.owner.points[this.index - 1];

            public bool IsLastInItsAxis() =>
                this.index == this.owner.points.LastIndex();

            public bool IsFirstInItsAxis() =>
                this.index == 0;

            public static Point operator +(Point point, int value) =>
                new Point(point.x + value, point.owner);

            public static Point operator *(Point point, int value) =>
                new Point(point.x * value, point.owner);

            public static Point operator *(Point point, float value) =>
                new Point((int)(point.x * value), point.owner);

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

            public Point Clone(int xValue) =>
                new Point(xValue, this.owner)
                {
                    index = this.index
                };
        }

        /// <summary> 这是<see cref="Interval" />的简洁版本，只包含关键信息 </summary>
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
            public readonly List<Point> contain;
            public readonly Point end;
            public readonly int index;
            public readonly Point start;

            public Interval(int index, Point start, Point end)
            {
                this.contain = new List<Point>();
                this.index = index;
                this.start = start;
                this.end = end;
            }

            public int Length =>
                this.end - this.start;

            // public bool IsEmpty() =>
            //     this.contain.Count == 0 || this.length == 0;

            public void AddPoint(Point point)
            {
                this.contain.Add(point);
            }

            public override int GetHashCode() =>
                this.index;

            [Obsolete]
            public Dictionary<int, int> GetContainInfo()
            {
                var containInfo = new Dictionary<int, int>();

                ForFunction();

                return containInfo;

                void ForFunction()
                {
                    var prevPoint = this.start; //启动至index=0
                    var nowPoint = this.contain.First();
                    containInfo.Add(nowPoint.index - 1, nowPoint - prevPoint);
                    for (int i = 1; i < this.contain.Count; i++)
                    {
                        nowPoint = this.contain[i];
                        prevPoint = this.contain[i - 1];
                        if (nowPoint.IsFirstInItsAxis())
                            continue;
                        //拿着区间末端往前找，所以-1
                        containInfo.Add(nowPoint.index - 1, nowPoint - prevPoint);
                        if (nowPoint.IsLastInItsAxis())
                            return;
                    }

                    containInfo.Add(nowPoint.index, this.end - nowPoint);
                }

                void ForFunction1()
                {
                    using var nowEnumerator = this.contain.GetEnumerator();
                    using var prevEnumerator = this.contain.GetEnumerator();
                    var prevPoint = this.start;
                    nowEnumerator.MoveNext(); //启动至index=0
                    var nowPoint = nowEnumerator.Current;
                    if (nowPoint == null)
                        throw new NullReferenceException();
                    containInfo.Add(nowPoint.index - 1, nowPoint - prevPoint);
                    nowEnumerator.MoveNext();
                    while (true)
                    {
                        prevPoint = nowEnumerator.Current;
                        if (!nowEnumerator.MoveNext())
                            break;
                        nowPoint = nowEnumerator.Current;
                        Debug.Assert(nowPoint != null, nameof(nowPoint) + " != null");
                        if (nowPoint.IsFirstInItsAxis())
                            continue;
                        //拿着区间末端往前找，所以-1
                        containInfo.Add(nowPoint.index - 1, nowPoint - prevPoint);
                        if (nowPoint.IsLastInItsAxis())
                            return;
                    }

                    containInfo.Add(nowPoint.index, this.end - nowPoint);
                }
            }
        }
    }

    /// <summary> <see cref="AxisOld.points" />是端点，<see cref="list" />则与区间一一对应
    /// <para> <see cref="list" />的元素比<see cref="AxisOld.points" />少一个 </para>
    /// </summary>
    /// <typeparam name="T"> </typeparam>
    [Obsolete]
    public class AxisWithListOld<T> : AxisOld
    {
        public readonly List<T> list;

        public AxisWithListOld(List<T> list)
        {
            this.list = list;
        }

        public void Reverse()
        {
            this.points = this.points.Select(point => this.StartPoint + (this.EndPoint - point)).ToList();
            this.list.Reverse();
        }

        public static AxisWithListOld<T> operator +(AxisWithListOld<T> axis, int value)
        {
            return new AxisWithListOld<T>(axis.list)
            {
                points = axis.points.Select(point => point + value).ToList()
            };
        }

        public static AxisWithListOld<T> operator *(AxisWithListOld<T> axis, float value)
        {
            return new AxisWithListOld<T>(axis.list)
            {
                points = axis.points.Select(point => point * value).ToList()
            };
        }

        /// <typeparam name="T1"> <paramref name="axisBackground" />的类型 </typeparam>
        /// <typeparam name="T2"> <paramref name="axisFocus" />的类型 </typeparam>
        [Obsolete]
        public static Dictionary<Interval<T1>, Dictionary<IntervalInfo<T2>, int>>
            GetTwoAxisAssignmentWithInterval<T1, T2>(
                AxisWithListOld<T1> axisBackground, AxisWithListOld<T2> axisFocus)
        {
            var baseIntervals = AssignTwoAxis(axisFocus, axisBackground);
            var intervals = new List<Interval<T1>>();
            foreach (var interval in baseIntervals)
                intervals.Add(new Interval<T1>(interval, axisBackground.list));

            var dictionary = new Dictionary<Interval<T1>, Dictionary<IntervalInfo<T2>, int>>();

            foreach (var interval in intervals)
                dictionary.Add(interval, interval.GetContainInfoWithIntervalInfo(axisFocus.list));
            return dictionary;
        }

        /// <summary> </summary>
        /// <param name="axisBackground"> </param>
        /// <param name="axisFocus"> </param>
        /// <typeparam name="T1"> <paramref name="axisBackground" />的类型 </typeparam>
        /// <typeparam name="T2"> <paramref name="axisFocus" />的类型 </typeparam>
        /// <returns> 双重字典，外层以<typeparamref name="T1" />为Key，内层以<typeparamref name="T2" />为Key </returns>
        [Obsolete]
        public static Dictionary<T1, Dictionary<T2, int>> GetTwoAxisAssignment<T1, T2>(
            AxisWithListOld<T1> axisBackground, AxisWithListOld<T2> axisFocus)
        {
            var baseIntervals = AssignTwoAxis(axisFocus, axisBackground);
            var intervals = new List<Interval<T1>>();
            foreach (var interval in baseIntervals)
                intervals.Add(new Interval<T1>(interval, axisBackground.list));

            var dictionary = new Dictionary<T1, Dictionary<T2, int>>();

            foreach (var interval in intervals)
                dictionary.Add(interval.represent, interval.GetContainInfo(axisFocus.list));
            return dictionary;
        }

        public readonly struct Interval<TRep>
        {
            private readonly Interval _baseInterval;

            public Point Start =>
                this._baseInterval.start;

            public Point End =>
                this._baseInterval.start;

            public int Index =>
                this._baseInterval.index;

            public List<Point> Contain =>
                this._baseInterval.contain;

            public readonly TRep represent;

            [Obsolete]
            public Dictionary<IntervalInfo<TInfo>, int> GetContainInfoWithIntervalInfo<TInfo>(
                IReadOnlyList<TInfo> represents)
            {
                var baseContainInfo = this._baseInterval.GetContainInfo();
                var containInfo = new Dictionary<IntervalInfo<TInfo>, int>();
                foreach (var i in baseContainInfo)
                    containInfo.Add(new IntervalInfo<TInfo>(i.Key, represents), i.Value);

                return containInfo;
            }

            public Dictionary<TInfo, int> GetContainInfo<TInfo>(IReadOnlyList<TInfo> represents)
            {
                var baseContainInfo = this._baseInterval.GetContainInfo();
                var containInfo = new Dictionary<TInfo, int>();
                foreach (var i in baseContainInfo)
                    containInfo.Add(represents[i.Key], i.Value);

                return containInfo;
            }

            public Interval(Interval interval, TRep represent)
            {
                this._baseInterval = interval;
                this.represent = represent;
            }

            public Interval(Interval interval, IReadOnlyList<TRep> list)
            {
                this._baseInterval = interval;
                this.represent = list[interval.index];
            }
        }

        public readonly struct IntervalInfo<TRep>
        {
            public readonly int index;
            public readonly TRep represent;

            public IntervalInfo(int index, TRep represent)
            {
                this.index = index;
                this.represent = represent;
            }

            public IntervalInfo(int index, IReadOnlyList<TRep> list)
            {
                this.index = index;
                this.represent = list[this.index];
            }

            public override int GetHashCode() =>
                this.index;
        }
    }
}