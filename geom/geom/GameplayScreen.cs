
#region Using Statements
using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
#endregion

namespace geom
{
    /// <summary>
    /// This screen implements the actual game logic. 
    /// </summary>
    class GameplayScreen : GameScreen
    {


        ContentManager content;
        SpriteFont gameFont;

        Random random = new Random();

        float pauseAlpha;

        InputAction pauseAction;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Game game;

        //main
        List<Enemy> enemyList;
        Effect effect;
        Bloom bloom;
       LevelManager level;
       

        public float bloomSat = 0;


        /// <summary>
        /// Constructor.
        /// </summary>
        public GameplayScreen(Game game, string uri)
        {
            //TransitionOnTime = TimeSpan.FromSeconds(1.5);
            //TransitionOffTime = TimeSpan.FromSeconds(0.5);
          
            pauseAction = new InputAction(
                new Keys[] { Keys.Escape },
                true);

            bloom = new Bloom(game);
            game.Components.Add(bloom);

            this.game = game;
            
            enemyList = new List<Enemy>();
            level = new LevelManager(uri);
            level.Initialize();
          
        }


        /// <summary>
        /// Load graphics content for the game.
        /// </summary>
        public override void Activate(bool instancePreserved)
        {
            if (!instancePreserved)
            {
                if (content == null)
                    content = new ContentManager(ScreenManager.Game.Services, "Content");

                gameFont = content.Load<SpriteFont>("gamefont");

          

               effect = GameServices.GetService<ContentManager>().Load<Effect>(@"Effect");
                  level.LoadContent();

                // once the load has finished, we use ResetElapsedTime to tell the game's
                // timing mechanism that we have just finished a very long frame, and that
                // it should not try to catch up.
                ScreenManager.Game.ResetElapsedTime();
            }

        }


        public override void Deactivate()
        {

            base.Deactivate();
        }


        /// <summary>
        /// Unload graphics content used by the game.
        /// </summary>
        public override void Unload()
        {
            content.Unload();


        }


     

        #region Update and Draw


        /// <summary>
        /// Updates the state of the game. This method checks the GameScreen.IsActive
        /// property, so the game will stop updating when the pause menu is active,
        /// or if you tab away to a different application.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

           

            // Gradually fade in or out depending on whether we are covered by the pause screen.
            if (coveredByOtherScreen)
                pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
            else
                pauseAlpha = Math.Max(pauseAlpha - 1f / 32, 0);

            //if (IsActive)
            //{
               level.Update(gameTime, 100);

              //}
        }


        /// <summary>
        /// Lets the game respond to player input. Unlike the Update method,
        /// this will only be called when the gameplay screen is active.
        /// </summary>
        public override void HandleInput(GameTime gameTime, InputState input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            // Look up inputs for the active player profile.

            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.Escape))
            {

                ScreenManager.AddScreen(new PauseMenuScreen(game));

            }
        }


        /// <summary>
        /// Draws the gameplay screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
          
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            GraphicsDevice gd = ScreenManager.GraphicsDevice;

            bloom.BeginDraw();
            gd.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
            level.Draw(gameTime, spriteBatch);
          

            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }
            spriteBatch.End();
        }
        


        #endregion
    }
}
