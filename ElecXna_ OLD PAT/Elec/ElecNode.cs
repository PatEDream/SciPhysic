using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace QM.Elec
{
    public class ElecNode
    {
        public float m_Potentiel = 0.0f;

        public Point m_Pos = new Point();
        public int m_DurtyCount = 0;
        public EWireConnection m_EWireConnection = null;

        public List<ElecDipole> m_LDipole = new List<ElecDipole>();

        public void Init() { m_Potentiel = 0.0f; m_LDipole.Clear(); m_EWireConnection = null; }
    }
}
