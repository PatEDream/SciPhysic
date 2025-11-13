using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using QM;
using QM.Phys;
using QM.Maths;
using QM.Util;

namespace QM.Object
{
    class OECharge : OGBase
    {
        //ElecScene m_ElecScene;
        public static int CountID = 0;
        public int ID;
        public Vector2 m_Speed = new Vector2(0, 0);
        public Vector2 m_Acceleration = new Vector2(0, 0);
        public Boolean m_CanMove = true;
        public float m_Charge = 1.0f;
        public float m_Masse = 1.0f;

        OGSprite TheSprite;

        public Color m_Color = Color.BlueViolet;
        public Color m_ColorText = Color.BlueViolet;
        public enum eType { Positiv, Negativ, PositivFixed, NegativFixed };

        public Boolean m_DrawText = true;

        public OECharge() { }
        public OECharge(MathPntSize Where, eType Type) { Create(Where, Type); }


        public void Create(MathPntSize Where, eType Type)
        {
            ID = CountID++;
            m_TRSRelativ.m_Pos = Where.m_Pos;
            m_PntSize.m_Size = Where.m_Size;

            SetType(Type);
            TheSprite = new OGSprite("ImagesQM\\Diamond.png", new MathPntSize(-Where.m_Size.X * 0.5f, -Where.m_Size.Y*0.5f, Where.m_Size.X, Where.m_Size.Y), OGSprite.ePosType.Centered, Color.White);
            AddObj(TheSprite);

            foreach (ObjBase obj in m_AObj)
                obj.m_ToSave = false;
        }

        override public void WriteTxt(ref List<String> _Lines)
        {
        }

        override public void ReadTxt(String[] _Lines, ref int _CurrentLine)
        {
        }

        public void SetType(eType type)
        {
            m_ColorText = Color.BlueViolet;
            switch (type)
            {
                case eType.Positiv:
                    m_Color = Color.Gray;
                    m_Charge = 1.0f;
                    m_Masse = 1.0f;
                    m_CanMove = true;
                    break;
                case eType.Negativ:
                    m_Color = Color.Red;
                    m_Charge = -1.0f;
                    m_Masse = 1.0f;
                    m_CanMove = true;
                    break;
                case eType.PositivFixed:
                    m_Color = Color.DarkGray;
                    m_Charge = 1.0f;
                    m_Masse = 10000.0f;
                    m_CanMove = false;
                    break;
                case eType.NegativFixed:
                    m_Color = Color.DarkRed;
                    m_Charge = -1.0f;
                    m_Masse = 10000.0f;
                    m_CanMove = false;
                    break;
            }
        }

        public Vector2 GetCenter()
        {
            return TheSprite.GetCenter();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            TheSprite.m_ColorSprite = m_Color;
            TheSprite.Draw(spriteBatch);
        }
        public override void DrawText(SpriteBatch spriteBatch)
        {
            if (m_DrawText)
            {
                string HowMuch;
                if (m_Charge > 0.0f)
                {
                    HowMuch = "+" + (int)Math.Round(m_Charge);
                }
                else
                {
                    HowMuch = "-" + (int)Math.Round(-m_Charge);
                }

                GraphicManager.DrawString(FontManager.Font[2],
                    HowMuch,
                    new Vector2(m_TRSGlobal.m_Pos.X + 5, m_TRSGlobal.m_Pos.Y - 14), m_Color, 0, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0.5f);
            }
        }


        public void ComputeAcceleration(List<OECharge> LCharge)
        {
            if (m_CanMove)
            {
                float factElectroStatique = 10.0f;
                m_Acceleration = new Vector2(0, 0);
                foreach (OECharge oc in LCharge)
                {
                    if (oc != this)
                    {
                        Vector2 Diff = m_TRSRelativ.m_Pos - oc.m_TRSRelativ.m_Pos;
                        float Dist = Diff.Length();
                        if (Dist == 0.0f)
                            Dist = 1.0f;
                        Vector2 DecalageAccel = (Diff * m_Charge * oc.m_Charge * factElectroStatique) / (Dist * Dist * Dist) / m_Masse;
                        float L = DecalageAccel.Length();
                        if (L > 0.1f)
                            DecalageAccel *= (0.1f / L);
                        m_Acceleration += DecalageAccel;
                    }
                }
            }
        }
        public void ComputeMouvment()
        {
            if (m_CanMove)
            {
                m_Speed += m_Acceleration;
                m_TRSRelativ.m_Pos += m_Speed;
            }
        }

    }
}
