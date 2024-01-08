using System;

namespace eraSandBox.Coitus
{
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

        public static bool SameSign(int a, int b) =>
            (a ^ b) > 0;

        public static bool SameSign(float a, float b) =>
            (a > 0) ^ (b > 0);

        public static Comparing CompareToZero(this float value) =>
            (Comparing)value.CompareTo(0);

        public static Comparing CompareAbsTo(this float first, float last) =>
            (Comparing)Math.Abs(first).CompareTo(Math.Abs(last));
    }
}