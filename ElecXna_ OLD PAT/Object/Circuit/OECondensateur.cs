using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QM.Elec;
using QM.Util;
using QM;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace QM.Object
{
    public class OECondensateur : OEBase
    {
        //OEScene m_Scene;
        //ElecCircuit m_Circuit;

        ECondensateur m_ECondensateur = new ECondensateur();
        //OESegment m_Visuel = new OESegment();

        public OECondensateur(OEScene _Scene, ElecCircuit _Circuit) : base(_Scene,_Circuit) {  }

        public override void Destroy()
        {
            m_Visuel.ClearIntensity(m_Scene);
            if (m_Circuit!=null)
                m_Circuit.RemoveDipole(m_ECondensateur);
        }

        public void Create(Point _Start, Point _End)
        {
            m_ECondensateur.m_Start.m_Pos = _Start;
            m_ECondensateur.m_End.m_Pos = _End;
            if(m_Circuit!=null)
                m_Circuit.AddDipole((ElecDipole)m_ECondensateur);

            m_Visuel.m_ImageName = GetImageName(m_Scene.m_Mode);
            int DX = _End.X - _Start.X;
            int DY = _End.Y - _Start.Y;
            int L = Math.Max(Math.Abs(DX), Math.Abs(DY));
            m_Visuel.Create(Misc.Vector2FromPoint(_Start), Misc.Vector2FromPoint(_End), L, false); ;
            AddObj(m_Visuel);
        }

        public String GetImageName(OEScene.eMode _mode)
        {
            switch (_mode)
            {
                case OEScene.eMode.ModePlan:
                    return ("ImagesElec\\Condensateur.png");
                case OEScene.eMode.ModeReal:
                    return ("ImagesElec\\Condensateur.png");
            }
            return "";
        }

        override public void Update()
        {
            m_Visuel.m_IntensityGoal = m_ECondensateur.m_Intensity;
            m_Visuel.m_PotentielGoal = m_ECondensateur.m_Potentiel;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //if (m_Scene != null)
            //    m_Visuel.DrawIntensity(m_Scene);
            base.Draw(spriteBatch);
        }

    }
}
