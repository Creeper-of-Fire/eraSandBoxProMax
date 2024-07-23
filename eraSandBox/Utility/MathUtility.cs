using System;
using System.Collections;
using System.Collections.Generic;
using eraSandBox.Utility.GameThing;

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

    public static List<T> AddAll<T>(this List<T> originList, IEnumerable<T> needAddList)
    {
        originList.AddRange(needAddList);
        return originList;
    }



    public readonly struct TwoAction<T>(Action<T> action1, Action<T> action2)
    {
        public readonly Action<T> action1 = action1;
        public readonly Action<T> action2 = action2;

        public static TwoAction<T> Combine(params TwoAction<T>[] twoActions)
        {
            return new TwoAction<T>(t =>
            {
                foreach (var twoAction in twoActions)
                    twoAction.action1.Invoke(t);
            }, t =>
            {
                foreach (var twoAction in twoActions)
                    twoAction.action2.Invoke(t);
            });
        }

        public TwoAction<T> CombineWith(params TwoAction<T>[] twoActions)
        {
            var self = this;
            return new TwoAction<T>(t =>
            {
                self.action1.Invoke(t);
                foreach (var twoAction in twoActions)
                    twoAction.action1.Invoke(t);
            }, t =>
            {
                self.action2.Invoke(t);
                foreach (var twoAction in twoActions)
                    twoAction.action2.Invoke(t);
            });
        }

        public static TwoAction<T> Combine(List<TwoAction<T>> twoActions) => Combine(twoActions.ToArray());
        
        public static TwoAction<TAction> MapToProcessTwoAction<TAction, TList>(
            List<TList> enumerable,
            Func<TList, TwoAction<TAction>> twoActionMaker)
        {
            return new TwoAction<TAction>(t =>
            {
                foreach (var tInList in enumerable)
                {
                    twoActionMaker.Invoke(tInList).action1.Invoke(t);
                }
            }, t =>
            {
                foreach (var tInList in enumerable)
                {
                    twoActionMaker.Invoke(tInList).action2.Invoke(t);
                }
            });
        }

        // public Action<T> action1 { get; } = action1;
        // public Action<T> action2 { get; } = action2;
    }
}