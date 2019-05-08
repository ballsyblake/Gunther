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
        public static float worldTime;
        Sprite3 worldMap = null;
        Sprite3 points = null;
        Sprite3 land = null;
        Camera mainCamera;
        private Vector2 curPos;
        public static Sprite3 horse = null;
        SpriteList horseRun = null;
        int movementSpeed = 2;
        int top = 360;
        int bot = 2600;
        int left = 450;
        int right = 3490;
        float timer = 0f;
        int count = 0;
        bool touchingWater = false;
        int maxEnemies = 50;

        Vector2[] anim = new Vector2[8];
        Vector2[] animEnemy = new Vector2[50];

        Vector2[] enemySpawnPoints = new Vector2[12];

        public static uint[] pixelData;
        uint temp;

        bool leftCol = false;
        bool rightCol = false;
        bool topCol = false;
        bool botCol = false;
        public static List<Enemies> enemiesList = new List<Enemies>();
        
        Random rand = new Random();

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
            Console.WriteLine(pixelData.Count());
           
            //enemiesList.Add(new Enemies(Game1.texEnemy, new Vector2(rand.Next(400, 600), rand.Next(400, 600))));

            enemySpawnPoints[0] = new Vector2(282, 478);
            enemySpawnPoints[1] = new Vector2(754, 1058);
            enemySpawnPoints[2] = new Vector2(888, 1788);
            enemySpawnPoints[3] = new Vector2(278, 2746);
            enemySpawnPoints[4] = new Vector2(1170, 2530);
            enemySpawnPoints[5] = new Vector2(1604, 2176);
            enemySpawnPoints[6] = new Vector2(2052, 1966);
            enemySpawnPoints[7] = new Vector2(2302, 1040);
            enemySpawnPoints[8] = new Vector2(2552, 784);
            enemySpawnPoints[9] = new Vector2(3134, 402);
            enemySpawnPoints[10] = new Vector2(3684, 804);
            enemySpawnPoints[11] = new Vector2(2950, 1316);
        }

        public override void Update(GameTime gameTime)
        {
            
            //worldTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            if(worldTime % 2 >= 0 && worldTime % 2 <= 0.02 && enemiesList.Count() < maxEnemies && (int)worldTime > 0)
            {
                for (int o = 0; o < enemySpawnPoints.Count(); o++)
                {
                    float temp;
                    temp = Vector2.Distance(horse.getPos(), enemySpawnPoints[o]);
                    Console.WriteLine(temp);
                    if(temp > 500)
                        enemiesList.Add(new Enemies(Game1.texEnemy, enemySpawnPoints[rand.Next(0, 11)]));
                }
                
            }
                
            
            if (gameStateManager.getCurrentLevelNum() == 3)
            {
                for (int totalPoints = 0; totalPoints < Game1.pointsPos.Count(); totalPoints++)
                {
                    float nearestPoint = Vector2.Distance(horse.getPos(), Game1.pointsPos[totalPoints]);
                    if (nearestPoint < 30)
                    {
                        City.CurrentLocation(Game1.pointsPos[totalPoints]);
                        gameStateManager.setLevel(5);
                        return;
                    }
                        
                }
            }

            curPos = horse.getPos();
            

            bool keyDown = false;
            
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

            for (int i = 0; i < enemiesList.Count(); i++)
            {
                
                if (enemiesList[i].enemyScript == null)
                    enemiesList.RemoveAt(i);
                else
                    enemiesList[i].Update();
            }
            
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(transformMatrix: mainCamera.Transform);
            land.Draw(spriteBatch);
            worldMap.Draw(spriteBatch);
            points.Draw(spriteBatch);
            spriteBatch.DrawString(Game1.font, "Current position: " + curPos, new Vector2(horse.getPosX() - 200, horse.getPosY() - 200), Color.White);
            
            for (int i = 0; i < enemiesList.Count(); i++)
            {
                enemiesList[i].Draw(spriteBatch);
            }
            
            horse.Draw(spriteBatch);
            CheckWaterCollision();
            spriteBatch.End();
        }

        public void CheckWaterCollision()
        {
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
                                
                                leftCol = true;
                            }

                            else if (xx - curPos.X > horse.getWidth() / 2)
                            {
                                
                                rightCol = true;
                            }
                        }
                        if (!topCol && !botCol)
                        {
                            if (yy - curPos.Y < horse.getHeight() / 2)
                            {
                                
                                topCol = true;
                            }

                            else if (yy - curPos.Y > horse.getHeight() / 2)
                            {
                                
                                botCol = true;
                            }
                        }
                    }
                }
            }
        }
    }
}
