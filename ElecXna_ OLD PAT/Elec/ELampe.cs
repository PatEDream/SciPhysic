using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace QM.Elec
{
    public class ELampe : EResistance
    {
       // public float m_Ohm  = 10.0f;

        public ELampe()
            : base() 
        { 
            m_ColorDipole = Color.Yellow;
            m_Ohm = 10.0f;
            m_IsLight = true;
        }


        //override public void DoOneStep(float _PotentialToIntensityConvertor)
        //{
        //    float p0 = m_Start.m_EWireConnection.m_Potentiel;
        //    float p1 = m_End.m_EWireConnection.m_Potentiel;
        //    float tension = (p1 - p0);
        //    m_Intensity = tension / m_Ohm;
        //    float I = m_Intensity / _PotentialToIntensityConvertor;
        //    {
        //        m_Start.m_EWireConnection.m_Potentiel += I;
        //        m_End.m_EWireConnection.m_Potentiel -= I;
        //    }
        //}
    }
}
