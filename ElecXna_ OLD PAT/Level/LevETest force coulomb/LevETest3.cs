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
using QM.Level.Menu;
using QM.Object.Widget;


namespace QM.Level
{
    class LevETest3 : LevETestBase
    {
        //OGBase m_GamePlayBase;
        //OGFire objFire; 
        //OGGoal objGoal;
        public List<OECharge> m_LCharge = new List<OECharge>();
        public List<OECharge> m_LChargeIndy;
        OERect m_Rect1 = new OERect();
        OERect m_Rect2 = new OERect();
        OERect m_Rect3 = new OERect();


        WEndOfLevelBox m_EndOfLevelBox;

        public override void StartLevel(String Param1)
        {
            //LoadTxtFile("..\\..\\..\\Content\\Level\\LevelAttFirst.txt");
            int sx = QMGame.GetBackBufferWidth();
            int sy = QMGame.GetBackBufferHeight();

            m_LChargeIndy = new List<OECharge>();
            m_Rect1.Create(new MathPntSize(100,50,150,100), OERect.eType.Metal, false, 1000.0f);
            for (int i = 0; i < 20; i++)
                m_Rect1.AddCharge(OECharge.eType.Positiv);
            m_Rect2.Create(new MathPntSize(270, 50, 150, 100), OERect.eType.Metal, false, 1000.0f);
            for (int i = 0; i < 10; i++)
            {
                m_Rect2.AddCharge(OECharge.eType.Positiv);
                m_Rect2.AddCharge(OECharge.eType.Negativ);
            }
            m_Rect3.Create(new MathPntSize(440, 50, 150, 100), OERect.eType.Metal, false, 1000.0f);
            for (int i = 0; i < 20; i++)
                m_Rect3.AddCharge(OECharge.eType.Negativ);

            AddObj(m_Rect1);
            AddObj(m_Rect2);
            AddObj(m_Rect3);

            m_LCharge.AddRange(m_Rect1.m_LCharge);
            m_LCharge.AddRange(m_Rect2.m_LCharge);
            m_LCharge.AddRange(m_Rect3.m_LCharge);

            //Random random = new Random(10);
            //OECharge oc;
            //for (int i = 0; i < 40; i++)
            //{

            //    AddObj(oc = new OECharge(new MathPntSize(random.Next(300, 500), random.Next(100, 300), 8, 8), OECharge.eType.Negativ));
            //    oc.m_Speed = new Vector2((float)(random.NextDouble() * 0.02 - 0.01), (float)(random.NextDouble() * 0.02 - 0.01));
            //    oc.m_DrawText = false;
            //    m_LCharge.Add(oc);
            //    AddObj(oc = new OECharge(new MathPntSize(random.Next(300, 500), random.Next(100, 300), 8, 8), OECharge.eType.Positiv));
            //    oc.m_Speed = new Vector2((float)(random.NextDouble() * 0.02 - 0.01), (float)(random.NextDouble() * 0.02 - 0.01));
            //    oc.m_DrawText = false;
            //    m_LCharge.Add(oc);
            //}

            AddObj(m_EndOfLevelBox = new WEndOfLevelBox(new Vector2(sx * 2 / 8, sy * 3 / 8), ""));
            m_EndOfLevelBox.SetActiveAndVisibleState(true);


            //SaveTxtFile("..\\..\\..\\Content\\Level\\LevETest1_Test.txt");
        }




        override public void Update()
        {
            m_Rect1.ComputeAcceleration(m_LCharge);
            m_Rect2.ComputeAcceleration(m_LCharge);
            m_Rect3.ComputeAcceleration(m_LCharge);
            m_Rect1.ComputeMouvment();
            m_Rect2.ComputeMouvment();
            m_Rect3.ComputeMouvment();
            //foreach (OECharge oc in m_LChargeIndy)
            //{
            //    oc.ComputeAcceleration(m_LChargeIndy);
            //}
            //foreach (OECharge oc in m_LChargeIndy)
            //{
            //    oc.ComputeMouvment();
            //    //oc.m_Speed += oc.m_Acceleration;
            //    //oc.m_TRSRelativ.m_Pos += oc.m_Speed;
            //}
        }

    }
}
