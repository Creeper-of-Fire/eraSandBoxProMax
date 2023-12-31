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


        /// <summary> </summary>
        /// <param name="vaginaRoute"> </param>
        /// <param name="mentulaRoute"> </param>
        /// <param name="fuckAttitude"> </param>
        public static void FuckBetweenRoute(
            CoitusPatternVaginaRoute vaginaRoute,
            CoitusPatternMentulaRoute mentulaRoute,
            FuckAttitude fuckAttitude = FuckAttitude.ShouldComfort)
        {
            var vaginaParts = vaginaRoute.parts;
            var mentulaParts = mentulaRoute.parts;
            var beingFuckPart = new HashSet<CoitusPatternVaginaPart>();

            var fuckMode = GetFuckMode(vaginaRoute, mentulaRoute, fuckAttitude, out float expansionOrContractionRatio);
            AssignmentMentulaForVagina(vaginaParts, mentulaParts, fuckMode, expansionOrContractionRatio,
                in beingFuckPart);

            foreach (var part in beingFuckPart)
                Fuck(part);

            if (fuckMode == FuckModeType.Destructive)
                Destructive(vaginaParts.Last());
        }

        private static void Fuck(CoitusPatternVaginaPart part)
        {
        }

        private static void Destructive(CoitusPatternVaginaPart part)
        {
        }

        #region FuckBetweenRoute的子函数

        private static FuckModeType GetFuckMode(
            CoitusPatternVaginaRoute vaginaRoute,
            CoitusPatternMentulaRoute mentulaRoute,
            FuckAttitude fuckAttitude,
            out float expansionOrContractionRatio)
        {
            FuckModeType fuckMode;
            expansionOrContractionRatio = 0;
            switch (vaginaRoute.GetLength_ComfortType(mentulaRoute.GetLengthMillimeter()))
            {
                case ComfortType.ImPerceptible:
                    fuckMode = FuckModeType.ImPerceptible;
                    break;
                case ComfortType.Comfortable:
                    expansionOrContractionRatio =
                        (float)mentulaRoute.GetLengthMillimeter() / vaginaRoute.GetLength_PerceptMillimeter();
                    fuckMode = FuckModeType.Comfortable;
                    break;
                case ComfortType.UnComfortable when fuckAttitude == FuckAttitude.ShouldComfort:
                    fuckMode = FuckModeType.NotFullEntryComfortable;
                    break;
                case ComfortType.UnComfortable:
                    expansionOrContractionRatio =
                        (float)mentulaRoute.GetLengthMillimeter() / vaginaRoute.GetLength_UnComfortMillimeter();
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

            return fuckMode;
        }

        /// <summary> 为每个Vagina分配mentula，直到其中一个的长度被耗尽 </summary>
        /// <param name="vaginaParts"> vaginaParts </param>
        /// <param name="mentulaParts"> mentulaParts </param>
        /// <param name="fuckMode"> fuckMode </param>
        /// <param name="expansionOrContractionRatio"> 仅在fuckMode为Comfortable和UnComfortable时需要 </param>
        /// <param name="beingFuckPart"> 传出被Fuck的部分，因为是Vagina包住Mentula，所以只需要传出Vagina就行了 </param>
        private static void AssignmentMentulaForVagina(
            IEnumerable<CoitusPatternVaginaPart> vaginaParts,
            IEnumerable<CoitusPatternMentulaPart> mentulaParts,
            FuckModeType fuckMode,
            float expansionOrContractionRatio,
            in HashSet<CoitusPatternVaginaPart> beingFuckPart
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
                switch (fuckMode)
                {
                    case FuckModeType.ImPerceptible:
                        newVaginaLength = nowVagina.length.PerceptMillimeter();
                        break;
                    case FuckModeType.Comfortable:
                        newVaginaLength = (int)(nowVagina.length.PerceptMillimeter() * expansionOrContractionRatio);
                        break;
                    case FuckModeType.NotFullEntryComfortable:
                        newVaginaLength = nowVagina.length.ComfortMillimeter();
                        break;
                    case FuckModeType.UnComfortable:
                        newVaginaLength = (int)(nowVagina.length.UnComfortMillimeter() * expansionOrContractionRatio);
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

                switch (newMentulaLengthRemain.CompareTo(newVaginaLengthRemain))
                {
                    case 0:
                        //nowMentulaLengthRemain == nowVaginaLengthRemain，刚刚好
                        nowVagina.contents.Add(nowMentula, newMentulaLengthRemain);
                        oldVaginaLengthRemain = 0;
                        oldMentulaLengthRemain = 0;
                        if (nowVaginaEnumerator.MoveNext())
                            return;
                        if (nowMentulaEnumerator.MoveNext())
                            return;
                        break;
                    case -1:
                        //nowMentulaLengthRemain < nowVaginaLengthRemain，Vagina多出一大截
                        nowVagina.contents.Add(nowMentula, newMentulaLengthRemain);
                        oldVaginaLengthRemain = newVaginaLengthRemain - newMentulaLength;
                        //之前减去了newMentulaLengthRemain，所以实际上是：
                        //“newVaginaLengthRemain - newMentulaLengthRemain + oldVaginaLengthRemain”
                        oldMentulaLengthRemain = 0;
                        if (nowMentulaEnumerator.MoveNext())
                            return;
                        break;
                    case 1:
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