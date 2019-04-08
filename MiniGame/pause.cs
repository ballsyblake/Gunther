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
    class Pause : RC_GameStateParent
    {
        ImageBackground pause1 = null;
        ColorField trans = null;

        public override void LoadContent()
        {
            pause1 = new ImageBackground(Game1.texBook, Color.White, graphicsDevice);
            pause1.setPos(100, 100);
            pause1.setWidthHeight(600, 400);
            trans = new ColorField(new Color(255, 255, 255, 100), new Rectangle(0, 0, 800, 600));
        }
        public override void Update(GameTime gameTime)
        {
            //Console.WriteLine("this works");
            if (RC_GameStateParent.keyState.IsKeyDown(Keys.Enter) && RC_GameStateParent.prevKeyState.IsKeyUp(Keys.Enter)) // ***
            {
                //Console.WriteLine("paused");
                Game1.levelManager.popLevel();
            }

        }
        public override void Draw(GameTime gameTime)
        {
            Game1.levelManager.prevStatePlayLevel.Draw(gameTime);

            //spriteBatch.Begin();  // depending on version you may need this
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);

            trans.Draw(spriteBatch);
            pause1.Draw(spriteBatch);
            spriteBatch.End();

        }


    }
}
