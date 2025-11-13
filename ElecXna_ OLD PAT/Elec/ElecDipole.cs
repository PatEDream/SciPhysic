using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QM.Util;

namespace QM.Elec
{
    public class ElecDipole
    {
        public ElecNode m_Start = new ElecNode();
        public ElecNode m_End = new ElecNode();

        public float m_Potentiel = 0.0f;
        public float m_Intensity = 0.0f;

        public Boolean m_IsWire = false;
        public Boolean m_IsLight = false;
        public int m_DurtyCount = 0;
        public static float m_ThresholdIntensity = 0.05f;

        public Color m_ColorDipole = Color.BurlyWood;

        virtual public void Init()
        {
            m_Potentiel = 0.0f;
            m_Intensity = 0.0f;
            m_Start.Init();
            m_End.Init();
        }

        virtual public void DoOneStep(float _PotentialToIntensityConvertor)
        {
        }

        public Boolean IsON()
        {
            return (Math.Abs(m_Intensity) > m_ThresholdIntensity);
        }

        public void DrawDebug(Color[] _BlockTextureData, int sx, int sy)
        {
            ColorManager.DrawAALine(_BlockTextureData, sx, sy, m_Start.m_Pos, m_End.m_Pos, m_ColorDipole);
        }

        virtual public void DrawText(SpriteBatch spriteBatch)
        {
            if (!m_IsWire)
            {
                String HowMuch = ((float) (m_Intensity*1000.0f)).ToString("N1") + " mI";
                Vector2 Pos = new Vector2();
                Pos.X = (m_Start.m_Pos.X + m_End.m_Pos.X) * 0.5f;
                Pos.Y = (m_Start.m_Pos.Y + m_End.m_Pos.Y) * 0.5f;
                if (m_Start.m_Pos.X == m_End.m_Pos.X)
                    Pos.X += 12;
                else
                    Pos.Y -= 20;
                Color col = Color.White;
                if (m_IsWire)
                    col = Color.Turquoise;
                GraphicManager.DrawString(FontManager.Font[3], HowMuch, Pos, col, 0, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0.5f);
            }
        }

    }
}
