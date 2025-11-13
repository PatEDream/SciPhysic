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
using QM.Elec;
using QM.Util;

namespace QM.Level
{
    public class LevTilesBase : LevelBase
    {
        protected ETileBoard m_TileBoard = new ETileBoard();

        //protected ETileScene m_TileScene;
        //protected OEScene m_BackgroundScene;
        //protected ElecCircuit m_Circuit;

        //protected int m_TileSize = 80;
        //protected int m_Margin = 30;
        //protected Point m_PosTopLeft = new Point(10, 10);
        //protected Point m_SizeInTile = new Point(8, 5);

        //protected ObjBase m_OEdge = new ObjBase();
        public override LevelBase CreateLevel(String Param1)
        {

            Type[] AType = new Type[0];
            System.Reflection.ConstructorInfo ctcInfo = this.GetType().GetConstructor(AType);
            if (ctcInfo == null)
                return null;
            LevTilesBase dst = (LevTilesBase)ctcInfo.Invoke(null);

            dst.StartLevel(Param1);

            return dst;
        }

        public void StartLevel(int _TileSize,int  _Margin,Point _PosTopLeft,Point _SizeInTile)
        {
            m_TileBoard.CreateBoard( _TileSize, _Margin, _PosTopLeft, _SizeInTile);
            AddObj(m_TileBoard);

            //m_TileSize = _TileSize;
            //m_Margin = _Margin;
            //m_PosTopLeft = _PosTopLeft;
            //m_SizeInTile = _SizeInTile;

            //AddObj(m_BackgroundScene = new OEScene(m_TileSize, m_Margin, m_PosTopLeft, m_SizeInTile));

            //m_Circuit = new ElecCircuit();

            //Point Pos = new Point(m_PosTopLeft.X + m_Margin, m_PosTopLeft.Y + m_Margin);
            //m_TileScene = new ETileScene(m_BackgroundScene, m_Circuit, m_TileSize, Pos, m_SizeInTile);
            //AddObj(m_TileScene);

            //AddObj(m_OEdge);
        }


        override public void Update()
        {
            m_TileBoard.m_TileScene.UpdateEndOfFrame();
            //m_TileBoard.Update();
            //m_Circuit.DoOneStep();
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
        }
        public override void DrawText(SpriteBatch spriteBatch)
        {
            //m_Circuit.DrawText(spriteBatch);
        }

        //public void DestroyEdge()
        //{
        //    m_TileBoard.DestroyEdge();
        //    //foreach (ObjBase ob in m_OEdge.m_AObj)
        //    //{
        //    //    OEBase oeb = (OEBase)ob;
        //    //    oeb.Destroy();
        //    //}
        //    //m_OEdge.m_AObj.Clear();
        //}
        //public void CreateWiredEdgeH(Boolean _Up)
        //{
        //    m_TileBoard.CreateWiredEdgeH(_Up);
        //    //OEWireSegment owc;

        //    //for (int i = 0; i < m_SizeInTile.X; i++)
        //    //{
        //    //    int X0 = m_PosTopLeft.X + m_Margin + m_TileScene.m_TileWidth / 2 + m_TileScene.m_TileWidth * i;
        //    //    int X1 = X0 + m_TileScene.m_TileWidth;
        //    //    int Y0 = m_PosTopLeft.Y + m_Margin / 2;
        //    //    if (!_Up)
        //    //        Y0 = (m_PosTopLeft.Y + m_Margin * 2 + m_TileScene.m_TileWidth * m_SizeInTile.Y) - m_Margin / 2;
        //    //    if (i < m_SizeInTile.X - 1)
        //    //    {
        //    //        owc = new OEWireSegment(m_BackgroundScene, m_Circuit);
        //    //        owc.Create(new Point(X0, Y0), new Point(X1, Y0), m_TileScene.m_TileWidth * 1 / 10);
        //    //        m_OEdge.AddObj(owc);
        //    //    }

        //    //    int Y1;
        //    //    if (_Up)
        //    //        Y1 = Y0 + m_Margin / 2;
        //    //    else
        //    //        Y1 = Y0 - m_Margin / 2;
        //    //    owc = new OEWireSegment(m_BackgroundScene, m_Circuit);
        //    //    owc.Create(new Point(X0, Y0), new Point(X0, Y1), m_TileScene.m_TileWidth * 1 / 10);
        //    //    m_OEdge.AddObj(owc);
        //    //}
        //}
        //public void CreateWiredEdgeV(Boolean _Left)
        //{
        //    m_TileBoard.CreateWiredEdgeV(_Left);

        //    //OEWireSegment owc;

        //    //for (int j = 0; j < m_SizeInTile.Y; j++)
        //    //{
        //    //    int Y0 = m_PosTopLeft.Y + m_Margin + m_TileScene.m_TileWidth / 2 + m_TileScene.m_TileWidth * j;
        //    //    int Y1 = Y0 + m_TileScene.m_TileWidth;
        //    //    int X0 = m_PosTopLeft.X + m_Margin / 2;
        //    //    if (!_Left)
        //    //        X0 = (m_PosTopLeft.X + m_Margin * 2 + m_TileScene.m_TileWidth * m_SizeInTile.X) - m_Margin / 2;
        //    //    if (j < m_SizeInTile.Y - 1)
        //    //    {
        //    //        owc = new OEWireSegment(m_BackgroundScene, m_Circuit);
        //    //        owc.Create(new Point(X0, Y0), new Point(X0, Y1), m_TileScene.m_TileWidth * 1 / 10);
        //    //        m_OEdge.AddObj(owc);
        //    //    }

        //    //    int X1;
        //    //    if (_Left)
        //    //        X1 = X0 + m_Margin / 2;
        //    //    else
        //    //        X1 = X0 - m_Margin / 2;
        //    //    owc = new OEWireSegment(m_BackgroundScene, m_Circuit);
        //    //    owc.Create(new Point(X0, Y0), new Point(X1 , Y0), m_TileScene.m_TileWidth * 1 / 10);
        //    //    m_OEdge.AddObj(owc);
        //    //}
        //}


    }
}
