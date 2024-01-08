using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace eraSandBox.Coitus.Part
{
    /// <summary> 所有关于Fuck的内容都放在这里 </summary>
    public static class FuckUtility
    {
        public enum ComfortType
        {
            ImPerceptible = FuckModeType.ImPerceptible,
            Comfortable = FuckModeType.Comfortable,
            UnComfortable = FuckModeType.UnComfortable,
            Destructive = FuckModeType.Destructive
        }

        public enum ExpandType
        {
            ImPerceptible = FuckModeType.ImPerceptible,
            Comfortable = FuckModeType.Comfortable,
            UnComfortable = FuckModeType.UnComfortable
        }

        public enum FuckAttitude
        {
            ShouldComfort = 0,
            NoNeedComfortButNoDestruct = 1,
            Free = 2
        }

        /// <summary> 用在<see cref="FuckBetweenRoute" />内部，用来切换运行方式 </summary>
        public enum FuckModeType
        {
            /// <summary> 太小了，感觉不到 </summary>
            ImPerceptible = 0,

            /// <summary> 正常的舒适尺度 </summary>
            Comfortable = 1,

            /// <summary> 扩张，导致不适 </summary>
            UnComfortable = 2,

            /// <summary> 不完全插入，保持在舒适的尺度 </summary>
            NotFullEntryComfortable = 3,

            /// <summary> 破坏性，会导致末端受伤 </summary>
            Destructive = 4,

            /// <summary> 如果有两端的话，应该是捅出/两边贯通而非破坏 </summary>
            Implement = 5,

            /// <summary> 不完全插入，但是进行扩张 </summary>
            NotFullEntryUnComfortable = 6
        }


        public static void FuckBetweenRoute(
            CoitusVaginaRoute vaginaRoute,
            CoitusMentulaRoute mentulaRoute,
            int mentulaInsertLength,
            FuckAttitude fuckAttitude = FuckAttitude.ShouldComfort)
        {
            var vaginaParts = vaginaRoute.parts;
            var mentulaParts = mentulaRoute.parts;
            var beingFuckPart = new HashSet<CoitusVaginaPart>();

            var fuckMode = GetFuckMode(vaginaRoute, mentulaInsertLength, fuckAttitude);
            AssignmentMentulaForVagina(vaginaParts, mentulaParts, fuckMode,
                in beingFuckPart);

            foreach (var part in beingFuckPart)
                Fuck(part);

            if (fuckMode == FuckModeType.Destructive)
                Destructive(vaginaParts.Last());
        }

        private static void Fuck(CoitusVaginaPart part)
        {
        }

        private static void Destructive(CoitusVaginaPart part)
        {
        }

        public static void FuckBetweenRoute(
            CoitusVaginaRoute vaginaRoute,
            CoitusMentulaRoute mentulaRoute,
            FuckAttitude fuckAttitude = FuckAttitude.ShouldComfort)
        {
            FuckBetweenRoute(vaginaRoute, mentulaRoute, mentulaRoute.GetLengthMillimeter(), fuckAttitude);
        }

        #region FuckBetweenRoute的子函数

        private static FuckModeType GetFuckMode(
            CoitusVaginaRoute vaginaRoute,
            int mentulaInsertLength,
            FuckAttitude fuckAttitude)
        {
            FuckModeType fuckMode;
            switch (vaginaRoute.length.ComfortType(mentulaInsertLength))
            {
                case ComfortType.ImPerceptible:
                    fuckMode = FuckModeType.ImPerceptible;
                    break;
                case ComfortType.Comfortable:
                    fuckMode = FuckModeType.Comfortable;
                    break;
                case ComfortType.UnComfortable when fuckAttitude == FuckAttitude.ShouldComfort:
                    fuckMode = FuckModeType.NotFullEntryComfortable;
                    break;
                case ComfortType.UnComfortable:
                    fuckMode = FuckModeType.UnComfortable;
                    break;
                case ComfortType.Destructive when fuckAttitude == FuckAttitude.ShouldComfort:
                    fuckMode = FuckModeType.NotFullEntryComfortable;
                    break;
                case ComfortType.Destructive when fuckAttitude == FuckAttitude.NoNeedComfortButNoDestruct:
                    fuckMode = FuckModeType.NotFullEntryUnComfortable;
                    break;
                case ComfortType.Destructive:
                    if (vaginaRoute.HasTwoDirection())
                        fuckMode = FuckModeType.Destructive;
                    else
                        fuckMode = FuckModeType.Implement;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            vaginaRoute.length.expansionOrContractionRatio.Add(fuckMode switch
            {
                FuckModeType.ImPerceptible => CoitusUtility.ImPerceptRATIO,
                FuckModeType.Comfortable => ExpansionOrContractionRatio(
                    vaginaRoute.length,
                    mentulaInsertLength,
                    vaginaRoute.length.PerceptMillimeter()),
                FuckModeType.NotFullEntryComfortable => 0,
                FuckModeType.UnComfortable => ExpansionOrContractionRatio(
                    vaginaRoute.length,
                    mentulaInsertLength,
                    vaginaRoute.length.UnComfortMillimeter()),
                FuckModeType.NotFullEntryUnComfortable => CoitusUtility.UnComfortRATIO,
                FuckModeType.Destructive => CoitusUtility.UnComfortRATIO,
                FuckModeType.Implement => CoitusUtility.UnComfortRATIO,
                _ => throw new ArgumentOutOfRangeException()
            });

            foreach (var part in vaginaRoute.parts)
                part.length.expansionOrContractionRatio.Add(vaginaRoute.length.expansionOrContractionRatio.RatioMax());

            return fuckMode;
        }

        public static float ExpansionOrContractionRatio(IVaginaScale vagina, int mentulaInsert, int total) =>
            (float)(mentulaInsert - vagina.OriginalMillimeter())
            / Math.Abs(total - vagina.OriginalMillimeter());
        //使用Abs。这样的话，当输入为Percept时，就会返回负数

        [Obsolete]
        private static void AssignmentMentulaForVagina(
            IEnumerable<CoitusVaginaPart> vaginaParts,
            IEnumerable<CoitusMentulaPart> mentulaParts,
            FuckModeType fuckMode,
            in HashSet<CoitusVaginaPart> beingFuckPart
        )
        {
            if (fuckMode == FuckModeType.NotFullEntryComfortable || fuckMode == FuckModeType.NotFullEntryUnComfortable)
            {
                vaginaParts = vaginaParts.Reverse();
                mentulaParts = mentulaParts.Reverse();
            }

            using var nowVaginaEnumerator = vaginaParts.GetEnumerator();
            using var nowMentulaEnumerator = mentulaParts.GetEnumerator();
            nowVaginaEnumerator.MoveNext();
            nowMentulaEnumerator.MoveNext();
            var nowVagina = nowVaginaEnumerator.Current;
            var nowMentula = nowMentulaEnumerator.Current;
            int oldVaginaLengthRemain = 0;
            int oldMentulaLengthRemain = 0;

            while (true)
            {
                Debug.Assert(nowVagina != null, "nowVagina.Current == null");
                Debug.Assert(nowMentula != null, "nowMentula.Current == null");

                int newVaginaLength;
                int newMentulaLength = nowMentula.length.valueMillimeter;
                //TODO 修改为：nowVagina.length自己计算自己的ComfortType和尺寸(因为要考虑多个插入的情况)。或者在之前的计算中就考虑多个插入的情况
                switch (fuckMode)
                {
                    case FuckModeType.ImPerceptible:
                        newVaginaLength = nowVagina.length.PerceptMillimeter();
                        break;
                    case FuckModeType.Comfortable:
                        newVaginaLength = nowVagina.length.OriginalMillimeter() +
                                          nowVagina.length.ToPerceptMillimeter() *
                                          nowVagina.length.expansionOrContractionRatio;
                        break;
                    case FuckModeType.NotFullEntryComfortable:
                        newVaginaLength = nowVagina.length.ComfortMillimeter();
                        break;
                    case FuckModeType.UnComfortable:
                        newVaginaLength = nowVagina.length.OriginalMillimeter() +
                                          nowVagina.length.ToUnComfortMillimeter() *
                                          nowVagina.length.expansionOrContractionRatio;
                        break;
                    case FuckModeType.NotFullEntryUnComfortable:
                    case FuckModeType.Destructive:
                    case FuckModeType.Implement:
                    default:
                        newVaginaLength = nowVagina.length.UnComfortMillimeter();
                        break;
                    /*NotFullEntryUnComfortable、Destructive和Implement区别在于：
                    Destructive会伤害最后一个Vagina，NotFullEntryUnComfortable则是倒序迭代的*/
                }

                int newVaginaLengthRemain = newVaginaLength - oldVaginaLengthRemain;
                int newMentulaLengthRemain = newMentulaLength - oldMentulaLengthRemain;

                beingFuckPart.Add(nowVagina);

                switch ((MathUtility.Comparing)newMentulaLengthRemain.CompareTo(newVaginaLengthRemain))
                {
                    case MathUtility.Comparing.Equal:
                        //nowMentulaLengthRemain == nowVaginaLengthRemain，刚刚好
                        nowVagina.contents.Add(nowMentula, newMentulaLengthRemain);
                        oldVaginaLengthRemain = 0;
                        oldMentulaLengthRemain = 0;
                        if (nowVaginaEnumerator.MoveNext())
                            return;
                        if (nowMentulaEnumerator.MoveNext())
                            return;
                        break;
                    case MathUtility.Comparing.LastBigger:
                        //nowMentulaLengthRemain < nowVaginaLengthRemain，Vagina多出一大截
                        nowVagina.contents.Add(nowMentula, newMentulaLengthRemain);
                        oldVaginaLengthRemain = newVaginaLengthRemain - newMentulaLength;
                        //之前减去了newMentulaLengthRemain，所以实际上是：
                        //“newVaginaLengthRemain - newMentulaLengthRemain + oldVaginaLengthRemain”
                        oldMentulaLengthRemain = 0;
                        if (nowMentulaEnumerator.MoveNext())
                            return;
                        break;
                    case MathUtility.Comparing.FirstBigger:
                        //nowMentulaLengthRemain > nowVaginaLengthRemain，Mentula多出一大截
                        nowVagina.contents.Add(nowMentula, newVaginaLengthRemain);
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

        #endregion
    }
}