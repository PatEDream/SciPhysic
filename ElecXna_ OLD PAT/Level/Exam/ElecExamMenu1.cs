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
    public class ElecExamMenu1 : TestCurrent
    {

        //ElecExamCreator1 m_Creator;

        String m_Name;
        String m_Description;

        String m_TestFile;

        public override void StartLevel(String Param1)
        {
            m_Name = Param1 + "_Name";
            m_Description = Param1 + "_Description";
            m_TestFile = Param1;

            AddObj(new WText(new Vector2(300, 100), m_Name, FontManager.eFontID.Font1));
            AddObj(new WText(new Vector2(300, 150), m_Description, FontManager.eFontID.Font2));

            AddObj(new WButton(new Vector2(300, 400), "Button_Start", FontManager.eFontID.Font2, StartTest));
        }

        void StartTest()
        {
            String File = "..\\..\\..\\Content\\Level\\Elec\\" + m_TestFile + ".txt";
            LoadTxtFile(File);

            StartChoosedLevel();
        }


    }
}
