using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ShootEmUp_1514716
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GameRoot : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        /* Multiple levels*/ 
        enum GameState
        { MainMenu,
          LevelOne,
          LevelTwo,
          EndOfGame,
        }
        /*                  */
        GameState _state;

        public static Texture2D Player { get; private set; }
        public static Texture2D Seeker { get; private set; }
        public static Texture2D Wanderer { get; private set; }
        public static Texture2D Bullet { get; private set; }
        public static Texture2D Pointer { get; private set; }

        public GameRoot()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Instance = this;

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

            base.Initialize();
            graphics.PreferredBackBufferWidth = 920;
            graphics.PreferredBackBufferHeight = 600;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();

            EntityManager.Add(PlayerShip.Instance);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Player = Content.Load<Texture2D>("Art/Player");
            Seeker = Content.Load<Texture2D>("Art/Seeker");
            Wanderer = Content.Load<Texture2D>("Art/Wanderer");
            Bullet = Content.Load<Texture2D>("Art/Bullet");
            Pointer = Content.Load<Texture2D>("Art/Pointer");
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
            Input.Update();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            
            // TODO: Add your update logic here
            EntityManager.Update();
            EnemySpawner.Update();
            base.Update(gameTime);
            switch (_state)
            {
                case GameState.MainMenu :
                    UpdateMainMenu(gameTime);
                    break;
                case GameState.LevelOne:
                    UpdateLevelOne(gameTime);
                    break;
            }
        }

        void UpdateMainMenu(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.H))
            {
               // EnemySpawner.playGame();
                _state = GameState.LevelOne;

            }
            if (Keyboard.GetState().IsKeyDown(Keys.V))
            {
                EnemySpawner.PauseSpawn();

                _state = GameState.MainMenu;
                

            }
        }

        void UpdateLevelOne(GameTime gameTime)
        {
            // Respond to user actions
            // handle collisions
            // update enemies for level1
            if (Keyboard.GetState().IsKeyDown(Keys.V))
            {
                _state = GameState.MainMenu;

            }
        }

        void DrawMainMenu(GameTime gameTime)
        {
            //draw main menu
            spriteBatch.Begin(SpriteSortMode.Texture, BlendState.Additive);
            //EntityManager.Draw(spriteBatch);

            /* Draw the custom mouse cursor */
            spriteBatch.Draw(GameRoot.Pointer, Input.MousePosition, Color.White);

            spriteBatch.End();


        }

        void DrawLevelOne(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.Texture, BlendState.Additive);
            EntityManager.Draw(spriteBatch);

            /* Draw the custom mouse cursor */
            spriteBatch.Draw(GameRoot.Pointer, Input.MousePosition, Color.White);

            spriteBatch.End();



        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
           
            // TODO: Add your drawing code here

            /* Background Colour */
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteSortMode.Texture, BlendState.Additive);
            //EntityManager.Draw(spriteBatch);

            /* Draw the custom mouse cursor */
            spriteBatch.Draw(GameRoot.Pointer, Input.MousePosition, Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
            switch (_state)
            {
                case GameState.MainMenu:
                    DrawMainMenu(gameTime);
                    break;
                case GameState.LevelOne:
                    DrawLevelOne(gameTime);
                    break;
            }
        }

        public static GameRoot Instance { get; private set; }

        public static Viewport Viewport{ get { return Instance.GraphicsDevice.Viewport; } }

        public static Vector2 ScreenSize
        {
            get
            {
                return new
                Vector2(Viewport.Width, Viewport.Height);
            }
        }
    }
}
