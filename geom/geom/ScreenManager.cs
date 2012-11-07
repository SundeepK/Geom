using System;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using System.IO;
using System.IO.IsolatedStorage;
using System.Xml.Linq;

namespace geom
{
    public class ScreenManager : DrawableGameComponent
    {
      
        List<GameScreen> screens = new List<GameScreen>();
        List<GameScreen> tempScreensList = new List<GameScreen>();

        InputState input = new InputState();
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont font;
        Texture2D blankTexture;
        Bloom bloom;
        bool isInitialized;
   
        bool traceEnabled;

     

        /// <summary>
        /// A default SpriteBatch shared by all the screens. This saves
        /// each screen having to bother creating their own local instance.
        /// </summary>
        public SpriteBatch SpriteBatch
        {
            get { return spriteBatch; }
        }


        /// <summary>
        /// A default font shared by all the screens. This saves
        /// each screen having to bother loading their own local copy.
        /// </summary>
        public SpriteFont Font
        {
            get { return font; }
        }


        /// <summary>
        /// If true, the manager prints out a list of all the screens
        /// each time it is updated. This can be useful for making sure
        /// everything is being added and removed at the right times.
        /// </summary>
        public bool TraceEnabled
        {
            get { return traceEnabled; }
            set { traceEnabled = value; }
        }


        /// <summary>
        /// Gets a blank texture that can be used by the screens.
        /// </summary>
        public Texture2D BlankTexture
        {
            get { return blankTexture; }
        }


        protected Game game;
        /// <summary>
        /// Constructs a new screen manager component.
        /// </summary>
        public ScreenManager(Game game)
            : base(game)
        {
            this.game = game;   
        }


        /// <summary>
        /// Initializes the screen manager component.
        /// </summary>

        public Game GetGame()
        {
            return game;
        }

        /// <summary>
        /// Unload your graphics content.
        /// </summary>
        protected override void UnloadContent()
        {
            // Tell each of the screens to unload their content.
            foreach (GameScreen screen in screens)
            {
                screen.Unload();
            }
        }




        /// <summary>
        /// Allows each screen to run logic.
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            // Read the keyboard and gamepad.
            input.Update();

            tempScreensList.Clear();

            foreach (GameScreen screen in screens)
                tempScreensList.Add(screen);

            bool otherScreenHasFocus = !Game.IsActive;
            //Console.WriteLine(otherScreenHasFocus);
            bool coveredByOtherScreen = false;
            //Console.WriteLine(tempScreensList.Count);
            // Loop as long as there are screens waiting to be updated.
            while (tempScreensList.Count > 0)
            {
                // Pop the topmost screen off the waiting list.
                GameScreen screen = tempScreensList[tempScreensList.Count - 1];

                tempScreensList.RemoveAt(tempScreensList.Count - 1);

                // Update the screen.
                screen.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

                if (screen.ScreenState == ScreenState.TransitionOn ||
                    screen.ScreenState == ScreenState.Active)
                {
                    // If this is the first active screen we came across,
                    // give it a chance to handle input.
                    if (!otherScreenHasFocus)
                    {
                        screen.HandleInput(gameTime, input);

                        otherScreenHasFocus = true;
                    }

                    // If this is an active non-popup, inform any subsequent
                    // screens that they are covered by it.
                    if (!screen.IsPopup)
                        coveredByOtherScreen = true;
                }
            }
        }

        public override void Initialize()
        {
            base.Initialize();

            foreach (GameScreen screen in screens)
            {
                screen.Initialize();
            }

            isInitialized = true;
        }


        protected override void LoadContent()
        {
            // Load content belonging to the screen manager.
            ContentManager content = Game.Content;

            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = content.Load<SpriteFont>("menufont");
            blankTexture = content.Load<Texture2D>("blank");

            // Tell each of the screens to load their content.
            foreach (GameScreen screen in screens)
            {
                screen.Activate(false);
            }
        }
        /// <summary>
        /// Tells each screen to draw itself.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
          
            GraphicsDevice.Clear(Color.Black);
            //spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
            
           foreach (GameScreen screen in screens)
            {
            
                if (!(screen.ScreenState == ScreenState.Hidden))
                    screen.Draw(gameTime);
              
                    //continue;               
            }
            //spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);

           //spriteBatch.End();
        }

        /// <summary>
        /// Adds a new screen to the screen manager.
        /// </summary>
        public void AddScreen(GameScreen screen)
        {
         
            screen.ScreenManager = this;
            screen.IsExiting = false;

            // If we have a graphics device, tell the screen to load content.
            if (isInitialized)
            {
                screen.Activate(false);
            }

            screens.Add(screen);

        }

        /// <summary>

        /// </summary>
        public GameScreen[] GetScreens()
        {
            return screens.ToArray();
        }

        public void RemoveScreen(GameScreen screen)
        {
            //unload content
            if (isInitialized)
            {
                screen.Unload();
            }

            screens.Remove(screen);
            tempScreensList.Remove(screen);

        }


        /// <summary>

        /// </summary>
        public void FadeBackBufferToBlack(float alpha)
        {
            spriteBatch.End();
            spriteBatch.Begin();
            spriteBatch.Draw(blankTexture, GraphicsDevice.Viewport.Bounds, Color.Black * alpha);
            //spriteBatch.End();
        }
    }
}