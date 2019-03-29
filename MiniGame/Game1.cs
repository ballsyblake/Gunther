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
        string dir = @"D:\GitHubRepos\MiniGame\MiniGame\MiniGame\Content\Sprites\";

        //Graphics stuff
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //Storing screen width and height
        public static int screenWidth;
        public static int screenHeight;

        Camera mainCamera;

        //Planning ahead, going to use this for drawing different levels
        //0 is minigame for now, thinking ahead I will probably have 1 being world map, 2 being minigame and 3 being city
        public int level = 1;
        public int difficulty = 1;

        //All textures are declared here
        Texture2D texHorseRun = null;
        Texture2D texEnemy = null;
        Texture2D texBlood = null;
        Texture2D texArrow = null;
        Texture2D texStartBanner = null;
        Texture2D texGoldBanner = null;
        Texture2D texBook = null;
        Texture2D texWorldMap = null;

        //All sprite3 variables are delcared here
        Sprite3 book = null;
        Sprite3 startBanner = null;
        Sprite3 goldBanner = null;
        Sprite3 arrow = null;
        Sprite3 enemy = null;
        Sprite3 horse = null;
        Sprite3 worldMap = null;

        //All spritelists are declared here
        SpriteList bloodSplat = null;
        SpriteList enemies = null;
        SpriteList horseRun = null;
        SpriteList quiver = null;
        
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

        //All variable relating to text are declared here
        private SpriteFont font;
        private SpriteFont startText;
        private SpriteFont directions;
        private SpriteFont gameOverText;
        private SpriteFont difficultySelectText;
        private int score = 0;
        float textFadeTimer = 0f;
        float arrowTimer = 0f;


        //Random
        float deathTimer = 0;
        private Vector2 curPos;
        

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
            screenHeight = graphics.PreferredBackBufferHeight;
            screenWidth = graphics.PreferredBackBufferWidth;
            LineBatch.init(GraphicsDevice);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            //Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            mainCamera = new Camera();

            

            //Load all the textures and fonts
            gameOverText = Content.Load<SpriteFont>("MedievalFont");
            directions = Content.Load<SpriteFont>("Gold");
            font = Content.Load<SpriteFont>("MedievalFont");
            difficultySelectText = Content.Load<SpriteFont>("MedievalFont");
            
            startText = Content.Load<SpriteFont>("Gold");            
            texStartBanner = Util.texFromFile(GraphicsDevice, dir + "startBanner.png");
            texGoldBanner = Util.texFromFile(GraphicsDevice, dir + "goldBanner.png");
            texBook = Util.texFromFile(GraphicsDevice, dir + "openBookWithText.png");
            texHorseRun = Util.texFromFile(GraphicsDevice, dir + "horseRun.png");
            texEnemy = Util.texFromFile(GraphicsDevice, dir + "Enemy.png");
            texBlood = Util.texFromFile(GraphicsDevice, dir + "bloodSide.png");
            texArrow = Util.texFromFile(GraphicsDevice, dir + "Arrow.png");
            texWorldMap = Util.texFromFile(GraphicsDevice, dir + "FantasyWorldMap_2.png");

            //Define playarea
            playArea = new Rectangle(lhs, top, rhs - lhs, bot - top); // width and height

            //Load sprites and change size if necessary
            worldMap = new Sprite3(true, texWorldMap, -2000, -1000);
            worldMap.setWidthHeight(6400, 4800);
            book = new Sprite3(true, texBook, 50, 50);
            book.setWidthHeight(700, 500);
            startBanner = new Sprite3(true, texStartBanner, 0, 0);
            startBanner.setWidthHeight(800, 600);
            goldBanner = new Sprite3(true, texGoldBanner, 15, 15);
            goldBanner.setWidthHeight(130, 40);
            horse = new Sprite3(true, texHorseRun, xx, yy);

            
            
            

            //Load some empty spritelists
            enemies = new SpriteList();
            bloodSplat = new SpriteList();
            horseRun = new SpriteList();
            quiver = new SpriteList();

            for (int a = 0; a < 5; a++)
            {
                arrow = new Sprite3(false, texArrow, 0, 0);
                arrow.setWidthHeight(arrow.getWidth() * 0.09f, arrow.getHeight() * 0.09f);
                quiver.addSpriteReuse(arrow);
            }

            //Used to change size of sprites
            float scale = 0.5f;
            
            
            //Player animation setup
            horse.setXframes(8);
            horse.setWidthHeight(1568/8*scale,texHorseRun.Height*scale);
            horse.setBB(30,10,(horse.getWidth()/scale) - 60,horse.getHeight()/scale - 20);
            for (int h = 0; h < anim.Length; h++)
            {
                anim[h].X = h;
            }
            horseRun.addSpriteReuse(horse);

            //enemy.setBBToTexture();

            //Enemy animation setup
            for (int k = 0; k < 100; k++)
            {
                enemy = new Sprite3(false, texEnemy, 850, 400);
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
            //Console.WriteLine(enemies.count());
            curPos = horse.getPos();
            bool keyDown = false;
            switch (level)
            {
                case 1:
                    horse.setWidthHeight(1568 / 8 * 0.3f, texHorseRun.Height * 0.3f);
                    k = Keyboard.GetState();
                    if(!started)
                    {
                        started = true;
                        StartMovement(8);
                    }
                        
                    if (k.IsKeyDown(Keys.Down))
                    {
                        keyDown = true;
                        horse.setPosY(horse.getPosY() + movementSpeed - 2);
                    }
                        
                    if (k.IsKeyDown(Keys.Up))
                    {
                        keyDown = true;
                        horse.setPosY(horse.getPosY() - movementSpeed + 2);
                    }
                        
                    if (k.IsKeyDown(Keys.Left))
                    {
                        keyDown = true;
                        horse.setFlip(SpriteEffects.FlipHorizontally);
                        horse.setPosX(horse.getPosX() - movementSpeed + 2);
                    }
                        
                    if (k.IsKeyDown(Keys.Right))
                    {
                        keyDown = true;
                        horse.setFlip(SpriteEffects.None);
                        horse.setPosX(horse.getPosX() + movementSpeed - 2);
                    }
                    if (!keyDown)
                    {
                        horse.setAnimationSequence(anim, 0, 7, 0);
                    }
                    else
                    {
                        horse.setAnimationSequence(anim, 0, 7, 8);
                    }
                    //387 330
                    if(curPos.X >= -155 && curPos.X <= -75 && curPos.Y >=330 && curPos.Y <= 387)
                    {
                        horse.setPos(xx, yy);
                        started = false;
                        horse.setFlip(SpriteEffects.None);
                        level = 0;
                    }

                    horseRun.animationTick(gameTime);
                    mainCamera.Follow(horse);
                    
                    break;
                default:
                    if (!started)
                    {
                        if (k.IsKeyDown(Keys.D1) || k.IsKeyDown(Keys.NumPad1))
                        {
                            difficulty = 1;
                        }
                        else if (k.IsKeyDown(Keys.D2) || k.IsKeyDown(Keys.NumPad2))
                        {
                            difficulty = 2;
                        }
                        else if (k.IsKeyDown(Keys.D3) || k.IsKeyDown(Keys.NumPad3))
                        {
                            difficulty = 3;
                        }
                    }
                    switch (difficulty)
                    {
                        case 1:
                            difficultyOffset = 3;
                            break;
                        case 2:
                            difficultyOffset = 2;
                            break;
                        case 3:
                            difficultyOffset = 0.1f;
                            break;
                        default:
                            break;
                    }
                    horse.setWidthHeight(1568 / 8 * 0.5f, texHorseRun.Height * 0.5f);
                    //This timer makes basic instructions disappear after 3 seconds
                    if (textFadeTimer < 3 && started)
                        textFadeTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if(started)
                        enemySpawnTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                    if (enemySpawnTimer > difficultyOffset)
                    {
                        enemySpawnTimer = 0;
                        LoadEnemies();
                    }

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

                        for (int e = 0; e < enemies.count(); e++)
                        {
                            if(enemies[e].getVisible())
                                enemies[e].setPosX(enemies[e].getPosX() - enemyMovementSpeed);
                            if (enemies[e].getPosX() < 0 - texEnemy.Width)
                                
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
                                LoadEnemies();
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
                        else if (started)
                        {
                            enemyMovementSpeed = normalEnemy;
                            horse.setAnimationSequence(anim, 0, 7, normalPlayer);
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
                            
                        
                    }

                    //Animation ticks for anything that is being animated
                    horseRun.animationTick(gameTime);
                    enemies.animationTick(gameTime);
                    bloodSplat.animationTick(gameTime);

                    if (gameOver)
                        deathTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                    break;
            }
            
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
            Console.WriteLine("load enemy");
            int randY = random.Next(350, 550);
            for (int i = 0; i < enemies.count(); i++)
            {
                Console.WriteLine(enemies[i].getVisible());
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
            difficulty = diff;
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
            Console.WriteLine("called arrow");
            for (int i = 0; i < quiver.count(); i++)
            {
                if (!quiver[i].getVisible())
                {
                    Console.WriteLine("created arrow");
                    quiver[i].setPos(new Vector2(horse.getPosX() + arrowOffsetX, horse.getPosY() + arrowOffsetY));
                    quiver[i].setVisible(true);
                    quiver[i].setDeltaSpeed(new Vector2(arrowSpeed, 0));
                    return;
                }
            }
            

        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //All the elements that are displayed on screen are drawn here
            
            switch (level)
            {
                case 1:
                    spriteBatch.Begin(transformMatrix: mainCamera.Transform);
                    worldMap.Draw(spriteBatch);
                    spriteBatch.DrawString(font, "Current position: " + curPos, new Vector2(horse.getPosX() -200, horse.getPosY() - 200), Color.White);
                    //horse.Draw(spriteBatch);
                    horseRun.Draw(spriteBatch);
                    spriteBatch.End();
                    break;
                default:
                    spriteBatch.Begin();
                    scrolling1.Draw(spriteBatch);
                    scrolling2.Draw(spriteBatch);
                    enemies.Draw(spriteBatch);
                    horseRun.Draw(spriteBatch);
                    bloodSplat.Draw(spriteBatch);
                    //arrow.Draw(spriteBatch);
                    quiver.Draw(spriteBatch);
                    goldBanner.Draw(spriteBatch);
                    spriteBatch.DrawString(font, "Gold: " + score, new Vector2(30, 20), Color.Black);
                    //This is the start screen that goes away once player presses the enter key
                    if (!started)
                    {
                        startBanner.Draw(spriteBatch);
                        switch (difficulty)
                        {
                            case 1:
                                spriteBatch.DrawString(difficultySelectText, "Current difficulty: Easy", new Vector2(10, 500), Color.Black);
                                break;
                            case 2:
                                spriteBatch.DrawString(difficultySelectText, "Current difficulty: Medium", new Vector2(10, 500), Color.Black);
                                break;
                            case 3:
                                spriteBatch.DrawString(difficultySelectText, "Current difficulty: Hard", new Vector2(10, 500), Color.Black);
                                break;
                            default:
                                spriteBatch.DrawString(difficultySelectText, "Current difficulty: Easy", new Vector2(10, 500), Color.Black);
                                break;
                        }
                        spriteBatch.DrawString(difficultySelectText, "Click 1, 2 or 3 to assign difficulty. 1 = easy | 2 = medium | 3 = hard" + Environment.NewLine, new Vector2(10, 560), Color.Black);
                    }
                        

                    //This displays some basic instructions for the player
                    if (textFadeTimer < 3 && started)
                        spriteBatch.DrawString(directions, "< : slow down | > : speed up " + Environment.NewLine + "^ : move up | v : move down" + Environment.NewLine + "spacebar : shoot arrow", new Vector2(400, 10), Color.Black);

                    //When player dies text and a book appear on screen
                    if (gameOver && deathTimer > 1)
                    {
                        book.Draw(spriteBatch);

                    }

                    //Bounding boxes for player, enemies, arrows and the play area    
                    if (showbb)
                    {
                        enemies.drawInfo(spriteBatch, Color.Red, Color.DarkRed);
                        horse.drawBB(spriteBatch, Color.Black);
                        //horse.drawHS(spriteBatch, Color.Green); //don't know if this is required for assessment or not
                        quiver.drawInfo(spriteBatch, Color.Brown, Color.SandyBrown);
                        LineBatch.drawLineRectangle(spriteBatch, playArea, Color.Blue);
                    }
                    spriteBatch.End();
                    break;
            }
            ;
            
            
            

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
