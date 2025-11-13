using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace QM.Elec
{
    public class EWire : ElecDipole
    {
        Color m_ColorDebug;
        static int ColorCount = 55;

        public List<ElecDipole> m_LWireSegment = new List<ElecDipole>();

        public EWire() : base() { m_IsWire = true; }

        public void ChooseColorDebug()
        {
            ColorCount++;
            m_ColorDebug = new Color((byte)(100 + (ColorCount * 100) % 155), (byte)((ColorCount * 200) % 255), (byte)((ColorCount * 50) % 255));

            foreach (ElecDipole ed in m_LWireSegment)
            {
                if (ed.m_IsWire)
                    ed.m_ColorDipole = m_ColorDebug;
            }
        }

    }
}
