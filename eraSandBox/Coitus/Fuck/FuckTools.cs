using System;
using System.Collections.Generic;
using System.Linq;

namespace eraSandBox.Coitus
{
    /// <summary> 所有关于Fuck的内容都放在这里 </summary>
    public static partial class FuckUtility
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


        #region FuckBetweenRoute的子函数

        /// <summary> 不使用部件，只使用总尺寸 </summary>
        /// <param name="summableVaginaRoute"> </param>
        /// <param name="mentulaInsertLength"> </param>
        /// <param name="fuckAttitude"> </param>
        /// <returns> </returns>
        /// <exception cref="ArgumentOutOfRangeException"> </exception>
        private static FuckModeType GetFuckModeAndExpansionOrContractionRatio<T>(
            T vaginaRoute,
            int mentulaInsertLength,
            FuckAttitude fuckAttitude)
            where T : CoitusVaginaRouteScale, IVaginaScale
        {
            FuckModeType fuckMode;
            switch (vaginaRoute.ComfortType(mentulaInsertLength))
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
                    if (vaginaRoute.parent.HasTwoDirection())
                        fuckMode = FuckModeType.Destructive;
                    else
                        fuckMode = FuckModeType.Implement;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            vaginaRoute.ExpansionOrContractionRatio.Add(fuckMode switch
            {
                FuckModeType.ImPerceptible => CoitusUtility.ImPerceptRatio,
                FuckModeType.Comfortable => ExpansionOrContractionRatio(
                    vaginaRoute,
                    mentulaInsertLength,
                    vaginaRoute.PerceptMillimeter()),
                FuckModeType.NotFullEntryComfortable => 0,
                FuckModeType.UnComfortable => ExpansionOrContractionRatio(
                    vaginaRoute,
                    mentulaInsertLength,
                    vaginaRoute.UnComfortMillimeter()),
                FuckModeType.NotFullEntryUnComfortable => CoitusUtility.UnComfortRatio,
                FuckModeType.Destructive => CoitusUtility.UnComfortRatio,
                FuckModeType.Implement => CoitusUtility.UnComfortRatio,
                _ => throw new ArgumentOutOfRangeException()
            });

            foreach (var part in vaginaRoute.parent.parts)
                part.length.ExpansionOrContractionRatio.Add(vaginaRoute.ExpansionOrContractionRatio
                    .RatioMax());

            return fuckMode;
        }

        private static float ExpansionOrContractionRatio(IVaginaScale vagina, int mentulaInsert, int total) =>
            (float)(mentulaInsert - vagina.OriginalMillimeter())
            / Math.Abs(total - vagina.OriginalMillimeter());
        //使用Abs。这样的话，当输入为Percept时，就会返回负数

        private static void AssignVaginaForMentula(
            CoitusVaginaRoute vaginaRoute,
            CoitusMentulaRoute mentulaRoute,
            bool reverse,
            in HashSet<CoitusMentulaAspect> fuckerPart
        )
        {
            IEnumerable<CoitusVaginaAspect> vaginaIntervals = vaginaRoute.parts;
            IEnumerable<CoitusMentulaAspect> mentulaIntervals = mentulaRoute.parts;
            if (reverse)
            {
                vaginaIntervals = vaginaIntervals.Reverse();
                mentulaIntervals = mentulaIntervals.Reverse();
            }

            //var Assignment = new Dictionary<CoitusMentulaAspect, Dictionary<CoitusVaginaAspect, int>>();
            using var nowVaginaEnumerator = vaginaIntervals.GetEnumerator();
            using var nowMentulaEnumerator = mentulaIntervals.GetEnumerator();
            nowVaginaEnumerator.MoveNext();
            nowMentulaEnumerator.MoveNext();
            var nowVagina = nowVaginaEnumerator.Current;
            var nowMentula = nowMentulaEnumerator.Current;
            int oldVaginaLengthRemain = 0;
            int oldMentulaLengthRemain = 0;
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
                int newVaginaLength = nowVagina.length.NowScaleMillimeter();
                int newMentulaLength = nowMentula.length.OriginalMillimeter();

                int newVaginaLengthRemain = newVaginaLength - oldVaginaLengthRemain;
                int newMentulaLengthRemain = newMentulaLength - oldMentulaLengthRemain;

                fuckerPart.Add(nowMentula);

                switch ((MathUtility.Comparing)newMentulaLengthRemain.CompareTo(newVaginaLengthRemain))
                {
                    case MathUtility.Comparing.Equal: //MentulaRemain == VaginaRemain，刚刚好
                        Assignment(nowMentula, nowVagina, newVaginaLengthRemain);
                        oldVaginaLengthRemain = 0;
                        oldMentulaLengthRemain = 0;
                        if (!nowVaginaEnumerator.MoveNext())
                            return;
                        if (!nowMentulaEnumerator.MoveNext())
                            return;
                        break;
                    case MathUtility.Comparing.LastBigger: //MentulaRemain > VaginaRemain，Mentula多出一大截
                        Assignment(nowMentula, nowVagina, newVaginaLengthRemain);
                        oldVaginaLengthRemain = 0;
                        oldMentulaLengthRemain = newMentulaLengthRemain - newVaginaLength;
                        //之前减去了newVaginaLengthRemain，所以实际上是：
                        //“newMentulaLengthRemain - newVaginaLengthRemain + oldMentulaLengthRemain”
                        if (!nowVaginaEnumerator.MoveNext())
                            return;
                        break;
                    case MathUtility.Comparing.FirstBigger: //MentulaRemain < VaginaRemain，Vagina多出一大截
                        Assignment(nowMentula, nowVagina, newMentulaLengthRemain);
                        oldVaginaLengthRemain = newVaginaLengthRemain - newMentulaLength;
                        //之前减去了newMentulaLengthRemain，所以实际上是：
                        //“newVaginaLengthRemain - newMentulaLengthRemain + oldVaginaLengthRemain”
                        oldMentulaLengthRemain = 0;
                        if (!nowMentulaEnumerator.MoveNext())
                            return;
                        break;
                }
            }

            void Assignment(CoitusMentulaAspect mentula, CoitusVaginaAspect vagina, int scale)
            {
                nowMentula.insert.Add(nowVagina, scale);
            }
        }

        #endregion
    }
}