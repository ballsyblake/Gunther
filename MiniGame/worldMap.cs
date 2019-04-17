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
        Sprite3 land = null;
        Camera mainCamera;
        private Vector2 curPos;
        public static Sprite3 horse = null;
        SpriteList horseRun = null;
        int movementSpeed = 3;
        int top = 360;
        int bot = 2600;
        int left = 450;
        int right = 3490;
        float timer = 0f;
        int count = 0;
        bool touchingWater = false;

        Vector2[] anim = new Vector2[8];
        Vector2[] animEnemy = new Vector2[50];

        uint[] pixelData;
        uint temp;

        bool leftCol = false;
        bool rightCol = false;
        bool topCol = false;
        bool botCol = false;


        public override void LoadContent()
        {
            mainCamera = new Camera();
            worldMap = new Sprite3(true, Game1.texWorldMap, 0, 0);
            points = new Sprite3(true, Game1.texPoints, 0, 0);
            
            land = new Sprite3(true, Game1.texMapLand, 0, 0);
            //worldMap.setWidthHeight(6400, 4800);
            horseRun = new SpriteList();
            horse = new Sprite3(true, Game1.texHorseRun, 500, 400);
            horse.setXframes(8);
            horse.setWidthHeight((1568 / 8) * 0.2f, Game1.texHorseRun.Height * 0.2f);
            //horse.setBB(30, 10, horse.getWidth(), horse.getHeight());
            for (int h = 0; h < anim.Length; h++)
            {
                anim[h].X = h;
            }
            horseRun.addSpriteReuse(horse);


            pixelData = new uint[Game1.texMapLand.Width * Game1.texMapLand.Height];
            Game1.texMapLand.GetData(pixelData, 0, Game1.texMapLand.Width * Game1.texMapLand.Height);

        }

        public override void Update(GameTime gameTime)
        {
           
            if (gameStateManager.getCurrentLevelNum() == 3)
            {
                for (int totalPoints = 0; totalPoints < Game1.pointsPos.Count(); totalPoints++)
                {
                    float nearestPoint = Vector2.Distance(horse.getPos(), Game1.pointsPos[totalPoints]);
                    if (nearestPoint < 30)
                    {
                        City.CurrentLocation(Game1.pointsPos[totalPoints]);
                        gameStateManager.setLevel(0);
                        return;
                    }
                        
                }
            }
            curPos = horse.getPos();
            bool keyDown = false;
            //horse.setWidthHeight(1568 / 8 * 0.3f, Game1.texHorseRun.Height * 0.3f);

            
            
            if (RC_GameStateParent.keyState.IsKeyDown(Keys.Down) && !botCol)
            {
                keyDown = true;
                horse.setPosY(horse.getPosY() + movementSpeed);
            }

            if (RC_GameStateParent.keyState.IsKeyDown(Keys.Up) && !topCol)
            {
                keyDown = true;
                horse.setPosY(horse.getPosY() - movementSpeed);
            }

            if (RC_GameStateParent.keyState.IsKeyDown(Keys.Left) && !leftCol)
            {
                keyDown = true;
                horse.setFlip(SpriteEffects.FlipHorizontally);
                horse.setPosX(horse.getPosX() - movementSpeed);
            }

            if (RC_GameStateParent.keyState.IsKeyDown(Keys.Right) && !rightCol)
            {
                keyDown = true;
                horse.setFlip(SpriteEffects.None);
                horse.setPosX(horse.getPosX() + movementSpeed);
            }

            mainCamera.Follow(horse);

            if (!keyDown)
            {
                horse.setAnimationSequence(anim, 0, 7, 0);
            }
            else
            {
                horse.setAnimationSequence(anim, 0, 7, 4);
            }


            horseRun.animationTick(gameTime);



            /*timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (timer > 0.2f)
            {
                timer = 0;
                //CheckWaterCollision();
            }*/
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(transformMatrix: mainCamera.Transform);
            land.Draw(spriteBatch);
            worldMap.Draw(spriteBatch);
            points.Draw(spriteBatch);
            spriteBatch.DrawString(Game1.font, "Current position: " + curPos, new Vector2(horse.getPosX() - 200, horse.getPosY() - 200), Color.White);
            horse.Draw(spriteBatch);
            CheckWaterCollision();
            //horseRun.Draw(spriteBatch);
            spriteBatch.End();
        }

        public void CheckWaterCollision()
        {
            /*temp = pixelData[(int)curPos.X + ((int)horse.getWidth()/2) + (int)curPos.Y + ((int)horse.getHeight()/2) * tex.Width];
            if (temp == 0)
                return true;
            */
            botCol = false;
            topCol = false;
            leftCol = false;
            rightCol = false;
            for (int xx = (int)curPos.X; xx < (int)curPos.X + horse.getWidth(); xx++)
            {
                for (int yy = (int)curPos.Y; yy < (int)curPos.Y + horse.getHeight(); yy++)
                {
                    temp = pixelData[xx + yy * Game1.texMapLand.Width];
                    if (temp == 0)
                    {
                        if (!leftCol && !rightCol)
                        {
                            if (xx - curPos.X < horse.getWidth() / 2)
                            {
                                Console.WriteLine("stuck left");
                                leftCol = true;
                            }

                            else if (xx - curPos.X > horse.getWidth() / 2)
                            {
                                Console.WriteLine("stuck right");
                                rightCol = true;
                            }
                        }
                        if (!topCol && !botCol)
                        {
                            if (yy - curPos.Y < horse.getHeight() / 2)
                            {
                                Console.WriteLine("stuck top");
                                topCol = true;
                            }

                            else if (yy - curPos.Y > horse.getHeight() / 2)
                            {
                                Console.WriteLine("stuck bot");
                                botCol = true;
                            }
                        }
                    }
                }
            }
        }
    }
}
