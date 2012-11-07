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
    public class Particle
    {
        public Texture2D Texture { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public Vector2 Emission { get; set; }
        public float Angle { get; set; }
        public float AngularVelocity { get; set; }
        public Color Color { get; set; }
        public float Size { get; set; }
        public int TTL { get; set; }
        public int copyttl;
        Random random;

        public Particle(Texture2D texture, Vector2 position, Vector2 velocity,
            float angle, float angularVelocity, Color color, float size, int ttl, Vector2 emission)
        {
            random = new Random();
            Texture = texture;
            Position = position;
            Velocity = velocity;
            Angle = angle;
            AngularVelocity = 0;
            Color = color;
            Size = size * 1.2f;
            TTL = ttl;
            copyttl = ttl;
            Emission = emission;

        }

        public virtual void Update()
        {
            TTL--;
            Position += Velocity;

            if (TTL <= 20)
            {

                Size -= Size / 10;

                Position -= (Velocity * 0.5f);
            }

            if (TTL >= copyttl - 20)
            {

                Position += (Velocity * 2f);

            }

            if (TTL >= copyttl - 5)
            {

                //Position += (Velocity * 5.0f);

            }

            //Angle += AngularVelocity;
        }

        //interesting effect 
        //        Rectangle sourceRectangle = new Rectangle(0, 0, Texture.Width, Texture.Height);
        //Vector2 origin = new Vector2(Texture.Width / 2, Texture.Height / 2);

        //Vector2 direction = Position - Emission;

        //float angle = (float)Math.Atan2(direction.Y, direction.X);

        //Vector2 offset = new Vector2(0, 0);

        //offset = Emission + ((new Vector2((float)Math.Sin(angle), -(float)Math.Cos(angle))) * 20.0f);

        //spriteBatch.Draw(Texture, offset, null, Color, angle, origin, Size, SpriteEffects.None, 0f);

        //original 
        //        Rectangle sourceRectangle = new Rectangle(0, 0, Texture.Width, Texture.Height);
        //Vector2 origin = new Vector2(Texture.Width / 2, Texture.Height / 2);

        //Vector2 direction = Position - Emission;

        //float angle = (float)Math.Atan2(direction.Y, direction.X);

        //Vector2 offset = new Vector2(0, 0);

        //offset = Emission + ((new Vector2((float)Math.Sin(angle), -(float)Math.Cos(angle))) * 20.0f);

        //spriteBatch.Draw(Texture, Position, null, Color, angle, origin, Size, SpriteEffects.None, 0f);

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            Rectangle sourceRectangle = new Rectangle(0, 0, Texture.Width, Texture.Height);
            Vector2 origin = new Vector2(Texture.Width / 2, Texture.Height / 2);

            Vector2 direction = Position - Emission;

            float angle = (float)Math.Atan2(direction.Y, direction.X);

            Vector2 offset = new Vector2(0, 0);

            offset = Emission + ((new Vector2((float)Math.Sin(angle), -(float)Math.Cos(angle))) * 20.0f);

            spriteBatch.Draw(Texture, Position, null, Color, angle, origin, Size, SpriteEffects.None, 0f);

        }
    }
}
