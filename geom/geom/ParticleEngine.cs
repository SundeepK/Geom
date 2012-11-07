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
    class ParticleEngine
    {

        private Random random;
        public Vector2 EmitterLocation { get; set; }
        private List<Particle> particles;
        private List<Texture2D> textures;
        public double frameTimer = 301f;
        public int total = 15;
        Color color;

        public ParticleEngine(Vector2 pos)
        {

            EmitterLocation = pos;
            particles = new List<Particle>();
            textures = new List<Texture2D>();
            random = new Random();

            //color = new Color(243, 206, 34);
            color = ColorGenerator.getRanColor();
            Texture2D line = GameServices.GetService<ContentManager>().Load<Texture2D>(@"lineglow");
            Texture2D line2 = GameServices.GetService<ContentManager>().Load<Texture2D>(@"lineglowsmall");
            Texture2D line3 = GameServices.GetService<ContentManager>().Load<Texture2D>(@"lineglowmed");
            textures.Add(line);
            textures.Add(line2); 
            textures.Add(line3);
        
        }

        public void LoadContent()
        {


            //Texture2D line = GameServices.GetService<ContentManager>().Load<Texture2D>(@"line");
            //textures.Add(line);
        }

        public void Update(GameTime gameTime)
        {


            frameTimer += (gameTime.ElapsedGameTime.TotalMilliseconds);

            if (!(frameTimer > 250))
            {

                for (int i = 0; i < total; i++)
                {
                    particles.Add(CreateNewParticle());
                }
            }

            for (int particle = 0; particle < particles.Count; particle++)
            {
                particles[particle].Update();
                if (particles[particle].TTL <= 0)
                {
                    particles.RemoveAt(particle);
                    particle--;
                }
            }

        }



        //cool circule mition
            //        Texture2D tex = textures[0];
            //Texture2D texture = textures[0];
            //Vector2 position = EmitterLocation;
            //Vector2 velocity = new Vector2(3.0f * (float)(random.NextDouble() * 2 - 1), 3.0f * (float)(random.NextDouble() * 2 - 1));
        
            //float angularVelocity = 0;
            //Color color = new Color((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble());
            //float size = (float)random.NextDouble();
            //int ttl = 20 + random.Next(40);

            //Vector2 offset = new Vector2(0, 0);

            //Vector2 direction = EmitterLocation - (velocity * 1000);

            //float angle = (float)Math.Atan2(direction.Y, direction.X);
            
            //offset = EmitterLocation + ((new Vector2((float)Math.Sin(angle), -(float)Math.Cos(angle))) * 2.30f);

            //return new Particle(texture, offset, velocity, angle, angularVelocity, color, size, ttl, EmitterLocation);


        private Particle CreateNewParticle()
        {
            int i = random.Next(0,2);
            Texture2D texture = textures[i];
            Vector2 position = EmitterLocation;
            Vector2 velocity = new Vector2(3.0f * (float)(random.NextDouble() * 2 - 1), 3.0f * (float)(random.NextDouble() * 2 - 1));
        
            float angularVelocity = 0;
            
            float size = (float)random.NextDouble();
            int ttl = 30 + random.Next(50);

            Vector2 offset = new Vector2(0, 0);

             offset =   ( EmitterLocation + (velocity ));

             Vector2 direction = offset - EmitterLocation;
           
            float angle = (float)Math.Atan2(direction.Y, direction.X);
            
            Vector2 n = EmitterLocation + ((new Vector2((float)Math.Sin(angle), -(float)Math.Cos(angle))) * (float)random.Next(7, 20));
            //blue new Color(9,255, 229)
            //red 255,40,40
            // green 12 255 12
            // yellow 253 240 35
            return new Particle(texture, n, velocity, 0, angularVelocity, color, size, ttl, EmitterLocation);
   
        }


        public void AddParticleToList()
        {

            Texture2D texture = textures[0];
            Vector2 position = EmitterLocation;
            Vector2 velocity = new Vector2(3.0f * (float)(random.NextDouble() * 2 - 1), 3.0f * (float)(random.NextDouble() * 2 - 1));

            float angularVelocity = 0;
            Color color = new Color((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble());
            float size = (float)random.NextDouble();
            int ttl = 30 + random.Next(50);

            Vector2 offset = new Vector2(0, 0);

            offset = (EmitterLocation + (velocity));

            Vector2 direction = offset - EmitterLocation;

            float angle = (float)Math.Atan2(direction.Y, direction.X);

            Vector2 n = EmitterLocation + ((new Vector2((float)Math.Sin(angle), -(float)Math.Cos(angle))) * (float)random.Next(7, 18));

            particles.Add(new Particle(texture, n, velocity, 0, angularVelocity, color, size, ttl, EmitterLocation));

        }

        public void AddEnemyDeathParticle()
        {

            Texture2D texture = textures[0];
            Vector2 position = EmitterLocation;
            Vector2 velocity = new Vector2(3.0f * (float)(random.NextDouble() * 2 - 1), 3.0f * (float)(random.NextDouble() * 2 - 1));

            float angularVelocity = 0;
            Color color = new Color((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble());
            float size = (float)random.NextDouble();
            int ttl = 30 + random.Next(50);

            float angle = random.Next(1, 4);

           particles.Add(new Particle(texture, position, velocity, angle, angularVelocity, color, 1.5f, ttl, EmitterLocation));
        
        }

        public void Draw(SpriteBatch spriteBatch)
        {

            for (int index = 0; index < particles.Count; index++)
            {
                particles[index].Draw(spriteBatch);
            }

        }

    }
}
