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

namespace QM.Level
{
    class LevETest4 : LevETestBase
    {

        OESegment[] m_ASeg = new OESegment[56];
        OEScene m_Scene;

        Boolean m_Circuit1Open = true;
        Boolean m_Circuit2Open = false;
        
       // WEndOfLevelBox m_EndOfLevelBox;

        public override void StartLevel(String Param1)
        {
            AddObj(m_Scene = new OEScene(10, 10, 800, 600));

            int sx = QMGame.GetBackBufferWidth();
            int sy = QMGame.GetBackBufferHeight();

            for (int i = 0; i < m_ASeg.GetLength(0); i++)
            {
                m_ASeg[i] = new OESegment();
                //         m_ASeg[i].Create(new Vector2(200 + i * 30, 50), new Vector2(200 + (i + 1) * 30, 50),10);
                AddObj(m_ASeg[i]);
            }

            CreateCircuit1();//(0, ref Count, 10.0f, 1.0f, true);

            CreateCircuit2();//(300, ref Count, 20.0f, 2.0f, false);

            //AddObj(m_EndOfLevelBox = new WEndOfLevelBox(new Vector2(sx * 2 / 8, sy * 3 / 8), ""));
            //m_EndOfLevelBox.SetActiveAndVisibleState(true);

            AddObj(new WButton(new Vector2(400, 150), "  *  ", FontManager.eFontID.Font0, DoChangeInterrupt));

        }
        void CreateCircuit1()
        {
            int Count = 0;
            CreateCircuit(0, ref Count, 10.0f, 1.0f, m_Circuit1Open);
        }
        void CreateCircuit2()
        {
            int Count = 28;
            CreateCircuit(300, ref Count, 10.0f, 2.0f, m_Circuit2Open);
        }

        public void DoChangeInterrupt(InputManager.MouseInput newState, InputManager.MouseInput oldState, KeyboardState keyState)
        {
            m_Circuit1Open = !m_Circuit1Open;
            CreateCircuit1();
        }

        void CreateCircuit(int X, ref int Count, float Tension, float Intensity, Boolean Open)
        {
            if (Open)
                Intensity = 0.0f;

            //fil
            m_ASeg[Count].Create(new Vector2(X + 280, 255), new Vector2(X + 195, 255), 10, false);
            m_ASeg[Count + 1].Create(new Vector2(X + 200, 250), new Vector2(X + 200, 55), 10, false);
            m_ASeg[Count + 2].Create(new Vector2(X + 195, 50), new Vector2(X + 250, 50), 10, false);
            m_ASeg[Count + 0].m_PotentielGoal = m_ASeg[Count + 1].m_PotentielGoal = m_ASeg[Count + 2].m_PotentielGoal = Tension / 2.0f;
            m_ASeg[Count + 0].m_IntensityGoal = m_ASeg[Count + 1].m_IntensityGoal = m_ASeg[Count + 2].m_IntensityGoal = Intensity;
            Count += 3;

            //résistance
            int NbInR = 20;
            float tensionTmp;
            for (int i = 0; i < NbInR; i++)
            {
                m_ASeg[Count + i].Create(new Vector2(X + 250 + i * 100 / NbInR, 50), new Vector2(X + 250 + (i + 1) * 100 / NbInR, 50), 50, false);
                m_ASeg[Count + i].m_IntensityGoal = Intensity;
                if (Open)
                    tensionTmp = Tension / 2.0f;
                else
                    tensionTmp = Tension / 2.0f - Tension * (i + 0.5f) / NbInR;
                m_ASeg[Count + i].m_PotentielGoal = tensionTmp;
            }
            Count += 20;

            //fil
            m_ASeg[Count + 0].Create(new Vector2(X + 350, 50), new Vector2(X + 405, 50), 10, false);
            m_ASeg[Count + 1].Create(new Vector2(X + 400, 55), new Vector2(X + 400, 125), 10, false);
            if (Open)
                tensionTmp = Tension / 2.0f;
            else
                tensionTmp = -Tension / 2.0f;
            m_ASeg[Count + 0].m_PotentielGoal = m_ASeg[Count + 1].m_PotentielGoal = tensionTmp;
            m_ASeg[Count + 0].m_IntensityGoal = m_ASeg[Count + 1].m_IntensityGoal = Intensity;
            Count += 2;

            //interupteur
            int XInterrupt = X + 400 + (Open ? 20 : 0);
            m_ASeg[Count + 0].Create(new Vector2(XInterrupt, 125), new Vector2(XInterrupt, 175), 10, false);
            if (Open)
                tensionTmp = 0.0f;
            else
                tensionTmp = -Tension / 2.0f;
            m_ASeg[Count + 0].m_PotentielGoal = tensionTmp;
            m_ASeg[Count + 0].m_IntensityGoal = Intensity;
            Count += 1;

            //fil
            m_ASeg[Count + 0].Create(new Vector2(X + 400, 175), new Vector2(X + 400, 250), 10, false);
            m_ASeg[Count + 1].Create(new Vector2(X + 405, 255), new Vector2(X + 320, 255), 10, false);
            m_ASeg[Count + 0].m_PotentielGoal = m_ASeg[Count + 1].m_PotentielGoal = -Tension / 2.0f;
            m_ASeg[Count + 0].m_IntensityGoal = m_ASeg[Count + 1].m_IntensityGoal = Intensity;
            Count += 2;

        }

        //public override void DoMouse(InputManager.MouseInput newState, InputManager.MouseInput oldState, KeyboardState keyState)
        //{
        //    base.DoMouse(newState, oldState, keyState);
        //    if(newState.LeftButton == ButtonState.Pressed)
        //        for (int i = 0; i < m_ASeg.GetLength(0); i++)
        //        {
        //            m_ASeg[i].m_PotentielGoal = 4.0f * (float)Math.Sin(i * MathHelper.Pi / 20.0f + Mgr.m_GameTime.TotalRealTime.TotalSeconds);
        //            m_ASeg[i].m_IntensityGoal = 2.0f;
        //        } 

        //}

        override public void Update()
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < m_ASeg.GetLength(0); i++)
            {
                m_ASeg[i].DrawIntensity(m_Scene);
            }
            base.Draw(spriteBatch);
        }
    }
}
