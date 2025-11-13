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
    class LevTilesPipeDream : LevTilesBase
    {
        int m_Score = 0;



        protected ETileScene m_TileChoice;
        protected OEScene m_ChoiceScene;
        public Point m_PosChoiceSelected;
        protected ETileScene m_TileMouse;
        protected OEScene m_MouseScene;


        public enum eState { Normal, EnPose, Selected };
        public eState m_State = eState.Normal;

        protected WText m_TextTuto;
        protected Random random = new Random(21);

        WEndOfLevelBox m_EndOfLevelBox;
        WYouLooseBox m_YouLooseBox;

        Boolean m_LightWereOn = false;
        TimeSpan m_LightStartTime = new TimeSpan();

        double m_DelayBeforeSeePannelYouWin = 1000; //= 10000; ICIMOVIE

        public override void StartLevel(String Param1)
        {
            Point SizeInTile = new Point(6, 5);// = new Point(7, 5); ICIMOVIE
            base.StartLevel(80, 30, new Point(10, 10), SizeInTile);

            AddObj(m_TextTuto = new WText(new Vector2(600, 350), "ElecTutorial_Text", FontManager.eFontID.Font1));
            // ICIMOVIE AddObj(m_TextTuto = new WText(new Vector2(700, 350), "ElecTutorial_Text", FontManager.eFontID.Font1));

            #region creation m_TileChoice
            int TileSize = 40;
            int MarginChoice = 5;
            Point PosTopLeft = new Point(720, 20);

            AddObj(m_ChoiceScene = new OEScene(TileSize, MarginChoice, PosTopLeft, SizeInTile));

            Point Pos = new Point(PosTopLeft.X + MarginChoice, PosTopLeft.Y + MarginChoice);
            m_TileChoice = new ETileScene(m_ChoiceScene, null, TileSize, Pos, SizeInTile);
            AddObj(m_TileChoice);

            #endregion

            #region creation m_TileMouse
            int MarginMouse = 2;
            Point SizeInTileMouse = new Point(1, 1);

            AddObj(m_MouseScene = new OEScene(m_TileBoard.m_TileSize, MarginMouse, PosTopLeft, SizeInTileMouse));

            m_TileMouse = new ETileScene(m_MouseScene, null, m_TileBoard.m_TileSize, Pos, SizeInTileMouse);
            AddObj(m_TileMouse);
            m_TileMouse.SetActiveAndVisibleState(false);
            m_TileMouse.m_Scene.m_WImage.SetActiveAndVisibleState(false);

            #endregion
            
            ShowButton();

            AddObj(m_EndOfLevelBox = new WEndOfLevelBox(new Vector2(400, 100), "Lampe Allumée"));
            m_EndOfLevelBox.SetActiveAndVisibleState(false);

            AddObj(m_YouLooseBox = new WYouLooseBox(new Vector2(400, 100), "Perdu!"));
            m_YouLooseBox.SetActiveAndVisibleState(false);

            SetStartSituation(Param1);
        }

        void SetStartSituation(String Param1)
        {
            if (Param1 == "1")
            {
                m_TileBoard.m_TileScene.AddTile(ETile.eType.Pile, new Point(2, 2), Angle360.North);
                m_TileBoard.m_TileScene.AddTile(ETile.eType.Lampe, new Point(3, 2), Angle360.North);

                m_TileBoard.m_TileScene.AddTile(ETile.eType.Wire2, new Point(2, 1), Angle360.North);
                m_TileBoard.m_TileScene.AddTile(ETile.eType.Wire2, new Point(1, 1), Angle360.Est);
                m_TileBoard.m_TileScene.AddTile(ETile.eType.Wire2, new Point(4, 2), Angle360.South);
                m_TileBoard.m_TileScene.AddTile(ETile.eType.Wire2, new Point(3, 3), Angle360.West);

                m_TileChoice.AddTile(ETile.eType.Pile, new Point(2, 2), Angle360.North);
                m_TileChoice.AddTile(ETile.eType.Lampe, new Point(3, 2), Angle360.North);

                m_TileChoice.AddTile(ETile.eType.Wire2, new Point(2, 3), Angle360.North);
                m_TileChoice.AddTile(ETile.eType.Wire2, new Point(2, 1), Angle360.Est);
                m_TileChoice.AddTile(ETile.eType.Wire2, new Point(3, 1), Angle360.South);
                m_TileChoice.AddTile(ETile.eType.Wire2, new Point(3, 3), Angle360.West);
            }
            if (Param1 == "2")
            {
                m_TileBoard.m_TileScene.AddTile(ETile.eType.Pile, new Point(2, 2), Angle360.Est);
                m_TileBoard.m_TileScene.AddTile(ETile.eType.Lampe, new Point(3, 2), Angle360.North);

                m_TileBoard.m_TileScene.AddTile(ETile.eType.Wire2, new Point(2, 1), Angle360.North);
                m_TileBoard.m_TileScene.AddTile(ETile.eType.Wire2, new Point(1, 1), Angle360.Est);
                m_TileBoard.m_TileScene.AddTile(ETile.eType.Wire2, new Point(4, 2), Angle360.South);
                m_TileBoard.m_TileScene.AddTile(ETile.eType.Wire2, new Point(3, 3), Angle360.West);
                m_TileBoard.m_TileScene.AddTile(ETile.eType.Wire, new Point(1, 4), Angle360.North);
                m_TileBoard.m_TileScene.AddTile(ETile.eType.Wire, new Point(2, 4), Angle360.West);

                m_TileChoice.AddTile(ETile.eType.Pile, new Point(2, 1), Angle360.Est);
                m_TileChoice.AddTile(ETile.eType.Lampe, new Point(1, 2), Angle360.North);

                m_TileChoice.AddTile(ETile.eType.Wire2, new Point(3, 1), Angle360.South);
                m_TileChoice.AddTile(ETile.eType.Wire2, new Point(1, 1), Angle360.Est);
                m_TileChoice.AddTile(ETile.eType.Wire2, new Point(1, 3), Angle360.North);
                m_TileChoice.AddTile(ETile.eType.Wire2, new Point(3, 3), Angle360.West);

                m_TileChoice.AddTile(ETile.eType.Wire, new Point(3, 2), Angle360.North);
                m_TileChoice.AddTile(ETile.eType.Wire, new Point(2, 3), Angle360.West);
            }
            if (Param1 == "3")
            {
                m_TileBoard.m_TileScene.AddTile(ETile.eType.Pile, GetAFreePoint(), Angle360.Est);
                m_TileBoard.m_TileScene.AddTile(ETile.eType.Pile, GetAFreePoint(), Angle360.South);
                m_TileBoard.m_TileScene.AddTile(ETile.eType.Lampe, GetAFreePoint(), Angle360.North);
                m_TileBoard.m_TileScene.AddTile(ETile.eType.Lampe, GetAFreePoint(), Angle360.West);

                m_TileBoard.m_TileScene.AddTile(ETile.eType.Wire2, GetAFreePoint(), Angle360.North);
                m_TileBoard.m_TileScene.AddTile(ETile.eType.Wire2, GetAFreePoint(), Angle360.Est);
                m_TileBoard.m_TileScene.AddTile(ETile.eType.Wire2, GetAFreePoint(), Angle360.South);
                m_TileBoard.m_TileScene.AddTile(ETile.eType.Wire2, GetAFreePoint(), Angle360.West);

                m_TileChoice.AddTile(ETile.eType.Pile, new Point(2, 3), Angle360.Est);
                m_TileChoice.AddTile(ETile.eType.Pile, new Point(1, 2), Angle360.South);
                m_TileChoice.AddTile(ETile.eType.Lampe, new Point(3, 2), Angle360.North);
                m_TileChoice.AddTile(ETile.eType.Lampe, new Point(2, 1), Angle360.West);

                m_TileChoice.AddTile(ETile.eType.Wire2, new Point(3, 1), Angle360.South);
                m_TileChoice.AddTile(ETile.eType.Wire2, new Point(1, 1), Angle360.Est);
                m_TileChoice.AddTile(ETile.eType.Wire2, new Point(1, 3), Angle360.North);
                m_TileChoice.AddTile(ETile.eType.Wire2, new Point(3, 3), Angle360.West);
            }
            if (Param1 == "4")
            {
                m_TileBoard.m_TileScene.AddTile(ETile.eType.Pile, GetAFreePoint(), Angle360.North);
                m_TileBoard.m_TileScene.AddTile(ETile.eType.Lampe, GetAFreePoint(), Angle360.North);
                m_TileBoard.m_TileScene.AddTile(ETile.eType.Lampe, GetAFreePoint(), Angle360.North);

                m_TileBoard.m_TileScene.AddTile(ETile.eType.Wire2, GetAFreePoint(), Angle360.North);
                m_TileBoard.m_TileScene.AddTile(ETile.eType.Wire2, GetAFreePoint(), Angle360.Est);
                m_TileBoard.m_TileScene.AddTile(ETile.eType.Wire2, GetAFreePoint(), Angle360.South);
                m_TileBoard.m_TileScene.AddTile(ETile.eType.Wire2, GetAFreePoint(), Angle360.West);
                m_TileBoard.m_TileScene.AddTile(ETile.eType.Wire, GetAFreePoint(), Angle360.North);

                m_TileChoice.AddTile(ETile.eType.Pile, new Point(2, 2), Angle360.North);
                m_TileChoice.AddTile(ETile.eType.Lampe, new Point(3, 3), Angle360.North);
                m_TileChoice.AddTile(ETile.eType.Lampe, new Point(3, 2), Angle360.North);
                m_TileChoice.AddTile(ETile.eType.Wire, new Point(2, 3), Angle360.North);

                m_TileChoice.AddTile(ETile.eType.Wire2, new Point(3, 1), Angle360.South);
                m_TileChoice.AddTile(ETile.eType.Wire2, new Point(2, 1), Angle360.Est);
                m_TileChoice.AddTile(ETile.eType.Wire2, new Point(2, 4), Angle360.North);
                m_TileChoice.AddTile(ETile.eType.Wire2, new Point(3, 4), Angle360.West);
            }
            if (Param1 == "5")
            {
                m_TileBoard.m_TileScene.AddTile(ETile.eType.Pile, GetAFreePoint(), Angle360.North);
                m_TileBoard.m_TileScene.AddTile(ETile.eType.Lampe, GetAFreePoint(), Angle360.North);
                m_TileBoard.m_TileScene.AddTile(ETile.eType.Lampe, GetAFreePoint(), Angle360.North);

                m_TileBoard.m_TileScene.AddTile(ETile.eType.Wire2, GetAFreePoint(), Angle360.North);
                m_TileBoard.m_TileScene.AddTile(ETile.eType.Wire2, GetAFreePoint(), Angle360.Est);
                m_TileBoard.m_TileScene.AddTile(ETile.eType.Wire2, GetAFreePoint(), Angle360.South);
                m_TileBoard.m_TileScene.AddTile(ETile.eType.Wire2, GetAFreePoint(), Angle360.West);
                m_TileBoard.m_TileScene.AddTile(ETile.eType.Wire3, GetAFreePoint(), Angle360.North);
                m_TileBoard.m_TileScene.AddTile(ETile.eType.Wire3, GetAFreePoint(), Angle360.South);

                m_TileChoice.AddTile(ETile.eType.Pile, new Point(1, 2), Angle360.North);
                m_TileChoice.AddTile(ETile.eType.Lampe, new Point(2, 2), Angle360.North);
                m_TileChoice.AddTile(ETile.eType.Lampe, new Point(3, 2), Angle360.North);

                m_TileChoice.AddTile(ETile.eType.Wire2, new Point(3, 1), Angle360.South);
                m_TileChoice.AddTile(ETile.eType.Wire2, new Point(1, 1), Angle360.Est);
                m_TileChoice.AddTile(ETile.eType.Wire2, new Point(1, 3), Angle360.North);
                m_TileChoice.AddTile(ETile.eType.Wire2, new Point(3, 3), Angle360.West);

                m_TileChoice.AddTile(ETile.eType.Wire3, new Point(2, 1), Angle360.South);
                m_TileChoice.AddTile(ETile.eType.Wire3, new Point(2, 3), Angle360.North);
            }
            if (Param1 == "6")
            {
            }

        }

        Point GetAFreePoint()
        {
            Point dst = new Point();
            do 
            {
                dst.X = random.Next(m_TileBoard.m_TileScene.m_SizeInTile.X);
                dst.Y = random.Next(m_TileBoard.m_TileScene.m_SizeInTile.Y);
            } while (m_TileBoard.m_TileScene.FindTile(dst) != null);

            return dst;
        }

        String GetStringScore()
        {
            String dst;
            dst = StringManager.Get("Text_Score");
            dst += m_Score.ToString();
            return dst;
        }

        public override void Update()
        {
            base.Update();

            InputManager.MouseInput MS = InputManager.m_Current;
            Point PM = new Point(MS.X , MS.Y );
            Point NewPos = PM;
            if (m_TileBoard.m_BackgroundScene.Contains(PM.X, PM.Y))
            {
                Point PosCh = m_TileBoard.m_TileScene.GetTileFromPos(PM);
                Point NewPos2 = m_TileBoard.m_TileScene.GetPosFromTile(PosCh);
                if((Math.Abs(NewPos2.X-NewPos.X)<m_TileBoard.m_TileScene.m_TileWidth/4)
                    && (Math.Abs(NewPos2.Y-NewPos.Y)<m_TileBoard.m_TileScene.m_TileWidth/4))
                    NewPos =NewPos2;
            }
            NewPos.X -= m_TileBoard.m_TileSize / 2;
            NewPos.Y -= m_TileBoard.m_TileSize / 2;
            m_TileMouse.MoveAllScene(NewPos);



            //Si toutes les lampes sont allumées
            //  Si m_LightWereOn
            //      Si now > m_LightStarted + 1 sec -> Win
            //  Else m_LightWereOn = true, m_LightStarted = now
            //Else
            // m_LightWereOn = false

            if (m_TileBoard.m_Circuit.NbLightON() == m_TileBoard.m_TileScene.Count(ETile.eType.Lampe))//m_NbLightTarget)
            {
                if (m_LightWereOn)
                {
                    TimeSpan delay = Util.Mgr.m_GameTime.TotalRealTime - m_LightStartTime;
                    if(delay.TotalMilliseconds>m_DelayBeforeSeePannelYouWin)
                        m_EndOfLevelBox.SetActiveAndVisibleState(true);
                }
                else
                {
                    m_LightStartTime = Util.Mgr.m_GameTime.TotalRealTime;
                }
                m_LightWereOn = true;
            }
            else
                m_LightWereOn = false;

        }


        public override void DoMouse(InputManager.MouseInput newState, InputManager.MouseInput oldState, KeyboardState keyState)
        {
            base.DoMouse(newState, oldState, keyState);

            //Je clic dans le panneau de choix
            if (m_ChoiceScene.Contains(newState.X, newState.Y))
            {

                if (newState.LeftButton == ButtonState.Pressed && oldState.LeftButton == ButtonState.Released)
                {
                    Point PCh = new Point(newState.X, newState.Y);
                    Point PosCh = m_TileChoice.GetTileFromPos(PCh);
                    ETile et = m_TileChoice.FindTile(PosCh);
                    if (m_State == eState.Normal)
                    {
                        if (et != null)
                        {
                            m_PosChoiceSelected = PosCh;
                            m_TileMouse.RemoveAll();
                            m_TileMouse.AddTile(et.m_Type, new Point(0, 0), et.m_A);
                            m_TileMouse.SetActiveAndVisibleState(true);

                            ChangeState(eState.EnPose);
                            et.Destroy();
                        }
                    }
                    else //m_State == eState.EnPose
                    {
                        if(et==null)
                        {
                            ChangeState(eState.Normal);
                            ETile etm = m_TileMouse.FindTile(new Point(0, 0));
                            m_TileChoice.AddTile(etm.m_Type, PosCh, etm.m_A);
                            m_TileMouse.SetActiveAndVisibleState(false);
                        }
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
            // Je clic sur un Tile existant -> je passe en mode Select et je le selectionne
            // Je clic dans le vide en mode Select -> je passe en mode normal
            // Je clic dans le vide en mode Pose -> Je pose

            //En Pose, clic sur vide->pose la tuile , passe en mode Normal
            //En Pose, clic sur tuile -> pose la tuile, la prend en main, reste en mode Pose
            //En Normal, clic sur vide -> rien
            //EN Normal, clic sur tuile -> la prend en main, passe en Mode Pose

            //passe en normal
            //Clic sur quelquechose ->  prend la tuile en main, passe en pose
            //Etait En Pose -> pose la tuile

            if (newState.LeftButton == ButtonState.Pressed && oldState.LeftButton == ButtonState.Released)
            {
                ETile et = m_TileBoard.m_TileScene.FindTile(Pos);
                ETile etm = m_TileMouse.FindTile(new Point(0,0));
                
                //Je retiens l'état de la souris
                Boolean WasEnPose = ((m_State == eState.EnPose)  && etm!=null);

                ETile.eType MouseType = ETile.eType.Wire;
                Angle360 MouseAngle = Angle360.North;
                if (etm != null)
                {
                    MouseType = etm.m_Type;
                    MouseAngle = etm.m_A;
                }


                eState newPoseState = eState.Normal;

                // Je clic sur un Tile existant -> je vais le prendre en souris
                if (et != null)
                {
                    m_TileMouse.RemoveAll();
                    m_TileMouse.AddTile(et.m_Type, new Point(0, 0), et.m_A);
                    m_TileMouse.SetActiveAndVisibleState(true);

                    et.Destroy();

                    newPoseState = eState.EnPose;
                }

                if (WasEnPose)
                {
                    m_TileBoard.m_TileScene.AddTile(MouseType, Pos, MouseAngle);
                    //m_Circuit.SetDurty();
                }

                ChangeState(newPoseState);
                
            }

        }

        protected void ChangeState(eState _new)
        {
            m_State = _new;
            ShowButton();
        }
        protected void ShowButton()
        {
            //Boolean DoShow = false;
            //if (m_State == eState.Selected)
            //{
            //    DoShow = true;
            //    Point Pos;
            //    Pos.X = m_PosSelected.X * m_TileBoard.m_TileScene.m_TileWidth + m_TileBoard.m_TileScene.m_Pos.X;
            //    Pos.X += (m_TileBoard.m_TileScene.m_TileWidth - m_TurnImage.m_DstRect.Width) / 2;
            //    Pos.Y = m_PosSelected.Y * m_TileBoard.m_TileScene.m_TileWidth + m_TileBoard.m_TileScene.m_Pos.Y + 2;
            //    m_TurnImage.SetDstPos(Pos);
            //}

            m_TileMouse.SetActiveAndVisibleState(m_State==eState.EnPose);

            //m_ButtonDestroy.SetActiveAndVisibleState(DoShow);
            //m_ButtontTurnLeft.SetActiveAndVisibleState(DoShow);
            //m_ButtontTurnRight.SetActiveAndVisibleState(DoShow);
            //m_TurnImage.SetActiveAndVisibleState(DoShow);

            //m_ImageSelection.m_TRSRelativ.m_Pos = Misc.Vector2FromPoint(m_TileBoard.m_TileScene.GetPosFromTile(m_PosSelected));
            //m_ImageSelection.SetActiveAndVisibleState(DoShow);
        }

        //public void DoDestroy(InputManager.MouseInput newState, InputManager.MouseInput oldState, KeyboardState keyState)
        //{
        //    if (m_State == eState.Selected)
        //    {
        //        ETile et = m_TileBoard.m_TileScene.FindTile(m_PosSelected);
        //        if (et != null)
        //        {
        //            et.Destroy();
        //        }
        //        ChangeState(eState.Normal);
        //    }
        //}
        //public void DoTurnLeft(InputManager.MouseInput newState, InputManager.MouseInput oldState, KeyboardState keyState)
        //{
        //    if (m_State == eState.Selected)
        //    {
        //        m_TileBoard.m_TileScene.TurnLeftTile(m_PosSelected);
        //    }
        //}
        //public void DoTurnRight(InputManager.MouseInput newState, InputManager.MouseInput oldState, KeyboardState keyState)
        //{
        //    if (m_State == eState.Selected)
        //    {
        //        m_TileBoard.m_TileScene.TurnRightTile(m_PosSelected);
        //    }
        //}

    }
}
