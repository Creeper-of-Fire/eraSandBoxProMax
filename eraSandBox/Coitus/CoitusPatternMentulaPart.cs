using System;
using System.Linq;

namespace eraSandBox.Coitus
{
    /// <summary>
    /// 
    /// 
    /// </summary>
    public class CoitusPatternMentulaPart : CoitusPart
    {
        public void Agere() //启动效果，包括Vagina和Mentula效果
        {
        }

        public CoitusPatternVaginaRoute ChooseVagina()
        {
            var routes = pawn.System.totalVaginaRoutes;
            Random r = new Random();
            return routes.ElementAt(r.Next(routes.Count));
        }

        public void FuckIn()
        {
            
        }

        public CoitusPatternMentulaPart(TestPawn pawn) : base(pawn)
        {
        }
    }
}