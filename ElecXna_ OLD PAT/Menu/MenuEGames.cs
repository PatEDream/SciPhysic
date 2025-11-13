using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using QM.Phys; 
using QM.Maths;
using QM.Object;
using QM.Object.Widget;
using QM.Util;

namespace QM.Level.Menu
{
    class MenuEGames : MenuBase
    {

        public override LevelBase CreateLevel(String Param1)
        {
            MenuEGames dst = new MenuEGames();
            dst.StartLevel( Param1);
            return dst;
        }

        public override void StartLevel(String Param1)
        {
            //Mgr.m_GlobalManager.m_BackColor = Color.LightGray;
            Init();

            Mgr.m_GlobalManager.m_BackColor = Color.MediumSlateBlue;
            Mgr.m_GlobalManager.m_ColorWButtonNormal = Color.DarkBlue; //Color.BlueViolet;
            Mgr.m_GlobalManager.m_ColorWButtonGreyed = Color.Gray;
            Mgr.m_GlobalManager.m_ColorWButtonRolledOver = Color.White;
            Mgr.m_GlobalManager.m_ColorWButtonPushed = Color.Red;
            Mgr.m_GlobalManager.m_ColorWText = Color.White;

            //m_LLevelInfo.Add(new LevelInfo("MenuETest", ""));
            m_LLevelInfo.Add(new LevelInfo("MenuBase", "MenuEPipeDream"));
            m_LLevelInfo.Add(new LevelInfo("MenuBase", "MenuEBejewelled"));
            m_LLevelInfo.Add(new LevelInfo("MenuBase", "MenuETaquin"));
            m_LLevelInfo.Add(new LevelInfo("ElecExamMenu1", "ElecTest1"));
            m_LLevelInfo.Add(new LevelInfo("MenuBase", "MenuEBejewelledHard"));
            m_LLevelInfo.Add(new LevelInfo("ElecExamMenu1", "ElecTest2"));
            m_LLevelInfo.Add(new LevelInfo("LevTilesAdd", ""));

            m_LLevelInfo.Add(new LevelInfo("Retour", ""));

            AddObj(new WText(new Vector2(150, 340), "CoursJeux Electriques", FontManager.eFontID.Font1));
        }



        override public void Init()
        {

        }


    }
}
