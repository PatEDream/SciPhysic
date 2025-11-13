using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using QM.Phys; using QM.Maths;
using QM.Object;
using QM.Level.Menu;
using QM.Object.Widget;


namespace QM.Level
{
    class LevETest1 : LevETestBase
    {
        //OGBase m_GamePlayBase;
        //OGFire objFire; 
        //OGGoal objGoal;
        public List<OECharge> m_LCharge;
        WEndOfLevelBox  m_EndOfLevelBox;
        WYouLooseBox    m_YouLooseBox;

        public override void StartLevel(String Param1)
        {
            //LoadTxtFile("..\\..\\..\\Content\\Level\\LevelAttFirst.txt");
            int sx = QMGame.GetBackBufferWidth();
            int sy = QMGame.GetBackBufferHeight();

            m_LCharge = new List<OECharge>();
            OECharge oc;
            AddObj(oc = new OECharge(new MathPntSize(400, 200, 32, 32), OECharge.eType.Negativ));
            oc.m_Speed = new Vector2(0.0f, 0.2f);
            m_LCharge.Add(oc);
            AddObj(oc = new OECharge(new MathPntSize(500, 200, 32, 32), OECharge.eType.Positiv));
            oc.m_Speed = new Vector2(0.0f, -0.2f);
            //objGoal.m_Speed = new Vector2(3.0f, 0);
            m_LCharge.Add(oc);

            AddObj(m_EndOfLevelBox = new WEndOfLevelBox(new Vector2(sx * 2 / 8, sy * 3 / 8), ""));
            m_EndOfLevelBox.SetActiveAndVisibleState(true);

            AddObj(m_YouLooseBox = new WYouLooseBox(new Vector2(sx * 2 / 8, sy * 3 / 8), "Perdu!"));
            m_YouLooseBox.SetActiveAndVisibleState(false);

            //SaveTxtFile("..\\..\\..\\Content\\Level\\LevETest1_Test.txt");
        }



        //public override void ReadTxt(String[] _Lines, ref int _CurrentLine)
        //{
        //    base.ReadTxt(_Lines,ref  _CurrentLine);
        //}

        //public override void EndOfRead()
        //{
        //    base.EndOfRead();
        //    m_GamePlayBase = (OGBase)m_MainObj.m_AObj[0];
        //    objFire = (OGFire)m_GamePlayBase.m_AObj[0]; ;
        //    objGoal = (OGGoal)m_GamePlayBase.m_AObj[1]; ;
        //    m_EndOfLevelBox = (WEndOfLevelBox)m_MainObj.m_AObj[1]; ;
        //    m_YouLooseBox = (WYouLooseBox)m_MainObj.m_AObj[2]; ;
        //}

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
            }            //if (objGoal.m_TRSGlobal.m_Pos.X > QMGame.GetBackBufferWidth() * 7 / 8)
            //{
            //    m_YouLooseBox.SetActiveAndVisibleState(true);
            //    m_GamePlayBase.m_IsActive = false;
            //}
            //else if (objGoal.done && !m_EndOfLevelBox.m_IsVisible)
            //{
            //    m_EndOfLevelBox.SetActiveAndVisibleState(true);
            //}
        }

    }
}
