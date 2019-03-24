using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using RC_Framework;
using System.IO;

namespace MiniGame
{
    
    class Backgrounds
    {

        public Texture2D texture;
        public Rectangle rectangle;
        public int speed;
        
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rectangle, Color.White);
        }
    }

    class Scrolling : Backgrounds
    {
        
        public Scrolling(Texture2D newTexture, Rectangle newRectangle, int newSpeed)
        {
            texture = newTexture;
            rectangle = newRectangle;
            speed = newSpeed;
            
        }

        public void Update()
        {

            if (Keyboard.GetState().IsKeyDown(Keys.Right))
                rectangle.X -= speed * 2;
            else if (Keyboard.GetState().IsKeyDown(Keys.Left))
                rectangle.X -= speed / 2;
            else
                rectangle.X -= speed;
        }

       
    }
}
