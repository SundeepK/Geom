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
    class LevelManager
    {
        public List<Enemy> enemyList { get; private set; }
        Random random;
        double frameTimer =0;
        float elsTime = 0;
        Player player;
        public int highScore { get; private set; }
        public List<float> peaks = new List<float>();
        public List<float> peaksCopy;
        float pl = 0;
        public SoundEffect  song;
        public Song songs;

        

        public bool isPlaying = false;

        public float bloomSat = 0.0f;
        string uri;
        public LevelManager(string uri)
        {
            enemyList = new List<Enemy>();
            random = new Random();
            this.uri = uri;
        }

        public void Initialize()
        {
            
            player = new Player(new Vector2(500, 500));
        }

        public void LoadContent()
        {
            
            player.LoadContent();
            int THRESHOLD_WINDOW_SIZE = 10;
            float MULTIPLIER = 1.5f;




            //WAVDecoder wavDecoder = new WAVDecoder("down.wav");
            WAVDecoder wavDecoder = new WAVDecoder(uri);
            long startTime = 0;

            FFT fft = new FFT(1024, 44100);
            fft.window(FFT.HAMMING);
            float[] samples = new float[1024];
            float[] spectrum = new float[1024 / 2 + 1];
            float[] lastSpectrum = new float[1024 / 2 + 1];
            List<float> spectralFlux = new List<float>();

            List<float> threshold = new List<float>();
            List<float> prunnedSpectralFlux = new List<float>();



            while (wavDecoder.readSamples(samples) > 0)
            {
                fft.forward(samples);
                Array.Copy(spectrum, 0, lastSpectrum, 0, spectrum.Length);
                Array.Copy(fft.getSpectrum(), 0, spectrum, 0, spectrum.Length);

                float flux = 0;
                for (int i = 0; i < spectrum.Length; i++)
                {
                    float value = (spectrum[i] - lastSpectrum[i]);
                    flux += value < 0 ? 0 : value;
                }
                spectralFlux.Add(flux);
            }

            for (int i = 0; i < spectralFlux.Count(); i++)
            {
                int start = Math.Max(0, i - THRESHOLD_WINDOW_SIZE);
                int end = Math.Min(spectralFlux.Count() - 1, i + THRESHOLD_WINDOW_SIZE);
                float mean = 0;
                for (int j = start; j <= end; j++)
                    mean += spectralFlux[j];
                mean /= (end - start);
                threshold.Add(mean * MULTIPLIER);
            }

            for (int i = 0; i < threshold.Count(); i++)
            {
                if (threshold[i] <= spectralFlux[i])
                    prunnedSpectralFlux.Add(spectralFlux[i] - threshold[i]);
                else
                    prunnedSpectralFlux.Add((float)0);
            }

            int count = 0;
            for (int i = 0; i < prunnedSpectralFlux.Count() - 1; i++)
            {
                if (prunnedSpectralFlux[i] > prunnedSpectralFlux[i + 1])
                {
                    peaks.Add(prunnedSpectralFlux[i]);
                    count++;

                }
                else
                {
                    peaks.Add((float)0);
                }
            }


            //Array.Copy(
           
            peaksCopy = new List<float>(peaks);

            //Console.WriteLine(count + " "  + threshold.Count());
            //texture = GameServices.GetService<ContentManager>().Load<Texture2D>(@"player copy");
            //songs = GameServices.GetService<ContentManager>().Load<Song>("audio/quiet");
            songs = Song.FromUri("music", new Uri(@uri));
            //song = GameServices.GetService<ContentManager>().Load<SoundEffect>("audio/down");
            //song.Play();
            //MediaPlayer.Play(songs);
            MediaPlayer.Play(songs);
            MediaPlayer.Stop();
        }

        public void Update(GameTime gameTime, int score)
        {


            if (!isPlaying)
            {
                MediaPlayer.Play(songs);
                //song.Play();
                isPlaying = true;
            }

            pl = MediaPlayer.PlayPosition.Milliseconds /1000.0f;
            Console.WriteLine("pllll " + pl);


            

            if ( (pl > 0))
            {

                //elsTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f;
                elsTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f;
                int position = (int)(elsTime * (44100 / 1024));




                //    if (peaks[position] > 0)
                //{
                //Console.WriteLine("spawn " + position);
                //Console.WriteLine( "els time " + elsTime);

                BloomSettings.bloomThreshold = 0.20f;
                BloomSettings.blurAmount = 3f;
                BloomSettings.baseIntensity = 2.5f;

                if (position > peaks.Count())
                {

                }


                if (peaks[position] > 10)
                {
                    //Console.WriteLine("spawn " + position);

                    Color color = Color.White;

                    if (peaks[position] > 150)
                    {
                        BloomSettings.bloomThreshold = 0.10f;
                        BloomSettings.blurAmount = 2f;
                        BloomSettings.baseIntensity = 6f;
                    }

                    if (peaks[position] < 50)
                    {
                        color = Color.White;
                    }
                    else if (peaks[position] > 150 && peaks[position] < 350)
                    {
                        color = new Color(165, 252, 255);
                    }
                    else if (peaks[position] > 350 && peaks[position] < 650)
                    {
                        color = Color.PeachPuff;
                    }
                    else if (peaks[position] > 650)
                    {
                        color = Color.PaleVioletRed;

                    }

                    enemyList.Add(new Enemy(new Vector2(random.Next(10, 1100), random.Next(10, 760)), color));
                    peaks[position] = 0;

                }

                frameTimer += (gameTime.ElapsedGameTime.TotalMilliseconds);

                player.Update(gameTime);
                PerformPlayerEnemyCol();

                for (int i = 0; i < enemyList.Count; i++)
                {
                    enemyList[i].Update(gameTime, player.position);
                }


                KeyboardState input = Keyboard.GetState();
                if (input.IsKeyDown(Keys.B))
                {
                    foreach (Enemy e in enemyList)
                    {
                        e.isDead = true;
                    }
                }
            }



            
        }

        private void AddNormalEnemies()
        {
            for (int i = 0; i < 3; i++ )
            {
                enemyList.Add(new Enemy(new Vector2(random.Next(10, 1100), random.Next(10, 760)), Color.White));
            }
        }

        private void PerformPlayerEnemyCol()
        {
          int  height = GameServices.GetService<GraphicsDevice>().Viewport.Height;
          int  width = GameServices.GetService<GraphicsDevice>().Viewport.Width;

            for (int i = 0; i < enemyList.Count; i++)
            {

                for (int j = 0; j < player.bulletList.Count; j++)
                {

                    if (player.bulletList[j].isColliding(enemyList[i]) && !enemyList[i].isDead)
                    {

                        highScore += enemyList[i].score;
                        enemyList[i].isDead = true;
                        player.bulletList[j].isRemovable = true; 
                    }
                }
            }

            for (int i = 0; i < enemyList.Count; i++)
            {
                if (enemyList[i].readyToRemove)
                {
                    enemyList.RemoveAt(i);
                }

            }

   
        
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);

            //spriteBatch.End();
          
            int position = (int)(elsTime * (44100 / 1024));

            float var = 0.05f;
            if (peaksCopy[position] > 20)
            {
                var = 0.1f;
            }

            double time = gameTime.TotalGameTime.TotalSeconds;
            float pulsate = (float)Math.Sin(time * 6) + 1;
            float scale = 1 + pulsate * var;


            for (int i = 0; i < enemyList.Count; i++)
            {
                enemyList[i].Draw(gameTime, spriteBatch, scale);
            }
         
            player.Draw(gameTime, spriteBatch);
    
        }


    }
}
