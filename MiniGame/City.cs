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

        bool doneCheck = false;
        private static Vector2 currentLoc;
        int arrowHeadOffsetY = 5;
        int arrowJump = 100;
        int arrowCount = 0;
        bool mainScreen = true;
        bool shop = false;
        bool castle = false;
        bool pub = false;

        public override void LoadContent()
        {
            city = new Sprite3(true, Game1.texCityScreen, 0, 0);            
            city.setWidthHeight(800, 600);
            arrowHead = new Sprite3(true, Game1.texArrowHead, 600, 200 - arrowHeadOffsetY);
            arrowHead.setWidthHeight(40, 40);

        }
        public override void Update(GameTime gameTime)
        {
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

            if (RC_GameStateParent.keyState.IsKeyDown(Keys.Enter) && RC_GameStateParent.prevKeyState.IsKeyUp(Keys.Enter)) // ***
            {
                if (mainScreen)
                {
                    switch (arrowCount)
                    {
                        case 0:
                            city.setTexture(Game1.texKing, true);
                            castle = true;
                            mainScreen = false;
                            break;
                        case 1:
                            city.setTexture(Game1.texTavern, false);
                            pub = true;
                            mainScreen = false;
                            break;
                        case 2:
                            city.setTexture(Game1.texWeaponsShop, false);
                            shop = true;
                            mainScreen = false;
                            break;
                        case 3:
                            gameStateManager.setLevel(3);
                            break;
                        default:
                            gameStateManager.setLevel(3);
                            break;
                    }
                }
                else if (castle)
                {
                    switch (arrowCount)
                    {
                        case 0:
                            //chat stuff
                            break;
                        case 1:
                            //chat stuff
                            break;
                        case 2:
                            //chat stuff
                            break;
                        case 3:
                            mainScreen = true;
                            castle = false;
                            city.setTexture(Game1.texCityScreen, false);
                            break;
                        default:
                            mainScreen = true;
                            castle = false;
                            city.setTexture(Game1.texCityScreen, false);
                            break;
                    }
                }
                else if (pub)
                {
                    switch (arrowCount)
                    {
                        case 0:
                            //chat stuff
                            break;
                        case 1:
                            //chat stuff
                            break;
                        case 2:
                            //chat stuff
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
                            //chat stuff
                            break;
                        case 1:
                            //chat stuff
                            break;
                        case 2:
                            //chat stuff
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
            }
        }
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            city.Draw(spriteBatch);
            arrowHead.Draw(spriteBatch);
            if (mainScreen)
            {
                spriteBatch.DrawString(Game1.font, Game1.cities[currentLoc], new Vector2(100, 100), Color.Black);
                spriteBatch.DrawString(Game1.font, "Castle", new Vector2(650, 200), Color.Black);
                spriteBatch.DrawString(Game1.font, "Tavern", new Vector2(650, 300), Color.Black);
                spriteBatch.DrawString(Game1.font, "Shop", new Vector2(650, 400), Color.Black);
                spriteBatch.DrawString(Game1.font, "Leave", new Vector2(650, 500), Color.Black);
            }
            else if (castle)
            {
                graphicsDevice.Clear(Color.White);
                spriteBatch.DrawString(Game1.font, "All hail the King", new Vector2(100, 100), Color.Black);
                spriteBatch.DrawString(Game1.font, "Offer fealty", new Vector2(650, 200), Color.Black);
                spriteBatch.DrawString(Game1.font, "Ask for jobs", new Vector2(650, 300), Color.Black);
                spriteBatch.DrawString(Game1.font, "Ask about father", new Vector2(650, 400), Color.Black);
                spriteBatch.DrawString(Game1.font, "Leave", new Vector2(650, 500), Color.Black);
            }
            else if (pub)
            {
                graphicsDevice.Clear(Color.White);
                spriteBatch.DrawString(Game1.font, "Welcome to the tavern", new Vector2(100, 100), Color.Black);
                spriteBatch.DrawString(Game1.font, "Buy some beer", new Vector2(650, 200), Color.Black);
                spriteBatch.DrawString(Game1.font, "Ask about jobs", new Vector2(650, 300), Color.Black);
                spriteBatch.DrawString(Game1.font, "Ask about nearby raiders", new Vector2(650, 400), Color.Black);
                spriteBatch.DrawString(Game1.font, "Leave", new Vector2(650, 500), Color.Black);
            }
            else if (shop)
            {
                graphicsDevice.Clear(Color.White);
                spriteBatch.DrawString(Game1.font, "Weapons and Armor Shop", new Vector2(100, 100), Color.Black);
                spriteBatch.DrawString(Game1.font, "Buy Shield", new Vector2(650, 200), Color.Black);
                spriteBatch.DrawString(Game1.font, "Upgrade Bow", new Vector2(650, 300), Color.Black);
                spriteBatch.DrawString(Game1.font, "Buy Horse Armor", new Vector2(650, 400), Color.Black);
                spriteBatch.DrawString(Game1.font, "Leave", new Vector2(650, 500), Color.Black);
            }
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
