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
    public class EBobine : ElecDipole
    {
        public float m_Voltage  = 1.0f;
        public float m_VoltageToPotentialConvertor = 0.5f;

        public float m_Ohm = 0.1f;
        public float m_L = 0.9999f;
        public float m_Q = 0.0f;
        public float m_IntensityMax = 10.0f;

        public EBobine()
            : base() 
        { 
            m_ColorDipole = Color.Blue;
        }


        override public void DoOneStep(float _PotentialToIntensityConvertor)
        {
            float p0 = m_Start.m_EWireConnection.m_Potentiel;
            float p1 = m_End.m_EWireConnection.m_Potentiel;
            float tension = (p1 - p0);
            float newIntensity = tension / _PotentialToIntensityConvertor;// m_Ohm;
            float DI = newIntensity - m_Intensity;
            float E = -m_L * DI;
            m_Intensity = newIntensity + E;
            float I = m_Intensity / _PotentialToIntensityConvertor;
            {
                m_Start.m_EWireConnection.m_Potentiel += I;
                m_End.m_EWireConnection.m_Potentiel -= I;
            }
        }

    }
}
