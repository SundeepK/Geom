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
    class Enemy : GameObject
    {

        protected float damage;
        protected float rotation = 0;
        public bool isDead = false;
        public bool readyToRemove = false;
        public bool isExplo = false;
        List<DeathParticle> dpList;
        Random random;
        public Color color;
        private float pointsScale = 1.0f;
        public double frameTimer;
        private Texture2D pointsTex;
        public int score { get; protected set; }
        float angle = 0;
        float _timeline = 0;
        Vector2 wave;
        float waveAngle = 0;
        ParticleEngine particleEng;


        public Enemy(Vector2 pos, Color color)
            : base(pos)
        {
            texture = GameServices.GetService<ContentManager>().Load<Texture2D>(@"enemy2");
            pointsTex = GameServices.GetService<ContentManager>().Load<Texture2D>(@"50p");
            CreateBoundingShape();
            random = new Random();
            this.color = color;
            dpList = new List<DeathParticle>();
            score = 50;
            wave = new Vector2(0, 0);
            particleEng = new ParticleEngine(Vector2.Zero);
            particleEng.LoadContent();
        }

        public void Update(GameTime gameTime, Vector2 pos)
        {


            if (isDead == true)
            {

                frameTimer += (gameTime.ElapsedGameTime.TotalMilliseconds);
                pointsScale -= 0.01f * 2f;

                if (frameTimer > 2000)
                    readyToRemove = true;

                if (!isExplo)
                {
                    isExplo = true;
                    for (int i = 0; i < 4; i++)
                    {
                        Vector2 velocity = new Vector2((float)(random.NextDouble() * 2 - 1), (float)(random.NextDouble() * 2 - 1));
                        Texture2D texture1 = GameServices.GetService<ContentManager>().Load<Texture2D>(@"Deathline");
                        float angularVelocity = 0;
                        Color color = new Color((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble());
                        float size = (float)random.NextDouble();
                        int ttl = 40 + random.Next(60);
                        float angle = random.Next(1, 4);

                        dpList.Add(new DeathParticle(texture1, position + velocity * 20, velocity, angle, angularVelocity, new Color(87, 255, 87), 0.8f, ttl, position));

                    }

                    particleEng.EmitterLocation = position;
                    particleEng.frameTimer = 0;


                  
                }

                for (int i = 0; i < dpList.Count; i++)
                {
                    dpList[i].Update();
                }
            }
            else
            {
                //enemy only follows on one axis
                //position -= trackingPos * ((float)gameTime.ElapsedGameTime.Milliseconds + (float)random.Next(1, 30)) * 0.08f;
                //position.X = -(float)Math.Cos(position.Y / 100) + 800 / 2;

                //cool enemy effect 
                //angle += (float)gameTime.ElapsedGameTime.TotalSeconds * (float)Math.PI * 2f;
               //position.Y -= (float)(Math.Sin(angle) * 5) * trackingPos.Y;
                //position.X -= trackingPos.X * els * 0.05f;

                //Vector2 dir = target - position; // direction
                //dir.Normalize();
                //Vector2 perp( -dir.y, dir.x ); // perpendicular





                //wave like motion

                float els = gameTime.ElapsedGameTime.Milliseconds;
                float waveAmp = 5f; // adjust if needed
                 waveAngle += els * 3.14f * 8f; // adjust if needed
                Vector2 trackingPos = pos - position;
                   trackingPos.Normalize();
                Vector2 perp = new Vector2(trackingPos.X, -trackingPos.Y);

                wave = perp * (float)Math.Sin(waveAngle) * waveAmp;
                Vector2 vel = trackingPos * 0.08f;
                position += vel *  els + wave;


   
                //orig
                //angle += (float)gameTime.ElapsedGameTime.TotalSeconds * (float)Math.PI * 2f;

                //angle = (float)Math.Sin(gameTime.ElapsedGameTime.TotalSeconds);
                //Vector2 off = new Vector2(angle, angle);
                //position -= (trackingPos) * (float)gameTime.ElapsedGameTime.Milliseconds * 0.08f ;
               
                //position.Y -=   (float)(Math.Sin(angle) * 5) * trackingPos.Y;
                //position.X -= trackingPos.X * els * 0.05f;

                //offset = (float)Math.Sin(msElapsed) * radius;
                //Position.Y = center + offset;

                //position.Y += -(float)Math.Cos(position.X / 100);


                // _timeline += 0.01f;
                //if (_timeline >= 1.0f)
                //{
                //    _timeline = 0.01f;
                //}
                //position = GetPoint(_timeline, new Vector2(200, 200), new Vector2(100, 100), new Vector2(250, 250), pos) * els * 0.08f;
    
               
                //position -= position;

                UpdateBoundingShape();
            }

            particleEng.Update(gameTime);

        }


        public override void LoadContent()
        {

        }

        public override void UnloadContent()
        {

        }


        public  void Draw(GameTime gameTime, SpriteBatch spriteBatch, float scale)
        {

            Rectangle source = new Rectangle(0, 0, (int)texture.Width, (int)texture.Height);



            if (isDead == true)
            {
                for (int i = 0; i < dpList.Count; i++)
                {

                    dpList[i].Draw(spriteBatch);
                }

                if (frameTimer < 500)
                {

                    spriteBatch.Draw(pointsTex, position, source, color, rotation,
                new Vector2(texture.Width / 2, texture.Height / 2), pointsScale, SpriteEffects.None, 0);
                }

                particleEng.Draw(spriteBatch);


            }
            else
            {
                spriteBatch.Draw(texture, position, source, color, rotation,
                new Vector2(texture.Width / 2, texture.Height / 2), pointsScale * scale, SpriteEffects.None, 0);
            }

            //new Vector2(200,200), null,  Color.White, degreesToMouse, new Vector2(0,0), SpriteEffects.None,0);

            //base.Draw(gameTime, spriteBatch);
        }


    }
}
