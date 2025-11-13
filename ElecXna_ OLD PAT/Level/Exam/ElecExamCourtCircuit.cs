using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Audio;
//using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Input;
//using Microsoft.Xna.Framework.Storage;
using QM.Phys; using QM.Maths;
using QM.Object;
using QM.Level.Menu;
using QM.Object.Widget;
using QM.Util;
using QM.LevelUtil;

namespace QM.Level
{
    public class ElecExamCourtCircuit : TestQuestion
    {

        String m_Name;
        String m_Description;

        protected ETileBoard m_TileBoard1 = new ETileBoard();

        public override void StartLevel(String Param1)
        {
            base.StartLevel(Param1);

            m_Mark = m_MarkMaxPossible = 4;

            m_Name = "ElecExam_CourtCircuit";
            m_Description = StringManager.Get("Test_QuestionSur");
            m_Description += " " + m_MarkMaxPossible + " ";
            m_Description += StringManager.Get("Tests_Points");


            int sx = Mgr.m_GlobalManager.m_BackBufferSizeX;
            int sy = Mgr.m_GlobalManager.m_BackBufferSizeY;

            int TileSize = 40;
            int NbTile = 5;
            int Margin = 5;
            int SizeBoard = NbTile * TileSize + 2*Margin;
            m_TileBoard1.CreateBoard(TileSize, Margin, new Point(sx / 2 - 100 - SizeBoard, 150), new Point(NbTile, NbTile));
            AddObj(m_TileBoard1);
            CreateCircuitLampeCourtCircuit(m_TileBoard1);
            m_TileBoard1.m_IsActive = false;


            WText TheQuestion = new WText(new Vector2(0, 50), m_Name, FontManager.eFontID.Font1);
            TheQuestion.m_PntSize.m_Pos.X = sx / 2 - TheQuestion.GetSize().X / 2;
            AddObj(TheQuestion);

            WText TheScore = new WText(new Vector2(0, 80), m_Description, FontManager.eFontID.Font2);
            TheScore.m_PntSize.m_Pos.X = sx / 2 - TheScore.GetSize().X / 2;
            AddObj(TheScore);


            WButton BSee = new WButton(new Vector2(sx / 2 - 100 - SizeBoard / 2, 150 + SizeBoard + 30), "Button_HelpSeeMinus2", FontManager.eFontID.Font2, HelpSee);
            AddObj(BSee);
            BSee.m_PntSize.m_Pos.X = sx / 2 - 100 - SizeBoard / 2 - BSee.m_PntSize.m_Size.X / 2;

            Vector2[] TheTextPos = new Vector2[2];
            TheTextPos[0] = new Vector2(sx / 2 + 100, 150 + 20); //SizeBoard / 2 - 60);
            TheTextPos[1] = new Vector2(sx / 2 + 100, 150 + 120);//SizeBoard/2 + 60);
            int Which = Misc.random.Next(2);

            WButton BDerivation =  new WButton(TheTextPos[Which], "ElecExam_LaPileVaBruler", FontManager.eFontID.Font2, AnswerYes);
            AddObj(BDerivation);
            //BDerivation.m_PntSize.m_Pos.X = sx / 2 - 100 - SizeBoard/2 - BDerivation.m_PntSize.m_Size.X/2;

            WButton BSerie = new WButton(TheTextPos[1-Which], "ElecExam_PasDeCourant", FontManager.eFontID.Font2, AnswerNo);
            AddObj(BSerie);
            //BSerie.m_PntSize.m_Pos.X = sx / 2 + 100 + SizeBoard/2 - BSerie.m_PntSize.m_Size.X/2;

        }


        void CreateCircuitLampeCourtCircuit(ETileBoard _TileBoard)
        {
            _TileBoard.m_TileScene.RemoveAll();


            _TileBoard.m_TileScene.AddTile(ETile.eType.Wire2, new Point(1, 0), Angle360.Est);
            _TileBoard.m_TileScene.AddTile(ETile.eType.Wire, new Point(2, 0), Angle360.Est);
            _TileBoard.m_TileScene.AddTile(ETile.eType.Wire2, new Point(3, 0), Angle360.South);

            _TileBoard.m_TileScene.AddTile(ETile.eType.Wire, new Point(1, 1), Angle360.North);
            _TileBoard.m_TileScene.AddTile(ETile.eType.Wire, new Point(3, 1), Angle360.South);

            _TileBoard.m_TileScene.AddTile(ETile.eType.Pile, new Point(1, 2), Angle360.South);
            _TileBoard.m_TileScene.AddTile(ETile.eType.Wire, new Point(3, 2), Angle360.South);

            _TileBoard.m_TileScene.AddTile(ETile.eType.Wire, new Point(1, 3), Angle360.North);
            _TileBoard.m_TileScene.AddTile(ETile.eType.Wire, new Point(3, 3), Angle360.South);

            _TileBoard.m_TileScene.AddTile(ETile.eType.Wire2, new Point(1, 4), Angle360.North);
            _TileBoard.m_TileScene.AddTile(ETile.eType.Wire, new Point(2, 4), Angle360.Est);
            _TileBoard.m_TileScene.AddTile(ETile.eType.Wire2, new Point(3, 4), Angle360.West);

            _TileBoard.m_Circuit.ComputeCircuit();
            _TileBoard.m_TileScene.ShowBorderOfTiles(false);
        }


        public void HelpSee()
        {
            m_TileBoard1.m_IsActive = true;
            m_Mark = 2;
        }

        public void AnswerYes()
        {
            m_TileBoard1.m_IsActive = true;
            m_EndOfLevelBox.m_PntSize.m_Pos = new Vector2(Mgr.m_GlobalManager.m_BackBufferSizeX / 2 - 150, 400);
            m_EndOfLevelBox.m_WText.m_Text = StringManager.Get("Answer_Good");
            m_EndOfLevelBox.m_WText.m_Text += "\n" + m_Mark + " " + StringManager.Get("Tests_Points");
            m_EndOfLevelBox.m_WText.m_Text += " " + StringManager.Get("Tests_Sur") + " " + m_MarkMaxPossible;
            m_EndOfLevelBox.SetActiveAndVisibleState(true);
        }

        public void AnswerNo()
        {
            m_TileBoard1.m_IsActive = true;
            m_YouLooseBox.m_PntSize.m_Pos = new Vector2(Mgr.m_GlobalManager.m_BackBufferSizeX / 2 - 150, 430);
            m_YouLooseBox.m_WText.m_Text = StringManager.Get("Answer_Wrong");
            m_YouLooseBox.m_WText.m_Text += "\n" + "0 " + StringManager.Get("Tests_Points");
            m_YouLooseBox.m_WText.m_Text += " " + StringManager.Get("Tests_Sur") + " " + m_MarkMaxPossible;
            YouLoose();
        }
    }
}
