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
using QM.Elec;

namespace QM.Level
{
    class LevTilesAdd : LevTilesBase
    {
        int m_Score = 0;

        protected ETileScene m_TileChoice;
        protected OEScene m_ChoiceScene;
        public Point m_PosChoiceSelected;
        protected ETileScene m_TileMouse;
        protected OEScene m_MouseScene;

        public enum eState { Normal, EnPose, Selected };
        public eState m_State = eState.Normal;
        public Point m_PosSelected;
        protected OGSprite m_ImageSelection;

        protected WButton m_ButtonDestroy;
        //protected WButton m_ButtontTurnLeft;
        //protected WButton m_ButtontTurnRight;
        protected WButton m_ButtonModePlan;

        //protected WText m_TextScore;
        protected Random random = new Random(21);

        protected WImage m_TurnImage;

        public override void StartLevel(String Param1)
        {
            base.StartLevel(80, 30, new Point(10, 10), new Point(8, 5));

            m_TileBoard.CreateWiredEdgeH(true);
            m_TileBoard.CreateWiredEdgeH(false);
            m_TileBoard.CreateWiredEdgeV(true);
            m_TileBoard.CreateWiredEdgeV(false);

            //FillGap();

            int w = m_TileBoard.m_TileScene.m_TileWidth;
            AddObj(m_ImageSelection = new OGSprite("ImagesQM\\Border.png", new MathPntSize(0, 0, w, w), OGSprite.ePosType.Centered, Color.White));

            AddObj(m_TurnImage = new WImage("ImagesElec\\ArrowTurnX.png", new Rectangle(400, 200, 75, 23), Color.White));
            m_TurnImage.m_Depth = 0.01f;

            int X = 860;
            AddObj(m_ButtonDestroy = new WButton(new Vector2(X, 10), "Destroy", FontManager.eFontID.Font1, DoDestroy));
            //AddObj(m_ButtontTurnLeft = new WButton(new Vector2(X, 40), "TurnLeft", FontManager.eFontID.Font1, DoTurnLeft));
            //AddObj(m_ButtontTurnRight = new WButton(new Vector2(X, 70), "TurnRight", FontManager.eFontID.Font1, DoTurnRight));
            AddObj(new WText(new Vector2(X, 100), "-", FontManager.eFontID.Font1));
            AddObj(m_ButtonModePlan = new WButton(new Vector2(X, 130), "ModePlan", FontManager.eFontID.Font1, DoModePlan));
            AddObj(new WText(new Vector2(X, 160), "-", FontManager.eFontID.Font1));
            AddObj(new WButton(new Vector2(X, 190), "RemoveActiv", FontManager.eFontID.Font1, DoRemoveActiv));
            AddObj(new WButton(new Vector2(X, 220), "FillGap", FontManager.eFontID.Font1, DoFillGap));

            //String StrScore = GetStringScore();
            //AddObj(m_TextScore = new WText(new Vector2(X, 400), StrScore, FontManager.eFontID.Font0));

            #region m_TileChoice
            int TileSize = 50;
            int MarginChoice = 10;
            Point PosTopLeft = new Point(720, 20);
            Point SizeInTile = new Point(2, 6);

            AddObj(m_ChoiceScene = new OEScene(TileSize, MarginChoice, PosTopLeft, SizeInTile));



            Point Pos = new Point(PosTopLeft.X + MarginChoice, PosTopLeft.Y + MarginChoice);
            m_TileChoice = new ETileScene(m_ChoiceScene, null, TileSize, Pos, SizeInTile);
            AddObj(m_TileChoice);

            m_TileChoice.AddTile(ETile.eType.Pile, new Point(0, 0), Angle360.North);
            m_TileChoice.AddTile(ETile.eType.Resistance, new Point(0, 1), Angle360.North);
            m_TileChoice.AddTile(ETile.eType.Wire, new Point(0, 2), Angle360.North);
            m_TileChoice.AddTile(ETile.eType.Wire2, new Point(0, 3), Angle360.North);
            m_TileChoice.AddTile(ETile.eType.Wire3, new Point(0, 4), Angle360.North);
            m_TileChoice.AddTile(ETile.eType.Wire4, new Point(0, 5), Angle360.North);
            m_TileChoice.AddTile(ETile.eType.Lampe, new Point(1, 0), Angle360.North);
            m_TileChoice.AddTile(ETile.eType.Diode, new Point(1, 1), Angle360.North);
            m_TileChoice.AddTile(ETile.eType.Condensateur, new Point(1, 2), Angle360.North);
            m_TileChoice.AddTile(ETile.eType.Bobine, new Point(1, 3), Angle360.North);

            #endregion

            #region m_TileMouse
            //int TileSize = 50;
            int MarginMouse = 2;
            //Point PosTopLeft = new Point(720, 20);
            Point SizeInTileMouse = new Point(1, 1);

            AddObj(m_MouseScene = new OEScene(m_TileBoard.m_TileSize, MarginMouse, PosTopLeft, SizeInTileMouse));



           //Point Pos = new Point(PosTopLeft.X + MarginMouse, PosTopLeft.Y + MarginMouse);
            m_TileMouse = new ETileScene(m_MouseScene, null, m_TileBoard.m_TileSize, Pos, SizeInTileMouse);
            AddObj(m_TileMouse);
            m_TileMouse.SetActiveAndVisibleState(false);
            m_TileMouse.m_Scene.m_WImage.SetActiveAndVisibleState(false);


            #endregion


            ShowButton();

        }

        String GetStringScore()
        {
            String dst;
            dst = StringManager.Get("Text_Score");
            dst += m_Score.ToString();
            return dst;
        }

        ETile.eType GetRandomTile()
        {
            ETile.eType Type = (ETile.eType)random.Next((int)ETile.eType.None);
            while (Type == ETile.eType.WireNoCross)
                Type = (ETile.eType)random.Next((int)ETile.eType.None);
            return Type;
        }

        void FillGap()
        {
            for (int i = 0; i < m_TileBoard.m_TileScene.m_SizeInTile.X; i++)
            {
                for (int j = 0; j < m_TileBoard.m_TileScene.m_SizeInTile.Y; j++)
                {
                    if (m_TileBoard.m_TileScene.GetTile(i, j) == null)
                    {
                        ETile.eType Type = GetRandomTile();
                        Angle360 Angle = new Angle360(random.Next(4));
                        m_TileBoard.m_TileScene.AddTile(Type, new Point(i, j), Angle);
                    }
                }
            }
            m_TileBoard.m_Circuit.ComputeCircuit();
        }


        public override void Update()
        {
            base.Update();

            InputManager.MouseInput MS = InputManager.m_Current;
            Point PM = new Point(MS.X , MS.Y );
            if (m_TileBoard.m_BackgroundScene.Contains(PM.X, PM.Y))
            {
                Point PosCh = m_TileBoard.m_TileScene.GetTileFromPos(PM);
                Point NewPos = m_TileBoard.m_TileScene.GetPosFromTile(PosCh);
                NewPos.X -= m_TileBoard.m_TileSize / 2;
                NewPos.Y -= m_TileBoard.m_TileSize / 2;
                m_TileMouse.MoveAllScene(NewPos);
            }
            else
                m_TileMouse.MoveAllScene(PM);
        }

        public override void DoMouse(InputManager.MouseInput newState, InputManager.MouseInput oldState, KeyboardState keyState)
        {
            base.DoMouse(newState, oldState, keyState);

            
            if (m_ChoiceScene.Contains(newState.X, newState.Y))
            {

                if (newState.LeftButton == ButtonState.Pressed && oldState.LeftButton == ButtonState.Released)
                {
                    Point PCh = new Point(newState.X, newState.Y);
                    Point PosCh = m_TileChoice.GetTileFromPos(PCh);
                    ETile et = m_TileChoice.FindTile(PosCh);
                    if (et != null)
                    {
                        m_PosChoiceSelected = PosCh;
                        m_State = eState.EnPose;
                        m_TileMouse.RemoveAll();
                        m_TileMouse.AddTile(et.m_Type, new Point(0,0), et.m_A);
                        m_TileMouse.SetActiveAndVisibleState(true);
                    }
                    else
                    {
                        m_State = eState.Normal;
                    }
                }
                return;
            }


            if (!m_TileBoard.m_BackgroundScene.Contains(newState.X, newState.Y))
                return;
            Point P = new Point(newState.X, newState.Y);
            Point Pos = m_TileBoard.m_TileScene.GetTileFromPos(P);

            //cas à traiter:
            // Je clic sur un des boutons turn (je suis forcément en selection)
            // Je sur un Tile existant -> je passe en mode Select et je le selectionne
            // Je clic dans le vide en mode Select -> je passe en mode normal
            // Je clic dans le vide en mode Pose -> Je pose

            if (newState.LeftButton == ButtonState.Pressed && oldState.LeftButton == ButtonState.Released)
            {
                ETile et = m_TileBoard.m_TileScene.FindTile(Pos);

                //Je tourne si en mode select et clic sur bouton tourne
                if (et != null && m_State == eState.Selected && Pos == m_PosSelected)
                {
                    Point Pos2;
                    Pos2.X = m_PosSelected.X * m_TileBoard.m_TileScene.m_TileWidth + m_TileBoard.m_TileScene.m_Pos.X;
                    Pos2.Y = m_PosSelected.Y * m_TileBoard.m_TileScene.m_TileWidth + m_TileBoard.m_TileScene.m_Pos.Y;
                    Point Decal;
                    Decal.X = newState.X - Pos2.X;
                    Decal.Y = newState.Y - Pos2.Y;
                    if (Decal.Y < 30)
                    {
                        if (Decal.X < m_TileBoard.m_TileScene.m_TileWidth / 2)
                            DoTurnLeft(newState, oldState, keyState);
                        else
                            DoTurnRight(newState, oldState, keyState);
                    }
                    return;
                }

                // Je sur un Tile existant -> je passe en mode Select et je le selectionne
                if (et != null)
                {
                    m_PosSelected = Pos;
                    ChangeState(eState.Selected);
                }

                // Je clic dans le vide en mode Select -> je passe en mode normal
                if (et == null && m_State == eState.Selected)
                {
                    m_PosSelected = Pos;
                    ChangeState(eState.Normal);
                }


                // Je clic dans le vide en mode Pose
                if (et == null && m_State == eState.EnPose)
                {
                    ETile et1 = m_TileChoice.FindTile(m_PosChoiceSelected);
                    if (et1 != null)
                    {
                        ETile.eType type = et1.m_Type;
                        Angle360 A = et1.m_A;
                        m_TileBoard.m_TileScene.AddTile(type, Pos, A);
                        m_TileBoard.m_Circuit.SetDurty();
                    }
                }
            }

        }

        protected void ChangeState(eState _new)
        {
            m_State = _new;
            ShowButton();
        }
        protected void ShowButton()
        {
            Boolean DoShow = false;
            if (m_State == eState.Selected)
            {
                DoShow = true;
                Point Pos;
                Pos.X = m_PosSelected.X * m_TileBoard.m_TileScene.m_TileWidth + m_TileBoard.m_TileScene.m_Pos.X;
                Pos.X += (m_TileBoard.m_TileScene.m_TileWidth - m_TurnImage.m_DstRect.Width) / 2;
                Pos.Y = m_PosSelected.Y * m_TileBoard.m_TileScene.m_TileWidth + m_TileBoard.m_TileScene.m_Pos.Y + 2;
                m_TurnImage.SetDstPos(Pos);
            }

            m_TileMouse.SetActiveAndVisibleState(m_State==eState.EnPose);

            m_ButtonDestroy.SetActiveAndVisibleState(DoShow);
            //m_ButtontTurnLeft.SetActiveAndVisibleState(DoShow);
            //m_ButtontTurnRight.SetActiveAndVisibleState(DoShow);
            m_TurnImage.SetActiveAndVisibleState(DoShow);

            m_ImageSelection.m_TRSRelativ.m_Pos = Misc.Vector2FromPoint(m_TileBoard.m_TileScene.GetPosFromTile(m_PosSelected));
            m_ImageSelection.SetActiveAndVisibleState(DoShow);
        }

        public void DoDestroy(InputManager.MouseInput newState, InputManager.MouseInput oldState, KeyboardState keyState)
        {
            if (m_State == eState.Selected)
            {
                ETile et = m_TileBoard.m_TileScene.FindTile(m_PosSelected);
                if (et != null)
                {
                    et.Destroy();
                }
                ChangeState(eState.Normal);
            }
        }
        public void DoTurnLeft(InputManager.MouseInput newState, InputManager.MouseInput oldState, KeyboardState keyState)
        {
            if (m_State == eState.Selected)
            {
                m_TileBoard.m_TileScene.TurnLeftTile(m_PosSelected);
            }
        }
        public void DoTurnRight(InputManager.MouseInput newState, InputManager.MouseInput oldState, KeyboardState keyState)
        {
            if (m_State == eState.Selected)
            {
                m_TileBoard.m_TileScene.TurnRightTile(m_PosSelected);
            }
        }
        public void DoModePlan(InputManager.MouseInput newState, InputManager.MouseInput oldState, KeyboardState keyState)
        {
            OEScene.eMode newMode;

            if (m_TileBoard.m_BackgroundScene.m_Mode == OEScene.eMode.ModeReal)
            {
                m_ButtonModePlan.m_Text = "ModeReal";
                newMode = OEScene.eMode.ModePlan;
            }
            else
            {
                m_ButtonModePlan.m_Text = "ModePlan";
                newMode = OEScene.eMode.ModeReal;
            }
            m_TileBoard.m_BackgroundScene.SetMode(newMode);
            m_TileBoard.m_TileScene.SetMode(newMode);
            m_TileBoard.DestroyEdge();
            m_TileBoard.CreateWiredEdgeH(true);
            m_TileBoard.CreateWiredEdgeH(false);
            m_TileBoard.CreateWiredEdgeV(true);
            m_TileBoard.CreateWiredEdgeV(false);
        }
        public void DoRemoveActiv(InputManager.MouseInput newState, InputManager.MouseInput oldState, KeyboardState keyState)
        {
            m_Score += m_TileBoard.m_TileScene.RemoveAllActiv();
            //m_TextScore.m_Text = GetStringScore();
        }
        public void DoFillGap(InputManager.MouseInput newState, InputManager.MouseInput oldState, KeyboardState keyState)
        {
            FillGap();
        }

    }
}
