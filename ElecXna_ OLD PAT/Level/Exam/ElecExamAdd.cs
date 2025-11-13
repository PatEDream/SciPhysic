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
    class ElecExamAdd : LevTilesBase
    {
        int m_Score = 0;
        String m_Name;
        String m_Description;
        int m_Mark;
        int m_MarkMaxPossible = 6;

        protected ETileScene m_TileChoice;
        protected OEScene m_ChoiceScene;
        public Point m_PosChoiceSelected;
        protected ETileScene m_TileMouse;
        protected OEScene m_MouseScene;

        protected ETileBoard m_TileBoardModel = new ETileBoard();

        public enum eState { Normal, EnPose, Selected };
        public eState m_State = eState.Normal;
        public Point m_PosSelected;
        protected OGSprite m_ImageSelection;

        //protected WButton m_ButtonDestroy;
        protected WButton m_ButtonModePlan;

        protected WImage m_TurnImage;
        protected WImage m_DestroyImage;

        protected WEndOfLevelBox m_EndOfLevelBox;
        string m_ErrorStr = "";

        public override void StartLevel(String Param1)
        {
            m_Mark = m_MarkMaxPossible = 6;

            m_Name = "ElecExam_AddPile2LampeSerie";
            m_Description = StringManager.Get("Test_QuestionSur");
            m_Description += " " + m_MarkMaxPossible + " ";
            m_Description += StringManager.Get("Tests_Points");

            int sx = Mgr.m_GlobalManager.m_BackBufferSizeX;

            WText TheQuestion = new WText(new Vector2(50, 50), m_Name, FontManager.eFontID.Font1);
            //TheQuestion.m_PntSize.m_Pos.X = sx / 2 - TheQuestion.GetSize().X / 2;
            AddObj(TheQuestion);

            WText TheScore = new WText(new Vector2(600, 80), m_Description, FontManager.eFontID.Font2);
            //TheScore.m_PntSize.m_Pos.X = sx / 2 - TheScore.GetSize().X / 2;
            AddObj(TheScore);



            base.StartLevel(60, 30, new Point(50, 150), new Point(5, 4));

            m_TileBoard.m_BackgroundScene.SetMode(OEScene.eMode.ModePlan);
            m_TileBoard.m_TileScene.SetMode(OEScene.eMode.ModePlan);

            //m_TileBoard.CreateWiredEdgeH(true);
            //m_TileBoard.CreateWiredEdgeH(false);
            //m_TileBoard.CreateWiredEdgeV(true);
            //m_TileBoard.CreateWiredEdgeV(false);


            int w = m_TileBoard.m_TileScene.m_TileWidth;
            AddObj(m_ImageSelection = new OGSprite("ImagesQM\\Border.png", new MathPntSize(0, 0, w, w), OGSprite.ePosType.Centered, Color.White));

            AddObj(m_TurnImage = new WImage("ImagesElec\\ArrowTurnX.png", new Rectangle(400, 200, 75, 23), Color.White));
            m_TurnImage.m_Depth = 0.01f;
            AddObj(m_DestroyImage = new WImage("ImagesElec\\Destroy.png", new Rectangle(400, 200, 20, 20), Color.White));
            m_DestroyImage.m_Depth = 0.01f;

            int X = 700;
            //AddObj(m_ButtonDestroy = new WButton(new Vector2(X, 150), "Destroy", FontManager.eFontID.Font1, DoDestroy));
            //AddObj(new WButton(new Vector2(X, 180), "RemoveAll", FontManager.eFontID.Font1, DoRemoveAll));
            AddObj(m_ButtonModePlan = new WButton(new Vector2(X, 150), "Button_HelpSeeModeNormalMinus2", FontManager.eFontID.Font1, DoModePlan));
            AddObj(new WButton(new Vector2(X, 200), "Button_HelpSeeModelMinus4", FontManager.eFontID.Font1, DoShowModel));

            //String StrScore = GetStringScore();
            //AddObj(m_TextScore = new WText(new Vector2(X, 400), StrScore, FontManager.eFontID.Font0));

            #region m_TileChoice
            int TileSize = 40;
            int MarginChoice = 10;
            Point PosTopLeft = new Point(450, 150);
            Point SizeInTile = new Point(2, 4);

            AddObj(m_ChoiceScene = new OEScene(TileSize, MarginChoice, PosTopLeft, SizeInTile));



            Point Pos = new Point(PosTopLeft.X + MarginChoice, PosTopLeft.Y + MarginChoice);
            m_TileChoice = new ETileScene(m_ChoiceScene, null, TileSize, Pos, SizeInTile);
            m_ChoiceScene.SetMode(OEScene.eMode.ModePlan);
            m_TileChoice.SetMode(OEScene.eMode.ModePlan);
            AddObj(m_TileChoice);

            m_TileChoice.AddTile(ETile.eType.Pile, new Point(0, 0), Angle360.North);
            m_TileChoice.AddTile(ETile.eType.Resistance, new Point(0, 1), Angle360.North);
            m_TileChoice.AddTile(ETile.eType.Wire, new Point(0, 2), Angle360.North);
            m_TileChoice.AddTile(ETile.eType.Wire2, new Point(1, 2), Angle360.North);
            m_TileChoice.AddTile(ETile.eType.Wire3, new Point(0, 3), Angle360.North);
            m_TileChoice.AddTile(ETile.eType.Wire4, new Point(1, 3), Angle360.North);
            m_TileChoice.AddTile(ETile.eType.Lampe, new Point(1, 0), Angle360.North);
            m_TileChoice.AddTile(ETile.eType.Diode, new Point(1, 1), Angle360.North);

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

            TileSize = 40;
            int Margin = 5;
            m_TileBoardModel.CreateBoard(TileSize, Margin, new Point(450, 340), new Point(3, 4));
            m_TileBoardModel.m_BackgroundScene.SetMode(OEScene.eMode.ModePlan);
            m_TileBoardModel.m_TileScene.SetMode(OEScene.eMode.ModePlan);
            AddObj(m_TileBoardModel);
            CreateCircuitSerie(m_TileBoardModel);
            m_TileBoardModel.m_IsActive = false;
            m_TileBoardModel.m_IsVisible = false;

            ShowButton();

            AddObj(m_EndOfLevelBox = new WEndOfLevelBox(new Vector2(X , 350), "Bonne Réponse"));
            m_EndOfLevelBox.SetActiveAndVisibleState(false);

            SetMode(OEScene.eMode.ModePlan);
        }

        void CreateCircuitSerie(ETileBoard _TileBoard)
        {
            _TileBoard.m_TileScene.RemoveAll();

            _TileBoard.m_TileScene.AddTile(ETile.eType.Wire2, new Point(0, 0), Angle360.Est);
            _TileBoard.m_TileScene.AddTile(ETile.eType.Wire, new Point(1, 0), Angle360.Est);
            _TileBoard.m_TileScene.AddTile(ETile.eType.Wire2, new Point(2, 0), Angle360.South);

            _TileBoard.m_TileScene.AddTile(ETile.eType.Wire, new Point(0, 1), Angle360.South);
            _TileBoard.m_TileScene.AddTile(ETile.eType.Lampe, new Point(2, 1), Angle360.South);

            _TileBoard.m_TileScene.AddTile(ETile.eType.Pile, new Point(0, 2), Angle360.South);
            _TileBoard.m_TileScene.AddTile(ETile.eType.Lampe, new Point(2, 2), Angle360.South);

            _TileBoard.m_TileScene.AddTile(ETile.eType.Wire2, new Point(0, 3), Angle360.North);
            _TileBoard.m_TileScene.AddTile(ETile.eType.Wire, new Point(1, 3), Angle360.Est);
            _TileBoard.m_TileScene.AddTile(ETile.eType.Wire2, new Point(2, 3), Angle360.West);

            _TileBoard.m_Circuit.ComputeCircuit();
            _TileBoard.m_TileScene.ShowBorderOfTiles(false);
        }

        String GetStringScore()
        {
            String dst;
            dst = StringManager.Get("Text_Score");
            dst += m_Score.ToString();
            return dst;
        }


        public Boolean TestResult(ref string ErrorStr)
        {
            //Il y a une seule pile IsOn
            //Il y a deux lampes IsOn
            //Il n'y a pas de led
            //Il n'y a pas de résistance
            //Le potentiel au bornes de la pile est le double de celui aux bornes des lampes
            ErrorStr = "";

            if (m_TileBoard.m_TileScene.Count(ETile.eType.Pile) != 1)
            {
                ErrorStr = "Elec_Error_NbGenerateurDiff1";
                return false;
            }
            if (m_TileBoard.m_TileScene.Count(ETile.eType.Lampe) != 2)
            {
                ErrorStr = "Elec_Error_NbLampeDiff2";
                return false;
            }
            if (m_TileBoard.m_TileScene.Count(ETile.eType.Diode) != 0)
            {
                ErrorStr = "Elec_Error_NbLedDiff0";
                return false;
            }
            if (m_TileBoard.m_TileScene.Count(ETile.eType.Resistance) != 0)
            {
                ErrorStr = "Elec_Error_NbResistanceDiff0";
                return false;
            }

            if (m_TileBoard.m_TileScene.CountActiv(ETile.eType.Pile) != 1)
            {
                ErrorStr = "Elec_Error_NbActivGenerateurDiff1";
                return false;
            }
            if (m_TileBoard.m_TileScene.CountActiv(ETile.eType.Lampe) != 2)
            {
                ErrorStr = "Elec_Error_NbActivLampeDiff2";
                return false;
            }

            List<ETile> lt;
            lt = m_TileBoard.m_TileScene.GetAllTile(ETile.eType.Pile);
            float IPile = lt[0].GetMaxIntensity();

            lt = m_TileBoard.m_TileScene.GetAllTile(ETile.eType.Lampe);
            float ILampe0 = lt[0].GetMaxIntensity();
            float ILampe1 = lt[1].GetMaxIntensity();

            if(IPile == 0.0f)
            {
                ErrorStr = "Elec_Error_NbActivGenerateurDiff1";
                return false;
            }
            float rapportILampePile = Math.Abs( ILampe0/IPile);
            if( rapportILampePile<0.55f && rapportILampePile>0.45f)
            {
                ErrorStr = "Elec_Error_LampeEnDerivation";
                return false;
            }

            return true;
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

            if(TestResult(ref m_ErrorStr))
            {
                m_EndOfLevelBox.SetActiveAndVisibleState(true);
            }
        }
        public override void DrawText(SpriteBatch spriteBatch)
        {
            GraphicManager.DrawString(FontManager.Font[3], StringManager.Get(m_ErrorStr), new Vector2(700,400) , Color.White, 0, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0.1f);
            base.DrawText(spriteBatch);
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
                    if (Decal.Y > m_TileBoard.m_TileScene.m_TileWidth - 25 && Decal.X < 25)
                    {
                        DoDestroy(newState, oldState, keyState);
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
                Pos.X += 5;
                Pos.Y += m_TileBoard.m_TileScene.m_TileWidth / 2 + 10;
                m_DestroyImage.SetDstPos(Pos);
            }

            m_TileMouse.SetActiveAndVisibleState(m_State==eState.EnPose);

            //m_ButtonDestroy.SetActiveAndVisibleState(DoShow);
            //m_ButtontTurnLeft.SetActiveAndVisibleState(DoShow);
            //m_ButtontTurnRight.SetActiveAndVisibleState(DoShow);
            m_TurnImage.SetActiveAndVisibleState(DoShow);
            m_DestroyImage.SetActiveAndVisibleState(DoShow);

            m_ImageSelection.m_TRSRelativ.m_Pos = Misc.Vector2FromPoint(m_TileBoard.m_TileScene.GetPosFromTile(m_PosSelected));
            m_ImageSelection.SetActiveAndVisibleState(DoShow);
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
        public void SetMode(OEScene.eMode newMode)
        {
            m_ChoiceScene.SetMode(newMode);
            m_TileChoice.SetMode(newMode);
            m_MouseScene.SetMode(newMode);
            m_TileMouse.SetMode(newMode);
            m_TileBoardModel.m_BackgroundScene.SetMode(newMode);
            m_TileBoardModel.m_TileScene.SetMode(newMode);

            m_TileBoard.m_BackgroundScene.SetMode(newMode);
            m_TileBoard.m_TileScene.SetMode(newMode);
            m_TileBoard.DestroyEdge();
            m_TileBoard.CreateWiredEdgeH(true);
            m_TileBoard.CreateWiredEdgeH(false);
            m_TileBoard.CreateWiredEdgeV(true);
            m_TileBoard.CreateWiredEdgeV(false);        
        }

        public void DoModePlan(InputManager.MouseInput newState, InputManager.MouseInput oldState, KeyboardState keyState)
        {
            OEScene.eMode newMode;

            if (m_TileBoard.m_BackgroundScene.m_Mode == OEScene.eMode.ModeReal)
            {
                m_ButtonModePlan.m_Text = "ModeReal";
                newMode = OEScene.eMode.ModePlan;
                m_Mark -= 2;
            }
            else
            {
                m_ButtonModePlan.m_Text = "ModePlan";
                newMode = OEScene.eMode.ModeReal;
            }
            SetMode(newMode);
        }
        public void DoRemoveAll(InputManager.MouseInput newState, InputManager.MouseInput oldState, KeyboardState keyState)
        {
            m_TileBoard.m_TileScene.RemoveAll();
        }
        public void DoShowModel()
        {
            if (m_TileBoardModel.m_IsVisible == false)
            {
                m_Mark -= 4;
                m_TileBoardModel.m_IsVisible = true;
            }

        }

    }
}
