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
    class WorldMap : RC_GameStateParent
    {
        Sprite3 worldMap = null;
        Sprite3 points = null;
        Sprite3 water = null;
        Camera mainCamera;
        private Vector2 curPos;
        Sprite3 horse = null;
        int movementSpeed = 10;
        int top = 360;
        int bot = 2600;
        int left = 450;
        int right = 3490;

        public override void LoadContent()
        {
            mainCamera = new Camera();
            worldMap = new Sprite3(true, Game1.texWorldMap, 0, 0);
            points = new Sprite3(true, Game1.texPoints, 0, 0);
            
            water = new Sprite3(true, Game1.texMapWater, 0, 0);
            //worldMap.setWidthHeight(6400, 4800);
            horse = new Sprite3(true, Game1.texHorseRun, 500, 400);
            horse.setXframes(8);
            horse.setWidthHeight(1568 / 8, Game1.texHorseRun.Height);
        }

        public override void Update(GameTime gameTime)
        {
            
            curPos = horse.getPos();
            bool keyDown = false;
            horse.setWidthHeight(1568 / 8 * 0.3f, Game1.texHorseRun.Height * 0.3f);

            if (horse.Intersects(points.getBB()))
                Console.WriteLine("touching points");

            if (RC_GameStateParent.keyState.IsKeyDown(Keys.Down))
            {
                keyDown = true;
                horse.setPosY(horse.getPosY() + movementSpeed);
            }

            if (RC_GameStateParent.keyState.IsKeyDown(Keys.Up))
            {
                keyDown = true;
                horse.setPosY(horse.getPosY() - movementSpeed);
            }

            if (RC_GameStateParent.keyState.IsKeyDown(Keys.Left))
            {
                keyDown = true;
                horse.setFlip(SpriteEffects.FlipHorizontally);
                horse.setPosX(horse.getPosX() - movementSpeed);
            }

            if (RC_GameStateParent.keyState.IsKeyDown(Keys.Right))
            {
                keyDown = true;
                horse.setFlip(SpriteEffects.None);
                horse.setPosX(horse.getPosX() + movementSpeed);
            }
            /*if (!keyDown)
            {
                horse.setAnimationSequence(anim, 0, 7, 0);
            }
            else
            {
                horse.setAnimationSequence(anim, 0, 7, 8);
            }*/
            //387 330
            //Poisition of town, this loads battle scene atm
            if (curPos.X >= -155 && curPos.X <= -75 && curPos.Y >= 330 && curPos.Y <= 387)
            {
                
                horse.setFlip(SpriteEffects.None);
                
            }

            //horseRun.animationTick(gameTime);
            
            mainCamera.Follow(horse);
            
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(transformMatrix: mainCamera.Transform);
            worldMap.Draw(spriteBatch);
            points.Draw(spriteBatch);
            spriteBatch.DrawString(Game1.font, "Current position: " + curPos, new Vector2(horse.getPosX() - 200, horse.getPosY() - 200), Color.White);
            horse.Draw(spriteBatch);
            //horseRun.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}
