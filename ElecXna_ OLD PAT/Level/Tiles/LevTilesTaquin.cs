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
using QM.Util;
using QM.Level.Menu;
using QM.Object.Widget;


namespace QM.Level
{
    class LevTilesTaquin : LevTilesBase
    {
        //int m_Score = 0;
        protected WText m_TextCounter;

        public Point m_PosHole;
        public Point m_LastPosHole;
        protected TimeSpan m_StartTime;
        protected Boolean m_bHasToShuffle = false;
        protected double m_DelayShuffle = 3.0f;

        protected Random random = null;

        protected WEndOfLevelBox m_EndOfLevelBox;
        public override void StartLevel(String Param1)
        {
            //Pour mélanger différement à chaque fois.
            random = new Random(Mgr.m_GameTime.TotalRealTime.Milliseconds);


            Point SizeInTile = new Point();
            if (Param1.Contains("3x3"))
                SizeInTile = new Point(3, 3);
            if (Param1.Contains("3x4"))
                SizeInTile = new Point(3, 4);

            int TileSize = ((420 / SizeInTile.Y) /4) * 4;
            base.StartLevel(TileSize, 30, new Point(50, 5), SizeInTile);

            AddObj(new WButton(new Vector2(710, 220), "Shuffle", FontManager.eFontID.Font1, DoShuffle));
            AddObj(new WButton(new Vector2(710, 250), "Restart", FontManager.eFontID.Font1, DoRestartLevel));

            
            AddObj(m_EndOfLevelBox = new WEndOfLevelBox(new Vector2(400, 200), ""));
            m_EndOfLevelBox.SetActiveAndVisibleState(false);

            CreateCircuit(Param1);
            m_LastPosHole = m_PosHole = new Point(m_TileBoard.m_SizeInTile.X - 1, m_TileBoard.m_SizeInTile.Y - 1);

            //Counter before SHuffle
            m_StartTime = Mgr.m_GameTime.TotalRealTime;
            m_bHasToShuffle = true;
            AddObj(m_TextCounter = new WText(new Vector2(210, 100), "", FontManager.eFontID.Font0));
       }


        void CreateCircuit3x3_1()
        {
            m_TileBoard.m_TileScene.RemoveAll();

            m_TileBoard.m_TileScene.AddTile(ETile.eType.Wire2, new Point(0, 0), Angle360.Est);
            m_TileBoard.m_TileScene.AddTile(ETile.eType.Pile, new Point(1, 0), Angle360.Est);
            m_TileBoard.m_TileScene.AddTile(ETile.eType.Wire2, new Point(2, 0), Angle360.South);

            m_TileBoard.m_TileScene.AddTile(ETile.eType.Resistance, new Point(0, 1), Angle360.North);
            m_TileBoard.m_TileScene.AddTile(ETile.eType.Wire2, new Point(1, 1), Angle360.Est);
            m_TileBoard.m_TileScene.AddTile(ETile.eType.Wire2, new Point(2, 1), Angle360.West);

            m_TileBoard.m_TileScene.AddTile(ETile.eType.Wire2, new Point(0, 2), Angle360.North);
            m_TileBoard.m_TileScene.AddTile(ETile.eType.Wire2, new Point(1, 2), Angle360.West);

            m_LastPosHole = m_PosHole = new Point(m_TileBoard.m_SizeInTile.X - 1, m_TileBoard.m_SizeInTile.Y - 1);
            m_TileBoard.m_Circuit.ComputeCircuit();
        }
        void CreateCircuit3x4_1()
        {
            m_TileBoard.m_TileScene.RemoveAll();

            m_TileBoard.m_TileScene.AddTile(ETile.eType.Wire2, new Point(0, 0), Angle360.Est);
            m_TileBoard.m_TileScene.AddTile(ETile.eType.Wire3, new Point(1, 0), Angle360.South);
            m_TileBoard.m_TileScene.AddTile(ETile.eType.Wire2, new Point(2, 0), Angle360.South);

            m_TileBoard.m_TileScene.AddTile(ETile.eType.Pile, new Point(0, 1), Angle360.South);
            m_TileBoard.m_TileScene.AddTile(ETile.eType.Resistance, new Point(1, 1), Angle360.South);
            m_TileBoard.m_TileScene.AddTile(ETile.eType.Resistance, new Point(2, 1), Angle360.South);

            m_TileBoard.m_TileScene.AddTile(ETile.eType.Wire, new Point(0, 2), Angle360.North);
            m_TileBoard.m_TileScene.AddTile(ETile.eType.Wire3, new Point(1, 2), Angle360.Est);
            m_TileBoard.m_TileScene.AddTile(ETile.eType.Wire2, new Point(2, 2), Angle360.West);

            m_TileBoard.m_TileScene.AddTile(ETile.eType.Wire2, new Point(0, 3), Angle360.North);
            m_TileBoard.m_TileScene.AddTile(ETile.eType.Wire2, new Point(1, 3), Angle360.West);

            m_LastPosHole = m_PosHole = new Point(m_TileBoard.m_SizeInTile.X - 1, m_TileBoard.m_SizeInTile.Y - 1);
            m_TileBoard.m_Circuit.ComputeCircuit();
        }
        void CreateCircuit3x4_2()
        {
            m_TileBoard.m_TileScene.RemoveAll();

            m_TileBoard.m_TileScene.AddTile(ETile.eType.Wire2, new Point(0, 0), Angle360.Est);
            m_TileBoard.m_TileScene.AddTile(ETile.eType.Pile, new Point(1, 0), Angle360.West);
            m_TileBoard.m_TileScene.AddTile(ETile.eType.Wire2, new Point(2, 0), Angle360.South);

            m_TileBoard.m_TileScene.AddTile(ETile.eType.Wire3, new Point(0, 1), Angle360.Est);
            m_TileBoard.m_TileScene.AddTile(ETile.eType.Resistance, new Point(1, 1), Angle360.Est);
            m_TileBoard.m_TileScene.AddTile(ETile.eType.Wire3, new Point(2, 1), Angle360.West);

            m_TileBoard.m_TileScene.AddTile(ETile.eType.Resistance, new Point(0, 2), Angle360.North);
            m_TileBoard.m_TileScene.AddTile(ETile.eType.Wire2, new Point(1, 2), Angle360.Est);
            m_TileBoard.m_TileScene.AddTile(ETile.eType.Wire2, new Point(2, 2), Angle360.West);

            m_TileBoard.m_TileScene.AddTile(ETile.eType.Wire2, new Point(0, 3), Angle360.North);
            m_TileBoard.m_TileScene.AddTile(ETile.eType.Wire2, new Point(1, 3), Angle360.West);

            m_LastPosHole = m_PosHole = new Point(m_TileBoard.m_SizeInTile.X - 1, m_TileBoard.m_SizeInTile.Y - 1);
            m_TileBoard.m_Circuit.ComputeCircuit();
        }
        void CreateCircuit(String which)
        {
            if (which == "Taquin3x4_1")
                CreateCircuit3x4_1();
            if (which == "Taquin3x4_2")
                CreateCircuit3x4_2();
            if (which == "Taquin3x3_1")
                CreateCircuit3x3_1();
        }

        public override void Update()
        {
            base.Update();
            if (m_bHasToShuffle)
            {
                TimeSpan Delay = Mgr.m_GameTime.TotalRealTime - m_StartTime;
                if (Delay.TotalSeconds > m_DelayShuffle)
                {
                    Shuffle();
                    m_bHasToShuffle = false;
                    m_TextCounter.SetActiveAndVisibleState(false);
                }
                else
                {
                    m_TextCounter.SetActiveAndVisibleState(true);
                    m_TextCounter.m_Text = ((int)(m_DelayShuffle - Delay.TotalSeconds + 0.9)).ToString();
                }
            }
            else
            {
                int NbActiv = m_TileBoard.m_TileScene.CountActiv();
                if(NbActiv==m_TileBoard.m_SizeInTile.X*m_TileBoard.m_SizeInTile.Y-1)
                    m_EndOfLevelBox.SetActiveAndVisibleState(true);
            }
        }
        public Boolean PosAreContigue(Point p1, Point p2)
        {
            if (p1.X == p2.X && ((p1.Y == p2.Y + 1) || (p1.Y == p2.Y - 1)))
                return true;
            if (p1.Y == p2.Y && ((p1.X == p2.X + 1) || (p1.X == p2.X - 1)))
                return true;
            return false;
        }

        public override void DoMouse(InputManager.MouseInput newState, InputManager.MouseInput oldState, KeyboardState keyState)
        {
            base.DoMouse(newState, oldState, keyState);

            if (!m_TileBoard.m_BackgroundScene.Contains(newState.X, newState.Y))
                return;

            if (newState.LeftButton == ButtonState.Pressed && oldState.LeftButton == ButtonState.Released)
            {
                Point P = new Point(newState.X, newState.Y);
                Point PosTile = m_TileBoard.m_TileScene.GetTileFromPos(P);
                ETile et = m_TileBoard.m_TileScene.FindTile(PosTile);
                if (et != null)
                {
                    if (PosAreContigue(PosTile, m_PosHole))
                    {
                        m_TileBoard.m_TileScene.MoveTile(PosTile, m_PosHole);
                        m_PosHole = PosTile;
                    }
                }
            }

        }

        public void DoDestroyLL(Point _Which)
        {
            ETile et = m_TileBoard.m_TileScene.FindTile(_Which);
            if (et != null)
            {
                et.Destroy();
            }
        }


        void Shuffle()
        {
            for (int i = 0; i < 100; i++)
                ShuffleOnce();
        }
        void ShuffleOnce()
        {
            Point PosTile = m_PosHole;
            Boolean found = false;
            while (!found)
            {
                PosTile = m_PosHole;
                int which = random.Next(4);
                switch (which)
                {
                    case 0: PosTile.X -= 1; break;
                    case 1: PosTile.Y -= 1; break;
                    case 2: PosTile.X += 1; break;
                    case 3: PosTile.Y += 1; break;
                }
                found = PosTile.X >= 0 && PosTile.Y >= 0 && PosTile.X < m_TileBoard.m_SizeInTile.X && PosTile.Y < m_TileBoard.m_SizeInTile.Y;
                if (PosTile == m_LastPosHole)
                    found = false;
            }

            m_TileBoard.m_TileScene.MoveTile(PosTile, m_PosHole);
            m_LastPosHole = m_PosHole;
            m_PosHole = PosTile;
        }
        public void DoShuffle(InputManager.MouseInput newState, InputManager.MouseInput oldState, KeyboardState keyState)
        {
            Shuffle();
        }

    }
}
