using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace geom
{
    class Player : GameObject
    {

        float speed = 0.3f;
        float degreesToMouse = 0;
        Vector2 mousePosition;

        public List<Bullet> bulletList;

        private double frameTimer = 0f;
        int height;
        int width;


        public Player(Vector2 pos)
            : base(pos)
        {

            bulletList = new List<Bullet>();

        
        }

        public override void Update(GameTime gameTime)
        {



            MouseState mouse = Mouse.GetState();

            mousePosition = new Vector2(mouse.X, mouse.Y);

            Vector2 direction = mousePosition - position;
            direction.Normalize();

            degreesToMouse = (float)Math.Atan2((double)direction.Y, (double)direction.X);

            HandleInputs(gameTime);

            frameTimer += (gameTime.ElapsedGameTime.TotalMilliseconds);

            if (mouse.LeftButton == ButtonState.Pressed && (frameTimer > 100))
            {
                frameTimer = 0;
                bulletList.Add(new Bullet(position, mousePosition));           
            }

            if (bulletList.Count > 0)
            {
                foreach (Bullet bullet in bulletList)
                {
                    bullet.Update(gameTime);
                }
            }

            for (int i = 0; i < bulletList.Count; i++)
            {
                if (bulletList[i].isRemovable)
                    bulletList.RemoveAt(i);

            }

            //Console.WriteLine(bulletList.Count);

            base.Update(gameTime);
       
        }

        private void HandleInputs(GameTime gameTime)
        {

            KeyboardState input = Keyboard.GetState();
            if(input.IsKeyDown(Keys.W)){
                position.Y -= (float)gameTime.ElapsedGameTime.Milliseconds * speed;
            }
            if (input.IsKeyDown(Keys.A))
            {
                position.X -= (float)gameTime.ElapsedGameTime.Milliseconds * speed;
            }
            if (input.IsKeyDown(Keys.S))
            {
                position.Y += (float)gameTime.ElapsedGameTime.Milliseconds * speed;
            }
            if (input.IsKeyDown(Keys.D))
            {
                position.X += (float)gameTime.ElapsedGameTime.Milliseconds * speed;
            }

     


        }

        public override void LoadContent()
        {

            texture = GameServices.GetService<ContentManager>().Load<Texture2D>(@"player copy");

          CreateBoundingShape();
        }

        public override void UnloadContent()
        {

        }


        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Rectangle source = new Rectangle((int)position.X, (int)position.Y, (int)texture.Width, (int)texture.Height);
           
            //spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
                        
            spriteBatch.Draw(texture, position, null, Color.White, 0, 
                new Vector2(texture.Width / 2, texture.Height / 2), 1.0f, SpriteEffects.None, 0);
                
                //new Vector2(200,200), null,  Color.White, degreesToMouse, new Vector2(0,0), SpriteEffects.None,0);

            //base.Draw(gameTime, spriteBatch);


            if (bulletList.Count > 0)
            {

                foreach (Bullet bullet in bulletList)
                {

                    bullet.Draw(gameTime, spriteBatch);
                }
            }

            //spriteBatch.End();

        }

    }
}
