using System.Collections.Generic;
using System.Linq;

namespace eraSandBox.Coitus.Part
{
    /// <summary>
    /// 有四种类型：入口、通道、终点、表面。这些类型是根据它“连接”的对象实时获得的。
    /// 有三个形态属性：长度和粗细等级（以及对应的已扩张量），
    /// 还有具体形状； 两个存储属性：容积、表面积；
    /// 两个扩张属性：elasticity弹性（被张开的能力）、plasticity可塑性（恢复原状的能力） 它存储“连接”的方法是“连接口对象”
    /// </summary>
    public class CoitusPatternVaginaPart : CoitusPart
    {
        ///<value>Fucker</value>
        public List<CoitusPatternMentulaPart> contents;
        
        /// <summary>结构作为节点的类型</summary>
        public enum CoitusLinkType
        {
            /// <summary>暂未赋值</summary>
            Null = 0,

            /// <summary>腔道的底部</summary>
            End = 1,

            /// <summary>腔道的入口</summary>
            Entrance = 2,

            /// <summary>通道部分</summary>
            Corridor = 3,

            /// <summary>表面</summary>
            Surface = 4,

            /// <summary>标志“未启用”的状态</summary>
            Hidden = 5
        }

        public readonly List<CoitusPatternVaginaPart> links;
        public CoitusLinkType coitusLinkType;
        public Coitus_MatterContainedData Content;

        public int elasticityLevel;
        public int plasticityLevel;

        /// <summary>初始化</summary>
        /// <param name="pawn"></param>
        /// <param name="links"></param>
        /// <param name="coitusLinkType"></param>
        public CoitusPatternVaginaPart(TestPawn pawn, List<CoitusPatternVaginaPart> links,
            CoitusLinkType coitusLinkType = CoitusLinkType.Null) : base(pawn)
        {
            this.length =
                new CoitusScalePatternVagina(CalculateBaseLength(pawn, this.lengthPercentage), this.lengthLevel);
            this.diameter =
                new CoitusScalePatternVagina(CalculateBaseDiameter(pawn, this.lengthPercentage), this.diameterLevel);
            this.links = new List<CoitusPatternVaginaPart>(links);
            this.coitusLinkType = coitusLinkType;
            UpdateCoitusLinkType();
        }

        public new CoitusScalePatternVagina length
        {
            get => (CoitusScalePatternVagina)base.length;
            private set => base.length = value;
        }

        public new CoitusScalePatternVagina diameter
        {
            get => (CoitusScalePatternVagina)base.diameter;
            private set => base.diameter = value;
        }

        /**
         * <summary>
         * 更新节点类型 <see cref="CoitusLinkType" /> ：
         * <para><see cref="CoitusLinkType.Surface" /> -表面比较特殊，通过其他函数来设定</para>
         * <see cref="CoitusLinkType.Hidden" /> -不与任何东西连接，则为隐藏
         * <para><see cref="CoitusLinkType.End" /> -如果不是表面又只有一个相连，则为底部</para>
         * <see cref="CoitusLinkType.Entrance" /> -如果和外部相连，则为入口
         * <para><see cref="CoitusLinkType.Corridor" /> -其他情况，则为通道</para>
         * </summary>
         */
        public void UpdateCoitusLinkType()
        {
            this.coitusLinkType = UpdateCoitusLinkType_GetType();

            CoitusLinkType UpdateCoitusLinkType_GetType()
            {
                if (this.coitusLinkType == CoitusLinkType.Surface)
                    return CoitusLinkType.Surface; //Surface比较特殊，是通过其他函数来设定的
                if (this.links.Count == 0)
                    return CoitusLinkType.Hidden; //不与任何东西连接，则为隐藏
                if (this.links.Count == 1)
                    return CoitusLinkType.End; //如果不是Surface又只有一个东西相连，则为底部
                if (this.links.Any(link => link.coitusLinkType == CoitusLinkType.Surface))
                    return CoitusLinkType.Entrance; //如果和外部相连，则入口
                return CoitusLinkType.Corridor; //其他情况
            }
        }

        public struct Geometry
        {
            public string name;


            public Geometry(string name)
            {
                this.name = name;
            }

            //球体
            public static Geometry sphere => new Geometry();

            //椭球体
            public static Geometry spheroid => new Geometry();

            //圆柱体
            public static Geometry cylinder => new Geometry();
        }
    }

    public class Coitus_MatterContainedData
    {
    }
}