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
        static public string dir = @"C:\Repos\MiniGame\MiniGame\Content\Sprites\";

        //Graphics stuff
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //Level manager
        static public RC_GameStateManager levelManager;

        //Storing screen width and height
        public static int screenWidth;
        public static int screenHeight;

        static public int difficulty = 1;

        //All textures are declared here
        static public Texture2D texHorseRun = null;
        static public Texture2D texEnemy = null;
        static public Texture2D texBlood = null;
        static public Texture2D texArrow = null;
        static public Texture2D texStartBanner = null;
        static public Texture2D texGoldBanner = null;
        static public Texture2D texBook = null;
        static public Texture2D texWorldMap = null;

        //Random variable for, well, you know.. random things
        static public Random random = new Random();

        //All variable relating to text are declared here
        static public SpriteFont font;
        static public SpriteFont startText;
        static public SpriteFont directions;
        static public SpriteFont gameOverText;
        static public SpriteFont difficultySelectText;

        static public bool showbb = false;

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

            //Load all the textures and fonts
            gameOverText = Content.Load<SpriteFont>("MedievalFont");
            directions = Content.Load<SpriteFont>("Gold");
            font = Content.Load<SpriteFont>("MedievalFont");
            difficultySelectText = Content.Load<SpriteFont>("MedievalFont");
            startText = Content.Load<SpriteFont>("Gold");

            texWorldMap = Util.texFromFile(GraphicsDevice, Game1.dir + "FantasyWorldMap_2.png");
            texStartBanner = Util.texFromFile(GraphicsDevice, dir + "startBanner.png");
            texGoldBanner = Util.texFromFile(GraphicsDevice, dir + "goldBanner.png");
            texBook = Util.texFromFile(GraphicsDevice, dir + "openBookWithText.png");
            texHorseRun = Util.texFromFile(GraphicsDevice, dir + "horseRun.png");
            texEnemy = Util.texFromFile(GraphicsDevice, dir + "Enemy.png");
            texBlood = Util.texFromFile(GraphicsDevice, dir + "bloodSide.png");
            texArrow = Util.texFromFile(GraphicsDevice, dir + "Arrow.png");

            levelManager = new RC_GameStateManager();
            levelManager.AddLevel(0, new PlayLevel()); // note play level is level 0
            levelManager.getLevel(0).InitializeLevel(GraphicsDevice, spriteBatch, Content, levelManager);
            levelManager.getLevel(0).LoadContent();

            levelManager.AddLevel(1, new SplashScreen()); // note splash screen is level 1
            levelManager.getLevel(1).InitializeLevel(GraphicsDevice, spriteBatch, Content, levelManager);
            levelManager.getLevel(1).LoadContent();
            levelManager.setLevel(1);

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
            levelManager.getCurrentLevel().Update(gameTime);

            //Keyboard current and previous state
            RC_GameStateParent.getKeyboardAndMouse();

            //Escape key exits game
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //Bounding box activation key
            if (RC_GameStateParent.keyState.IsKeyDown(Keys.B) && RC_GameStateParent.prevKeyState.IsKeyUp(Keys.B)) 
                showbb = !showbb;
            

            base.Update(gameTime);
        }

        

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.Clear(Color.CornflowerBlue);

            //All the elements that are displayed on screen are drawn here
            levelManager.getCurrentLevel().Draw(gameTime);

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
