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
        Sprite3 table = null;
        Sprite3 startBanner = null;
        Sprite3 arrowHead = null;
        SpriteFont startGame = null;
        SpriteFont difficulty = null;
        SpriteFont controls = null;
        SpriteFont endGame = null;
        Rectangle menuArea;

        int arrowHeadOffsetY = 5;
        int arrowJump = 40;
        int arrowCount = 0;

        public override void LoadContent()
        {
            menuArea = new Rectangle(150, 80, 200, 400);
            table = new Sprite3(true, Game1.texWood, 0, 0);
            table.setWidthHeight(800, 600);
            startBanner = new Sprite3(true, Game1.texOpenBook, 50, 36);
            startBanner.setWidthHeight(700, 525);
            arrowHead = new Sprite3(true, Game1.texArrowHead, 300, 320 - arrowHeadOffsetY);
            arrowHead.setFlip(SpriteEffects.FlipHorizontally);
            arrowHead.setWidthHeight(40, 40);
            
            startGame = Content.Load<SpriteFont>("MedievalFont");
            difficulty = Content.Load<SpriteFont>("MedievalFont");
            controls = Content.Load<SpriteFont>("MedievalFont");
            endGame = Content.Load<SpriteFont>("MedievalFont");
        }
        public override void Update(GameTime gameTime)
        {
            if (RC_GameStateParent.keyState.IsKeyDown(Keys.Down) && !RC_GameStateParent.prevKeyState.IsKeyDown(Keys.Down) && arrowCount < 3)
            {
                arrowHead.setPosY(arrowHead.getPosY() + arrowJump);
                arrowCount++;
            }
            if (RC_GameStateParent.keyState.IsKeyDown(Keys.Up) && !RC_GameStateParent.prevKeyState.IsKeyDown(Keys.Up) && arrowCount > 0)
            {
                arrowHead.setPosY(arrowHead.getPosY() - arrowJump);
                arrowCount--;
            }

            if (arrowCount > 3)
                arrowCount = 3;

            if (arrowCount < 0)
                arrowCount = 0;

            if (RC_GameStateParent.keyState.IsKeyDown(Keys.Enter))
            {
                switch (arrowCount)
                {
                    case 0:
                        Game1.levelManager.setLevel(3);
                        break;
                    case 1:
                        //difficulty options
                        break;
                    case 2:
                        //display controls
                        break;
                    case 3:
                        Game1.endGame = true;
                        break;
                    default:
                        Game1.levelManager.setLevel(3);
                        break;
                }
            }

            /*if (RC_GameStateParent.keyState.IsKeyDown(Keys.D1) || RC_GameStateParent.keyState.IsKeyDown(Keys.NumPad1))
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
            }*/

            //Begin all game functionality essentially
            if (RC_GameStateParent.keyState.IsKeyDown(Keys.Enter))
            {
               //Level manager stuff               
            }
        }
        public override void Draw(GameTime gameTime)
        {
            
            spriteBatch.Begin();
            table.Draw(spriteBatch);
            startBanner.Draw(spriteBatch);
            LineBatch.drawLineRectangle(spriteBatch, menuArea, Color.Blue);
            arrowHead.Draw(spriteBatch);
            spriteBatch.DrawString(startGame, "Gunthers Problematic Tale", new Vector2(150, 80), Color.Black);
            spriteBatch.DrawString(startGame, "Start Game", new Vector2(150, 320), Color.Black);
            spriteBatch.DrawString(difficulty, "Select Difficulty", new Vector2(150, 360), Color.Black);
            spriteBatch.DrawString(controls, "Controls", new Vector2(150, 400), Color.Black);
            spriteBatch.DrawString(endGame, "Leave Game", new Vector2(150, 440), Color.Black);
            /*switch (Game1.difficulty)
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
            }*/
            //spriteBatch.DrawString(Game1.difficultySelectText, "Click 1, 2 or 3 to assign difficulty. 1 = easy | 2 = medium | 3 = hard" + Environment.NewLine, new Vector2(10, 560), Color.Black);
            spriteBatch.End();
        }
    }
}
