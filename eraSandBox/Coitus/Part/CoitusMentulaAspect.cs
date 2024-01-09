using System.Collections.Generic;
using eraSandBox.Coitus;

namespace eraSandBox.Coitus
{
    /// <summary> </summary>
    [NeedDefInitialize]
    public class CoitusMentulaAspect : CoitusAspect
    {
        /// <summary> 有多个，但是一定同属于一条CoitusVaginaRoute,int为插入长度 </summary>
        public readonly Dictionary<CoitusVaginaAspect, int> insert = new Dictionary<CoitusVaginaAspect, int>();

        [NeedInitialize]
        public readonly List<CoitusVaginaAspect> links;


        public void Agere() //启动效果，包括Vagina和Mentula效果
        {
        }

        public CoitusMentulaAspect(Part owner, List<CoitusVaginaAspect> links) :
            base(owner)
        {
            this.links = links;
        }
    }
}