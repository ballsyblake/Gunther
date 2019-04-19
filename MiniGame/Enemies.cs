using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RC_Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;

namespace MiniGame
{
    class Enemies
    {
        //Enemy components
        int movementSpeed = 1;
        public Sprite3 enemyScript = null;
        Vector2[] waypoints = new Vector2[5];
        int ArmySize;

        //Random variables
        bool moving = false;
        float stopTimer;
        float plus =0.01f ;
        Random random = new Random();
        int randomValue;
        float movingTimer;

        public Enemies(Texture2D newTexture, Vector2 newPosition)
        {
            Console.WriteLine("spawned enemy");
            enemyScript = new Sprite3(true, newTexture, newPosition.X, newPosition.Y);
            enemyScript.setXframes(10);
            enemyScript.setYframes(5);
            enemyScript.setWidthHeight(320 / 10, 160 / 5);
            waypoints[0] = new Vector2(enemyScript.getPosX(), enemyScript.getPosY());
            waypoints[1] = new Vector2(enemyScript.getPosX() + random.Next(50, 300), enemyScript.getPosY());
            waypoints[2] = new Vector2(enemyScript.getPosX() - random.Next(50, 300), enemyScript.getPosY());
            waypoints[3] = new Vector2(enemyScript.getPosX(), enemyScript.getPosY() + random.Next(50, 300));
            waypoints[4] = new Vector2(enemyScript.getPosX(), enemyScript.getPosY() - random.Next(50, 300));
            ArmySize = 4;//random.Next(1, 5);
            Console.WriteLine(ArmySize);
        }

        public void Update()
        {
            if (WorldMap.gameStateManager.getCurrentLevelNum() == 3)
            {
                
                if (Vector2.Distance(enemyScript.getPos(), WorldMap.horse.getPos()) < 100 && Vector2.Distance(enemyScript.getPos(), WorldMap.horse.getPos()) > 9)
                    Attack();
                else if (Vector2.Distance(enemyScript.getPos(), WorldMap.horse.getPos()) < 10)
                {
                    for (int i = 0; i < WorldMap.enemiesList.Count(); i++)
                    {
                        if(WorldMap.enemiesList[i] == this)
                        {
                            PlayLevel.LoadLevelDetails(ArmySize, i);
                        }
                    }

                    WorldMap.gameStateManager.setLevel(0);
                }
                else if(!moving && stopTimer > 1)
                {
                    
                    stopTimer = 0;
                    moving = true;
                    
                    randomValue = random.Next(0, 4);
                    //Console.WriteLine(randomValue);
                }
                if (!moving)
                {
                    stopTimer += plus;
                    
                }
                //Console.WriteLine(stopTimer);
                if (moving)
                {
                    movingTimer += plus;
                    Move();
                }
                    
            }
        }

        void Move()
        {
            Vector2 dir = waypoints[randomValue] - enemyScript.getPos();
            dir.Normalize();
            enemyScript.setPos(enemyScript.getPos() + dir * movementSpeed);
            if (Vector2.Distance(enemyScript.getPos(), waypoints[randomValue]) < 10)
                moving = false;
        }

        void ChangeRandom()
        {
            randomValue = random.Next(0,4);
        }

        void Attack()
        {
            Vector2 dir = WorldMap.horse.getPos() - enemyScript.getPos();
            dir.Normalize();
            enemyScript.setPos(enemyScript.getPos() + dir * movementSpeed);
        }

        void CheckWater()
        {
            uint temp;
            for (int xx = (int)enemyScript.getPosX(); xx < (int)enemyScript.getPosX() + enemyScript.getWidth(); xx++)
            {
                for (int yy = (int)enemyScript.getPosY(); yy < (int)enemyScript.getPosY() + enemyScript.getHeight(); yy++)
                {
                    if (xx + yy * Game1.texMapLand.Width < 12000000)
                    {
                        temp = WorldMap.pixelData[xx + yy * Game1.texMapLand.Width];
                        if (temp == 0)
                        {
                            moving = false;
                            if (randomValue == 1)
                                enemyScript.setPosX(enemyScript.getPosX() - enemyScript.getWidth());
                            else if (randomValue == 2)
                                enemyScript.setPosX(enemyScript.getPosX() + enemyScript.getWidth());
                            else if (randomValue == 3)
                                enemyScript.setPosY(enemyScript.getPosX() - enemyScript.getHeight());
                            else if (randomValue == 4)
                                enemyScript.setPosX(enemyScript.getPosX() + enemyScript.getHeight());
                            else if (randomValue == 0)
                                enemyScript = null;
                        }
                    }
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            enemyScript.Draw(spriteBatch);
            if(moving && movingTimer > 0.5f)
                CheckWater();
        }
    }
}
