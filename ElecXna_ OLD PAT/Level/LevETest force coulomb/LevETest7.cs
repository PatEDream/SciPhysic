using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using QM.Phys;
using QM.Maths;
using QM.Object;
using QM.Level.Menu;
using QM.Object.Widget;
using QM.Util;
using QM.Elec;

namespace QM.Level
{
    class LevETest7 : LevETestBase
    {

        OEScene m_Scene;
        ElecCircuit m_Circuit;
        

        public override void StartLevel(String Param1)
        {
            AddObj(m_Scene = new OEScene(10, 10, 800, 600));

            int sx = QMGame.GetBackBufferWidth();
            int sy = QMGame.GetBackBufferHeight();

            m_Circuit = new ElecCircuit();

            OEPile pile = new OEPile(m_Scene, m_Circuit);
            pile.Create(new Point(150, 50),new Point(100, 50));
            AddObj(pile);

            OEWireSegment ews0 = new OEWireSegment(m_Scene, m_Circuit);
            ews0.Create(new Point(150, 50), new Point(150, 150), 10, false);
            AddObj(ews0);

            OEWireSegment ews1 = new OEWireSegment(m_Scene, m_Circuit);
            ews1.Create(new Point(100, 50), new Point(100, 150), 10, false);
            AddObj(ews1);

            OEResistance er0 = new OEResistance(m_Scene, m_Circuit);
            er0.Create(new Point(100, 150), new Point(150, 150));
            AddObj(er0);

            m_Circuit.ComputeCircuit();

        }



        override public void Update()
        {
            m_Circuit.DoOneStep();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            m_Circuit.DrawDebug(m_Scene.m_BlockTextureData, m_Scene.sx, m_Scene.sy);
            base.Draw(spriteBatch);
        }

        public override void DrawText(SpriteBatch spriteBatch)
        {
            m_Circuit.DrawText(spriteBatch);
        }

    }
}
