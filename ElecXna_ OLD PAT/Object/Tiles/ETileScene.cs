using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QM.Object;
using QM.Object.Widget;
using QM.Elec;
using QM.Util;
using QM.Maths;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace QM.Level
{
    public class ETileScene : OGBase
    {
        public int m_TileWidth = 64;
        public Point m_Pos = new Point();
        public Point m_SizeInTile = new Point();

        public OEScene m_Scene;
        public ElecCircuit m_Circuit;

        protected ETile[,] m_ATile;

        public ETileScene(OEScene _Scene, ElecCircuit _Circuit, int _TileWidth,  Point _Pos, Point _SizeInTile)
        {
            m_Scene = _Scene;
            m_Circuit = _Circuit;
            m_TileWidth = _TileWidth;
            m_Pos = _Pos;
            m_SizeInTile = _SizeInTile;
            m_ATile = new ETile[_SizeInTile.X, _SizeInTile.Y];
        }


        public  void UpdateEndOfFrame()
        {
            //base.Update();
            for (int i = 0; i < m_ATile.GetLength(0); i++)
            {
                for (int j = 0; j < m_ATile.GetLength(1); j++)
                {
                    ETile et = m_ATile[i, j];
                    if (et != null && et.m_A.m_Value != et.m_AGoal.m_Value)
                    {
                        Angle360 D = Angle360.Diff(et.m_AGoal, et.m_A);
                        if (D.m_Value > 5) D.m_Value = 5;
                        if (D.m_Value < -5) D.m_Value = -5;
                        ETile.eType type = et.m_Type;
                        Point Pos = et.m_PosInTile;
                        Angle360 A = et.m_A.Add(D);
                        Angle360 AGoal = et.m_AGoal;
                        //QMGame.m_ListObjToDestroy.Add(et);
                        et.Destroy();
                        AddTile(type, Pos, A);
                        et = m_ATile[i, j];
                        et.m_AGoal = AGoal;
                        m_Circuit.SetDurty();
                    }
                }
            }
        }


        public void MoveAllScene(Point Pos)
        {
            Point Decal;
            Decal.X = Pos.X - m_Pos.X;
            Decal.Y = Pos.Y - m_Pos.Y;

            m_Pos = Pos;
            m_Scene.m_TRSRelativ.m_Pos.X += Decal.X;
            m_Scene.m_TRSRelativ.m_Pos.Y += Decal.Y;

            foreach (ETile et in m_ATile)
            {
                if (et != null)
                {
                    foreach (ObjBase ob in et.m_ObjBorder.m_AObj)
                    {
                        WImage wim = (WImage)ob;
                        wim.m_DstRect.X += Decal.X;
                        wim.m_DstRect.Y += Decal.Y;
                    }
                    foreach (OEBase oeb in et.m_AOEBase)
                    {
                        oeb.m_TRSRelativ.m_Pos.X += Decal.X;
                        oeb.m_TRSRelativ.m_Pos.Y += Decal.Y;
                    }
                }
            }
        }


        public Point GetPosFromTile(Point _PosInTile)
        {
            return new Point(m_Pos.X + m_TileWidth * _PosInTile.X + m_TileWidth/2, m_Pos.Y + m_TileWidth * _PosInTile.Y + m_TileWidth/2);
        }
        public Point GetTileFromPos(Point _PosInScreen)
        {
            Point dst = new Point((_PosInScreen.X - m_Pos.X) / m_TileWidth, (_PosInScreen.Y - m_Pos.Y) / m_TileWidth);
            dst.X = dst.X > 0 ? dst.X : 0;
            dst.X = dst.X < m_ATile.GetLength(0) ? dst.X : m_ATile.GetLength(0) - 1;
            dst.Y = dst.Y > 0 ? dst.Y : 0;
            dst.Y = dst.Y < m_ATile.GetLength(1) ? dst.Y : m_ATile.GetLength(1) - 1;
            return dst;
        }

        public ETile GetTile(int _PosX,int _PosY)
        {
            return m_ATile[_PosX,_PosY];
        }
        public ETile GetTile(Point _PosInTile)
        {
            return GetTile(_PosInTile.X, _PosInTile.Y);
        }
        public void AddTile(ETile.Info inf)
        { AddTile(inf.m_Type, inf.m_PosInTile, inf.m_A); }
        public void AddTile(ETile.eType _Type, Point _PosInTile, Angle360 _A)
        {
            ETile t = new ETile();
            t.Create(_Type, _PosInTile, _A, this);
            AddObj(t);
            m_ATile[_PosInTile.X, _PosInTile.Y] = t;
        }
        public void Remove(ETile et)
        {
            m_ATile[et.m_PosInTile.X, et.m_PosInTile.Y] = null;
            m_AObj.Remove(et);
            if(m_Circuit!=null)
                m_Circuit.SetDurty();
        }
        public ETile FindTile(Point _PosInTile)
        {
            if (_PosInTile.X < 0 || _PosInTile.Y < 0 || _PosInTile.X >= m_ATile.GetLength(0) || _PosInTile.Y >= m_ATile.GetLength(1))
                return null;
            return m_ATile[_PosInTile.X, _PosInTile.Y];
        }
        public void RemoveTile(Point _PosInTile)
        {
            ETile et = FindTile(_PosInTile);
            if (et != null)
            {
                et.Destroy();
                //Remove(et);
            }
        }


        public void TurnRightTile(Point _PosInTile)
        {
            ETile et = FindTile(_PosInTile);
            if (et != null)
            {
                et.m_AGoal = et.m_AGoal.GetRight();
            }
        }
        public void TurnLeftTile(Point _PosInTile)
        {
            ETile et = FindTile(_PosInTile);
            if (et != null)
            {
                et.m_AGoal = et.m_AGoal.GetLeft();
            }
        }
        public Boolean MoveTile(Point _PosSrcInTile, Point _PosDstInTile)
        {
            ETile et = FindTile(_PosSrcInTile);
            ETile et2 = FindTile(_PosDstInTile);
            if (et2 != null)
            {
                DebugManager.DoArret();
                return false;
            }
            if (et != null)
            {
                ETile.eType type = et.m_Type;
                Point Pos = _PosDstInTile;
                Angle360 A = et.m_A;
                et.Destroy();
                AddTile(type, Pos, A);
                m_Circuit.SetDurty();
            }
            return true;
        }
        public Boolean ExchangeTile(Point _PosSrcInTile, Point _PosDstInTile)
        {
            ETile et = FindTile(_PosSrcInTile);
            ETile et2 = FindTile(_PosDstInTile);
            if (et2 == null)
            {
                DebugManager.DoArret();
                return false;
            }
            if (et != null)
            {
                ETile.eType type1 = et.m_Type;
                Point Pos1 = _PosDstInTile;
                Angle360 A1 = et.m_A; 
                ETile.eType type2 = et2.m_Type;
                Point Pos2 = _PosSrcInTile;
                Angle360 A2 = et2.m_A;
                et.Destroy();
                et2.Destroy();
                AddTile(type1, Pos1, A1);
                AddTile(type2, Pos2, A2);
                m_Circuit.SetDurty();
            }
            return true;
        }

        public void RemoveAll()
        {
            for (int i = 0; i < m_ATile.GetLength(0); i++)
            {
                for (int j = 0; j < m_ATile.GetLength(1); j++)
                {
                    ETile et = m_ATile[i, j];
                    if (et != null)
                    {
                        et.Destroy();
                    }
                }
            }
        }
        public int RemoveAllActiv()
        {
            int Count = 0;
            for (int i = 0; i < m_ATile.GetLength(0); i++)
            {
                for (int j = 0; j < m_ATile.GetLength(1); j++)
                {
                    ETile et = m_ATile[i, j];
                    if (et != null && et.GetMaxIntensity() > ElecDipole.m_ThresholdIntensity)
                    {
                        et.Destroy();
                        Count += 1;
                    }
                }
            }
            return Count;
        }
        public int CountActiv()
        {
            int Count = 0;
            for (int i = 0; i < m_ATile.GetLength(0); i++)
            {
                for (int j = 0; j < m_ATile.GetLength(1); j++)
                {
                    ETile et = m_ATile[i, j];
                    if (et != null && et.GetMaxIntensity() > ElecDipole.m_ThresholdIntensity)
                    {
                        Count += 1;
                    }
                }
            }
            return Count;
        }
        public int CountActiv(ETile.eType _whichType)
        {
            int Count = 0;
            for (int i = 0; i < m_ATile.GetLength(0); i++)
            {
                for (int j = 0; j < m_ATile.GetLength(1); j++)
                {
                    ETile et = m_ATile[i, j];
                    if (et != null && et.m_Type == _whichType && et.GetMaxIntensity() > ElecDipole.m_ThresholdIntensity)
                    {
                        Count += 1;
                    }
                }
            }
            return Count;
        }
        public int CountActivDipole()
        {
            int Count = 0;
            for (int i = 0; i < m_ATile.GetLength(0); i++)
            {
                for (int j = 0; j < m_ATile.GetLength(1); j++)
                {
                    ETile et = m_ATile[i, j];
                    if (et != null && et.IsWire() == false && et.GetMaxIntensity() > ElecDipole.m_ThresholdIntensity)
                    {
                        Count += 1;
                    }
                }
            }
            return Count;
        }
        public int CountAll()
        {
            int Count = 0;
            for (int i = 0; i < m_ATile.GetLength(0); i++)
            {
                for (int j = 0; j < m_ATile.GetLength(1); j++)
                {
                    Count += 1;
                }
            }
            return Count;
        }
        public int Count(ETile.eType _whichType)
        {
            int Count = 0;
            for (int i = 0; i < m_ATile.GetLength(0); i++)
            {
                for (int j = 0; j < m_ATile.GetLength(1); j++)
                {
                    ETile et = m_ATile[i, j];
                    if (et != null && et.m_Type == _whichType)
                    {
                        Count += 1;
                    }
                }
            }
            return Count;
        }
        public int CountDipole()
        {
            int Count = 0;
            for (int i = 0; i < m_ATile.GetLength(0); i++)
            {
                for (int j = 0; j < m_ATile.GetLength(1); j++)
                {
                    ETile et = m_ATile[i, j];
                    if (et != null && et.IsWire()==false)
                    {
                        Count += 1;
                    }
                }
            }
            return Count;
        }
        public int CountShortCut()
        {
            int Count = 0;
            for (int i = 0; i < m_ATile.GetLength(0); i++)
            {
                for (int j = 0; j < m_ATile.GetLength(1); j++)
                {
                    ETile et = m_ATile[i, j];
                    if (et != null && et.GetMaxIntensity() > ElecCircuit.m_IntensityOfShortCut)
                    {
                        Count += 1;
                    }
                }
            }
            return Count;
        }

        public List<ETile> GetAllTile()
        {
            List<ETile> dst = new List<ETile>();
            for (int i = 0; i < m_ATile.GetLength(0); i++)
            {
                for (int j = 0; j < m_ATile.GetLength(1); j++)
                {
                    ETile et = m_ATile[i, j];
                    if (et != null )
                    {
                        dst.Add(et);
                    }
                }
            }
            return dst;
        }
        public List<ETile> GetAllTile(ETile.eType _whichType)
        {
            List<ETile> dst = new List<ETile>();
            for (int i = 0; i < m_ATile.GetLength(0); i++)
            {
                for (int j = 0; j < m_ATile.GetLength(1); j++)
                {
                    ETile et = m_ATile[i, j];
                    if (et != null && et.m_Type == _whichType)
                    {
                        dst.Add(et);
                    }
                }
            }
            return dst;
        }


        public void SetMode(OEScene.eMode _Mode)
        {
            List<ETile.Info> LInfo = new List<ETile.Info>();
            foreach (ObjBase ob in m_AObj)
            {
                ETile t = (ETile)ob;
                //t.SetMode(_Mode);
                LInfo.Add(t.GetInfo());
            }
            RemoveAll();

            foreach (ETile.Info inf in LInfo)
            {
                AddTile(inf);
            }
        }
        public void EmptyActivity()
        {
            foreach (ETile et in m_ATile)
            {
                if (et != null)
                {
                    foreach (OEBase oeb in et.m_AOEBase)
                    {
                        oeb.m_Visuel.m_Potentiel = oeb.m_Visuel.m_PotentielGoal = 0.0f;
                    }
                }
            }
        }
        public void ShowBorderOfTiles(Boolean _b)
        {
            for (int i = 0; i < m_ATile.GetLength(0); i++)
            {
                for (int j = 0; j < m_ATile.GetLength(1); j++)
                {
                    ETile et = m_ATile[i, j];
                    if (et != null)
                    {
                        et.m_ObjBorder.SetActiveAndVisibleState(_b);
                    }
                }
            }
        }
    }
}
