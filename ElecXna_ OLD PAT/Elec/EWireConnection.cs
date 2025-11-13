using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QM.Util;

namespace QM.Elec
{
    public class EWireConnection
    {
        Color m_ColorDebug;
        static int ColorCount = 0;
        public List<ElecNode> m_LNode = new List<ElecNode>();
        public int m_CenteredNode = 0;

        public float m_Potentiel = 0.0f;

        //        : base() { m_IsWire = true; }
        public EWireConnection()
        {
            ColorCount++;
            m_ColorDebug = new Color((byte) ((ColorCount*100)%255), (byte)((ColorCount*200)%255), (byte)((ColorCount*50)%255));
        }

        public void ComputeCenter()
        {
            Vector2 V = new Vector2();
            foreach (ElecNode en in m_LNode)
            {
                V.X += en.m_Pos.X;
                V.Y += en.m_Pos.Y;
            }
            V.X /= m_LNode.Count();
            V.Y /= m_LNode.Count();
            float DistMin = 100000000.0f;
            for (int i=0;i<m_LNode.Count();i++)
            {
                float u = V.X - m_LNode[i].m_Pos.X;
                float v = V.Y - m_LNode[i].m_Pos.Y;
                float dist = (u*u+v*v);
                if(dist<DistMin)
                {
                    DistMin = dist;
                    m_CenteredNode = i;
                 }
            }
        }

        public void DrawDebug(Color[] _BlockTextureData, int sx, int sy)
        {
            foreach (ElecNode en in m_LNode)
            {
                ColorManager.DrawDisk(_BlockTextureData, sx, sy, en.m_Pos, 3.5f, en.m_EWireConnection.m_ColorDebug);
            }
        }

        virtual public void DrawText(SpriteBatch spriteBatch)
        {
            if (m_CenteredNode>=m_LNode.Count())
                return;

            ElecNode en = m_LNode[m_CenteredNode];
            String HowMuch = m_Potentiel.ToString("N3") + " V";
                Vector2 Pos = new Vector2();
                Pos.X = en.m_Pos.X;
                Pos.Y = en.m_Pos.Y;
                    Pos.X += 12;
                    Pos.Y -= 20;
                GraphicManager.DrawString(FontManager.Font[3], HowMuch, Pos, Color.White, 0, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0.5f);
        }
    }
}
