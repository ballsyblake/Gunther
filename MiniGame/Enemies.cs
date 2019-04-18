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
        int movementSpeed = 1;
        public Sprite3 enemy = null;
        public bool isVisible = true;
        Vector2[] waypoints = new Vector2[5];
        bool moving = false;
        float stopTimer;
        float plus =0.01f ;
        Random random = new Random();
        int randomValue;
        
        public Enemies(Texture2D newTexture, Vector2 newPosition)
        {
            Console.WriteLine("spawned enemy");
            enemy = new Sprite3(true, newTexture, newPosition.X, newPosition.Y);
            enemy.setXframes(10);
            enemy.setYframes(5);
            enemy.setWidthHeight(320 / 10, 160 / 5);
            waypoints[0] = new Vector2(enemy.getPosX(), enemy.getPosY());
            waypoints[1] = new Vector2(enemy.getPosX() + 50, enemy.getPosY());
            waypoints[2] = new Vector2(enemy.getPosX() - 50, enemy.getPosY());
            waypoints[3] = new Vector2(enemy.getPosX(), enemy.getPosY() + 50);
            waypoints[4] = new Vector2(enemy.getPosX(), enemy.getPosY() - 50);

        }

        public void Update()
        {
            if (WorldMap.gameStateManager.getCurrentLevelNum() == 3)
            {
                
                if (Vector2.Distance(enemy.getPos(), WorldMap.horse.getPos()) < 100 && Vector2.Distance(enemy.getPos(), WorldMap.horse.getPos()) > 9)
                    Attack();
                else if (Vector2.Distance(enemy.getPos(), WorldMap.horse.getPos()) < 10)
                {
                    WorldMap.gameStateManager.setLevel(0);
                }
                else if(!moving && stopTimer % 5 == 0 && stopTimer > 0)
                {
                    
                    moving = true;
                    randomValue = random.Next(0,4);
                }
                if (!moving)
                {
                    stopTimer += plus;
                    
                }
                Console.WriteLine(stopTimer);
                if (moving)
                    Move();
                
            }
        }

        void Move()
        {
            
            Vector2 dir = waypoints[randomValue] - enemy.getPos();
            dir.Normalize();
            enemy.setPos(enemy.getPos() + dir * movementSpeed);
            if (Vector2.Distance(enemy.getPos(), waypoints[randomValue]) < 10)
                moving = false;
        }

        void Attack()
        {
            Vector2 dir = WorldMap.horse.getPos() - enemy.getPos();
            dir.Normalize();
            enemy.setPos(enemy.getPos() + dir * movementSpeed);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            enemy.Draw(spriteBatch);
        }
    }
}
