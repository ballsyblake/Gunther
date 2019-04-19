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
        Sprite3 arrowHead = null;
        ColorField trans = null;

        int arrowHeadOffsetY = 5;
        int arrowJump = 100;
        int arrowCount = 0;

        public override void LoadContent()
        {
            pause1 = new ImageBackground(Game1.texPaper, Color.White, graphicsDevice);
            pause1.setPos(100, 100);
            pause1.setWidthHeight(600, 400);
            trans = new ColorField(new Color(255, 255, 255, 100), new Rectangle(0, 0, 800, 600));
            arrowHead = new Sprite3(true, Game1.texArrowHead, 250, 200 - arrowHeadOffsetY);
            arrowHead.setWidthHeight(40, 40);
        }
        public override void Update(GameTime gameTime)
        {
            if (RC_GameStateParent.keyState.IsKeyDown(Keys.Down) && !RC_GameStateParent.prevKeyState.IsKeyDown(Keys.Down) && arrowCount < 1)
            {
                Game1.soundEffects[3].Play(0.5f, 0, 0);
                arrowHead.setPosY(arrowHead.getPosY() + arrowJump);
                arrowCount++;
            }
            if (RC_GameStateParent.keyState.IsKeyDown(Keys.Up) && !RC_GameStateParent.prevKeyState.IsKeyDown(Keys.Up) && arrowCount > 0)
            {
                Game1.soundEffects[3].Play(0.5f, 0, 0);
                arrowHead.setPosY(arrowHead.getPosY() - arrowJump);
                arrowCount--;
            }
            if (arrowCount > 1)
                arrowCount = 1;

            if (arrowCount < 0)
                arrowCount = 0;

            if (RC_GameStateParent.keyState.IsKeyDown(Keys.Enter) && RC_GameStateParent.prevKeyState.IsKeyUp(Keys.Enter)) // ***
            {
                if(arrowCount == 0)
                    Game1.levelManager.popLevel();
                else if(arrowCount == 1)
                    Game1.endGame = true;

            }

        }
        public override void Draw(GameTime gameTime)
        {
            Game1.levelManager.prevStatePlayLevel.Draw(gameTime);

            //spriteBatch.Begin();  // depending on version you may need this
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
            trans.Draw(spriteBatch);
            pause1.Draw(spriteBatch);
            spriteBatch.DrawString(Game1.font, "Continue", new Vector2(300, 200), Color.Black);
            spriteBatch.DrawString(Game1.font, "Exit Game", new Vector2(300, 300), Color.Black);
            arrowHead.Draw(spriteBatch);
            spriteBatch.End();

        }


    }
}
