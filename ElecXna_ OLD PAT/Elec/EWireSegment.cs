using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace QM.Elec
{
    public class EWireSegment : ElecDipole
    {
        public EWireSegment() : base() { m_IsWire = true; m_ColorDipole = Color.Coral; }

    }
}
