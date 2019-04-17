using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using RC_Framework;


namespace MiniGame
{
    class City : RC_GameStateParent
    {
        Sprite3 city = null;
        
        bool doneCheck = false;
        private static Vector2 currentLoc;

        public override void LoadContent()
        {
            city = new Sprite3(true, Game1.texCityScreen, 0, 0);            
            city.setWidthHeight(800, 600);
            
        }
        public override void Update(GameTime gameTime)
        {
            
        }
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            city.Draw(spriteBatch);
            spriteBatch.DrawString(Game1.font, Game1.cities[currentLoc], new Vector2(100, 100), Color.Black);
            spriteBatch.End();
        }

        public static void CurrentLocation(Vector2 loc)
        {
            currentLoc = loc;
        }

        //Some functions for calculating unassigned integers to and from XNA Color
        private Color UIntToColor(uint color)
        {
            byte a = (byte)(color >> 24);
            byte b = (byte)(color >> 16);
            byte g = (byte)(color >> 8);
            byte r = (byte)(color >> 0);
            return new Color(r, g, b, a);
        }
        private uint ColorToUInt(Color color)
        {
            return (uint)((color.A << 24) | (color.B << 16) |
                          (color.G << 8) | (color.R << 0));
        }

        private uint ColorToUIntBlake(Color color)
        {
            byte[] colorBytes = new byte[4];
            colorBytes[3] = color.A;
            colorBytes[2] = color.R;
            colorBytes[1] = color.G;
            colorBytes[0] = color.B;
            return BitConverter.ToUInt32(colorBytes, 0);
             
        }

        
    }
}
