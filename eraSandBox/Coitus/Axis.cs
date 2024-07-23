using System;
using System.Collections.Generic;
using System.Linq;
using eraSandBox.Utility;

namespace eraSandBox.Coitus;

/// <summary> 核心代码为：<see cref="AssignTwoAxis" />和<see cref="" /> </summary>
[Obsolete]
public class Axis
{
    protected List<Point> points = new();

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
        var oldVaginaLengthRemain = 0;
        var oldMentulaLengthRemain = 0;
        if (nowVagina == null)
            throw new NullReferenceException();
        if (nowMentula == null)
            throw new NullReferenceException();

        while (true)
        {
            nowVagina = nowVaginaEnumerator.Current;
            nowMentula = nowMentulaEnumerator.Current;
            if (nowVagina == null)
                throw new NullReferenceException();
            if (nowMentula == null)
                throw new NullReferenceException();
            var newVaginaLength = nowVagina.Length;
            var newMentulaLength = nowMentula.Length;
            var newVaginaLengthRemain = newVaginaLength - oldVaginaLengthRemain;
            var newMentulaLengthRemain = newMentulaLength - oldMentulaLengthRemain;
            switch ((MathUtility.Comparing)newMentulaLengthRemain.CompareTo(newVaginaLengthRemain))
            {
                case MathUtility.Comparing.Equal:
                    //nowMentulaLengthRemain == nowVaginaLengthRemain，刚刚好
                    assignment[nowVagina].Add(nowMentula.index, newMentulaLengthRemain);
                    oldVaginaLengthRemain = 0;
                    oldMentulaLengthRemain = 0;
                    if (!nowVaginaEnumerator.MoveNext())
                        return assignment;
                    if (!nowMentulaEnumerator.MoveNext())
                        return assignment;
                    break;
                case MathUtility.Comparing.LastBigger:
                    //nowMentulaLengthRemain < nowVaginaLengthRemain，Vagina多出一大截
                    assignment[nowVagina].Add(nowMentula.index, newMentulaLengthRemain);
                    oldVaginaLengthRemain = newVaginaLengthRemain - newMentulaLength;
                    //之前减去了newMentulaLengthRemain，所以实际上是：
                    //“newVaginaLengthRemain - newMentulaLengthRemain + oldVaginaLengthRemain”
                    oldMentulaLengthRemain = 0;
                    if (!nowMentulaEnumerator.MoveNext())
                        return assignment;
                    break;
                case MathUtility.Comparing.FirstBigger:
                    //nowMentulaLengthRemain > nowVaginaLengthRemain，Mentula多出一大截
                    assignment[nowVagina].Add(nowMentula.index, newVaginaLengthRemain);
                    oldVaginaLengthRemain = 0;
                    oldMentulaLengthRemain = newMentulaLengthRemain - newVaginaLength;
                    //之前减去了newVaginaLengthRemain，所以实际上是：
                    //“newMentulaLengthRemain - newVaginaLengthRemain + oldMentulaLengthRemain”
                    if (!nowVaginaEnumerator.MoveNext())
                        return assignment;
                    break;
            }
        }
    }

    public void Append(int x)
    {
        this.points.Add(new Point(x, this));
    }

    public void MakeAxis(IEnumerable<int> distanceList)
    {
        var x = 0;
        this.Append(x);
        foreach (var distance in distanceList)
        {
            x += distance;
            this.Append(x);
        }

        this.Sort();
    }

    public void Sort()
    {
        this.points.Sort();
        for (var index = 0; index < this.points.Count; index++)
            this.points[index].index = index;
    }

    public Axis Clone()
    {
        return new Axis()
        {
            points = this.points.ToArray().ToList()
        };
    }


    public static Axis operator +(Axis axis, int value)
    {
        return new Axis
        {
            points = axis.points.Select(point => point + value).ToList()
        };
    }

    public static Axis operator *(Axis axis, float value)
    {
        return new Axis
        {
            points = axis.points.Select(point => point * value).ToList()
        };
    }

    public class Point : IComparable<Point>, ICloneable
    {
        public readonly Axis owner;
        public readonly int x;
        public int index;

        public Point(int x, Axis owner)
        {
            this.x = x;
            this.owner = owner;
        }

        public object Clone()
        {
            return new Point(this.x, this.owner)
            {
                index = this.index
            };
        }

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
        public Point NextPoint()
        {
            return this.IsLastInItsAxis() ? this : this.owner.points[this.index + 1];
        }

        /// <returns> 返回上一个；当为第一个时，返回自身 </returns>
        public Point PrevPoint()
        {
            return this.IsFirstInItsAxis() ? this : this.owner.points[this.index - 1];
        }

        public bool IsLastInItsAxis()
        {
            return this.index == this.owner.points.LastIndex();
        }

        public bool IsFirstInItsAxis()
        {
            return this.index == 0;
        }

        public static Point operator +(Point point, int value)
        {
            return new Point(point.x + value, point.owner);
        }

        public static Point operator *(Point point, int value)
        {
            return new Point(point.x * value, point.owner);
        }

        public static Point operator *(Point point, float value)
        {
            return new Point((int)(point.x * value), point.owner);
        }

        public static int operator -(Point pointA, Point pointB)
        {
            return pointA.x - pointB.x;
        }

        public static bool operator <=(Point pointA, Point pointB)
        {
            return pointA.x <= pointB.x;
        }

        public static bool operator >=(Point pointA, Point pointB)
        {
            return pointA.x >= pointB.x;
        }

        public static bool operator <(Point pointA, Point pointB)
        {
            return pointA.x < pointB.x;
        }

        public static bool operator >(Point pointA, Point pointB)
        {
            return pointA.x > pointB.x;
        }

        public Point Clone(int xValue)
        {
            return new Point(xValue, this.owner)
            {
                index = this.index
            };
        }
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

        public void AddPoint(Point point)
        {
            this.contain.Add(point);
        }

        public override int GetHashCode()
        {
            return this.index;
        }
    }
}

/// <summary>
///     <see cref="Axis.points" />是端点，<see cref="list" />则与区间一一对应
///     <para> <see cref="list" />的元素比<see cref="Axis.points" />少一个 </para>
/// </summary>
/// <typeparam name="T"> </typeparam>
[Obsolete]
public class AxisWithList<T> : Axis
{
    public readonly List<T> list;

    public AxisWithList(List<T> list)
    {
        this.list = list;
    }

    public void Reverse()
    {
        this.points = this.points.Select(point => this.StartPoint + (this.EndPoint - point)).ToList();
        this.list.Reverse();
    }

    public static AxisWithList<T> operator +(AxisWithList<T> axis, int value)
    {
        return new AxisWithList<T>(axis.list)
        {
            points = axis.points.Select(point => point + value).ToList()
        };
    }

    public static AxisWithList<T> operator *(AxisWithList<T> axis, float value)
    {
        return new AxisWithList<T>(axis.list)
        {
            points = axis.points.Select(point => point * value).ToList()
        };
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

        public override int GetHashCode()
        {
            return this.index;
        }
    }
}

[Obsolete]
public interface IWithAxis<T>
{
    public AxisWithList<T> BaseAxis { get; }
    public void MakeAxis();
}