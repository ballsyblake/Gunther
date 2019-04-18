using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using System;
using RC_Framework;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Audio;

namespace MiniGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game                                                                                                                                                                                                 
    {
        //Direction for art, please change according to where you have put the project files
        static public string dir = @"D:\GitHubRepos\MiniGame\MiniGame\MiniGame\Content\Sprites\";

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
        static public Texture2D texPoints = null;
        static public Texture2D texMapWater = null;
        static public Texture2D texOpenBook = null;
        static public Texture2D texWood = null;
        static public Texture2D texArrowHead = null;
        static public Texture2D texControls = null;
        static public Texture2D texCity = null;
        static public Texture2D texVillage = null;
        static public Texture2D texWeaponsShop = null;
        static public Texture2D texScroll = null;
        static public Texture2D texPaper = null;
        static public Texture2D texCityScreen = null;
        static public Texture2D texMapLand = null;

        //Sound
        static public List<SoundEffect> soundEffects;
        static public SoundEffect music;

        //Cities, towns and villages
        public static Vector2[] pointsPos = new Vector2[22];
        public static Dictionary<Vector2, string> cities = new Dictionary<Vector2, string>();

        //Random variable for, well, you know.. random things
        static public Random random = new Random();

        //All variable relating to text are declared here
        static public SpriteFont font;
        static public SpriteFont startText;
        static public SpriteFont directions;
        static public SpriteFont gameOverText;
        static public SpriteFont difficultySelectText;

        static public bool showbb = false;
        static public bool endGame = false;


        //Set screen size here, declare the content directory and graphics
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            soundEffects = new List<SoundEffect>();
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

            texWorldMap = Util.texFromFile(GraphicsDevice, Game1.dir + "GPTMap.png");
            texPoints = Util.texFromFile(GraphicsDevice, Game1.dir + "Interaction Points.png");
            texMapWater = Util.texFromFile(GraphicsDevice, Game1.dir + "MapWater.png");
            texStartBanner = Util.texFromFile(GraphicsDevice, dir + "startBanner.png");
            texGoldBanner = Util.texFromFile(GraphicsDevice, dir + "goldBanner.png");
            texOpenBook = Util.texFromFile(GraphicsDevice, dir + "openBook.png");
            texBook = Util.texFromFile(GraphicsDevice, dir + "openBookWithText.png");
            texHorseRun = Util.texFromFile(GraphicsDevice, dir + "horseRun.png");
            texEnemy = Util.texFromFile(GraphicsDevice, dir + "Enemy.png");
            texBlood = Util.texFromFile(GraphicsDevice, dir + "bloodSide.png");
            texArrow = Util.texFromFile(GraphicsDevice, dir + "Arrow.png");
            texWood = Util.texFromFile(GraphicsDevice, dir + "wood1.jpg");
            texArrowHead = Util.texFromFile(GraphicsDevice, dir + "arrowHead.png");
            texControls = Util.texFromFile(GraphicsDevice, dir + "Controls.png");
            texCity = Util.texFromFile(GraphicsDevice, dir + "Concept_City1.jpg");
            texScroll = Util.texFromFile(GraphicsDevice, dir + "scroll.png");
            texPaper = Util.texFromFile(GraphicsDevice, dir + "oldpaper.png");
            texCityScreen = Util.texFromFile(GraphicsDevice, dir + "cityScreen.png");
            texMapLand = Util.texFromFile(GraphicsDevice, dir + "GPTMapLand.png");

            //Points positions
            pointsPos[0] = new Vector2(277, 260);
            pointsPos[1] = new Vector2(460, 636);
            pointsPos[2] = new Vector2(1028, 496);
            pointsPos[3] = new Vector2(672, 1050);
            pointsPos[4] = new Vector2(1083, 976);
            pointsPos[5] = new Vector2(859, 1444);
            pointsPos[6] = new Vector2(901, 1708);
            pointsPos[7] = new Vector2(1332, 2102);
            pointsPos[8] = new Vector2(1003, 2385);
            pointsPos[9] = new Vector2(1796, 2279);
            pointsPos[10] = new Vector2(1816, 1862);
            pointsPos[11] = new Vector2(2290, 1789);
            pointsPos[12] = new Vector2(2557, 1293);
            pointsPos[13] = new Vector2(3168, 1187);
            pointsPos[14] = new Vector2(3699, 711);
            pointsPos[15] = new Vector2(2998, 213);
            pointsPos[16] = new Vector2(2501, 716);
            pointsPos[17] = new Vector2(2418, 296);
            pointsPos[18] = new Vector2(2570, 2770);
            pointsPos[19] = new Vector2(3150, 2460);
            pointsPos[20] = new Vector2(3620, 2050);
            pointsPos[21] = new Vector2(1870, 1450);

            cities.Add(pointsPos[0], "Pandia");
            cities.Add(pointsPos[1], "Uyarmore");
            cities.Add(pointsPos[2], "Chuby");
            cities.Add(pointsPos[3], "Truulnard");
            cities.Add(pointsPos[4], "Shosmouth");
            cities.Add(pointsPos[5], "Clournard");
            cities.Add(pointsPos[6], "Ihester");
            cities.Add(pointsPos[7], "Ifrore");
            cities.Add(pointsPos[8], "Tico");
            cities.Add(pointsPos[9], "Ouisbus");
            cities.Add(pointsPos[10], "Aresmore");
            cities.Add(pointsPos[11], "Creavale");
            cities.Add(pointsPos[12], "Prafburg");
            cities.Add(pointsPos[13], "Kreunsa");
            cities.Add(pointsPos[14], "Hieydiff");
            cities.Add(pointsPos[15], "Bocrough");
            cities.Add(pointsPos[16], "Chia");
            cities.Add(pointsPos[17], "Evlouver");
            cities.Add(pointsPos[18], "Zlale");
            cities.Add(pointsPos[19], "Arkshire");
            cities.Add(pointsPos[20], "Arioledo");
            cities.Add(pointsPos[21], "Qisa");

            //Sound
            soundEffects.Add(Content.Load<SoundEffect>("Audio/deathSound01"));//Death sounds in position 0
            soundEffects.Add(Content.Load<SoundEffect>("Audio/horseRun"));//horse run sounds in position 1
            soundEffects.Add(Content.Load<SoundEffect>("Audio/shoot"));//shoot arrow sounds in position 2
            soundEffects.Add(Content.Load<SoundEffect>("Audio/arrowHit01"));//Menu arrow in position 3
            music = Content.Load<SoundEffect>("Audio/cave theme");

            levelManager = new RC_GameStateManager();
            levelManager.AddLevel(0, new PlayLevel()); // note play level is level 0
            levelManager.getLevel(0).InitializeLevel(GraphicsDevice, spriteBatch, Content, levelManager);
            levelManager.getLevel(0).LoadContent();

            levelManager.AddLevel(1, new SplashScreen()); // note splash screen is level 1
            levelManager.getLevel(1).InitializeLevel(GraphicsDevice, spriteBatch, Content, levelManager);
            levelManager.getLevel(1).LoadContent();
            

            levelManager.AddLevel(2, new Pause()); // note pause screen is level 2
            levelManager.getLevel(2).InitializeLevel(GraphicsDevice, spriteBatch, Content, levelManager);
            levelManager.getLevel(2).LoadContent();

            levelManager.AddLevel(3, new WorldMap()); // note world map is level 3
            levelManager.getLevel(3).InitializeLevel(GraphicsDevice, spriteBatch, Content, levelManager);
            levelManager.getLevel(3).LoadContent();

            levelManager.AddLevel(4, new MainMenu()); // note main menu is level 4
            levelManager.getLevel(4).InitializeLevel(GraphicsDevice, spriteBatch, Content, levelManager);
            levelManager.getLevel(4).LoadContent();

            levelManager.AddLevel(5, new City()); // note main menu is level 5
            levelManager.getLevel(5).InitializeLevel(GraphicsDevice, spriteBatch, Content, levelManager);
            levelManager.getLevel(5).LoadContent();
            levelManager.setLevel(3);
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

            if (endGame)
                Exit();

            //Bounding box activation key
            if (RC_GameStateParent.keyState.IsKeyDown(Keys.B) && RC_GameStateParent.prevKeyState.IsKeyUp(Keys.B))
            {
                //Console.WriteLine("bounding boxes");
                showbb = !showbb;
            }
            
            if (RC_GameStateParent.keyState.IsKeyDown(Keys.P) && RC_GameStateParent.prevKeyState.IsKeyUp(Keys.P)) // ***
            {
                //Console.WriteLine("paused");
                levelManager.pushLevel(2);
            }
            


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
