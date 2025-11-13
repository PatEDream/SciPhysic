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
    class LevETest2 : LevETestBase
    {
        //OGBase m_GamePlayBase;
        //OGFire objFire; 
        //OGGoal objGoal;
        public List<OECharge> m_LCharge;
        WEndOfLevelBox m_EndOfLevelBox;
        WYouLooseBox m_YouLooseBox;

        public override void StartLevel(String Param1)
        {
            //LoadTxtFile("..\\..\\..\\Content\\Level\\LevelAttFirst.txt");
            int sx = QMGame.GetBackBufferWidth();
            int sy = QMGame.GetBackBufferHeight();

            m_LCharge = new List<OECharge>();
            OECharge oc;
            Random random = new Random(10);

            for (int i = 0; i < 40; i++)
            {

                AddObj(oc = new OECharge(new MathPntSize(random.Next(300, 500), random.Next(100, 300), 8, 8), OECharge.eType.Negativ));
                oc.m_Speed = new Vector2((float)(random.NextDouble() * 0.02 - 0.01), (float)(random.NextDouble() * 0.02 - 0.01));
                oc.m_DrawText = false;
                m_LCharge.Add(oc);
                AddObj(oc = new OECharge(new MathPntSize(random.Next(300, 500), random.Next(100, 300), 8, 8), OECharge.eType.Positiv));
                oc.m_Speed = new Vector2((float)(random.NextDouble() * 0.02 - 0.01), (float)(random.NextDouble() * 0.02 - 0.01));
                oc.m_DrawText = false;
                m_LCharge.Add(oc);
            }


            AddObj(m_EndOfLevelBox = new WEndOfLevelBox(new Vector2(sx * 2 / 8, sy * 3 / 8), ""));
            m_EndOfLevelBox.SetActiveAndVisibleState(true);

            AddObj(m_YouLooseBox = new WYouLooseBox(new Vector2(sx * 2 / 8, sy * 3 / 8), "Perdu!"));
            m_YouLooseBox.SetActiveAndVisibleState(false);

            //SaveTxtFile("..\\..\\..\\Content\\Level\\LevETest1_Test.txt");
        }




        override public void Update()
        {
            foreach (OECharge oc in m_LCharge)
            {
                oc.ComputeAcceleration(m_LCharge);
            }
            foreach (OECharge oc in m_LCharge)
            {
                oc.ComputeMouvment();
                //oc.m_Speed += oc.m_Acceleration;
                //oc.m_TRSRelativ.m_Pos += oc.m_Speed;
            }
        }

    }
}
