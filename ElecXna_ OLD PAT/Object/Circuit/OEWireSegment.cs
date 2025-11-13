using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QM.Elec;
using QM.Util;
using QM.Object.Widget;
using QM;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace QM.Object
{
    public class OEWireSegment : OEBase
    {
        //OEScene m_Scene;
        //ElecCircuit m_Circuit;

        EWireSegment m_EWireSegment = new EWireSegment();
        //OESegment m_Visuel = new OESegment();

        static public WImage m_ImagePotentiel;
        static Random random = new Random(21);

        public OEWireSegment(OEScene _Scene, ElecCircuit _Circuit) : base(_Scene, _Circuit) { }

        public override void Destroy()
        {
            m_Visuel.ClearIntensity(m_Scene);
            if(m_Circuit!=null)
                m_Circuit.RemoveDipole(m_EWireSegment);
        }

        public void Create(Point _Start, Point _End, int _Width, Boolean _ExtendEnd)
        {
            m_EWireSegment.m_Start.m_Pos = _Start;
            m_EWireSegment.m_End.m_Pos = _End;
            if(m_Circuit!=null)
                m_Circuit.AddDipole((ElecDipole)m_EWireSegment);

            m_Visuel.m_ImageName = GetImageName(m_Scene.m_Mode);
            m_Visuel.Create(Misc.Vector2FromPoint(_Start), Misc.Vector2FromPoint(_End), _Width, _ExtendEnd);
            if(m_Scene.m_Mode==OEScene.eMode.ModePlan)
                m_Visuel.TheSprite.m_TextureArea = OGSprite.eTextureArea.AllTexture;
            else
                m_Visuel.TheSprite.m_TextureArea = OGSprite.eTextureArea.AllTexture;  // OGSprite.eTextureArea.OnXY;
            AddObj(m_Visuel);

            if (m_ImagePotentiel == null)
            {
                Color Col = Color.White;
                m_ImagePotentiel = new WImage("ImagesElec\\arc.png", new Rectangle(0, 0, _Width * 3, _Width * 3), Col) ;
                m_ImagePotentiel.m_Depth = 0.88f;
            }

        }
        public String GetImageName(OEScene.eMode _mode)
        {
            switch (_mode)
            {
                case OEScene.eMode.ModePlan:
                    return ("ImagesElec\\PlanFil.png");
                case OEScene.eMode.ModeReal:
                    return ("ImagesElec\\cable.png"); //("ImagesElec\\MetalBare0099_2_S.jpg");
            }
            return "";
        }


        override public void Update()
        {
            if (m_Circuit != null)
            {
                m_Visuel.m_IntensityGoal = m_EWireSegment.m_Intensity;
                if (m_EWireSegment.m_Start.m_EWireConnection != null)
                    m_Visuel.m_PotentielGoal = m_EWireSegment.m_Start.m_EWireConnection.m_Potentiel;
                else
                    DebugManager.DoArret();
            }
            //else
            //    DebugManager.DoArret();
        }

        protected SpriteEffects GetRandomSpriteEffect()
        {
            SpriteEffects dst = SpriteEffects.None;
            if (random.Next(2) == 1)
                dst |= SpriteEffects.FlipVertically;
            if (random.Next(2) == 1)
                dst |= SpriteEffects.FlipHorizontally;
            return dst;
        }

        protected void DrawPotentielOnNode(SpriteBatch spriteBatch,ElecNode en)
        {
            if (en.m_LDipole.Count == 1)
            {
                m_ImagePotentiel.SetDstPos(Misc.Add(en.m_Pos, -m_ImagePotentiel.m_DstRect.Width  / 2));
                float pot = m_Visuel.m_PotentielGoal * 50.0f;
                //m_ImagePotentiel.m_ColorSprite.A = (byte)Math.Min(255, Math.Abs(pot * Math.Cos(Mgr.m_GameTime.TotalRealTime.TotalSeconds)));
                m_ImagePotentiel.m_ColorSprite.A = (byte)Math.Min(255, Math.Abs(pot * random.NextDouble()));
                m_ImagePotentiel.m_ColorSprite.R = m_ImagePotentiel.m_ColorSprite.G = m_ImagePotentiel.m_ColorSprite.B = 0;
                if (pot > 0)
                    m_ImagePotentiel.m_ColorSprite.B = 255;
                else
                    m_ImagePotentiel.m_ColorSprite.R = 255;
                m_ImagePotentiel.m_SpriteEffect = GetRandomSpriteEffect();
                m_ImagePotentiel.Draw(spriteBatch);
            }
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            if (m_Scene != null)
                m_Visuel.DrawIntensity(m_Scene);
            if (m_Visuel.m_PotentielGoal != 0.0f)
            {
                DrawPotentielOnNode(spriteBatch, m_EWireSegment.m_End);
                DrawPotentielOnNode(spriteBatch, m_EWireSegment.m_Start);
            }
            base.Draw(spriteBatch);
        }

    }
}
