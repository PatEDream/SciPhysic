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
    class LevETest5 : LevETestBase
    {

        //OESegment[] m_ASeg = new OESegment[56];
        OEScene m_Scene;
        ElecCircuit m_Circuit;

        

        public override void StartLevel(String Param1)
        {
            AddObj(m_Scene = new OEScene(10, 10, 800, 600));

            int sx = QMGame.GetBackBufferWidth();
            int sy = QMGame.GetBackBufferHeight();

            m_Circuit = new ElecCircuit();

            EPile ed = new EPile();
            ed.m_Start.m_Pos = new Point(150, 50);
            ed.m_End.m_Pos = new Point(100, 50);
            m_Circuit.AddDipole((ElecDipole)ed);

            EWireSegment ed0 = new EWireSegment();
            ed0.m_Start.m_Pos = new Point(150, 50);
            ed0.m_End.m_Pos = new Point(200, 50);
            m_Circuit.AddDipole(ed0);

            ed0 = new EWireSegment();
            ed0.m_Start.m_Pos = new Point(50, 50);
            ed0.m_End.m_Pos = new Point(100, 50);
            m_Circuit.AddDipole(ed0);

            AddBoucle(0);
            AddBoucle(100);


            m_Circuit.ComputeCircuit();

        }

        void AddBoucle(int decaly)
        {
            EResistance ed1 = new EResistance();
            ed1.m_Start.m_Pos = new Point(200, 50+decaly);
            ed1.m_End.m_Pos = new Point(200, 150+decaly);
            m_Circuit.AddDipole((ElecDipole)ed1);

            EWireSegment ed = new EWireSegment(); 
            ed.m_Start.m_Pos = new Point(200, 150 + decaly);
            ed.m_End.m_Pos = new Point(150, 150 + decaly);
            m_Circuit.AddDipole(ed);

            ed = new EWireSegment(); 
            ed.m_Start.m_Pos = new Point(100, 150 + decaly);
            ed.m_End.m_Pos = new Point(50, 150 + decaly);
            m_Circuit.AddDipole(ed);

            ed = new EWireSegment();
            ed.m_Start.m_Pos = new Point(50, 50 + decaly);
            ed.m_End.m_Pos = new Point(50, 100 + decaly);
            m_Circuit.AddDipole(ed);

            ed = new EWireSegment();
            ed.m_Start.m_Pos = new Point(50, 100 + decaly);
            ed.m_End.m_Pos = new Point(50, 150 + decaly);
            m_Circuit.AddDipole(ed);

            EResistance ed6 = new EResistance();
            ed6.m_Start.m_Pos = new Point(150, 150 + decaly);
            ed6.m_End.m_Pos = new Point(100, 150 + decaly);
            m_Circuit.AddDipole((ElecDipole)ed6);
            if (decaly > 0)
                ed6.m_Ohm = 20.0f;
        }


        override public void Update()
        {
            m_Circuit.DoOneStep();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //for (int i = 0; i < m_ASeg.GetLength(0); i++)
            //{
            //    m_ASeg[i].DrawIntensity(m_Scene);
            //}
            m_Circuit.DrawDebug(m_Scene.m_BlockTextureData, m_Scene.sx, m_Scene.sy);
            base.Draw(spriteBatch);
        }

        public override void DrawText(SpriteBatch spriteBatch)
        {
            m_Circuit.DrawText(spriteBatch);
        }

    }
}
