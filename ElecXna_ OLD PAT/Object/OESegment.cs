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
using QM.Elec;
using QM.Maths;
using QM.Util;

namespace QM.Object
{
    public class OESegment : OGBase
    {
        static Random random = new Random(10);
        static public float m_PointDensityPerAmpere = 1.0f;
        static public float m_DelayFactor = 1.0f;
        static public Color m_ColorIntensityPos = Color.Black;
        static public Color m_ColorIntensityNeg = Color.Red;
        static public Boolean m_OnlyIntensityPos = true;
        static public float m_FactWidth = 0.5f;

        public class PointIntensity
        {
            public Boolean m_IsPositiv = true;
            public Vector2 m_Where;
            public Boolean m_IsActiv = false;
            public int m_Life;
            static public int LifeLength = 10;
        }


        public OGSprite TheSprite;
        public Color m_Color = Color.MediumSeaGreen;

        public float m_Potentiel = 0.0f;
        public float m_Intensity = 0.0f;
        public float m_PotentielGoal = 0.0f;
        public float m_IntensityGoal = 0.0f;

        Vector2 m_Start;
        Vector2 m_End;
        int m_Width;
        float m_Length;
        Vector2 m_Dir;
        Vector2 m_Ortho;

        public String m_ImageName = "ImagesQM\\vide.bmp";

        public List<PointIntensity> m_LPointIntensity = new List<PointIntensity>();

        public OESegment()
        {
        }
        public OESegment(Vector2 _Start, Vector2 _End, int _Width, Boolean _ExtendEnd)
        {
            Create(_Start, _End, _Width, _ExtendEnd);
        }
        public OESegment(Vector2 _Start, Vector2 _End, int _Width, Boolean _ExtendEnd, String _ImageName)
        {
            m_ImageName = _ImageName;
            Create(_Start, _End, _Width, _ExtendEnd);
        }



        public void Create(Vector2 _Start, Vector2 _End, int _Width,Boolean _ExtendEnd)
        {
            if (_End == _Start)
                _End.X += 1.0f;
            m_Start = _Start;
            m_End = _End;
            m_Width = _Width;

            m_Dir = m_End - m_Start;
            m_Length = m_Dir.Length()+1;
            m_Dir /= m_Length;
            m_Ortho.X = -m_Dir.Y;
            m_Ortho.Y = m_Dir.X;


            //m_TRSRelativ.m_Pos = m_Start + m_Dir*m_Length*0.5f;
            m_PntSize.m_Size.X = m_Length;
            m_PntSize.m_Size.Y = m_Width;

            m_AObj.Clear();
            if (_ExtendEnd)
                m_Length += m_Width / 2 -1;
            TheSprite = new OGSprite(m_ImageName, new MathPntSize(m_Start.X - m_Ortho.X * m_Width / 2, m_Start.Y - m_Ortho.Y * m_Width / 2,
                m_Length, m_Width), OGSprite.ePosType.TopLeft, Color.White);
            TheSprite.m_TRSRelativ.m_Angle = (float)Math.Atan2(m_Dir.Y, m_Dir.X);
            AddObj(TheSprite);

            //foreach (ObjBase obj in m_AObj)
            //    obj.m_ToSave = false;


        }


        public Vector2 GetCenter()
        {
            return TheSprite.GetCenter();
        }

        public void LoadTexture(String _TextureName)
        {
            m_ImageName = _TextureName;
            TheSprite.LoadTexture(_TextureName);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            m_Potentiel += (m_PotentielGoal - m_Potentiel) / m_DelayFactor;
            TheSprite.m_ColorSprite = OEScene.ColorFromPotentiel(m_Potentiel);
            TheSprite.Draw(spriteBatch);
        }

        public virtual void ClearIntensity(OEScene _Scene)
        {
            Color colTrans = Color.TransparentBlack;
            foreach (PointIntensity pi in m_LPointIntensity)
            {
                if (pi.m_IsActiv)
                {
                    _Scene.DrawPoint3x3((int)pi.m_Where.X, (int)pi.m_Where.Y, colTrans);
                }
            }
        }

        public virtual void DrawIntensity(OEScene _Scene)
        {
            if (Math.Abs(m_Intensity) > Math.Abs(m_IntensityGoal))
                m_Intensity = m_IntensityGoal;
            else
                m_Intensity += (m_IntensityGoal - m_Intensity) / m_DelayFactor;
            //int NbToDraw = Math.Min((int)(m_Intensity * m_PointDensityPerAmpere * m_Length), m_APointIntensity.GetLength(0));
            int NbToDraw = (int)Math.Abs(m_Intensity * m_PointDensityPerAmpere * m_Length);
            if(Math.Abs(m_Intensity)>10)
                NbToDraw = (int)Math.Abs(10 * m_PointDensityPerAmpere * m_Length);
            Vector2 Speed = new Vector2();
            Speed = -m_Dir * Math.Sign(m_Intensity) ;
            int CountActiv = 0;

            if (m_Intensity == 0.0f)
                Speed = m_Dir;

            Color colTrans = Color.TransparentBlack;
            foreach( PointIntensity pi in m_LPointIntensity)
            {
                if (pi.m_IsActiv)
                {
                    _Scene.DrawPoint3x3((int)pi.m_Where.X, (int)pi.m_Where.Y, colTrans);
                }
            }


            for (int i = 0; i < m_LPointIntensity.Count(); )
            //foreach (PointIntensity pi in m_LPointIntensity)
            {
                PointIntensity pi = m_LPointIntensity[i];
                if (pi.m_IsActiv)
                {
                    if (pi.m_IsPositiv)
                        pi.m_Where += Speed;
                    else
                        pi.m_Where -= Speed;
                    pi.m_Life++;
                    if (pi.m_Life >= PointIntensity.LifeLength)
                    {
                        pi.m_IsActiv = false;
                        m_LPointIntensity.Remove(pi);
                    }
                    else
                    {
                        CountActiv++;
                        Vector2 Pos = TheSprite.m_TRSRelativ.ApplyInverse(pi.m_Where);
                        if (Pos.X > m_Length)
                        {
                            Pos.X -= m_Length;
                            pi.m_Where = TheSprite.m_TRSRelativ.Apply(Pos);
                        }
                        if (Pos.X < 0.0f)
                        {
                            Pos.X += m_Length;
                            pi.m_Where = TheSprite.m_TRSRelativ.Apply(Pos);
                        }
                        i++;
                    }
                }
            }

            //int which = 0;
            //int NbNew = Math.Min(NbToDraw - CountActiv, m_APointIntensity.Count() / PointIntensity.LifeLength);
            int NbNew = NbToDraw - CountActiv;
            //int N = 0;
            //while (N < NbNew)
            for(int i=0;i<NbNew;i++)
            {
                //PointIntensity pi = m_APointIntensity[which];
                PointIntensity pi = new PointIntensity();
                //which++;
                //if (pi.m_IsActiv == false)
                {
                    pi.m_IsActiv = true;
                    if (m_OnlyIntensityPos)
                        pi.m_IsPositiv = true;
                    else
                      pi.m_IsPositiv = (random.Next(2) == 0);
                    pi.m_Life = 0;
                    Vector2 pos0 = new Vector2((float)random.NextDouble() * m_PntSize.m_Size.X, ((float)random.NextDouble() * m_FactWidth + (1.0f-m_FactWidth)/2.0f) * m_PntSize.m_Size.Y);
                    Vector2 pos = TheSprite.m_TRSRelativ.Apply(pos0);
                    //pi.m_Where.X = m_TRSRelativ.m_Pos.X + ;
                    //pi.m_Where.Y = m_TRSRelativ.m_Pos.Y + ;
                    pi.m_Where = pos;
                    m_LPointIntensity.Add(pi);
                    //N++;
                }
            }

            foreach (PointIntensity pi in m_LPointIntensity)
            {
                //if (pi.m_IsActiv)
                {
                    //Color col = pi.m_IsPositiv ? m_ColorIntensityPos : m_ColorIntensityNeg;
                    _Scene.DrawPoint3x3((int)pi.m_Where.X, (int)pi.m_Where.Y, OEScene.m_ColorIntensity);

                }
            }

        }
    }
}
