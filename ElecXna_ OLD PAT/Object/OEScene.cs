using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using QM;
using QM.Phys; 
using QM.Maths;
using QM.Util;
using QM.Object.Widget;

namespace QM.Object
{
    public class OEScene : OGSprite
    {
        public enum eMode { ModePlan, ModeReal };
        public eMode m_Mode = eMode.ModePlan;

        public Color[] m_BlockTextureData;
        public int sx, sy;
        public int px, py;


        protected int m_TileSize = 64;
        public int m_Margin = 10;
        protected Point m_PosTopLeft = new Point(10, 10);
        protected Point m_SizeInTile = new Point(10, 6);
        protected int m_ShadowWidth = 2;

        public WImage m_WImage;



        static float m_MaxPot = 5.0f;

        static public Color m_ColorMin = Color.Red;
        static public Color m_ColorZero = Color.LightGray;
        static public Color m_ColorMax = Color.Blue;

        static public Color m_ColorIntensity = Color.Gold;//Black;



        public OEScene() { }

        public OEScene(int _TileSize, int _Margin, Point _PosTopLeft, Point _SizeInTile)
        {
            m_TileSize = _TileSize;
            m_Margin = _Margin;
            m_PosTopLeft = _PosTopLeft;
            m_SizeInTile = _SizeInTile;

            Create(_PosTopLeft.X, _PosTopLeft.Y, _SizeInTile.X * _TileSize + _Margin * 2, _SizeInTile.Y * _TileSize + _Margin * 2);

        }

        public OEScene(int _px, int _py, int _sx, int _sy)
        {
            Create(_px,  _py,  _sx,  _sy);
        }

        protected void Create( int _px, int _py, int _sx, int _sy)
        {
            sx = _sx;
            sy = _sy;
            px = _px;
            py = _py;
            m_PntSize = new MathPntSize(_px, _py, _sx, _sy); 
            //vide.bmp
            TextureCreationParameters tcp = new TextureCreationParameters(sx, sy, 1, 1, SurfaceFormat.Rgba32, TextureUsage.None, Color.White, FilterOptions.None, FilterOptions.None);
            m_BlockTexture = Texture2D.FromFile(QMGame.graphics.GraphicsDevice, "..\\..\\..\\Content\\ImagesQM\\vide.bmp", tcp);
            m_BlockTextureData = new Color[sx * sy];


            m_SrcRect = new Rectangle(0, 0, m_BlockTexture.Width, m_BlockTexture.Height);
            m_Depth = 0.3f;
            m_ColorSprite = Color.White;
            SetPosition(m_PntSize.m_Pos, 1.0f, 1.0f, ePosType.TopLeft);

            Rectangle rect = new Rectangle(_px,_py,_sx,_sy);
            m_WImage = new WImage("ImagesElec\\Bois.jpg", rect, Color.White);
            m_WImage.m_Depth = 0.9f;

            SetMode(eMode.ModeReal);
        }

        public void SetMode( eMode _mode)
        {
            if(m_Mode!=_mode)
            {
                m_Mode = _mode;
                switch(m_Mode)
                {
                    case eMode.ModePlan:
                        m_WImage.LoadTexture("ImagesElec\\Paper0040_1_S.jpg");
                        m_ColorZero = Color.Gray;
                        m_ColorIntensity = Color.White;
                        break;
                    case eMode.ModeReal:
                        m_WImage.LoadTexture("ImagesElec\\Bois.jpg");
                        m_ColorZero = Color.LightGray;
                        m_ColorIntensity = Color.Gold;
                        break;
                }
            }

        }

        static public Color ColorFromPotentiel(float _Potentiel)
        {
            float t = Math.Min(m_MaxPot, Math.Max(-m_MaxPot, _Potentiel));
            t = (t) / (m_MaxPot);
            if (t > 0.0f)
                return ColorManager.MixTwoColor(m_ColorMax, m_ColorZero, t);
            else
                return ColorManager.MixTwoColor(m_ColorMin, m_ColorZero, -t);
        }

        public void DrawPoint(int x, int y, Color col)
        {
            int u = x - px;
            int v = y - py;
            if (u >= 0 && v >= 0 && u < sx && v < sy)
            {
                m_BlockTextureData[u + sx * v] = col;
            }
        }

        public void DrawPoint3x3(int x, int y, Color col)
        {
            int u = x - px;
            int v = y - py;
            Color col2 = col;
            col2.A = (byte) (col.A*2/4);
            Color col3 = col;
            col3.A /= 4;
            if (u >= 0 && v >= 0 && u < sx - 1 && v < sy - 1)
            {
                m_BlockTextureData[u - 1 + sx * (v - 1)] = col3;
                m_BlockTextureData[u + sx * (v - 1)] = col2;
                m_BlockTextureData[u + 1 + sx * (v - 1)] = col3;
                m_BlockTextureData[u - 1 + sx * v] = col2;
                m_BlockTextureData[u + sx * v] = col;
                m_BlockTextureData[u + 1 + sx * v] = col2;
                m_BlockTextureData[u - 1 + sx * (v + 1)] = col3;
                m_BlockTextureData[u + sx * (v + 1)] = col2;
                m_BlockTextureData[u+1 + sx * (v+1)] = col3;
            }
        }

        override public void Update()
        {
            //for(int i=0;i<16;i++)
                QMGame.graphics.GraphicsDevice.Textures[0] = null; //truc important pour forcer l'absence de référence. Sinon plantage du au multithread!!
            m_BlockTexture.SetData(m_BlockTextureData);

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            m_WImage.Draw(spriteBatch);
            base.Draw(spriteBatch);
        }

    }
}
