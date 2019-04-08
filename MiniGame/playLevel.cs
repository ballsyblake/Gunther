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
    class PlayLevel : RC_GameStateParent
    {
        Sprite3 arrow = null;
        Sprite3 enemy = null;
        Sprite3 horse = null;
        Sprite3 goldBanner = null;

        SpriteList bloodSplat = null;
        SpriteList enemies = null;
        SpriteList horseRun = null;
        SpriteList quiver = null;

        float xx = 50;
        float yy = 500;

        //All booleans that are used, kind of a mess but meh
        bool gameOver = false;
        bool arrowShot = false;
        
        
        float enemySpawnTimer = 0f;
        float difficultyOffset = 0f;

        //Boundaries
        Rectangle playArea;
        int top = 320;
        int bot = 599;
        int lhs = 1;
        int rhs = 800;

        //Movement
        int movementSpeed = 3;
        int enemyMovementSpeed = 4;
        int scrollingSpeed = 2;
        int normalPlayer = 8;
        int normalEnemy = 4;
        int slowEnemy = 2;
        int slowPlayer = 15;
        int fastEnemy = 6;
        int fastPlayer = 3;
        int arrowSpeed = 10;

        //Arrow related variables
        int arrowOffsetX = 60;
        int arrowOffsetY = 10;


        //Arrays for animations
        Vector2[] anim = new Vector2[8];
        Vector2[] animEnemy = new Vector2[50];

        //Scrolling background variables declared here
        Scrolling scrolling1;
        Scrolling scrolling2;

        private int score = 0;
        float textFadeTimer = 0f;
        float arrowTimer = 0f;

        float deathTimer = 0;
        

        public override void LoadContent()
        {
            //Define playarea
            playArea = new Rectangle(lhs, top, rhs - lhs, bot - top); // width and height

            //Load sprites and change size if necessary
            goldBanner = new Sprite3(true, Game1.texGoldBanner, 15, 15);
            goldBanner.setWidthHeight(130, 40);
            horse = new Sprite3(true, Game1.texHorseRun, xx, yy);

            //Load some empty spritelists
            enemies = new SpriteList();
            bloodSplat = new SpriteList();
            horseRun = new SpriteList();
            quiver = new SpriteList();

            for (int a = 0; a < 5; a++)
            {
                arrow = new Sprite3(false, Game1.texArrow, 0, 0);
                arrow.setWidthHeight(arrow.getWidth() * 0.09f, arrow.getHeight() * 0.09f);
                quiver.addSpriteReuse(arrow);
            }

            //Used to change size of sprites
            float scale = 0.5f;


            //Player animation setup
            horse.setXframes(8);
            horse.setWidthHeight(1568 / 8 * scale, Game1.texHorseRun.Height * scale);
            horse.setBB(30, 10, (horse.getWidth() / scale) - 60, horse.getHeight() / scale - 20);
            for (int h = 0; h < anim.Length; h++)
            {
                anim[h].X = h;
            }
            horseRun.addSpriteReuse(horse);

            //enemy.setBBToTexture();

            //Enemy animation setup
            for (int k = 0; k < 100; k++)
            {
                enemy = new Sprite3(false, Game1.texEnemy, 850, 400);
                enemy.setXframes(10);
                enemy.setYframes(5);
                enemy.setWidthHeight(320 / 10, 160 / 5);
                enemy.setBB(0, 0, enemy.getWidth(), enemy.getHeight());
                int count = 0;
                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        animEnemy[count].X = j;
                        animEnemy[count].Y = i;
                        count++;
                    }
                }
                enemy.setAnimationSequence(animEnemy, 20, 29, 15);
                enemy.setAnimFinished(0);
                enemy.animationStart();
                enemy.setVisible(false);
                enemies.addSpriteReuse(enemy);

            }

            //Load scrolling background images
            scrolling1 = new Scrolling(Util.texFromFile(graphicsDevice, Game1.dir + "GPT Background 800x600.png"), new Rectangle(0, 0, 800, 600), scrollingSpeed);
            scrolling2 = new Scrolling(Util.texFromFile(graphicsDevice, Game1.dir + "GPT Background2 800x600.png"), new Rectangle(800, 0, 800, 600), scrollingSpeed);
        }
        public override void Update(GameTime gameTime)
        {
            switch (Game1.difficulty)
            {
                case 1:
                    difficultyOffset = 1;
                    break;
                case 2:
                    difficultyOffset = 0.5f;
                    break;
                case 3:
                    difficultyOffset = 0.1f;
                    break;
                default:
                    break;
            }

            horse.setWidthHeight(1568 / 8 * 0.5f, Game1.texHorseRun.Height * 0.5f);
            //This timer makes basic instructions disappear after 3 seconds
            if (textFadeTimer < 3)
                textFadeTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            enemySpawnTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (enemySpawnTimer > difficultyOffset)
            {
                enemySpawnTimer = 0;
                LoadEnemies();
            }

            //Game over is a bool that is used to ensure the player can't move horse after it is dead, causes errors otherwise
            if (!gameOver)
            {
                //Player movement down
                if (RC_GameStateParent.keyState.IsKeyDown(Keys.Down))
                {
                    if (horseRun.getSprite(0).getPosY() < bot - horse.getHeight()) horseRun.getSprite(0).setPosY(horseRun.getSprite(0).getPosY() + movementSpeed);
                }
                //Player movement up
                if (RC_GameStateParent.keyState.IsKeyDown(Keys.Up))
                {
                    if (horseRun.getSprite(0).getPosY() > top) horseRun.getSprite(0).setPosY(horseRun.getSprite(0).getPosY() - movementSpeed);
                }
            }

            for (int e = 0; e < enemies.count(); e++)
            {
                if (enemies[e].getVisible())
                    enemies[e].setPosX(enemies[e].getPosX() - enemyMovementSpeed);
                if (enemies[e].getPosX() < 0 - Game1.texEnemy.Width)
                    enemies[e].setVisible(false);
            }

            //Collision detection, arrow to enemy
            for (int ea = 0; ea < quiver.count(); ea++)
            {
                int ac = enemies.collisionWithRect(quiver[ea].getBoundingBoxAA());
                if (ac != -1)
                {
                    score++;
                    Blood(enemies.getSprite(ac).getPosX(), enemies.getSprite(ac).getPosY(), false);
                    enemies.getSprite(ac).setVisible(false);
                    quiver[ea].setVisible(false);
                    quiver[ea].setPos(new Vector2(0, 0));
                    //LoadEnemies();
                }
            }


            //Allow player to increase or decrease speed 
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                enemyMovementSpeed = fastEnemy;
                horse.setAnimationSequence(anim, 0, 7, fastPlayer);
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                enemyMovementSpeed = slowEnemy;
                horse.setAnimationSequence(anim, 0, 7, slowPlayer);
            }
            else
            {
                enemyMovementSpeed = normalEnemy;
                horse.setAnimationSequence(anim, 0, 7, normalPlayer);
            }
            //Console.WriteLine(enemyMovementSpeed);
            //Scrolling background functionality
            if (scrolling1.rectangle.X + scrolling1.texture.Width <= 0)
                scrolling1.rectangle.X = scrolling2.rectangle.X + scrolling2.texture.Width;
            if (scrolling2.rectangle.X + scrolling2.texture.Width <= 0)
                scrolling2.rectangle.X = scrolling1.rectangle.X + scrolling1.texture.Width;
            scrolling1.Update();
            scrolling2.Update();

            //Game over is a bool that is used to ensure the player can't move horse after it is dead, causes errors otherwise
            if (!gameOver)
            {
                //Collision detection, enemy to player
                int rc = enemies.collisionWithRect(horse.getBoundingBoxAA());
                if (rc != -1)
                {
                    Blood(horse.getPosX(), horse.getPosY(), true);
                    movementSpeed = 0;
                    enemyMovementSpeed = 0;
                    scrolling1.speed = 0;
                    scrolling2.speed = 0;
                    for (int i = 0; i < horseRun.count(); i++)
                    {
                        horseRun.deleteSprite(i);
                    }
                    gameOver = true;
                }
            }

            //Shooting arrow functionality
            if (Keyboard.GetState().IsKeyDown(Keys.Space) && !arrowShot)
            {
                Arrow();
                arrowShot = true;
            }
            for (int a = 0; a < quiver.count(); a++)
            {
                if (quiver[a].getVisible())
                {
                    quiver[a].savePosition();
                    quiver[a].moveByDeltaXY();
                }
                if (quiver[a].getPosX() > 800)
                {
                    quiver[a].setVisible(false);
                }
            }
            if (arrowShot)
            {
                arrowTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (arrowTimer > 0.5f)
            {
                arrowTimer = 0;
                arrowShot = false;
            }
            //Animation ticks for anything that is being animated
            horseRun.animationTick(gameTime);
            enemies.animationTick(gameTime);
            bloodSplat.animationTick(gameTime);

            if (gameOver)
                deathTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        //This function starts the movement and animation of the player
        public void StartMovement(int horseSpeed)
        {
            horse.setAnimationSequence(anim, 0, 7, horseSpeed);
            horse.setAnimFinished(0);
            horse.animationStart();
        }

        //Sets position of enemy to a random spawn point on the right side of the screen
        public void LoadEnemies()
        {
            int randY = Game1.random.Next(350, 550);
            for (int i = 0; i < enemies.count(); i++)
            {
                //Console.WriteLine(enemies[i].getVisible());
                if (!enemies[i].getVisible())
                {
                    enemies[i].setPos(new Vector2(850, randY));
                    enemies[i].setVisible(true);
                    return;
                }
            }

        }

        public void ChangeDifficulty(int diff)
        {
            Game1.difficulty = diff;
        }

        //Creates blood splatter animation when player or enemy gets hit
        public void Blood(float x, float y, bool isHorse)
        {
            Sprite3 blood = new Sprite3(true, Game1.texBlood, x - 10, y - 20);
            blood.setYframes(6);
            if (isHorse)
                blood.setWidthHeight(Game1.texBlood.Width * 0.2f, Game1.texBlood.Height / 6 * 0.2f);
            else
                blood.setWidthHeight(Game1.texBlood.Width * 0.1f, Game1.texBlood.Height / 6 * 0.1f);
            Vector2[] animBlood = new Vector2[6];
            for (int h = 0; h < animBlood.Length; h++)
            {
                animBlood[h].Y = h;
            }

            blood.setAnimationSequence(animBlood, 0, 5, 5);
            blood.setAnimFinished(2);
            blood.animationStart();

            bloodSplat.addSpriteReuse(blood);
        }

        //Creates arrow that is shot from player
        public void Arrow()
        {
            //Console.WriteLine("called arrow");
            for (int i = 0; i < quiver.count(); i++)
            {
                if (!quiver[i].getVisible())
                {
                    //Console.WriteLine("created arrow");
                    quiver[i].setPos(new Vector2(horse.getPosX() + arrowOffsetX, horse.getPosY() + arrowOffsetY));
                    quiver[i].setVisible(true);
                    quiver[i].setDeltaSpeed(new Vector2(arrowSpeed, 0));
                    return;
                }
            }


        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            scrolling1.Draw(spriteBatch);
            scrolling2.Draw(spriteBatch);
            enemies.Draw(spriteBatch);
            horseRun.Draw(spriteBatch);
            bloodSplat.Draw(spriteBatch);
            //arrow.Draw(spriteBatch);
            quiver.Draw(spriteBatch);
            goldBanner.Draw(spriteBatch);
            spriteBatch.DrawString(Game1.font, "Gold: " + score, new Vector2(30, 20), Color.Black);
            //This is the start screen that goes away once player presses the enter key
            
            


            //This displays some basic instructions for the player
            if (textFadeTimer < 3)
                spriteBatch.DrawString(Game1.directions, "< : slow down | > : speed up " + Environment.NewLine + "^ : move up | v : move down" + Environment.NewLine + "spacebar : shoot arrow", new Vector2(400, 10), Color.Black);

          
            //Bounding boxes for player, enemies, arrows and the play area    
            if (Game1.showbb)
            {
                Console.WriteLine("bb stuff");
                enemies.drawInfo(spriteBatch, Color.Red, Color.DarkRed);
                horse.drawBB(spriteBatch, Color.Black);
                //horse.drawHS(spriteBatch, Color.Green); //don't know if this is required for assessment or not
                quiver.drawInfo(spriteBatch, Color.Brown, Color.SandyBrown);
                LineBatch.drawLineRectangle(spriteBatch, playArea, Color.Blue);
            }
            spriteBatch.End();
        }
    }
}
