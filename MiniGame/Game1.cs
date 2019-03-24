using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using System;
using RC_Framework;
using System.Collections.Generic;
using System.Linq;

namespace MiniGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        //Direction for art, please change if running from different device than where it was created
        string dir = @"D:\GitHubRepos\MiniGame\MiniGame\MiniGame\Content\Sprites\";
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D texBack = null;
        Texture2D texHorseRun = null;
        Texture2D texEnemy = null;
        Texture2D texBlood = null;
        Texture2D texArrow = null;
        Texture2D texStartBanner = null;
        Texture2D texGoldBanner = null;

        SpriteList bloodSplat = null;

        Sprite3 goldBanner = null;
        Sprite3 arrow = null;
        Sprite3 enemy = null;
        SpriteList enemies = null;
        Random random = new Random();
        
        SpriteList horseRun = null;
        Sprite3 horse = null;

        
        float xx = 50;
        float yy = 500;

        KeyboardState k;
        //ImageBackground back1 = null;

        Rectangle playArea;
        bool gameOver = false;
        bool arrowShot = false;
        bool showbb = false;
        bool started = false;
       

        KeyboardState prevK;

        int top = 320;
        int bot = 599;
        int lhs = 1;
        int rhs = 800;
        int movementSpeed = 3; //this is not a boundary obviously... pretty self explanatory what this is 
        int enemyMovementSpeed = 3;
        int scrollingSpeed = 2;
        int arrowOffsetX = 60;
        int arrowOffsetY = 10;
        float spawn = 0;
        Vector2[] anim = new Vector2[8];
        Vector2[] animEnemy = new Vector2[50];

        Scrolling scrolling1;
        Scrolling scrolling2;

        private SpriteFont font;
        private SpriteFont startText;
        private SpriteFont directions;
        private SpriteFont gameOverText;
        private int score = 0;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            //Setting the screen resoultion to 600x800
            graphics.PreferredBackBufferHeight = 600;
            graphics.PreferredBackBufferWidth = 800;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            LineBatch.init(GraphicsDevice);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            gameOverText = Content.Load<SpriteFont>("Gold");
            directions = Content.Load<SpriteFont>("Gold");
            font = Content.Load<SpriteFont>("Gold");
            startText = Content.Load<SpriteFont>("Gold");
            playArea = new Rectangle(lhs, top, rhs - lhs, bot - top); // width and height
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            texStartBanner = Util.texFromFile(GraphicsDevice, dir + "startBanner.png");
            texGoldBanner = Util.texFromFile(GraphicsDevice, dir + "goldBanner.png");
            
            texBack = Util.texFromFile(GraphicsDevice, dir + "back2.png"); //***
            texHorseRun = Util.texFromFile(GraphicsDevice, dir + "horseRun.png");
            texEnemy = Util.texFromFile(GraphicsDevice, dir + "Enemy.png");
            texBlood = Util.texFromFile(GraphicsDevice, dir + "bloodSide.png");
            texArrow = Util.texFromFile(GraphicsDevice, dir + "Arrow.png");

            goldBanner = new Sprite3(true, texGoldBanner, 15, 15);
            goldBanner.setWidthHeight(130, 40);
            horse = new Sprite3(true, texHorseRun, xx, yy);
            arrow = new Sprite3(false, texArrow, 0, 0);
            arrow.setWidthHeight(arrow.getWidth() * 0.09f, arrow.getHeight() * 0.09f);
            enemies = new SpriteList();
            bloodSplat = new SpriteList();
            
            horseRun = new SpriteList();
            float scale = 0.5f;

            
            //Player animation setup
            horse.setXframes(8);
            horse.setWidthHeight(1568/8*scale,texHorseRun.Height*scale);
            horse.setBB(0,0,horse.getWidth()/scale,horse.getHeight()/scale);
            
            for (int h = 0; h < anim.Length; h++)
            {
                anim[h].X = h;
            }
            

            horseRun.addSpriteReuse(horse);

            enemy = new Sprite3(true, texEnemy, 850, 400);
            enemy.setBBToTexture();
            //Enemy animation setup
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

            enemy.setAnimationSequence(animEnemy, 20, 29, 5);
            enemy.setAnimFinished(0);
            enemy.animationStart();

            enemies.addSpriteReuse(enemy);
            //back1 = new ImageBackground(texBack, Color.White, GraphicsDevice);



            scrolling1 = new Scrolling(Util.texFromFile(GraphicsDevice, dir + "GPT Background 800x600.png"), new Rectangle(0, 0, 800, 600), scrollingSpeed);
            scrolling2 = new Scrolling(Util.texFromFile(GraphicsDevice, dir + "GPT Background2 800x600.png"), new Rectangle(800, 0, 800, 600), scrollingSpeed);


            
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            k = Keyboard.GetState();

            if (k.IsKeyDown(Keys.Enter) && !started)
            {
                started = true;
                StartMovement(8);
            }
            if (started)
            {
                if (k.IsKeyDown(Keys.B) && prevK.IsKeyUp(Keys.B))
                {
                    showbb = !showbb;
                }

                if (k.IsKeyDown(Keys.Down))
                {

                    if (horseRun.getSprite(0).getPosY() < bot - horse.getHeight()) horseRun.getSprite(0).setPosY(horseRun.getSprite(0).getPosY() + movementSpeed);
                }

                if (k.IsKeyDown(Keys.Up))
                {

                    if (horseRun.getSprite(0).getPosY() > top) horseRun.getSprite(0).setPosY(horseRun.getSprite(0).getPosY() - movementSpeed);
                }




                enemies[0].setPosX(enemies[0].getPosX() - enemyMovementSpeed);
                if (enemies[0].getPosX() < 0 - texEnemy.Width)
                {
                    enemies[0].setVisible(false);
                    LoadEnemies();
                }


                int ac = enemies.collisionWithRect(arrow.getBoundingBoxAA());

                if (ac != -1)
                {
                    score++;
                    Blood(enemies.getSprite(ac).getPosX(), enemies.getSprite(ac).getPosY(), false);
                    enemies.getSprite(ac).setVisible(false);
                    RepositionArrow();
                    LoadEnemies();
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                {
                    enemyMovementSpeed = 5;
                    horse.setAnimationSequence(anim, 0, 7, 3);
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Left))
                {
                    enemyMovementSpeed = 1;
                    horse.setAnimationSequence(anim, 0, 7, 15);
                }
                else if (started)
                {
                    enemyMovementSpeed = 3;
                    horse.setAnimationSequence(anim, 0, 7, 8);
                }


                

                if (scrolling1.rectangle.X + scrolling1.texture.Width <= 0)
                    scrolling1.rectangle.X = scrolling2.rectangle.X + scrolling2.texture.Width;
                if (scrolling2.rectangle.X + scrolling2.texture.Width <= 0)
                    scrolling2.rectangle.X = scrolling1.rectangle.X + scrolling1.texture.Width;

                scrolling1.Update();
                scrolling2.Update();
                if (!gameOver)
                {
                    int rc = enemies.collisionWithRect(horse.getBoundingBoxAA());
                    if (rc != -1)
                    {
                        Blood(horse.getPosX(), horse.getPosY(),true);
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

                

                if (Keyboard.GetState().IsKeyDown(Keys.Space) && !arrowShot)
                {
                    Arrow();
                    arrowShot = true;
                }

                if (arrowShot)
                {
                    arrow.savePosition();
                    arrow.moveByDeltaXY();
                }

                if (arrow.getPosX() > 800)
                {
                    RepositionArrow();
                }
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            horseRun.animationTick(gameTime);
            enemies.animationTick(gameTime);
            bloodSplat.animationTick(gameTime);
            base.Update(gameTime);
        }

        public void StartMovement(int horseSpeed)
        {
            horse.setAnimationSequence(anim, 0, 7, horseSpeed);
            horse.setAnimFinished(0);
            horse.animationStart();
        }

        //Gonna be honest, this is a mess
        public void LoadEnemies()
        {
            int randY = random.Next(350, 550);
            enemies[0].setPos(new Vector2(850, randY));
            enemies[0].setVisible(true);
        }

        public void Blood(float x, float y, bool isHorse)
        {
            Sprite3 blood = new Sprite3(true, texBlood, x+10, y-10);
            blood.setYframes(6);
            if(isHorse)
                blood.setWidthHeight(texBlood.Width * 0.2f, texBlood.Height / 6 * 0.2f);
            else
                blood.setWidthHeight(texBlood.Width * 0.1f, texBlood.Height / 6 * 0.1f);
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

        public void Arrow()
        {
            arrow.setPos(new Vector2(horse.getPosX() + arrowOffsetX, horse.getPosY() + arrowOffsetY));
            arrow.setVisible(true);
            arrow.setDeltaSpeed(new Vector2(5, 0));

        }

        public void RepositionArrow()
        {
            arrowShot = false;
            arrow.setVisible(false);
            arrow.setPos(new Vector2(0, 0));
        }
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            
            scrolling1.Draw(spriteBatch);
            scrolling2.Draw(spriteBatch);

            enemies.Draw(spriteBatch);
            
            horseRun.Draw(spriteBatch);
            bloodSplat.Draw(spriteBatch);
            arrow.Draw(spriteBatch);
            goldBanner.Draw(spriteBatch);
            spriteBatch.DrawString(font, "Gold: " + score, new Vector2(30, 20), Color.Black);
            
            if (!started)
            {
                
                spriteBatch.DrawString(directions, "< : slow down | > : speed up " + Environment.NewLine + "^ : move up | v : move down" + Environment.NewLine + "spacebar : shoot arrow", new Vector2(400, 10), Color.Black);
                spriteBatch.DrawString(startText, "Press Enter to Start", new Vector2(300, 400), Color.Black);
            }

            if (gameOver)
                spriteBatch.DrawString(gameOverText, "You suck, press escape to leave game" + Environment.NewLine + "Return when you get better", new Vector2(200, 400), Color.Red);
                
            if (showbb)
            {
                enemies.drawInfo(spriteBatch, Color.Red, Color.DarkRed);
                horse.drawBB(spriteBatch, Color.Black);
                horse.drawHS(spriteBatch, Color.Green);
                arrow.drawBB(spriteBatch, Color.Brown);
                //horseRun.drawInfo(spriteBatch, Color.Brown, Color.Aqua);
                LineBatch.drawLineRectangle(spriteBatch, playArea, Color.Blue);
            }
            
            spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        public static Texture2D texFromFile(GraphicsDevice gd, String fName)
        {
            // note needs :using System.IO;
            Stream fs = new FileStream(fName, FileMode.Open);
            Texture2D rc = Texture2D.FromStream(gd, fs);
            fs.Close();
            return rc;
        }
    }
}
