using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace geom
{
    class Bullet : GameObject
    {
        private float angle = 0;
        private Vector2 direction;
        public bool isRemovable = false;

        public Bullet(Vector2 pos, Vector2 dir)
            : base(pos)
        {


            texture = GameServices.GetService<ContentManager>().Load<Texture2D>(@"Defaultbullet");

            direction = dir - position;

            Vector2 dirCopy = dirCopy = direction;

            angle = (float)Math.Atan2(dirCopy.Y, dirCopy.X);
            CreateBoundingShape();
            direction.Normalize();


        }

        public override void Update(GameTime gameTime)
        {

            int height = GameServices.GetService<GraphicsDevice>().Viewport.Height;
            int width = GameServices.GetService<GraphicsDevice>().Viewport.Width;

            float speed = 2.0f;
            position += direction * speed * (float)gameTime.ElapsedGameTime.Milliseconds;

            if ((position.Y > height || position.Y < 0 || position.X > width || position.X < 0) && !isRemovable)
                    isRemovable = true;

            base.Update(gameTime);



        }


        public override void LoadContent()
        {

            //texture = GameServices.GetService<ContentManager>().Load<Texture2D>(@"Defaultbullet");

  
        }

        public override void UnloadContent()
        {

        }




        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {



            Rectangle source = new Rectangle((int)position.X, (int)position.Y, (int)texture.Width, (int)texture.Height);
            spriteBatch.Draw(texture, position, null, Color.White, angle ,
                new Vector2(texture.Width / 2, texture.Height / 2), 1.0f, SpriteEffects.None, 0);
        }


    }
}
