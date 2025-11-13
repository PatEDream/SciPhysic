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
    public class OELampe : OEResistance
    {
        //OEScene m_Scene;
        //ElecCircuit m_Circuit;

        ELampe m_ELampe = new ELampe();
        //OESegment m_Visuel = new OESegment();
        //TimeSpan m_TimeOfLightning = new TimeSpan();

        
        public OELampe(OEScene _Scene, ElecCircuit _Circuit) : base(_Scene, _Circuit) { }
        public WImage m_Light;
        public WImage m_Light2;

        public override void Destroy()
        {
            m_Visuel.ClearIntensity(m_Scene);
            if (m_Circuit!=null)
                m_Circuit.RemoveDipole(m_ELampe);
        }

        public override void Create(Point _Start, Point _End)
        {
            m_ELampe.m_Start.m_Pos = _Start;
            m_ELampe.m_End.m_Pos = _End;
            if(m_Circuit!=null)
                m_Circuit.AddDipole((ElecDipole)m_ELampe);

            m_Visuel.m_ImageName = GetImageName(m_Scene.m_Mode);
            int DX = _End.X - _Start.X;
            int DY = _End.Y - _Start.Y;
            int L = Math.Max(Math.Abs(DX), Math.Abs(DY));
            m_Visuel.Create(Misc.Vector2FromPoint(_Start), Misc.Vector2FromPoint(_End), L, false);
            AddObj(m_Visuel);

            m_Light = new WImage("ImagesElec\\lampeAllumee.png", new Rectangle((_End.X+_Start.X) / 2 -L*2, (_End.Y+_Start.Y) / 2 -L*2, L * 4, L * 4), Color.White);
            AddObj(m_Light);
            m_Light.m_Depth -= 0.01f;
            m_Light.SetActiveAndVisibleState(false);

            L /= 4;
            m_Light2 = new WImage("ImagesElec\\lampeAllumee.png", new Rectangle((_End.X + _Start.X) / 2 - L * 2, (_End.Y + _Start.Y) / 2 - L * 2, L * 4, L * 4), Color.White);
            AddObj(m_Light2);
            m_Light2.m_Depth -= 0.01f;
            m_Light2.SetActiveAndVisibleState(false);
        }

        public override String GetImageName(OEScene.eMode _mode)
        {
            switch (_mode)
            {
                case OEScene.eMode.ModePlan:
                    return ("ImagesElec\\PlanLampe.png");
                case OEScene.eMode.ModeReal:
                    return ("ImagesElec\\lampe.png");
            }
            return "";
        }

        override public void Update()
        {
            m_Visuel.m_IntensityGoal = m_ELampe.m_Intensity;
            m_Visuel.m_PotentielGoal = m_ELampe.m_Potentiel;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (m_Scene != null)
                m_Visuel.DrawIntensity(m_Scene);
            if (m_ELampe.IsON())//Math.Abs(m_Visuel.m_IntensityGoal) >= 0.001f)
            {
                m_Light.SetActiveAndVisibleState(true);
                m_Light.m_ColorSprite.A = (byte)Math.Min(128, (int)(Math.Abs(m_Visuel.m_IntensityGoal) * 0.5f * 255.0f));
                m_Light2.SetActiveAndVisibleState(true);
                m_Light2.m_ColorSprite.A = (byte)Math.Min(255, (int)(Math.Abs(m_Visuel.m_IntensityGoal) * 1.0f * 255.0f));
            }
            else
            {
                m_Light.SetActiveAndVisibleState(false);
                m_Light2.SetActiveAndVisibleState(false);
            }

            base.Draw(spriteBatch);
        }

    }
}
