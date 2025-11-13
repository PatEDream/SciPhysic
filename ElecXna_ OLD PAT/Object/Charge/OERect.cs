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
    class OERect : OGBase
    {
        //ElecScene m_ElecScene;
        static Random random = new Random(10);

        public Vector2 m_Speed = new Vector2(0, 0);
        public Vector2 m_Acceleration = new Vector2(0, 0);
        public Boolean m_CanMove = true;
        public float m_Masse = 100.0f;

        OGSprite TheSprite;

        public Color m_Color = Color.MediumSeaGreen;
        public enum eType { Metal, Wood };

        public List<OECharge> m_LCharge = new List<OECharge>();

        public OERect() { }
        public OERect(MathPntSize Where, eType Type, Boolean CanMove, float Masse) { Create(Where, Type, CanMove, Masse); }


        public void Create(MathPntSize Where, eType Type, Boolean CanMove, float Masse)
        {
            m_TRSRelativ.m_Pos = Where.m_Pos;
            m_PntSize.m_Size = Where.m_Size;

            TheSprite = new OGSprite("ImagesQM\\vide.bmp", new MathPntSize(0, 0, Where.m_Size.X, Where.m_Size.Y), OGSprite.ePosType.Centered, Color.White);
            TheSprite.m_Depth += 0.1f;
            AddObj(TheSprite);

            foreach (ObjBase obj in m_AObj)
                obj.m_ToSave = false;
            SetType(Type);
            m_CanMove = CanMove;
            m_Masse = Masse;
            m_LCharge = new List<OECharge>();
        }

        public void SetNoCharge()
        {
            m_LCharge.Clear();
        }
        public void AddCharge(OECharge.eType Type)
        {
            float X = m_TRSRelativ.m_Pos.X + random.Next(1, (int)m_PntSize.m_Size.X - 1);
            float Y = m_TRSRelativ.m_Pos.Y + random.Next(1, (int)m_PntSize.m_Size.Y - 1);
            //X = m_TRSRelativ.m_Pos.X + m_PntSize.m_Size.X; Y = m_TRSRelativ.m_Pos.Y + m_PntSize.m_Size.Y;
            OECharge oc = new OECharge(new MathPntSize(X-4,Y-4, 8, 8), Type);
            oc.m_Speed = new Vector2((float)(random.NextDouble() * 0.02 - 0.01), (float)(random.NextDouble() * 0.02 - 0.01));
            oc.m_DrawText = false;
            m_LCharge.Add(oc);
            AddObj(oc);
            oc.m_Parent = null;
        }

        public void SetType(eType type)
        {
            switch (type)
            {
                case eType.Metal:
                    m_Color = Color.MediumSeaGreen;
                    m_Masse = 10.0f;
                    m_CanMove = true;
                    break;
                case eType.Wood:
                    m_Color = Color.BurlyWood;
                    m_Masse = 1.0f;
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
            foreach (OECharge oc in m_LCharge)
            {
                oc.Draw(spriteBatch);
            }
        }


        public void ComputeAcceleration(List<OECharge> LCharge)
        {
            foreach (OECharge oc in m_LCharge)
            {
                oc.ComputeAcceleration(LCharge);
            }
        }
        public void ComputeMouvment()
        {
            foreach (OECharge oc in m_LCharge)
            {
                if (oc.m_CanMove)
                {
                    oc.m_Speed += oc.m_Acceleration;
                    if (oc.m_TRSRelativ.m_Pos.X < m_TRSRelativ.m_Pos.X && oc.m_Speed.X < 0)
                        oc.m_Speed.X = -oc.m_Speed.X * 0.5f;
                    if (oc.m_TRSRelativ.m_Pos.X > m_TRSRelativ.m_Pos.X + m_PntSize.m_Size.X && oc.m_Speed.X > 0)
                        oc.m_Speed.X = -oc.m_Speed.X *0.5f;
                    if (oc.m_TRSRelativ.m_Pos.Y < m_TRSRelativ.m_Pos.Y && oc.m_Speed.Y < 0)
                        oc.m_Speed.Y = -oc.m_Speed.Y * 0.5f;
                    if (oc.m_TRSRelativ.m_Pos.Y > m_TRSRelativ.m_Pos.Y + m_PntSize.m_Size.Y && oc.m_Speed.Y > 0)
                        oc.m_Speed.Y = -oc.m_Speed.Y * 0.5f;
                    oc.m_TRSRelativ.m_Pos += oc.m_Speed;
                    oc.m_TRSGlobal = oc.m_TRSRelativ;
                }
            }
        }

    }
}
