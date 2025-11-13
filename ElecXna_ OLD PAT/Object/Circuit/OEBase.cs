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
    public class OEBase : OGBase
    {
        protected OEScene m_Scene;
        protected ElecCircuit m_Circuit;

        //EPile m_EPile = new EPile();
        public OESegment m_Visuel = new OESegment();

        public OEBase(OEScene _Scene, ElecCircuit _Circuit) : base() { m_Scene = _Scene; m_Circuit = _Circuit; }

        public override void Destroy()
        {
            //DebugManager.DoArret();
        }


        //public virtual void SetMode(OEScene.eMode _Mode)
        //{

        //}

        public float GetIntensityGoal()
        {
            return m_Visuel.m_IntensityGoal;
        }
    }
}
