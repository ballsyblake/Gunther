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
    class City : RC_GameStateParent
    {
        Sprite3 city = null;
        Sprite3 arrowHead = null;
        Sprite3 border = null;
        Sprite3 dialoguePaper = null;
        Sprite3 questionsPaper = null;
        //bool doneCheck = false;
        public static Vector2 currentLoc;
        int arrowHeadOffsetY = 5;
        int arrowJump = 100;
        int arrowCount = 0;
        bool mainScreen = true;
        bool shop = false;
        bool castle = false;
        bool pub = false;
        bool inChat = false;
        int maxArrowCount = 3;
        int currentPick;
        string dialogueForced = "";

        public override void LoadContent()
        {
            city = new Sprite3(true, Game1.texCityScreen, 0, 0);            
            city.setWidthHeight(800, 600);
            arrowHead = new Sprite3(true, Game1.texArrowHead, 600, 90 - arrowHeadOffsetY);
            arrowHead.setWidthHeight(40, 40);
            border = new Sprite3(true, Game1.texBorder, 5, 505);
            border.setWidthHeight(600, 90);
            dialoguePaper = new Sprite3(true, Game1.texPaper, -25, 500);
            dialoguePaper.setWidthHeight(635, 100);
            questionsPaper = new Sprite3(true, Game1.texPaper, 600, -50);
            questionsPaper.setWidthHeight(250, 570);

        }
        public override void Update(GameTime gameTime)
        {
            if (RC_GameStateParent.keyState.IsKeyDown(Keys.Down) && !RC_GameStateParent.prevKeyState.IsKeyDown(Keys.Down) && arrowCount < maxArrowCount)
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
            if (arrowCount > maxArrowCount)
                arrowCount = maxArrowCount;

            if (arrowCount < 0)
                arrowCount = 0;

            if (RC_GameStateParent.keyState.IsKeyDown(Keys.Enter) && RC_GameStateParent.prevKeyState.IsKeyUp(Keys.Enter)) // ***
            {
                Console.WriteLine(inChat);
                if (inChat)
                {
                    Console.WriteLine("inchat");
                    Console.WriteLine(arrowCount);
                    switch (arrowCount)
                    {
                        case 0:
                            if (!Game1.onQuest)
                            {
                                Game1.onQuest = true;
                                inChat = false;
                                maxArrowCount = 3;
                            }
                            else
                            {

                                inChat = false;
                                maxArrowCount = 3;
                            }
                            break;
                        case 1:
                            inChat = false;
                            maxArrowCount = 3;
                            break;
                        default:
                            inChat = false;
                            maxArrowCount = 3;
                            break;
                    }
                    
                }
                else if (mainScreen)
                {
                    switch (arrowCount)
                    {
                        case 0:
                            city.setTexture(Game1.texKing, true);
                            castle = true;
                            mainScreen = false;
                            break;
                        case 1:
                            city.setTexture(Game1.texTavern, true);
                            pub = true;
                            mainScreen = false;
                            break;
                        case 2:
                            city.setTexture(Game1.texWeaponsShop, true);
                            shop = true;
                            mainScreen = false;
                            break;
                        case 3:
                            gameStateManager.setLevel(3);
                            WorldMap.horse.setPos(WorldMap.horse.getPosX() - 30, WorldMap.horse.getPosX() - 30);
                            break;
                        default:
                            gameStateManager.setLevel(3);
                            WorldMap.horse.setPos(WorldMap.horse.getPosX() - 30, WorldMap.horse.getPosX() - 30);
                            break;
                    }
                }
                else if (castle)
                {
                    switch (arrowCount)
                    {
                        case 0:
                            inChat = true;
                            currentPick = 0;
                            break;
                        case 1:
                            inChat = true;
                            currentPick = 1;
                            break;
                        case 2:
                            currentPick = 2;
                            inChat = true;
                            break;
                        case 3:
                            mainScreen = true;
                            castle = false;
                            city.setTexture(Game1.texCityScreen, true);
                            break;
                        default:
                            mainScreen = true;
                            castle = false;
                            city.setTexture(Game1.texCityScreen, true);
                            break;
                    }
                }
                else if (pub)
                {
                    switch (arrowCount)
                    {
                        case 0:
                            if(Game1.gold > 5)
                                Game1.gold = Game1.gold - 5;
                            inChat = true;
                            break;
                        case 1:
                            inChat = true;
                            break;
                        case 2:
                            inChat = true;
                            break;
                        case 3:
                            mainScreen = true;
                            pub = false;
                            city.setTexture(Game1.texCityScreen, false);
                            break;
                        default:
                            mainScreen = true;
                            shop = false;
                            city.setTexture(Game1.texCityScreen, false);
                            break;
                    }
                }
                else if (shop)
                {
                    switch (arrowCount)
                    {
                        case 0:
                            if (Game1.gold > 10000)
                            {
                                Game1.gold = Game1.gold - 10000;
                                Game1.shieldBought = true;
                            }
                            inChat = true;
                            break;
                        case 1:
                            if (Game1.gold > 20000)
                            {
                                Game1.gold = Game1.gold - 20000;
                                Game1.upgradedBow = true;
                            }
                            inChat = true;
                            break;
                        case 2:
                            if (Game1.gold > 30000)
                            {
                                Game1.gold = Game1.gold - 30000;
                                Game1.horseArmorBought = true;
                            }
                            inChat = true;
                            break;
                        case 3:
                            mainScreen = true;
                            shop = false;
                            city.setTexture(Game1.texCityScreen, false);
                            break;
                        default:
                            mainScreen = true;
                            shop = false;
                            city.setTexture(Game1.texCityScreen, false);
                            break;
                    }
                }

                arrowCount = 0;
                arrowHead.setPosY(90);
            }
        }
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            city.Draw(spriteBatch);
            spriteBatch.DrawString(Game1.font,dialogueForced, new Vector2(20, 520), Color.Black);
            if (mainScreen)
            {
                city.setWidthHeight(800, 600);
                spriteBatch.DrawString(Game1.font, Game1.cities[currentLoc], new Vector2(100, 100), Color.Black);
                spriteBatch.DrawString(Game1.font, "Castle", new Vector2(650, 100), Color.Black);
                spriteBatch.DrawString(Game1.font, "Tavern", new Vector2(650, 200), Color.Black);
                spriteBatch.DrawString(Game1.font, "Shop", new Vector2(650, 300), Color.Black);
                spriteBatch.DrawString(Game1.font, "Leave", new Vector2(650, 400), Color.Black);
            }
            else if (castle)
            {
                graphicsDevice.Clear(Color.White);
                city.setWidthHeight(610, 510);
                questionsPaper.Draw(spriteBatch);
                dialoguePaper.Draw(spriteBatch);
                border.Draw(spriteBatch);
                
                if (!inChat)
                {
                    spriteBatch.DrawString(Game1.font, Game1.WrapText(Game1.font,"I am the king of this marvelous city. Please, tell me why you have come?", 480), new Vector2(20, 520), Color.Black);
                    spriteBatch.DrawString(Game1.font, "All hail the King", new Vector2(100, 100), Color.Black);
                    spriteBatch.DrawString(Game1.font, "Offer fealty", new Vector2(650, 100), Color.Black);
                    spriteBatch.DrawString(Game1.font, "Ask for jobs", new Vector2(650, 200), Color.Black);
                    spriteBatch.DrawString(Game1.font, "Ask about father", new Vector2(650, 300), Color.Black);
                    spriteBatch.DrawString(Game1.font, "Leave", new Vector2(650, 400), Color.Black);
                }
                else if (inChat)
                {
                    
                    switch (currentPick)
                    {
                        case 0:
                            maxArrowCount = 0;
                            spriteBatch.DrawString(Game1.font, Game1.WrapText(Game1.font, Game1.dialogueList["kingallegianceno"], 480), new Vector2(20, 520), Color.Black);
                            break;
                        case 1:
                            maxArrowCount = 1;
                            spriteBatch.DrawString(Game1.font, "Yes", new Vector2(650, 100), Color.Black);
                            spriteBatch.DrawString(Game1.font, "No", new Vector2(650, 200), Color.Black);
                            spriteBatch.DrawString(Game1.font, Game1.WrapText(Game1.font, Game1.dialogueList["kingquest1"],480), new Vector2(20, 520), Color.Black);
                            
                            break;
                        case 2:
                            maxArrowCount = 0;
                            spriteBatch.DrawString(Game1.font, Game1.WrapText(Game1.font, Game1.dialogueList["kingfatherno"],480), new Vector2(20, 520), Color.Black);
                            break;
                        default:
                            break;
                    }
                    spriteBatch.DrawString(Game1.font, "All hail the King", new Vector2(100, 100), Color.Black);
                    
                }
                    
            }
            else if (pub)
            {
                graphicsDevice.Clear(Color.White);
                city.setWidthHeight(610, 510);
                questionsPaper.Draw(spriteBatch);
                dialoguePaper.Draw(spriteBatch);
                border.Draw(spriteBatch);
                if (!inChat)
                {
                    spriteBatch.DrawString(Game1.font, "Come, sit. Drink away your worldly problems with some nice cold beer.", new Vector2(20, 520), Color.Black);
                    spriteBatch.DrawString(Game1.font, "Welcome to the tavern", new Vector2(100, 100), Color.Black);
                    spriteBatch.DrawString(Game1.font, "Buy some beer", new Vector2(650, 100), Color.Black);
                    spriteBatch.DrawString(Game1.font, "Ask about jobs", new Vector2(650, 200), Color.Black);
                    spriteBatch.DrawString(Game1.font, "Ask about nearby raiders", new Vector2(650, 300), Color.Black);
                    spriteBatch.DrawString(Game1.font, "Leave", new Vector2(650, 400), Color.Black);
                }
                else if (inChat)
                {
                    switch (currentPick)
                    {
                        case 0:
                            if(Game1.gold > 5)
                            {
                                spriteBatch.DrawString(Game1.font, "Here you go", new Vector2(20, 520), Color.Black);
                                
                            }
                            else
                                spriteBatch.DrawString(Game1.font, "Sorry buddy but you are gonna need more gold than that. Come back when you have at least 5 gold pieces.", new Vector2(20, 520), Color.Black);
                            break;
                        case 1:
                            spriteBatch.DrawString(Game1.font, "Yes", new Vector2(650, 100), Color.Black);
                            spriteBatch.DrawString(Game1.font, "No", new Vector2(650, 200), Color.Black);
                            spriteBatch.DrawString(Game1.font, Game1.WrapText(Game1.font, Game1.dialogueList["pubQuest1"],480), new Vector2(20, 520), Color.Black);
                            
                            break;
                        case 2:
                            spriteBatch.DrawString(Game1.font, "I will mark them on your map for you. ", new Vector2(20, 520), Color.Black);
                            break;
                        default:
                            break;
                    }
                    spriteBatch.DrawString(Game1.font, "Welcome to the tavern", new Vector2(100, 100), Color.Black);

                }

            }
            else if (shop)
            {
                graphicsDevice.Clear(Color.White);
                city.setWidthHeight(610, 510);
                questionsPaper.Draw(spriteBatch);
                dialoguePaper.Draw(spriteBatch);
                border.Draw(spriteBatch);
                if (!inChat)
                {
                    spriteBatch.DrawString(Game1.font, "Welcome to the shop, what can I do for you?", new Vector2(20, 520), Color.Black);
                    spriteBatch.DrawString(Game1.font, "Weapons and Armor Shop", new Vector2(100, 100), Color.Black);
                    spriteBatch.DrawString(Game1.font, "Buy Shield", new Vector2(650, 100), Color.Black);
                    spriteBatch.DrawString(Game1.font, "Upgrade Bow", new Vector2(650, 200), Color.Black);
                    spriteBatch.DrawString(Game1.font, "Buy Horse Armor", new Vector2(650, 300), Color.Black);
                    spriteBatch.DrawString(Game1.font, "Leave", new Vector2(650, 400), Color.Black);
                }
                else if (inChat)
                {
                    Console.WriteLine(Game1.shieldBought);
                    switch (currentPick)
                    {
                        case 0:
                            if (!Game1.shieldBought)
                            {
                                if (Game1.gold > 10000)
                                {
                                    spriteBatch.DrawString(Game1.font, "Enjoy the brand new shield. Don't forget to click tab to use it.", new Vector2(20, 520), Color.Black);
                                    
                                }
                                else
                                    spriteBatch.DrawString(Game1.font, Game1.WrapText(Game1.font, "Sorry buddy but you are gonna need more gold than that. Come back when you have at least 10,000 gold pieces.", 480), new Vector2(20, 520), Color.Black);
                            }
                            else
                                spriteBatch.DrawString(Game1.font, "You already own that.", new Vector2(20, 520), Color.Black);
                            break;
                        case 1:
                            if (!Game1.upgradedBow)
                            {
                                if (Game1.gold > 20000)
                                {
                                    spriteBatch.DrawString(Game1.font, "Enjoy the brand new shield. Don't forget to click 'v' to use it.", new Vector2(20, 520), Color.Black);
                                    
                                }
                                else
                                    spriteBatch.DrawString(Game1.font, Game1.WrapText(Game1.font, "Sorry buddy but you are gonna need more gold than that. Come back when you have at least 20,000 gold pieces.", 480), new Vector2(20, 520), Color.Black);
                            }
                            else
                                spriteBatch.DrawString(Game1.font, "You already own that.", new Vector2(20, 520), Color.Black);
                            break;
                        case 2:
                            if (!Game1.horseArmorBought)
                            {
                                if (Game1.gold > 30000)
                                {
                                    spriteBatch.DrawString(Game1.font, "Enjoy the brand new shield. Don't forget to click v to use it.", new Vector2(20, 520), Color.Black);
                                    
                                }
                                else
                                    spriteBatch.DrawString(Game1.font, Game1.WrapText(Game1.font,"Sorry buddy but you are gonna need more gold than that. Come back when you have at least 30,000 gold pieces.",480), new Vector2(20, 520), Color.Black);
                            }
                            else
                                spriteBatch.DrawString(Game1.font, "You already own that.", new Vector2(20, 520), Color.Black);
                            break;
                        default:
                            break;
                    }
                    spriteBatch.DrawString(Game1.font, "Weapons and Armor Shop", new Vector2(100, 100), Color.Black);

                }

            }
            arrowHead.Draw(spriteBatch);
            PlayLevel.goldBanner.Draw(spriteBatch);
            spriteBatch.DrawString(Game1.font, "Gold: " + Game1.gold, new Vector2(30, 20), Color.Black);
            spriteBatch.End();
        }

        public static void CurrentLocation(Vector2 loc)
        {
            currentLoc = loc;
        }

        //Some functions for calculating unassigned integers to and from XNA Color
        private Color UIntToColor(uint color)
        {
            byte a = (byte)(color >> 24);
            byte b = (byte)(color >> 16);
            byte g = (byte)(color >> 8);
            byte r = (byte)(color >> 0);
            return new Color(r, g, b, a);
        }
        private uint ColorToUInt(Color color)
        {
            return (uint)((color.A << 24) | (color.B << 16) |
                          (color.G << 8) | (color.R << 0));
        }

        private uint ColorToUIntBlake(Color color)
        {
            byte[] colorBytes = new byte[4];
            colorBytes[3] = color.A;
            colorBytes[2] = color.R;
            colorBytes[1] = color.G;
            colorBytes[0] = color.B;
            return BitConverter.ToUInt32(colorBytes, 0);
             
        }

        
    }
}
