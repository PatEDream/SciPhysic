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
    public class VictoryTest
    {
        public delegate Boolean FuncTest(ref VictoryTest _vt, ETileScene _scene);

        public String m_Title;
        public String m_Message;
        public Boolean m_HasBeenWon = false;
        public int m_TestScore = 0;
        public Boolean m_IsPositiv = true;
        public FuncTest m_FuncTest;

        public Boolean TestCourtCircuit(ref VictoryTest _vt, ETileScene _scene)
        {
            int Nb = _scene.CountShortCut();
            return Nb > 0;
        }

        public Boolean TestAll_LL(ref VictoryTest _vt, ETile.eType _Type, int _BonusPerDipole, ETileScene _scene)
        {
            int Nb = _scene.CountActiv(_Type);
            int Nb2 = _scene.Count(_Type);
            if (Nb < Nb2 || Nb<3)
                return false;
            _vt.m_TestScore = Nb * _BonusPerDipole;
            return true;
        }
        public Boolean TestAllPile(ref VictoryTest _vt, ETileScene _scene)
        {
            Boolean b = TestAll_LL(ref _vt, ETile.eType.Pile, 100, _scene);

            return b;
        }
        public Boolean TestAllResistance(ref VictoryTest _vt, ETileScene _scene)
        {
            return TestAll_LL(ref _vt, ETile.eType.Resistance, 100, _scene);
        }
        public Boolean TestAllLampe(ref VictoryTest _vt, ETileScene _scene)
        {
            return TestAll_LL(ref _vt, ETile.eType.Lampe, 100, _scene);
        }

        public Boolean TestNbMin(ref VictoryTest _vt, int _NbMin, ETile.eType _Type, int _BonusPerDipole, ETileScene _scene)
        {
            int Nb = _scene.CountActiv(_Type);
            if (Nb < _NbMin)
                return false;
            _vt.m_TestScore = Nb * _BonusPerDipole;
            return true;
        }
        public Boolean Test3Pile(ref VictoryTest _vt, ETileScene _scene)
        {
            return TestNbMin(ref _vt, 3, ETile.eType.Pile, 100, _scene);
        }
        public Boolean Test3Resistance(ref VictoryTest _vt, ETileScene _scene)
        {
            return TestNbMin(ref _vt, 3, ETile.eType.Resistance, 100, _scene);
        }
        public Boolean Test3Lampe(ref VictoryTest _vt, ETileScene _scene)
        {
            return TestNbMin(ref _vt, 3, ETile.eType.Lampe, 100, _scene);
        }

        public Boolean Test20Tile(ref VictoryTest _vt, ETileScene _scene)
        {
            int Nb = _scene.CountActiv();
            if (Nb < 20)
                return false;
            _vt.m_TestScore = Nb * 30;
            return true;
        }
        public Boolean TestAllDipole(ref VictoryTest _vt, ETileScene _scene)
        {
            int Nb = _scene.CountActivDipole();
            int Nb2 = _scene.CountDipole();
            if (Nb != Nb2)
                return false;
            _vt.m_TestScore = 1000;
            return true;
        }
    }



    class LevTilesTurnTurn : LevTilesBase
    {
        int m_Score = 0;
        int m_NbLife = 3;

        public enum eState { Normal, EnPose, Selected };
        public eState m_State = eState.Normal;
        public Point m_PosSelected;
        protected OGSprite m_ImageSelection;

        protected WButton m_ButtonGo;
        protected WMessageBox m_WMessageGo;
        protected WText m_TextScore;

        //gérer un petit lapse de temps avant de remplir
        TimeSpan m_WaitForFillGap;
        Boolean m_HasToFill = false;

        //// on gère ici l'augmentation de difficulté
        Boolean m_GameIsHard = false;  //On ne montre pas le courant avant le GO, on est en mode plan
        Boolean m_HasToStop = false;
        int m_WillShowResult = -1;

        protected Random random = new Random(21);
        //ICIMOVIE protected Random random = new Random(2);

        protected WImage m_TurnImage;

        public List<VictoryTest> m_LVictoryTest = new List<VictoryTest>();
        public List<VictoryTest> m_LNewVictory = new List<VictoryTest>();

        public override void StartLevel(String Param1)
        {
            Point SizeInTile = new Point(8, 5);
            if (Param1.Contains("4x3"))
                SizeInTile = new Point(4, 3);
            if (Param1.Contains("5x3"))
                SizeInTile = new Point(5, 3);
            if (Param1.Contains("6x4"))
                SizeInTile = new Point(6, 4);
            if (Param1.Contains("7x5"))
                SizeInTile = new Point(7, 5);
            if (Param1.Contains("8x5"))
                SizeInTile = new Point(8, 5);

            if (Param1.Contains("Hard"))
                m_GameIsHard = true;

            int TileSize = 320 / SizeInTile.Y;

            base.StartLevel(TileSize, 30, new Point(20, 20), SizeInTile); //ICIMOVIE
            int SizeTile = 80;
            int SizeEdge = 30;
            int SizeX = SizeTile * SizeInTile.X + SizeEdge * 2;
            int SizeY = SizeTile * SizeInTile.Y + SizeEdge * 2;

            //ICIMOVIE base.StartLevel(SizeTile, SizeEdge, new Point((640-SizeX)/2, (480 - SizeY)/2), SizeInTile);

            if (m_GameIsHard)
            {
                m_TileBoard.m_BackgroundScene.SetMode(OEScene.eMode.ModePlan);
                m_TileBoard.m_TileScene.SetMode(OEScene.eMode.ModePlan);
                m_TileBoard.m_IsActive = false; // ICIMOVIE
            }
            else
            {
                m_TileBoard.m_BackgroundScene.SetMode(OEScene.eMode.ModeReal);
                m_TileBoard.m_TileScene.SetMode(OEScene.eMode.ModeReal);
                m_TileBoard.m_IsActive = true;
            }

            m_TileBoard.DestroyEdge();
            m_TileBoard.CreateWiredEdgeH(true);
            m_TileBoard.CreateWiredEdgeH(false);
            m_TileBoard.CreateWiredEdgeV(true);
            m_TileBoard.CreateWiredEdgeV(false);

            FillGap();


            int w = m_TileBoard.m_TileScene.m_TileWidth;
            AddObj(m_ImageSelection = new OGSprite("ImagesQM\\Border.png", new MathPntSize(0, 0, w, w), OGSprite.ePosType.Centered, Color.White));

            AddObj(m_TurnImage = new WImage("ImagesElec\\ArrowTurnX.png", new Rectangle(400, 200, 75, 23), Color.White));
            m_TurnImage.m_Depth = 0.3f;

            AddObj(m_ButtonGo = new WButton(new Vector2(230, 420), "GO !!", FontManager.eFontID.Font0, DoGo)); 
            // ICIMOVIE AddObj(m_ButtonGo = new WButton(new Vector2(730, 420), "GO !!", FontManager.eFontID.Font0, DoGo));
            ShowButton();

            String StrScore = GetStringScore();
            AddObj(m_TextScore = new WText(new Vector2(600, 20), StrScore, FontManager.eFontID.Font1)); 
            //ICIMOVIE AddObj(m_TextScore = new WText(new Vector2(700, 20), StrScore, FontManager.eFontID.Font1));

            m_WMessageGo = new WMessageBox(new Vector2(580, 250), "", DoStartAgain); 
            //ICIMOVIE m_WMessageGo = new WMessageBox(new Vector2(580, 250), "", DoStartAgain);
            AddObj(m_WMessageGo);
            m_WMessageGo.SetActiveAndVisibleState(false);


            AddTests();
        }


        void AddATest(String _Title, String _Message, VictoryTest.FuncTest _Func, Boolean _IsPositiv)
        {
            VictoryTest vt = new VictoryTest();
            vt.m_Title = _Title;
            vt.m_Message = _Message;
            vt.m_FuncTest = _Func;
            vt.m_IsPositiv = _IsPositiv;
            m_LVictoryTest.Add(vt);
        }
        void AddTests()
        {
            VictoryTest vt = new VictoryTest();
            AddATest("ElecTurn_TestShortCutTitle", "ElecTurn_TestShortCutMessage", vt.TestCourtCircuit, false);

            AddATest("ElecTurn_Test3PileTitle", "ElecTurn_Test3PileMessage", vt.Test3Pile, true);
            AddATest("ElecTurn_Test3ResistanceTitle", "ElecTurn_Test3ResistanceMessage", vt.Test3Resistance, true);
            AddATest("ElecTurn_Test3LampeTitle", "ElecTurn_Test3LampeMessage", vt.Test3Lampe, true);

            AddATest("ElecTurn_TestAllPileTitle", "ElecTurn_TestAllPileMessage", vt.TestAllPile, true);
            AddATest("ElecTurn_TestAllResistanceTitle", "ElecTurn_TestAllResistanceMessage", vt.TestAllResistance, true);
            AddATest("ElecTurn_TestAllLampeTitle", "ElecTurn_TestAllLampeMessage", vt.TestAllLampe, true);

            AddATest("ElecTurn_Test20TileTitle", "ElecTurn_Test20TileMessage", vt.Test20Tile, true);
            AddATest("ElecTurn_TestAllDipoleTitle", "ElecTurn_TestAllDipoleMessage", vt.TestAllDipole, true);
        }

        String GetStringScore()
        {
            String dst;
            dst = StringManager.Get("Text_LifeLeft");
            dst += this.m_NbLife.ToString();
            dst += "\n" + StringManager.Get("Text_Score");
            dst += m_Score.ToString();
            return dst;
        }

        ETile.eType GetRandomTile()
        {
            ETile.eType Type = (ETile.eType)random.Next(1 + (int)ETile.eType.Lampe);
            while (Type == ETile.eType.WireNoCross)
                Type = (ETile.eType)random.Next(1 + (int)ETile.eType.Lampe);
            return Type;
        }

        void FillGap(List<Point> _LPoint)
        {
            for (int k = 0; k < _LPoint.Count; k++)
            {
                m_TileBoard.m_TileScene.RemoveTile(_LPoint[k]);
                ETile.eType Type = GetRandomTile();
                Angle360 Angle = new Angle360(random.Next(4) * 90);
                m_TileBoard.m_TileScene.AddTile(Type, _LPoint[k], Angle);
            }
        }

        void FillGap()
        {
            List<Point> LPoint = new List<Point>();
            for (int i = 0; i < m_TileBoard.m_TileScene.m_SizeInTile.X; i++)
            {
                for (int j = 0; j < m_TileBoard.m_TileScene.m_SizeInTile.Y; j++)
                {
                    if (m_TileBoard.m_TileScene.GetTile(i, j) == null)
                    {
                        LPoint.Add(new Point(i, j));
                    }
                }
            }

            Boolean DoAgain = true;
            while (DoAgain)
            {
                FillGap(LPoint);
                DoAgain = (m_TileBoard.m_TileScene.Count(ETile.eType.Pile)==0);
            }

            m_TileBoard.m_Circuit.ComputeCircuit();
        }

        public override void DrawText(SpriteBatch spriteBatch)
        {
            base.DrawText(spriteBatch);
            int count = 0;
            foreach (VictoryTest vt in m_LVictoryTest)
            {
                if (vt.m_IsPositiv)
                {
                    Color col = Color.Pink;
                    if (vt.m_HasBeenWon)
                        col = Color.Green;
                    String text = StringManager.Get(vt.m_Title);
                    GraphicManager.DrawString(FontManager.Get(FontManager.eFontID.Font2), text, new Vector2(800, 20 + count * 30), col);
                    count++;
                }
            }
        }
        public override void Update()
        {
            if (m_WillShowResult >= 0)
                m_WillShowResult--;
            if (m_WillShowResult == 0)
                ShowResult();

            if (m_HasToFill)
            {
                TimeSpan now = Mgr.m_GameTime.TotalRealTime;
                if ((now - m_WaitForFillGap).TotalSeconds > 0.5)
                {
                    m_HasToFill = false;
                    FillGap();
                }
            }

            if (m_HasToStop)
            {
                m_TileBoard.m_IsActive = false;
                m_HasToStop = false;
            }
            base.Update();
            ShowButton();
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
                            Point P = new Point(newState.X, newState.Y);
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
            if (m_State == eState.Selected)
            {
                DoShow = true;
                Point Pos;
                Pos.X = m_PosSelected.X * m_TileBoard.m_TileScene.m_TileWidth + m_TileBoard.m_TileScene.m_Pos.X;
                Pos.X += (m_TileBoard.m_TileScene.m_TileWidth - m_TurnImage.m_DstRect.Width) / 2;
                Pos.Y = m_PosSelected.Y * m_TileBoard.m_TileScene.m_TileWidth + m_TileBoard.m_TileScene.m_Pos.Y + 2;
                m_TurnImage.SetDstPos(Pos);
            }

            m_TurnImage.SetActiveAndVisibleState(DoShow);

            m_ImageSelection.m_TRSRelativ.m_Pos = Misc.Vector2FromPoint(m_TileBoard.m_TileScene.GetPosFromTile(m_PosSelected));
            m_ImageSelection.SetActiveAndVisibleState(DoShow);

            if (m_GameIsHard)
                m_ButtonGo.SetActiveAndVisibleState(true);
            else
            {
                if (m_TileBoard.m_TileScene.CountActiv() > 3)
                    m_ButtonGo.SetActiveAndVisibleState(true);
                else
                    m_ButtonGo.SetActiveAndVisibleState(false);
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
        //public void DoModePlan(InputManager.MouseInput newState, InputManager.MouseInput oldState, KeyboardState keyState)
        //{
        //    OEScene.eMode newMode;

        //    if (m_BackgroundScene.m_Mode == OEScene.eMode.ModeReal)
        //    {
        //        m_ButtonModePlan.m_Text = "ModeReal";
        //        newMode = OEScene.eMode.ModePlan;
        //    }
        //    else
        //    {
        //        m_ButtonModePlan.m_Text = "ModePlan";
        //        newMode = OEScene.eMode.ModeReal;
        //    }
        //    m_BackgroundScene.SetMode(newMode);
        //    m_TileScene.SetMode(newMode);
        //    DestroyEdge();
        //    CreateWiredEdgeH(true);
        //    CreateWiredEdgeH(false);
        //    CreateWiredEdgeV(true);
        //    CreateWiredEdgeV(false);
        //}
        //public void DoRemoveActiv(InputManager.MouseInput newState, InputManager.MouseInput oldState, KeyboardState keyState)
        //{
        //    m_Score += m_TileScene.RemoveAllActiv();
        //    m_TextScore.m_Text = GetStringScore();
        //}
        //public void DoFillGap(InputManager.MouseInput newState, InputManager.MouseInput oldState, KeyboardState keyState)
        //{
        //    FillGap();
        //}

        public void DoGo()
        {
            if (m_GameIsHard)
            {
                m_TileBoard.m_IsActive = true;
                m_WillShowResult = 100;
            }
            else
                ShowResult();
        }

        public void ShowResult()
        {
            m_NbLife--;

            //tester les victoires
            //calculer le score
            //Afficher les messages
            Boolean FindANegativ = false;
            foreach (VictoryTest vtFor in m_LVictoryTest)
            {
                VictoryTest vt = vtFor;
                if (vt.m_HasBeenWon == false)
                {
                    if (vt.m_FuncTest(ref vt, m_TileBoard.m_TileScene))
                    {
                        m_LNewVictory.Add(vt);
                        vt.m_HasBeenWon = true;
                        if (vt.m_IsPositiv == false)
                        {
                            FindANegativ = true;
                            break;
                        }
                    }
                }
            }

            if (FindANegativ)
            {
                DoStartAgainLL();
            }
            else
            {
                int Nb = m_TileBoard.m_TileScene.CountActiv();
                m_Score += Nb * Nb;
                String MessageScore = "Nombre de Tiles : " + Nb + "\nScore = " + Nb * Nb + "\nTotal = " + m_Score;
                m_WMessageGo.SetText(MessageScore);
                m_WMessageGo.SetActiveAndVisibleState(true);
            }
        }
        public void DoStartAgain(InputManager.MouseInput newState, InputManager.MouseInput oldState, KeyboardState keyState)
        {
            DoStartAgainLL();
        }
        public void DoStartAgainLL()//InputManager.MouseInput newState, InputManager.MouseInput oldState, KeyboardState keyState)
        {
            if (m_LNewVictory.Count > 0)
            {
                String MessageScore;
                VictoryTest vt = m_LNewVictory[0];
                m_Score += vt.m_TestScore;
                if (vt.m_IsPositiv)
                {
                    m_NbLife++;
                    MessageScore = StringManager.Get(vt.m_Message) + "\n\nVous Gagnez une vie\n\nPoints de Bonus = " + vt.m_TestScore;// +"\nScore Total = " + m_Score;
                }
                else
                {
                    MessageScore = StringManager.Get(vt.m_Message) + "\n\nPas de gains\n\n";
                }
                m_WMessageGo.SetText(MessageScore);
                m_WMessageGo.SetActiveAndVisibleState(true);
                m_LNewVictory.Remove(vt);
                return;
            }

            if (m_NbLife == 0)
            {
                String Text = "Vouz avez obtenu " + m_Score + " Points";
                int Count = 0;
                foreach (VictoryTest vt in m_LVictoryTest)
                {
                    if (vt.m_HasBeenWon)
                        Count++;
                }
                Text += "\n\nVous avez franchi " + Count + " Objectifs\n";

                WEndOfLevelBox weolb = new WEndOfLevelBox(m_WMessageGo.m_PntSize.m_Pos, Text);
                AddObj(weolb);
            }

            m_TileBoard.m_TileScene.RemoveAllActiv();
            m_TextScore.m_Text = GetStringScore();
            m_WaitForFillGap = Mgr.m_GameTime.TotalRealTime;
            m_HasToFill = true;

            if (m_GameIsHard)
            {
                m_TileBoard.EmptyActivity();
                m_HasToStop = true;
            }

        }

    }
}
