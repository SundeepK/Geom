#region File Description
//-----------------------------------------------------------------------------
// MenuEntry.cs
//
// XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


#endregion

namespace geom
{

    class MenuEntry
    {

        string text;

       Vector2 position;

        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public event EventHandler Selected;

        protected internal virtual void OnSelectEntry()
        {
            if (Selected != null)
                    Selected(this, new EventArgs());
            
        }

        #region Initialization


        /// <summary>
        /// 
        /// </summary>
        public MenuEntry(string text)
        {
            this.text = text;
        }


        #endregion

        #region Update and Draw


        /// <summary>
        /// Updates the menu entry.
        /// </summary>
        public virtual void Update(MenuScreen screen, bool isSelected, GameTime gameTime)
        {
            // there is no such thing as a selected item on Windows Phone, so we always
            // force isSelected to be false

            // When the menu selection changes, entries gradually fade between
            // their selected and deselected appearance, rather than instantly

            // popping to the new state.
            //float fadeSpeed = (float)gameTime.ElapsedGameTime.TotalSeconds * 4;

            //if (isSelected)
            //    selectionFade = Math.Min(selectionFade + fadeSpeed, 1);
            //else
            //    selectionFade = Math.Max(selectionFade - fadeSpeed, 0);
        }


        /// <summary>
        /// Draws the menu entry. This can be overridden to customize the appearance.
        /// </summary>
        public virtual void Draw(MenuScreen screen, bool isSelected, GameTime gameTime)
        {

           

           
            Color color = isSelected ? Color.Yellow : Color.White;


            // Modify the alpha to fade text out during transitions.
            color *= screen.TransitionAlpha;

            // Draw text, centered on the middle of each line.
            ScreenManager screenManager = screen.ScreenManager;
            SpriteBatch spriteBatch = screenManager.SpriteBatch;
            SpriteFont font = screenManager.Font;
            //spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
            Vector2 origin = new Vector2(0, font.LineSpacing / 2);

            spriteBatch.DrawString(font, text, position, color, 0,
                                   origin, 1f, SpriteEffects.None, 0);
            //spriteBatch.End();
        }


        /// <summary>
        /// Queries how much space this menu entry requires.
        /// </summary>
        public virtual int GetHeight(MenuScreen screen)
        {
            return screen.ScreenManager.Font.LineSpacing;
        }


        /// <summary>
        /// Queries how wide the entry is, used for centering on the screen.
        /// </summary>
        public virtual int GetWidth(MenuScreen screen)
        {
            return (int)screen.ScreenManager.Font.MeasureString(Text).X;
        }


        #endregion
    }
}
