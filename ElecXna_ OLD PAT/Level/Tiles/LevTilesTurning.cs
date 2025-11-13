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
    class LevTilesTurning : LevTilesBase
    {
        int m_Score = 0;

        //protected ETileScene m_TileChoice;
        //protected ETileScene m_TileMouse;

        public enum eState { Normal, EnPose, Selected };
        public eState m_State = eState.Normal;
        public Point m_PosSelected;
        protected OGSprite m_ImageSelection;

        protected WButton m_ButtonDestroy;
        protected WButton m_ButtontTurnLeft;
        protected WButton m_ButtontTurnRight;
        protected WButton m_ButtonModePlan;

        protected WText m_TextScore;
        protected Random random = new Random(21);

        protected WImage m_TurnImage;

        public override void StartLevel(String Param1)
        {
            Point SizeInTile = new Point(8, 5);
            if (Param1.Contains("6x4"))
                SizeInTile = new Point(6, 4);
            if (Param1.Contains("8x5"))
                SizeInTile = new Point(8, 5);


            base.StartLevel(80, 30, new Point(10, 10), SizeInTile);

            m_TileBoard.CreateWiredEdgeH(true);
            m_TileBoard.CreateWiredEdgeH(false);
            m_TileBoard.CreateWiredEdgeV(true);
            m_TileBoard.CreateWiredEdgeV(false);

            FillGap();

            int w = m_TileBoard.m_TileScene.m_TileWidth;
            AddObj(m_ImageSelection = new OGSprite("ImagesQM\\Border.png", new MathPntSize(0, 0, w, w), OGSprite.ePosType.Centered, Color.White));

            AddObj(m_TurnImage = new WImage("ImagesElec\\ArrowTurnX.png", new Rectangle(400, 200, 75, 23), Color.White));
            m_TurnImage.m_Depth = 0.01f;

            AddObj(m_ButtonDestroy = new WButton(new Vector2(710, 10), "Destroy", FontManager.eFontID.Font1, DoDestroy));
            AddObj(m_ButtontTurnLeft = new WButton(new Vector2(710, 40), "TurnLeft", FontManager.eFontID.Font1, DoTurnLeft));
            AddObj(m_ButtontTurnRight = new WButton(new Vector2(710, 70), "TurnRight", FontManager.eFontID.Font1, DoTurnRight));
            AddObj(new WText(new Vector2(710, 100), "-", FontManager.eFontID.Font1));
            AddObj(m_ButtonModePlan = new WButton(new Vector2(710, 130), "ModePlan", FontManager.eFontID.Font1, DoModePlan));
            AddObj(new WText(new Vector2(710, 160), "-", FontManager.eFontID.Font1));
            AddObj(new WButton(new Vector2(710, 190), "RemoveActiv", FontManager.eFontID.Font1, DoRemoveActiv));
            AddObj(new WButton(new Vector2(710, 220), "FillGap", FontManager.eFontID.Font1, DoFillGap));
            ShowButton();

            String StrScore = GetStringScore();
            AddObj(m_TextScore = new WText(new Vector2(710, 400), StrScore, FontManager.eFontID.Font0));
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
            ETile.eType Type = (ETile.eType)random.Next(1+(int)ETile.eType.Lampe);
            while (Type == ETile.eType.WireNoCross)
                Type = (ETile.eType)random.Next(1+(int)ETile.eType.Lampe);
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
                        Angle360 Angle = new Angle360(random.Next(4)*90);
                        m_TileBoard.m_TileScene.AddTile(Type, new Point(i, j), Angle);
                    }
                }
            }
            m_TileBoard.m_Circuit.ComputeCircuit();
        }


        public override void DoMouse(InputManager.MouseInput newState, InputManager.MouseInput oldState, KeyboardState keyState)
        {
            base.DoMouse(newState, oldState, keyState);

            if (!m_TileBoard.m_BackgroundScene.Contains(newState.X, newState.Y))
                return;
            
            switch (m_State)
            {
                case eState.Normal:
                case eState.Selected:
                    {
                        if (newState.LeftButton == ButtonState.Pressed && oldState.LeftButton == ButtonState.Released)
                        {
                            Point P = new Point(newState.X,newState.Y);
                            Point Pos = m_TileBoard.m_TileScene.GetTileFromPos(P);
                            if (m_State == eState.Selected && Pos == m_PosSelected)
                            {
                                Point Pos2;
                                Pos2.X = m_PosSelected.X * m_TileBoard.m_TileScene.m_TileWidth + m_TileBoard.m_TileScene.m_Pos.X;
                                Pos2.Y = m_PosSelected.Y * m_TileBoard.m_TileScene.m_TileWidth + m_TileBoard.m_TileScene.m_Pos.Y;
                                //Pos.X += (m_TileBoard.m_TileScene.m_TileWidth - m_TurnImage.m_DstRect.Width) / 2;
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
                            }
                            else
                            {
                                m_PosSelected = Pos;
                                ETile et = m_TileBoard.m_TileScene.FindTile(m_PosSelected);
                                if (et != null)
                                {
                                    ChangeState(eState.Selected);
                                }
                            }
                        }
                    }
                    break;
                case eState.EnPose:
                    {
                        if (newState.LeftButton == ButtonState.Pressed && oldState.LeftButton == ButtonState.Released)
                        {

                        }
                    }
                    break;
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
            if(m_State==eState.Selected)
            {
                DoShow = true;
                Point Pos;
                Pos.X = m_PosSelected.X * m_TileBoard.m_TileScene.m_TileWidth + m_TileBoard.m_TileScene.m_Pos.X;
                Pos.X += (m_TileBoard.m_TileScene.m_TileWidth - m_TurnImage.m_DstRect.Width) / 2;
                Pos.Y = m_PosSelected.Y * m_TileBoard.m_TileScene.m_TileWidth + m_TileBoard.m_TileScene.m_Pos.Y + 2;
                m_TurnImage.SetDstPos(Pos);
            }

            m_ButtonDestroy.SetActiveAndVisibleState(DoShow);
            m_ButtontTurnLeft.SetActiveAndVisibleState(DoShow);
            m_ButtontTurnRight.SetActiveAndVisibleState(DoShow);
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
            m_TextScore.m_Text = GetStringScore();
        }
        public void DoFillGap(InputManager.MouseInput newState, InputManager.MouseInput oldState, KeyboardState keyState)
        {
            FillGap();
        }

    }
}
