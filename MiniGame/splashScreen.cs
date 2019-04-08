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
    class SplashScreen : RC_GameStateParent
    {
        static public Texture2D texSplash = null;
        int timerTicks = 50;
        public override void LoadContent()
        {
            texSplash = Util.texFromFile(graphicsDevice, Game1.dir + "BBLogoSmall.png");
        }

        public override void Update(GameTime gameTime)
        {
            timerTicks--;
            if (timerTicks <= 0)
            {
                Game1.levelManager.setLevel(4);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            graphicsDevice.Clear(Color.Gray);
            ImageBackground back2 = null;
            back2 = new ImageBackground(texSplash, Color.White, graphicsDevice);
            back2.setPos(220, 130);
            back2.setWidthHeight(300, 297);
            spriteBatch.Begin();
            back2.Draw(spriteBatch);
            spriteBatch.End();

        }
    }
}
