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
    public class ETile : ObjBase
    {
        ETileScene m_TileScene;
        public enum eType { Wire, Wire2, Wire3, Wire4, WireNoCross, Resistance, Pile, Lampe, Diode, Condensateur, Bobine, None };
        public Boolean IsWire() { return m_Type == eType.Wire || m_Type == eType.Wire2 || m_Type == eType.Wire3 || m_Type == eType.Wire4 || m_Type == eType.WireNoCross; }        
        public eType m_Type = eType.None;

        public Point m_PosInTile;
        public Angle360 m_A;
        public Angle360 m_AGoal;



        public Point GetPosStart(Angle360 A)
        {
            Point dst = new Point();
            dst.X = (int)Math.Round(Math.Cos((Math.PI * A.m_Value) / 180.0) * m_TileScene.m_TileWidth / 2);
            dst.Y = (int)Math.Round(Math.Sin((Math.PI * A.m_Value) / 180.0) * m_TileScene.m_TileWidth / 2);
            return dst;
        }
        public Point GetPosEnd(Angle360 A)
        {
            return GetPosStart(A.GetOpposite());
        }

        public Point GetPosStart(Angle360 A, float _Percent)
        {
            Point dst = GetPosStart(A);
            dst.X = (int)((float)dst.X * _Percent);
            dst.Y = (int)((float)dst.Y * _Percent);
            return dst;
        }
        public Point GetPosEnd(Angle360 A, float _Percent)
        {
            return GetPosStart(A.GetOpposite(), _Percent);
        }


        public List<OEBase> m_AOEBase = new List<OEBase>();
        public ObjBase m_ObjBorder = null;

        public class Info
        {
            public eType m_Type;
            public Point m_PosInTile;
            public Angle360 m_A;
            public Angle360 m_AGoal;
        }

        override public void Destroy()
        {
            foreach (OEBase oeb in m_AOEBase)
            {
                oeb.Destroy();
            }
            m_AObj.Clear();
            m_TileScene.Remove(this);
        }


        public void AddBorder()
        {
            m_ObjBorder = new ObjBase();
            AddObj(m_ObjBorder);

            Point Pos = m_TileScene.GetPosFromTile(m_PosInTile);
            Rectangle rect;
            WImage Wtmp;
            Color ColW = Color.White;
            ColW.A = 64;
            Color ColG = Color.Black;
            ColG.A = 128;
            int m_ShadowWidth = 2;

            int _Margin = m_TileScene.m_Scene.m_Margin;
            int _TileSize = m_TileScene.m_TileWidth;
            Point _PosTopLeft = m_TileScene.m_Pos;
            Point _SizeInTile = m_TileScene.m_SizeInTile;

            int X0 = _PosTopLeft.X + m_PosInTile.X * _TileSize;// -m_TileScene.m_TileWidth / 2;
            int X1 = _PosTopLeft.X + (m_PosInTile.X + 1) * _TileSize;// -m_TileScene.m_TileWidth / 2;
            int Y0 = _PosTopLeft.Y + m_PosInTile.Y * _TileSize;// +-m_TileScene.m_TileWidth / 2;
            int Y1 = _PosTopLeft.Y + (m_PosInTile.Y + 1) * _TileSize;// +-m_TileScene.m_TileWidth / 2;

            rect = new Rectangle(X0,Y0,X1-X0,m_ShadowWidth);
            Wtmp = new WImage("ImagesQM\\vide.bmp", rect, ColW);
            Wtmp.m_Depth = 0.89f;
            m_ObjBorder.AddObj(Wtmp);
            rect = new Rectangle(X0, Y1 - m_ShadowWidth , X1 - X0, m_ShadowWidth);
            Wtmp = new WImage("ImagesQM\\vide.bmp", rect, ColG);
            Wtmp.m_Depth = 0.89f;
            m_ObjBorder.AddObj(Wtmp);


            rect = new Rectangle(X0, Y0, m_ShadowWidth, Y1 - Y0);
            Wtmp = new WImage("ImagesQM\\vide.bmp", rect, ColW);
            Wtmp.m_Depth = 0.89f;
            m_ObjBorder.AddObj(Wtmp);
            rect = new Rectangle(X1 - m_ShadowWidth, Y0, m_ShadowWidth, Y1 - Y0);
            Wtmp = new WImage("ImagesQM\\vide.bmp", rect, ColG);
            Wtmp.m_Depth = 0.89f;
            m_ObjBorder.AddObj(Wtmp);

        }
        public void AddOEBase(OEBase oeb)
        {
            m_AOEBase.Add(oeb);
            m_AObj.Add(oeb);
        }

        public void Create(eType _Type, Point _PosInTile, Angle360 _A, ETileScene _TileScene)
        {
            m_TileScene = _TileScene;
            m_Type = _Type;
            m_PosInTile = _PosInTile;
            m_A = m_AGoal = _A;

            Point Pos = _TileScene.GetPosFromTile(_PosInTile);
            switch (_Type)
            {
                case eType.Wire:
                    {
                        OEWireSegment owc = new OEWireSegment(_TileScene.m_Scene, _TileScene.m_Circuit);
                        owc.Create(Misc.Add(Pos, GetPosStart(_A)), Misc.Add(Pos, GetPosEnd(_A)), m_TileScene.m_TileWidth * 1 / 10, false);
                        AddOEBase(owc);
                        break;
                    }
                case eType.Wire2:
                    {
                        OEWireSegment owc = new OEWireSegment(_TileScene.m_Scene, _TileScene.m_Circuit);
                        owc.Create(Misc.Add(Pos, GetPosStart(_A)), Pos, m_TileScene.m_TileWidth * 1 / 10, true);
                        AddOEBase(owc);
                        OEWireSegment owc2 = new OEWireSegment(_TileScene.m_Scene, _TileScene.m_Circuit);
                        owc2.Create(Misc.Add(Pos, GetPosStart(_A.GetRight())), Pos, m_TileScene.m_TileWidth * 1 / 10, true);
                        AddOEBase(owc2);
                        break;
                    }
                case eType.Wire3:
                    {
                        OEWireSegment owc = new OEWireSegment(_TileScene.m_Scene, _TileScene.m_Circuit);
                        owc.Create(Misc.Add(Pos, GetPosStart(_A)), Pos, m_TileScene.m_TileWidth * 1 / 10, false);
                        AddOEBase(owc);
                        Angle360 A2 = _A.GetRight();
                        OEWireSegment owc2 = new OEWireSegment(_TileScene.m_Scene, _TileScene.m_Circuit);
                        owc2.Create(Pos, Misc.Add(Pos, GetPosStart(A2)), m_TileScene.m_TileWidth * 1 / 10, false);
                        AddOEBase(owc2);
                        OEWireSegment owc3 = new OEWireSegment(_TileScene.m_Scene, _TileScene.m_Circuit);
                        owc3.Create(Pos, Misc.Add(Pos, GetPosEnd(A2)), m_TileScene.m_TileWidth * 1 / 10, false);
                        AddOEBase(owc3);
                        break;
                    }
                case eType.Wire4:
                    {
                        OEWireSegment owc = new OEWireSegment(_TileScene.m_Scene, _TileScene.m_Circuit);
                        owc.Create(Misc.Add(Pos, GetPosStart(_A)), Pos, m_TileScene.m_TileWidth * 1 / 10, false);
                        AddOEBase(owc);
                        OEWireSegment owc1 = new OEWireSegment(_TileScene.m_Scene, _TileScene.m_Circuit);
                        owc1.Create(Pos, Misc.Add(Pos, GetPosEnd(_A)), m_TileScene.m_TileWidth * 1 / 10, false);
                        AddOEBase(owc1);
                        Angle360 A2 = _A.GetRight();
                        OEWireSegment owc2 = new OEWireSegment(_TileScene.m_Scene, _TileScene.m_Circuit);
                        owc2.Create(Misc.Add(Pos, GetPosStart(A2)), Pos, m_TileScene.m_TileWidth * 1 / 10, false);
                        AddOEBase(owc2);
                        OEWireSegment owc3 = new OEWireSegment(_TileScene.m_Scene, _TileScene.m_Circuit);
                        owc3.Create(Pos, Misc.Add(Pos, GetPosEnd(A2)), m_TileScene.m_TileWidth * 1 / 10, false);
                        AddOEBase(owc3);
                        break;
                    }
                case eType.WireNoCross:
                    {
                        OEWireSegment owc = new OEWireSegment(_TileScene.m_Scene, _TileScene.m_Circuit);
                        owc.Create(Misc.Add(Pos, GetPosStart(_A)), Misc.Add(Pos, GetPosEnd(_A)), m_TileScene.m_TileWidth * 1 / 10, false);
                        AddOEBase(owc);
                        Angle360 A2 = _A.GetRight();
                        OEWireSegment owc2 = new OEWireSegment(_TileScene.m_Scene, _TileScene.m_Circuit);
                        owc2.Create(Misc.Add(Pos, GetPosStart(A2)), Misc.Add(Pos, GetPosEnd(A2)), m_TileScene.m_TileWidth * 1 / 10, false);
                        AddOEBase(owc2);
                        break;
                    }
                case eType.Resistance:
                    {
                        OEResistance or = new OEResistance(_TileScene.m_Scene, _TileScene.m_Circuit);
                        or.Create(Misc.Add(Pos, GetPosStart(_A, 0.6f)), Misc.Add(Pos, GetPosEnd(_A, 0.6f)));
                        AddOEBase(or);
                        OEWireSegment owc = new OEWireSegment(_TileScene.m_Scene, _TileScene.m_Circuit);
                        owc.Create(Misc.Add(Pos, GetPosStart(_A, 0.6f)), Misc.Add(Pos, GetPosStart(_A)), m_TileScene.m_TileWidth * 1 / 10, false);
                        AddOEBase(owc);
                        OEWireSegment owc2 = new OEWireSegment(_TileScene.m_Scene, _TileScene.m_Circuit);
                        owc2.Create(Misc.Add(Pos, GetPosEnd(_A, 0.6f)), Misc.Add(Pos, GetPosEnd(_A)), m_TileScene.m_TileWidth * 1 / 10, false);
                        AddOEBase(owc2);
                        break;
                    }
                case eType.Pile:
                    {
                        float fact = 0.75f;
                        if (_TileScene.m_Scene.m_Mode == OEScene.eMode.ModePlan)
                            fact = 0.25f;
                        OEPile op = new OEPile(_TileScene.m_Scene, _TileScene.m_Circuit);
                        op.Create(Misc.Add(Pos, GetPosStart(_A, fact)), Misc.Add(Pos, GetPosEnd(_A, fact)));
                        AddOEBase(op);
                        OEWireSegment owc = new OEWireSegment(_TileScene.m_Scene, _TileScene.m_Circuit);
                        owc.Create(Misc.Add(Pos, GetPosStart(_A, fact)), Misc.Add(Pos, GetPosStart(_A)), m_TileScene.m_TileWidth * 1 / 10, false);
                        AddOEBase(owc);
                        OEWireSegment owc2 = new OEWireSegment(_TileScene.m_Scene, _TileScene.m_Circuit);
                        owc2.Create(Misc.Add(Pos, GetPosEnd(_A, fact)), Misc.Add(Pos, GetPosEnd(_A)), m_TileScene.m_TileWidth * 1 / 10, false);
                        AddOEBase(owc2);
                        break;
                    }
                case eType.Lampe:
                    {
                        OELampe or = new OELampe(_TileScene.m_Scene, _TileScene.m_Circuit);
                        or.Create(Misc.Add(Pos, GetPosStart(_A, 0.65f)), Misc.Add(Pos, GetPosEnd(_A, 0.65f)));
                        AddOEBase(or);
                        OEWireSegment owc = new OEWireSegment(_TileScene.m_Scene, _TileScene.m_Circuit);
                        owc.Create(Misc.Add(Pos, GetPosStart(_A, 0.65f)), Misc.Add(Pos, GetPosStart(_A)), m_TileScene.m_TileWidth * 1 / 10, false);
                        AddOEBase(owc);
                        OEWireSegment owc2 = new OEWireSegment(_TileScene.m_Scene, _TileScene.m_Circuit);
                        owc2.Create(Misc.Add(Pos, GetPosEnd(_A, 0.65f)), Misc.Add(Pos, GetPosEnd(_A)), m_TileScene.m_TileWidth * 1 / 10, false);
                        AddOEBase(owc2);
                        break;
                    }
                case eType.Diode:
                    {
                        OEDiode or = new OEDiode(_TileScene.m_Scene, _TileScene.m_Circuit);
                        or.Create(Misc.Add(Pos, GetPosStart(_A, 0.65f)), Misc.Add(Pos, GetPosEnd(_A, 0.65f)));
                        AddOEBase(or);
                        OEWireSegment owc = new OEWireSegment(_TileScene.m_Scene, _TileScene.m_Circuit);
                        owc.Create(Misc.Add(Pos, GetPosStart(_A, 0.65f)), Misc.Add(Pos, GetPosStart(_A)), m_TileScene.m_TileWidth * 1 / 10, false);
                        AddOEBase(owc);
                        OEWireSegment owc2 = new OEWireSegment(_TileScene.m_Scene, _TileScene.m_Circuit);
                        owc2.Create(Misc.Add(Pos, GetPosEnd(_A, 0.65f)), Misc.Add(Pos, GetPosEnd(_A)), m_TileScene.m_TileWidth * 1 / 10, false);
                        AddOEBase(owc2);
                        break;
                    }
                case eType.Condensateur:
                    {
                        OECondensateur or = new OECondensateur(_TileScene.m_Scene, _TileScene.m_Circuit);
                        or.Create(Misc.Add(Pos, GetPosStart(_A, 0.75f)), Misc.Add(Pos, GetPosEnd(_A, 0.75f)));
                        AddOEBase(or);
                        OEWireSegment owc = new OEWireSegment(_TileScene.m_Scene, _TileScene.m_Circuit);
                        owc.Create(Misc.Add(Pos, GetPosStart(_A, 0.75f)), Misc.Add(Pos, GetPosStart(_A)), m_TileScene.m_TileWidth * 1 / 10, false);
                        AddOEBase(owc);
                        OEWireSegment owc2 = new OEWireSegment(_TileScene.m_Scene, _TileScene.m_Circuit);
                        owc2.Create(Misc.Add(Pos, GetPosEnd(_A, 0.75f)), Misc.Add(Pos, GetPosEnd(_A)), m_TileScene.m_TileWidth * 1 / 10, false);
                        AddOEBase(owc2);
                        break;
                    }
                case eType.Bobine:
                    {
                        OEBobine or = new OEBobine(_TileScene.m_Scene, _TileScene.m_Circuit);
                        or.Create(Misc.Add(Pos, GetPosStart(_A, 0.75f)), Misc.Add(Pos, GetPosEnd(_A, 0.75f)));
                        AddOEBase(or);
                        OEWireSegment owc = new OEWireSegment(_TileScene.m_Scene, _TileScene.m_Circuit);
                        owc.Create(Misc.Add(Pos, GetPosStart(_A, 0.75f)), Misc.Add(Pos, GetPosStart(_A)), m_TileScene.m_TileWidth * 1 / 10, false);
                        AddOEBase(owc);
                        OEWireSegment owc2 = new OEWireSegment(_TileScene.m_Scene, _TileScene.m_Circuit);
                        owc2.Create(Misc.Add(Pos, GetPosEnd(_A, 0.75f)), Misc.Add(Pos, GetPosEnd(_A)), m_TileScene.m_TileWidth * 1 / 10, false);
                        AddOEBase(owc2);
                        break;
                    }
            }
            AddBorder();
        }
        public void Create(ETile.Info _Info, ETileScene _TileScene)
        {
            Create( _Info.m_Type,  _Info.m_PosInTile,  _Info.m_A,  _TileScene);
            m_AGoal = _Info.m_AGoal;
        }
        public ETile.Info GetInfo()
        {
            ETile.Info dst = new ETile.Info();
            dst.m_Type = m_Type;
            dst.m_PosInTile = m_PosInTile;
            dst.m_A = m_A;
            dst.m_AGoal = m_AGoal;
            return dst;
        }


        public float GetMaxIntensity()
        {
            float MaxIntensity = 0.0f;
            foreach (OEBase oeb in m_AOEBase)
            {
                if (Math.Abs(oeb.GetIntensityGoal()) > MaxIntensity)
                    MaxIntensity = Math.Abs(oeb.GetIntensityGoal());
            }
            return MaxIntensity;
        }
    }
}
