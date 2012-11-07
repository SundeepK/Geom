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
    public abstract class GameObject 
    {
       public Vector2 position;
       protected Texture2D texture;

       public Rectangle collisionShape;
        public GameObject(Vector2 pos)
        {
           position = pos;
           collisionShape = new Rectangle(0, 0, 0, 0);
        
        }

        protected void CreateBoundingShape()
        {
        
          int x = (int)Math.Round(position.X - (texture.Width / 2));
          int y = (int)Math.Round(position.Y - (texture.Height / 2));
           collisionShape = new Rectangle(x, y, texture.Width, texture.Height);
        
        }

        protected void UpdateBoundingShape()
        {
            collisionShape.X = (int)Math.Round(position.X - (texture.Width / 2));
            collisionShape.Y = (int)Math.Round(position.Y - (texture.Height / 2)); 
        }

        public virtual void Update(GameTime gameTime)
        {

            UpdateBoundingShape();
        
        }

        public virtual void LoadContent()
        {

        }

        public virtual void UnloadContent()
        {
        
        }


        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        { 
            Rectangle source = new Rectangle((int)position.X,(int) position.Y, (int)texture.Width, (int)texture.Height);
            spriteBatch.Draw(texture, position , source, Color.White);

        }



        public Boolean isColliding(GameObject collidableObject)
        {

            return  this.collisionShape.Intersects(collidableObject.collisionShape) ? true : false;
        }
    }
}
