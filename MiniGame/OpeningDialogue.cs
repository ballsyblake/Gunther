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
    class OpeningDialogue : RC_GameStateParent
    {
        Sprite3 blackSquare = null;
        Sprite3 gunther = null;
        Sprite3 background = null;
        Vector2 dialoguePosition = new Vector2(330, 20);
        int dialogueTick = 1;
        float timer = 0;
        public override void LoadContent()
        {
            gunther = new Sprite3(true, Game1.texGunther, 20, 20);
            gunther.setWidthHeight(300, 560);
            background = new Sprite3(true, Game1.texPaper, -25, -25);
            background.setWidthHeight(850, 650);
            blackSquare = new Sprite3(true, Game1.texBlackSquare, 0, 0);
            blackSquare.setWidthHeight(800, 600);
            blackSquare.setFadeDetails(true, Color.Black, Color.Transparent, 100, false);
        }
        public override void Update(GameTime gameTime)
        {
            //timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            blackSquare.doTheFade();
            if (RC_GameStateParent.keyState.IsKeyDown(Keys.Enter) && !RC_GameStateParent.prevKeyState.IsKeyDown(Keys.Enter))
                dialogueTick++;
            if (dialogueTick > 3)
            {
                gameStateManager.setLevel(0);
                PlayLevel.LoadLevelDetails(10,-1);
                dialogueTick = 1;
                //blackSquare.setActive(false);
            }
                
        }
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            background.Draw(spriteBatch);
            gunther.Draw(spriteBatch);
            if (dialogueTick < 4)
            {
                spriteBatch.DrawString(Game1.font, WrapText(Game1.font, Game1.dialogueList["opening" + dialogueTick.ToString()], 450), dialoguePosition, Color.Black);
                spriteBatch.DrawString(Game1.font, "Press enter to continue...", new Vector2(330, 560), Color.Black);
            }
            blackSquare.Draw(spriteBatch);
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
