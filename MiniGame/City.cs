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
        bool test = false;
        Sprite3 before = null;
        Sprite3 after = null;
        Color waterColor = new Color(36, 36, 215, 255);
        Color testColor;
        
        
        public override void LoadContent()
        {
            city = new Sprite3(true, Game1.texCityScreen, 0, 0);

            testColor = UIntToColor(4007076900);
            
            before = new Sprite3(true, Game1.texMapWater, 0, 0);
            before.setWidthHeight(400, 300);
            
            
            city.setWidthHeight(800, 600);
            
        }
        public override void Update(GameTime gameTime)
        {
            if (keyState.IsKeyDown(Keys.D) && !prevKeyState.IsKeyDown(Keys.D))
            {
                Util.ChangeColourInTexturePNG(Game1.texMapWater, waterColor, Color.Black, 4291577919);
                after = new Sprite3(true, Game1.texMapWater, 400, 300);
                after.setWidthHeight(400, 300);
                test = true;
            }
                
            /*  4280556781
                4280556780
                4280556781
                4280622573
                4280556781
                4280622573
                4280556781
                4280556782
                4280556781
                4280557038
                4280556780
                4280490988
                4280556781
                4280622573
                4280556781
                4278190080
                4280556781
                4280622573
                4280556781
                4280556782
                4280556781
                4280558807
            */
            //Min blue 4292433000
            //Max blue 4292578930
            //Difference 145930


        }
        public override void Draw(GameTime gameTime)
        {
            graphicsDevice.Clear(UIntToColor(4294967040));
            //Console.WriteLine("red uint is: " + ColorToUIntBlake(waterColor));
            spriteBatch.Begin();
            //city.Draw(spriteBatch);
            before.Draw(spriteBatch);
            if (test)
                after.Draw(spriteBatch);
            
            spriteBatch.End();
        }
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
