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
    class Dialogue : RC_GameStateParent
    {
        Sprite3 background = null;
        Sprite3 portrait = null;
        Vector2 portraitLoc = new Vector2(0, 0);
        Vector2 dialogueLoc = new Vector2(20, 300);
        Vector2 answersLoc = new Vector2(400, 0);
        Sprite3 arrowHead = null;
        int arrowJump = 100;
        int arrowCount = 0;
        string button1;
        string button2;
        string button3;
        string button4;
        public static string dialogue;
        public static bool buttonPressed = false;
        bool inChat = false;
        string currentDialogue;
        int currentNum;
        

        public static string dialogueType;
        public static int portraitType = 0; // 0 = enemy, 1 = king, 2 = bartender, 3 = shop owner
        //int buttonNum;
        int chatCount = 1;
        public override void LoadContent()
        {
            background = new Sprite3(true, Game1.texPaper, 0, 0);
            background.setWidthHeight(800, 600);
            arrowHead = new Sprite3(true, Game1.texArrowHead, answersLoc.X, answersLoc.Y);

        }
        public override void Update(GameTime gameTime)
        {
            if(buttonPressed && !inChat)
                switch (portraitType)
                {
                    case 0:

                        break;
                    case 1:
                        button1 = "Ask About Father";
                        button2 = "Ask for a Job";
                        button3 = "Offer Allegiance";
                    
                        switch (arrowCount)
                        {
                            case 0:
                                if (Game1.cities[City.currentLoc] == "Pandia")
                                    dialogue = Game1.dialogueList["kingfatheryes"];
                                else
                                    dialogue = Game1.dialogueList["kingfatherno"];

                                break;
                            case 1:
                                if (!Game1.onQuest)
                                {
                                    inChat = true;
                                    currentDialogue = "kingquest";
                                    currentNum = Game1.random.Next(1, 3);
                                    dialogue = Game1.dialogueList["kingquest"];
                                }
                                    
                                else
                                    dialogue = Game1.dialogueList["kingquestno"];
                                break;
                            case 2:

                                break;
                            case 3:

                                break;
                            default:

                                break;
                        }
                        break;
                    default:
                        break;
                }
            if(!inChat)
                buttonPressed = false;
            if (inChat)
            {

                if(RC_GameStateParent.keyState.IsKeyDown(Keys.Enter) && !RC_GameStateParent.prevKeyState.IsKeyDown(Keys.Enter))
                {
                    dialogue = Game1.dialogueList[currentDialogue + currentNum.ToString()];
                }
            }
            
            if (RC_GameStateParent.keyState.IsKeyDown(Keys.Down) && !RC_GameStateParent.prevKeyState.IsKeyDown(Keys.Down) && arrowCount < 3)
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

            if (arrowCount > 3)
                arrowCount = 3;

            if (arrowCount < 0)
                arrowCount = 0;

            if (RC_GameStateParent.keyState.IsKeyDown(Keys.Enter) && !RC_GameStateParent.prevKeyState.IsKeyDown(Keys.Enter) && !inChat)
            {
                buttonPressed = true;
                switch (arrowCount)
                {
                    case 0:
                        if(Game1.cities[City.currentLoc] == "Pandia")
                            dialogue = Game1.dialogueList["kingfatheryes"];
                        else
                            dialogue = Game1.dialogueList["kingfatherno"];

                        break;
                    case 1:
                        
                        break;
                    case 2:

                        break;
                    case 3:
                        
                        break;
                    default:
                        
                        break;
                }
            }
        }
        public override void Draw(GameTime gameTime)
        {
            background.Draw(spriteBatch);
            portrait.Draw(spriteBatch);
            spriteBatch.DrawString(Game1.font,dialogue,dialogueLoc,Color.Black);
        }

        public static void LoadDialogueDetails(string person, int spriteNum)
        {
            buttonPressed = true;
            dialogue = Game1.dialogueList[person];
            portraitType = spriteNum;
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
