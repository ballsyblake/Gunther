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
    class MainMenu : RC_GameStateParent
    {
        Sprite3 startBanner = null;
        public override void LoadContent()
        {
            startBanner = new Sprite3(true, Game1.texStartBanner, 0, 0);
            startBanner.setWidthHeight(800, 600);
            
        }
        public override void Update(GameTime gameTime)
        {
            RC_GameStateParent.prevKeyState = RC_GameStateParent.keyState;
            RC_GameStateParent.keyState = Keyboard.GetState();

            if (RC_GameStateParent.keyState.IsKeyDown(Keys.D1) || RC_GameStateParent.keyState.IsKeyDown(Keys.NumPad1))
            {
                Game1.difficulty = 1;
            }
            else if (RC_GameStateParent.keyState.IsKeyDown(Keys.D2) || RC_GameStateParent.keyState.IsKeyDown(Keys.NumPad2))
            {
                Game1.difficulty = 2;
            }
            else if (RC_GameStateParent.keyState.IsKeyDown(Keys.D3) || RC_GameStateParent.keyState.IsKeyDown(Keys.NumPad3))
            {
                Game1.difficulty = 3;
            }
            //Begin all game functionality essentially
            if (RC_GameStateParent.keyState.IsKeyDown(Keys.Enter))
            {
               //Level manager stuff               
            }
        }
        public override void Draw(GameTime gameTime)
        {
            startBanner.Draw(spriteBatch);
            switch (Game1.difficulty)
            {
                case 1:
                    spriteBatch.DrawString(Game1.difficultySelectText, "Current difficulty: Easy", new Vector2(10, 500), Color.Black);
                    break;
                case 2:
                    spriteBatch.DrawString(Game1.difficultySelectText, "Current difficulty: Medium", new Vector2(10, 500), Color.Black);
                    break;
                case 3:
                    spriteBatch.DrawString(Game1.difficultySelectText, "Current difficulty: Hard", new Vector2(10, 500), Color.Black);
                    break;
                default:
                    spriteBatch.DrawString(Game1.difficultySelectText, "Current difficulty: Easy", new Vector2(10, 500), Color.Black);
                    break;
            }
            spriteBatch.DrawString(Game1.difficultySelectText, "Click 1, 2 or 3 to assign difficulty. 1 = easy | 2 = medium | 3 = hard" + Environment.NewLine, new Vector2(10, 560), Color.Black);

        }
    }
}
