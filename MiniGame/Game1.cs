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
        //Direction for art, please change according to where you have put the project files
        string dir = @"C:\Repos\MiniGame\MiniGame\Content\Sprites\";

        //Graphics stuff
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //Planning ahead, going to use this for drawing different levels
        public int level = 1;

        //All textures are declared here
        Texture2D texHorseRun = null;
        Texture2D texEnemy = null;
        Texture2D texBlood = null;
        Texture2D texArrow = null;
        Texture2D texStartBanner = null;
        Texture2D texGoldBanner = null;
        Texture2D texBook = null;

        //All sprite3 variables are delcared here
        Sprite3 book = null;
        Sprite3 startBanner = null;
        Sprite3 goldBanner = null;
        Sprite3 arrow = null;
        Sprite3 enemy = null;
        Sprite3 horse = null;

        //All spritelists are declared here
        SpriteList bloodSplat = null;
        SpriteList enemies = null;
        SpriteList horseRun = null;
        
        //Random variable for, well, you know.. random things
        Random random = new Random();

        //Starting location for player
        float xx = 50;
        float yy = 500;

        //Keyboard variables
        KeyboardState k;
        KeyboardState prevK;

        //All booleans that are used, kind of a mess but meh
        bool gameOver = false;
        bool arrowShot = false;
        bool showbb = false;
        bool started = false;

        //Boundaries
        Rectangle playArea;
        int top = 320;
        int bot = 599;
        int lhs = 1;
        int rhs = 800;

        //Movement
        int movementSpeed = 3;
        int enemyMovementSpeed = 3;
        int scrollingSpeed = 2;

        //Arrow related variables
        int arrowOffsetX = 60;
        int arrowOffsetY = 10;

        
        //Arrays for animations
        Vector2[] anim = new Vector2[8];
        Vector2[] animEnemy = new Vector2[50];

        //Scrolling background variables declared here
        Scrolling scrolling1;
        Scrolling scrolling2;

        //All variable relating to text are declared here
        private SpriteFont font;
        private SpriteFont startText;
        private SpriteFont directions;
        private SpriteFont gameOverText;
        private int score = 0;
        float textFadeTimer = 0;

        //Random
        float deathTimer = 0;

        //Set screen size here, declare the content directory and graphics
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
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Load all the textures and fonts
            gameOverText = Content.Load<SpriteFont>("MedievalFont");
            directions = Content.Load<SpriteFont>("Gold");
            font = Content.Load<SpriteFont>("MedievalFont");
            startText = Content.Load<SpriteFont>("Gold");            
            texStartBanner = Util.texFromFile(GraphicsDevice, dir + "startBanner.png");
            texGoldBanner = Util.texFromFile(GraphicsDevice, dir + "goldBanner.png");
            texBook = Util.texFromFile(GraphicsDevice, dir + "openBookWithText.png");
            texHorseRun = Util.texFromFile(GraphicsDevice, dir + "horseRun.png");
            texEnemy = Util.texFromFile(GraphicsDevice, dir + "Enemy.png");
            texBlood = Util.texFromFile(GraphicsDevice, dir + "bloodSide.png");
            texArrow = Util.texFromFile(GraphicsDevice, dir + "Arrow.png");

            //Define playarea
            playArea = new Rectangle(lhs, top, rhs - lhs, bot - top); // width and height

            //Load sprites and change size if necessary
            book = new Sprite3(true, texBook, 50, 50);
            book.setWidthHeight(700, 500);
            startBanner = new Sprite3(true, texStartBanner, 0, 0);
            startBanner.setWidthHeight(800, 600);
            goldBanner = new Sprite3(true, texGoldBanner, 15, 15);
            goldBanner.setWidthHeight(130, 40);
            horse = new Sprite3(true, texHorseRun, xx, yy);
            enemy = new Sprite3(true, texEnemy, 850, 400);
            arrow = new Sprite3(false, texArrow, 0, 0);
            arrow.setWidthHeight(arrow.getWidth() * 0.09f, arrow.getHeight() * 0.09f);

            //Load some empty spritelists
            enemies = new SpriteList();
            bloodSplat = new SpriteList();
            horseRun = new SpriteList();

            //Used to change size of sprites
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
            
            //enemy.setBBToTexture();

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
            
            //Load scrolling background images
            scrolling1 = new Scrolling(Util.texFromFile(GraphicsDevice, dir + "GPT Background 800x600.png"), new Rectangle(0, 0, 800, 600), scrollingSpeed);
            scrolling2 = new Scrolling(Util.texFromFile(GraphicsDevice, dir + "GPT Background2 800x600.png"), new Rectangle(800, 0, 800, 600), scrollingSpeed);
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
        /// This is a mess, please forgive me
        protected override void Update(GameTime gameTime)
        {
            //This timer makes basic instructions disappear after 3 seconds
            if (textFadeTimer < 3 && started)
                textFadeTimer += (float) gameTime.ElapsedGameTime.TotalSeconds;

            //Escape key exits game
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //Keyboard current and previous state
            prevK = k;
            k = Keyboard.GetState();

            //Begin all game functionality essentially
            if (k.IsKeyDown(Keys.Enter) && !started)
            {
                started = true;
                StartMovement(8);
            }

            if (started)
            {
                //Bounding box activation key
                if (k.IsKeyDown(Keys.B) && prevK.IsKeyUp(Keys.B))
                    showbb = !showbb;
                
                //Game over is a bool that is used to ensure the player can't move horse after it is dead, causes errors otherwise
                if (!gameOver)
                {
                    //Player movement down
                    if (k.IsKeyDown(Keys.Down))
                    {
                        if (horseRun.getSprite(0).getPosY() < bot - horse.getHeight()) horseRun.getSprite(0).setPosY(horseRun.getSprite(0).getPosY() + movementSpeed);
                    }
                    //Player movement up
                    if (k.IsKeyDown(Keys.Up))
                    {
                        if (horseRun.getSprite(0).getPosY() > top) horseRun.getSprite(0).setPosY(horseRun.getSprite(0).getPosY() - movementSpeed);
                    }
                }

                //Enemies movement
                enemies[0].setPosX(enemies[0].getPosX() - enemyMovementSpeed);
                //For when enemies walk off screen after passing player, just repositions them
                if (enemies[0].getPosX() < 0 - texEnemy.Width)
                {
                    enemies[0].setVisible(false);
                    LoadEnemies();
                }

                //Collision detection, arrow to enemy
                int ac = enemies.collisionWithRect(arrow.getBoundingBoxAA());
                if (ac != -1)
                {
                    score++;
                    Blood(enemies.getSprite(ac).getPosX(), enemies.getSprite(ac).getPosY(), false);
                    enemies.getSprite(ac).setVisible(false);
                    RepositionArrow();
                    LoadEnemies();
                }

                //Allow player to increase or decrease speed 
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

                //Shooting arrow functionality
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

            //Animation ticks for anything that is being animated
            horseRun.animationTick(gameTime);
            enemies.animationTick(gameTime);
            bloodSplat.animationTick(gameTime);

            if (gameOver)
                deathTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            base.Update(gameTime);
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
            int randY = random.Next(350, 550);
            enemies[0].setPos(new Vector2(850, randY));
            enemies[0].setVisible(true);
        }

        //Creates blood splatter animation when player or enemy gets hit
        public void Blood(float x, float y, bool isHorse)
        {
            Sprite3 blood = new Sprite3(true, texBlood, x-10, y-20);
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

        //Creates arrow that is shot from player
        public void Arrow()
        {
            arrow.setPos(new Vector2(horse.getPosX() + arrowOffsetX, horse.getPosY() + arrowOffsetY));
            arrow.setVisible(true);
            arrow.setDeltaSpeed(new Vector2(5, 0));

        }

        //Essentially deactives arrow but technically the arrow still exists but its out of the way of play area, will probably change later
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

            //All the elements that are displayed on screen are drawn here
            spriteBatch.Begin();
            switch (level)
            {
                default:
                    scrolling1.Draw(spriteBatch);
                    scrolling2.Draw(spriteBatch);
                    enemies.Draw(spriteBatch);
                    horseRun.Draw(spriteBatch);
                    bloodSplat.Draw(spriteBatch);
                    arrow.Draw(spriteBatch);
                    goldBanner.Draw(spriteBatch);
                    spriteBatch.DrawString(font, "Gold: " + score, new Vector2(30, 20), Color.Black);
                    break;
            }
            ;
            
            //This is the start screen that goes away once player presses the enter key
            if (!started)
                startBanner.Draw(spriteBatch);

            //This displays some basic instructions for the player
            if(textFadeTimer < 3 && started)
                spriteBatch.DrawString(directions, "< : slow down | > : speed up " + Environment.NewLine + "^ : move up | v : move down" + Environment.NewLine + "spacebar : shoot arrow", new Vector2(400, 10), Color.Black);

            //When player dies text and a book appear on screen
            if (gameOver)
            {
                book.Draw(spriteBatch);
                
            }
                
            //Bounding boxes for player, enemies, arrows and the play area    
            if (showbb)
            {
                enemies.drawInfo(spriteBatch, Color.Red, Color.DarkRed);
                horse.drawBB(spriteBatch, Color.Black);
                //horse.drawHS(spriteBatch, Color.Green); //don't know if this is required for assessment or not
                arrow.drawBB(spriteBatch, Color.Brown);
                LineBatch.drawLineRectangle(spriteBatch, playArea, Color.Blue);
            }
            spriteBatch.End();

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
