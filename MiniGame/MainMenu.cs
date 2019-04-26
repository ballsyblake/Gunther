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
using Microsoft.Xna.Framework.Audio;

namespace MiniGame
{
    class MainMenu : RC_GameStateParent
    {
        Sprite3 table = null;
        Sprite3 startBanner = null;
        Sprite3 arrowHead = null;
        Sprite3 controls = null;
        Sprite3 fadeBlack = null;
        SpriteFont startGame = null;
        SpriteFont difficulty = null;
        SpriteFont controlsText = null;
        SpriteFont endGame = null;
        
        Rectangle menuArea;
        Rectangle menuArea2;

        int arrowHeadOffsetY = 5;
        int arrowJump = 100;
        int arrowCount = 0;

        bool showStory = true;
        bool showDifficulty = false;
        bool showControls = false;
        bool changeScene = false;
        float sceneTicks = 0;

        SoundEffectInstance instanceMusic = Game1.music.CreateInstance();

        string story = "Welcome to a video game created by Blake Baldwin. This game is inspired from another game called Mount and Blade. This game is brutal. Losing a battle results in severe punishments so be careful. But please do enjoy playing.";

        public override void LoadContent()
        {
            menuArea = new Rectangle(150, 80, 200, 400);
            menuArea2 = new Rectangle(410, 80, 250, 400);
            table = new Sprite3(true, Game1.texWood, 0, 0);
            table.setWidthHeight(800, 600);
            startBanner = new Sprite3(true, Game1.texOpenBook, 50, 36);
            startBanner.setWidthHeight(700, 525);
            arrowHead = new Sprite3(true, Game1.texArrowHead, 150, 120 - arrowHeadOffsetY);
            controls = new Sprite3(true, Game1.texControls, 500, 200);
            controls.setWidthHeight(150,200);
            arrowHead.setWidthHeight(40, 40);
            instanceMusic.IsLooped = false;

            fadeBlack = new Sprite3(false, Game1.texBlackSquare, 0, 0);
            fadeBlack.setColor(Color.Transparent);
            fadeBlack.setWidthHeight(800, 600);
            fadeBlack.setFadeDetails(true, Color.Transparent, Color.Black, 100, false);

            startGame = Content.Load<SpriteFont>("MedievalFont");
            difficulty = Content.Load<SpriteFont>("MedievalFont");
            controlsText = Content.Load<SpriteFont>("MedievalFont");
            endGame = Content.Load<SpriteFont>("MedievalFont");
            

        }
        public override void Update(GameTime gameTime)
        {
            if (changeScene)
            {
                fadeBlack.setActive(true);
                sceneTicks += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                fadeBlack.doTheFade();
            }
            if(sceneTicks > 2000)
            {
                instanceMusic.Stop();
                gameStateManager.setLevel(6);
                sceneTicks = 0;
                changeScene = false;
                fadeBlack.setActive(false);
            }
            if (gameStateManager.getCurrentLevelNum() == 4)
                instanceMusic.Play();
            if (RC_GameStateParent.keyState.IsKeyDown(Keys.Down) && !RC_GameStateParent.prevKeyState.IsKeyDown(Keys.Down) && arrowCount < 3)
            {
                Game1.soundEffects[3].Play(0.5f, 0, 0);
                arrowHead.setPosY(arrowHead.getPosY() + arrowJump);
                arrowCount++;
            }
            if (RC_GameStateParent.keyState.IsKeyDown(Keys.Up) && !RC_GameStateParent.prevKeyState.IsKeyDown(Keys.Up) && arrowCount > 0)
            {
                Game1.soundEffects[3].Play(0.5f,0,0);
                arrowHead.setPosY(arrowHead.getPosY() - arrowJump);
                arrowCount--;
            }

            if (arrowCount > 3)
                arrowCount = 3;

            if (arrowCount < 0)
                arrowCount = 0;

            if (RC_GameStateParent.keyState.IsKeyDown(Keys.Enter) && !RC_GameStateParent.prevKeyState.IsKeyDown(Keys.Enter) && !showDifficulty)
            {
                
                switch (arrowCount)
                {
                    case 0:
                        //Game1.levelManager.setLevel(3);
                        
                        changeScene = true;
                        fadeBlack.setVisible(true);
                        break;
                    case 1:
                        arrowHead.setPosX(410);
                        showDifficulty = true;
                        showControls = false;
                        showStory = false;
                        break;
                    case 2:
                        showDifficulty = false;
                        showControls = true;
                        showStory = false;
                        break;
                    case 3:
                        Game1.endGame = true;
                        break;
                    default:
                        Game1.levelManager.setLevel(3);
                        break;
                }
            }
            else if (RC_GameStateParent.keyState.IsKeyDown(Keys.Enter) && !RC_GameStateParent.prevKeyState.IsKeyDown(Keys.Enter) && showDifficulty)
            {
                switch (arrowCount)
                {
                    case 0:
                        Game1.difficulty = 1;
                        break;
                    case 1:
                        Game1.difficulty = 2;
                        break;
                    case 2:
                        Game1.difficulty = 3;
                        break;
                    case 3:
                        //Nothing
                        break;
                    default:
                        //Nothing
                        break;
                }
                showDifficulty = false;
                showControls = false;
                showStory = true;
                arrowHead.setPosX(150);
                Console.WriteLine(Game1.difficulty);
            }
        }
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            table.Draw(spriteBatch);
            startBanner.Draw(spriteBatch);
            
            arrowHead.Draw(spriteBatch);
            spriteBatch.DrawString(startGame, "Gunthers Problematic Tale", new Vector2(420, 80), Color.Black);
            spriteBatch.DrawString(startGame, "Start Game", new Vector2(200, 120), Color.Black);
            spriteBatch.DrawString(difficulty, "Select Difficulty", new Vector2(200, 220), Color.Black);
            spriteBatch.DrawString(controlsText, "Controls", new Vector2(200, 320), Color.Black);
            spriteBatch.DrawString(endGame, "Leave Game", new Vector2(200, 420), Color.Black);
            
            if (showStory)
                spriteBatch.DrawString(startGame, WrapText(startGame, story, 340f), new Vector2(420, 120), Color.Black, 0f, new Vector2(0, 0), 0.70f, SpriteEffects.None, 0f);
            else if (showDifficulty)
            {
                spriteBatch.DrawString(startGame, "Easy", new Vector2(460, 120), Color.Black);
                spriteBatch.DrawString(difficulty, "Average", new Vector2(460, 220), Color.Black);
                spriteBatch.DrawString(controlsText, "Hard", new Vector2(460, 320), Color.Black);
                spriteBatch.DrawString(endGame, "Back", new Vector2(460, 420), Color.Black);
            }
            else if (showControls)
            {
                controls.Draw(spriteBatch);
                spriteBatch.DrawString(endGame, "Movement", new Vector2(420, 220), Color.Black);
                spriteBatch.DrawString(endGame, "Interact", new Vector2(420, 300), Color.Black);
                spriteBatch.DrawString(endGame, "Attack", new Vector2(420, 360), Color.Black);
            }
            fadeBlack.Draw(spriteBatch);
            spriteBatch.End();
        }

        public string WrapText(SpriteFont spriteFont, string text, float maxLineWidth)
        {
            string[] words = text.Split(' ');
            StringBuilder sb = new StringBuilder();
            float lineWidth = 0f;
            float spaceWidth = spriteFont.MeasureString(" ").X;

            foreach (string word in words)
            {
                Vector2 size = spriteFont.MeasureString(word);

                if (lineWidth + size.X < maxLineWidth)
                {
                    sb.Append(word + " ");
                    lineWidth += size.X + spaceWidth;
                }
                else
                {
                    sb.Append("\n" + word + " ");
                    lineWidth = size.X + spaceWidth;
                }
            }

            return sb.ToString();
        }
    }
}
