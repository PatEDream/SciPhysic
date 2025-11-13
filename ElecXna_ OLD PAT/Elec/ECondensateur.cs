using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QM.Object.Widget;
using QM.Util;

namespace QM.Elec
{
    public class ECondensateur : ElecDipole
    {
        public float m_Voltage  = 1.0f;
        public float m_PotentialSpeed = 0.5f;

        public float m_C = 0.01f;
        public float m_Q = 0.0f;
        public float m_IntensityMax = 10.0f;

        public ECondensateur()
            : base() 
        { 
            m_ColorDipole = Color.Blue;
        }


        override public void DoOneStep(float _PotentialToIntensityConvertor)
        {
            float p0 = m_Start.m_EWireConnection.m_Potentiel;
            float p1 = m_End.m_EWireConnection.m_Potentiel;
            float diff = (p1 - p0);
            float diff2 = m_C * (m_Q + m_Q);  //(Q1 - Q0)

            m_Start.m_EWireConnection.m_Potentiel -= (diff2 - diff) * m_PotentialSpeed;
            m_End.m_EWireConnection.m_Potentiel += (diff2 - diff) * m_PotentialSpeed;
            m_Q -= (diff2 - diff) * m_PotentialSpeed;
            m_Intensity = -(diff2 - diff) * m_PotentialSpeed* _PotentialToIntensityConvertor;
            if (float.IsNaN(m_Intensity))
                DebugManager.DoArret();
        }

    }
}
