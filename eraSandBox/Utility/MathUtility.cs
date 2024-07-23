using System;
using System.Collections.Generic;

namespace eraSandBox.Utility;

public static class MathUtility
{
    public enum Comparing
    {
        ThisBigger = 1,
        FirstBigger = 1,
        LastSmaller = 1,
        Equal = 0,
        FirstSmaller = -1,
        ThisSmaller = -1,
        LastBigger = -1
    }

    public static bool SameSign(int a, int b)
    {
        return (a ^ b) > 0;
    }

    public static bool SameSign(float a, float b)
    {
        return (a > 0) ^ (b > 0);
    }

    public static Comparing CompareToZero(this float value)
    {
        return (Comparing)value.CompareTo(0);
    }

    public static Comparing CompareAbsTo(this float first, float last)
    {
        return (Comparing)Math.Abs(first).CompareTo(Math.Abs(last));
    }

    /// <summary> 最后一项的序号 </summary>
    public static int LastIndex<T>(this List<T> list)
    {
        return list.Count - 1;
    }

    // public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action) => 
    //     enumerable.ToList().ForEach(action);
}