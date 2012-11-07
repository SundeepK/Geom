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
    class DeathParticle : Particle
    {

        Random random;

        public DeathParticle(Texture2D texture, Vector2 position, Vector2 velocity,
            float angle, float angularVelocity, Color color, float size, int ttl, Vector2 emission)
            : base( texture,  position,  velocity,
             angle,  angularVelocity,  color,  size,  ttl,  emission)
        { 
        
        random = new Random();
        
        }

        public  Particle CreateNewParticle(Texture2D tex, Vector2 pos)
        {

           Vector2 velocity = new Vector2(3.0f * (float)(random.NextDouble() * 2 - 1), 3.0f * (float)(random.NextDouble() * 2 - 1));

            float angularVelocity = 0;
            Color color = new Color((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble());
            float size = (float)random.NextDouble();
            int ttl = 40 + random.Next(60);
            float angle = random.Next(90, 180);

            return new Particle(tex, pos, velocity, angle, angularVelocity, color, 1, ttl, pos);

        }

        public void setDeathPosition(Vector2 pos)
        {
            Position = pos;

        }

        public override void Update()
        {
            TTL--;
            Position += Velocity / 2;
           if (TTL <=  20)
            {
                Size -= Size / 10;
                Position -= (Velocity *1.5f);
            }

           if (TTL >= copyttl - 5)
           {

               Position += (Velocity * 1.5f);

           }

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Rectangle sourceRectangle = new Rectangle(0, 0, Texture.Width, Texture.Height);
            Vector2 origin = new Vector2(Texture.Width / 2, Texture.Height / 2);

            //float angle = 0;
            spriteBatch.Draw(Texture, Position, null, Color, Angle, origin, Size, SpriteEffects.None, 0f);

        }

    }
}
