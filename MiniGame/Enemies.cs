using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RC_Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;

namespace MiniGame
{
    class Enemies
    {
        public Texture2D texture;
        public Vector2 position;
        public Vector2 velocity;

        public bool isVisible = true;

        Random random = new Random();
        int randX, randY;

        public Enemies(Texture2D newTexture, Vector2 newPosition, bool randSpeed)
        {
            texture = newTexture;
            position = newPosition;

            randX = random.Next(-4,4);
            randY = random.Next(-4,-1);

            if (randSpeed)
                velocity = new Vector2(randX, randY);
            else
                velocity = new Vector2(1, 1);
        }

        public void Update(GraphicsDevice graphics)
        {
            position += velocity;

            if (position.X < 0 - texture.Width)
                isVisible = false;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }
    }
}
