

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnassignedField.Global

namespace eraSandBox.Coitus
{
    public class CoitusAspectDef : Def
    {
        /// <value> 每个CoitusPart都有不同的lengthTenThousandth，其数值由xml决定 </value>
        public int lengthTenThousandth; //单位：万分之一;大概在10%(即1000)数量级，比较合适
        
        public bool ableToTransform;
        
        public int diameterLevel;

        public int lengthLevel;
        
        public int elasticityLevel;
        
        public int plasticityLevel;

        public int tighticityLevel;
    }

    public class Def
    {
        public string defName;
    }
}