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
    public class OEResistanceTest : OEBase
    {
        //OEScene m_Scene;
        //ElecCircuit m_Circuit;

        EResistance m_EResistance = new EResistance();
        //OESegment m_Visuel = new OESegment();

        public OEResistanceTest(OEScene _Scene, ElecCircuit _Circuit) : base(_Scene, _Circuit) { m_EResistance.m_Ohm = 0.01f; }

        public override void Destroy()
        {
            m_Visuel.ClearIntensity(m_Scene);
            m_Circuit.RemoveDipole(m_EResistance);
        }

        public void SetPosition(Point _Start, Point _End)
        {
            m_EResistance.m_Start.m_Pos = _Start;
            m_EResistance.m_End.m_Pos = _End;
            if(m_Circuit!=null)
                m_Circuit.AddDipole((ElecDipole)m_EResistance);

            m_Visuel.m_ImageName = "ImagesElec\\Resistance.jpg";
            int DX = _End.X - _Start.X;
            int DY = _End.Y - _Start.Y;
            int L = Math.Max(Math.Abs(DX), Math.Abs(DY));
            m_Visuel.Create(Misc.Vector2FromPoint(_Start), Misc.Vector2FromPoint(_End), L * 3 / 10, false); ;
            AddObj(m_Visuel);
        }

        override public void Update()
        {
            m_Visuel.m_IntensityGoal = m_EResistance.m_Intensity;
            m_Visuel.m_PotentielGoal = m_EResistance.m_Potentiel;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (m_Scene != null)
                m_Visuel.DrawIntensity(m_Scene);
            base.Draw(spriteBatch);
        }

    }
}
