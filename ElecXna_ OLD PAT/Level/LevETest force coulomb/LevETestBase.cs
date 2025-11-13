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
    public class LevETestBase : LevelBase
    {
        //public OGScene m_OGScene;
        //public PhyScene physScene;

        public LevETestBase() { }

        public override LevelBase CreateLevel(String Param1)
        {
            Type[] AType = new Type[0];
            System.Reflection.ConstructorInfo ctcInfo = this.GetType().GetConstructor(AType);
            if (ctcInfo == null)
                return null;
            LevETestBase dst = (LevETestBase)ctcInfo.Invoke(null);

            //Prepare();
            //dst.m_OGScene = PhyManager.m_OGScene;
            //dst.physScene = PhyManager.m_OGScene.m_Scene;
            dst.StartLevel(Param1);
            //dst.AddObj(dst.m_OGScene);
            //dst.m_OGScene.m_ToSave = false;
            return dst;
        }

        public void Prepare()
        {
        }


        //public override void ReadTxt(String[] _Lines, ref int _CurrentLine)
        //{
        //    m_OGScene = PhyManager.m_OGScene;
        //    physScene = PhyManager.m_OGScene.m_Scene;
        //    base.ReadTxt(_Lines, ref  _CurrentLine);
        //}
        //public override void EndOfRead()
        //{
        //    base.EndOfRead();
        //    AddObj(m_OGScene);
        //    m_OGScene.m_ToSave = false;
        //}

        override public void Init()
        {

        }


    }
}
