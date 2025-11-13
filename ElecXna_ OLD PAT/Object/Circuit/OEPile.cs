using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QM.Elec;
using QM.Util;
using QM;
using QM.Object.Widget;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace QM.Object
{
    public class OEPile : OEBase
    {
        //OEScene m_Scene;
        //ElecCircuit m_Circuit;

        EPile m_EPile = new EPile();
        //OESegment m_Visuel = new OESegment();
        WImage m_WImageBurning = null;
        WImage m_WImageWrongDirection = null;
        Boolean m_IsBurned = false;
        int m_WarningSize = 60;

        public OEPile(OEScene _Scene, ElecCircuit _Circuit) : base(_Scene,_Circuit) {  }

        public override void Destroy()
        {
            m_Visuel.ClearIntensity(m_Scene);
            if(m_Circuit!=null)
                m_Circuit.RemoveDipole(m_EPile);
        }

        public void Create(Point _Start, Point _End)
        {
            m_EPile.m_Start.m_Pos = _Start;
            m_EPile.m_End.m_Pos = _End;
            m_EPile.m_Voltage = 5.0f;
            if(m_Circuit!=null)
                m_Circuit.AddDipole((ElecDipole)m_EPile);
     
            m_Visuel.m_ImageName = GetImageName(m_Scene.m_Mode);
            int DX = _End.X - _Start.X;
            int DY = _End.Y - _Start.Y;
            int L = Math.Max(Math.Abs(DX), Math.Abs(DY));
            Point TextSize = TextureManager.GetTextureSize(m_Visuel.m_ImageName);
            m_Visuel.Create(Misc.Vector2FromPoint(_Start), Misc.Vector2FromPoint(_End), L * TextSize.Y / TextSize.X, false);
            AddObj(m_Visuel);
        }
        public String GetImageName(OEScene.eMode _mode)
        {
            String EndName = ".png";
            if (m_EPile.m_IsBurned)
                EndName = "Burn.png";
            switch (_mode)
            {
                case OEScene.eMode.ModePlan:
                    return ("ImagesElec\\PlanPile" + EndName);
                case OEScene.eMode.ModeReal:
                    return ("ImagesElec\\Pile" + EndName);
            }
            return "";
        }

        void ShowImageWrongDirection()
        {
            if (m_EPile.m_IsWrongDirection)
            {
                if (m_WImageWrongDirection == null)
                {
                    Vector2 Pos = m_Visuel.GetCenter();
                    Rectangle rect = new Rectangle((int)Pos.X - m_WarningSize / 2, (int)Pos.Y - m_WarningSize / 2, m_WarningSize, m_WarningSize);
                    m_WImageWrongDirection = new WImage("ImagesElec\\WrongDirection.png", rect, Color.White);
                    m_WImageWrongDirection.m_Depth = 0.2f;
                    AddObj(m_WImageWrongDirection);
                    WActivText wat = new WActivText(Pos, 40.0f, false, "WrongDirection", FontManager.eFontID.Font1);
                    m_WImageWrongDirection.AddObj(wat);
                }
                else
                {
                    Vector2 Pos = m_Visuel.GetCenter();
                    Rectangle rect = new Rectangle((int)Pos.X - m_WarningSize / 2, (int)Pos.Y - m_WarningSize / 2, m_WarningSize, m_WarningSize);
                    rect.X += Misc.random.Next(3) - 1;
                    rect.Y += Misc.random.Next(3) - 1;
                    m_WImageWrongDirection.m_DstRect = rect;
                }
            }
            else
            {
                if (m_WImageWrongDirection != null)
                {
                    m_AObj.Remove(m_WImageWrongDirection);
                    m_WImageWrongDirection = null;
                }
            }
        }

        void ShowImageBurning()
        {
            if (m_EPile.m_IsBurning)
            {
                if (m_WImageBurning == null)
                {
                    Vector2 Pos = m_Visuel.GetCenter();
                    Rectangle rect = new Rectangle((int)Pos.X - m_WarningSize/2, (int)Pos.Y - m_WarningSize / 2, m_WarningSize, m_WarningSize);
                    m_WImageBurning = new WImage("ImagesElec\\CourtCircuit.png", rect, Color.White);
                    m_WImageBurning.m_Depth = 0.2f;
                    AddObj(m_WImageBurning);
                    WActivText wat = new WActivText(Pos, 40.0f, false, "CourtCircuit", FontManager.eFontID.Font1);
                    m_WImageBurning.AddObj(wat);
                }
                else
                {
                    Vector2 Pos = m_Visuel.GetCenter();
                    Rectangle rect = new Rectangle((int)Pos.X - m_WarningSize/2, (int)Pos.Y - m_WarningSize / 2, m_WarningSize, m_WarningSize);
                    rect.X += Misc.random.Next(5) - 2;
                    rect.Y += Misc.random.Next(5) - 2;
                    m_WImageBurning.m_DstRect = rect;
                }
            }
            else
            {
                if (m_WImageBurning != null)
                {
                    m_AObj.Remove(m_WImageBurning);
                    m_WImageBurning = null;
                }
            }
        }

        override public void Update()
        {
            m_Visuel.m_IntensityGoal = m_EPile.m_Intensity;
            m_Visuel.m_PotentielGoal = m_EPile.m_Potentiel;
            if (m_EPile.m_IsBurned && m_IsBurned == false)
            {
                m_IsBurned = true;
                String name = GetImageName(m_Scene.m_Mode);
                m_Visuel.LoadTexture(name);
            }

            ShowImageBurning();
            ShowImageWrongDirection();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //if (m_Scene != null)
            //    m_Visuel.DrawIntensity(m_Scene);
            //else
            //    DebugManager.DoArret();
            base.Draw(spriteBatch);
        }

    }
}
