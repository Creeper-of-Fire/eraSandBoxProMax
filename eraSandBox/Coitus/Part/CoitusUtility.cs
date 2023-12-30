using System.Collections.Generic;
using System.Linq;

namespace eraSandBox.Coitus.Part
{
    public static class CoitusUtility
    {
        public static void Fuck(CoitusPatternVaginaRoute vaginaRoute, CoitusPatternMentulaRoute mentulaRoute)
        {
            float vaginaLength = vaginaRoute.GetComfortLength();
            float mentulaLength = mentulaRoute.GetLength();
            var vaginaParts = new List<CoitusPatternVaginaPart>();
            var mentulaParts = new List<CoitusPatternMentulaPart>();
            if (vaginaLength >= mentulaLength) //mentula可以完全进入
            {
                mentulaParts = mentulaRoute.parts.ToList();
                foreach (CoitusPatternVaginaPart part in vaginaRoute.parts)
                {
                    if (vaginaLength <= part.length.Comfort())
                        break;
                    vaginaLength -= part.length.Comfort();
                    vaginaParts.Add(part);
                }
            }
            else //vaginaLength < mentulaLength，mentula太大了无法完全进入
            {
                vaginaParts = vaginaRoute.parts.ToList();
                foreach (CoitusPatternMentulaPart part in mentulaRoute.parts)
                {
                    if (mentulaLength <= part.length.value)
                        break;
                    mentulaLength -= part.length.value;
                    mentulaParts.Add(part);
                }
            }

            foreach (CoitusPatternMentulaPart part in mentulaParts)
                part.Agere();
            foreach (CoitusPatternVaginaPart part in vaginaParts)
            {
                part.Agere();
                Fuck(part);
            }
        }

        public static void Fuck(CoitusPatternVaginaPart part)
        {
        }
    }
}