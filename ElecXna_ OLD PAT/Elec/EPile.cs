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
    public class EPile : ElecDipole
    {
        public float m_Voltage  = 1.0f;
        public float m_VoltageToPotentialConvertor = 0.5f;

        public Boolean m_IsWrongDirection = false;
        public Boolean m_IsBurning = false;
        
        //public float m_IntensityMax = 10.0f;
        public Boolean m_IsBurned = false;
        public int m_HealthMax = 300000;  //en appel de DoOneStep... de survie à un cours-circuit
        public int m_Health = 300000;  //en appel de DoOneStep... de survie à un cours-circuit

        public EPile() : base() 
        { 
            m_ColorDipole = Color.Blue;
            m_Health = m_HealthMax = 300000 * (75 + Misc.random.Next(50)) / 100;
        }


        override public void DoOneStep(float _PotentialToIntensityConvertor)
        {
            m_IsBurning = false;
            m_IsWrongDirection = false;

            if (m_IsBurned)
            {
                m_Intensity = 0.0f;
                return;
            }

            float p0 = m_Start.m_EWireConnection.m_Potentiel;
            float p1 = m_End.m_EWireConnection.m_Potentiel;
            float diff = m_Voltage - (p1 - p0);

            m_Health++;
            m_Health = Math.Min(m_HealthMax, m_Health);

            if (p1 - p0 < m_Voltage)
            {
                m_Start.m_EWireConnection.m_Potentiel -= diff * m_VoltageToPotentialConvertor;
                m_End.m_EWireConnection.m_Potentiel += diff * m_VoltageToPotentialConvertor;
                m_Intensity =  - diff * m_VoltageToPotentialConvertor * _PotentialToIntensityConvertor;
                if (Math.Abs(m_Intensity) > ElecCircuit.m_IntensityOfShortCut)
                {
                    m_IsBurning = true;
                    m_Health-=2;
                }
            }
            else
            {
                m_Start.m_EWireConnection.m_Potentiel -= diff * m_VoltageToPotentialConvertor;
                m_End.m_EWireConnection.m_Potentiel += diff * m_VoltageToPotentialConvertor;
                m_Intensity = -diff * m_VoltageToPotentialConvertor * _PotentialToIntensityConvertor;
                //if (Math.Abs((m_Voltage - (p1 - p0)) * m_VoltageToPotentialConvertor * _PotentialToIntensityConvertor) > 0.1f)
                if (Math.Abs(m_Intensity) > 0.1f)
                {
                    m_Health-=2;
                    m_IsWrongDirection = true;
                }
            }
            if (m_Health <= 0)
            {
                m_IsBurned = true;
            }

            //pour tester la qualité de la simu
            //diff = m_Voltage - (p1 - p0);
            //if(Math.Abs(diff)>0.01f)
            //{
            //        m_IsWrongDirection = true;
            //        m_IsBurning = true;
            //}

        }

    }
}
