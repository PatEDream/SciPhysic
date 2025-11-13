using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace QM.Elec
{
    public class EDiode : ElecDipole
    {
        public float m_Ohm  = 10.0f;

 //       public Color m_ColorDipole = Color.BurlyWood;

        public EDiode()
            : base() 
        { 
            m_ColorDipole = Color.Green; 
        }


        override public void DoOneStep(float _PotentialToIntensityConvertor)
        {
            float p0 = m_Start.m_EWireConnection.m_Potentiel;
            float p1 = m_End.m_EWireConnection.m_Potentiel;
            float tension = (p1 - p0);
            if (tension > 0)
            {
                m_Intensity = tension / m_Ohm;
                float I = m_Intensity / _PotentialToIntensityConvertor;
                if (Math.Abs(I) > Math.Abs(tension))
                    I *= Math.Abs(tension / I);

                m_Start.m_EWireConnection.m_Potentiel += I;
                m_End.m_EWireConnection.m_Potentiel -= I;
            }
            else
                m_Intensity = 0.0f;

        }
    }
}
