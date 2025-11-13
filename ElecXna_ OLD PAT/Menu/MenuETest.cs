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
    class MenuETest : MenuBase
    {

        public override LevelBase CreateLevel(String Param1)
        {
            MenuETest dst = new MenuETest();
            dst.StartLevel(Param1);
            return dst;
        }

        public override void StartLevel(String Param1)
        {
            //Mgr.m_GlobalManager.m_BackColor = Color.LightGray;
            Init();

            m_LLevelInfo.Add(new LevelInfo("LevETest1", ""));
            m_LLevelInfo.Add(new LevelInfo("LevETest2", ""));
            m_LLevelInfo.Add(new LevelInfo("LevETest3", ""));
            m_LLevelInfo.Add(new LevelInfo("LevETest4", ""));
            m_LLevelInfo.Add(new LevelInfo("LevETest5", ""));
            m_LLevelInfo.Add(new LevelInfo("LevETest6", ""));
            m_LLevelInfo.Add(new LevelInfo("LevETest7", ""));
            m_LLevelInfo.Add(new LevelInfo("Retour", ""));

            AddObj(new WText(new Vector2(150, 340), "Tests Electricité", FontManager.eFontID.Font1));
        }



        override public void Init()
        {

        }


    }
}
