using System.Collections.Generic;

namespace eraSandBox.Coitus.Part
{
    /// <summary> </summary>
    public class CoitusMentulaPart : CoitusPart
    {
        /// <summary> 有多个，但是一定同属于一条CoitusVaginaRoute,int为插入长度 </summary>
        public readonly Dictionary<CoitusVaginaPart, int> insert = new Dictionary<CoitusVaginaPart, int>();

        /// <value> </value>
        public bool transformAble;

        public CoitusMentulaPart(TestPawn pawn) : base(pawn)
        {
        }


        public void Agere() //启动效果，包括Vagina和Mentula效果
        {
        }
    }
}