using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace geom
{
    class Bloom : DrawableGameComponent
    {

        SpriteBatch spriteBatch;
        Effect bloomEffect;
        Effect bloomCombine;
        Effect blurEffect;

        RenderTarget2D sceneRenderT;
        RenderTarget2D renderT1;
        RenderTarget2D renderT2;
        

        public enum IntermediateBuffer
        {
            PreBloom,
            BlurredHorizontally,
            BlurredBothWays,
            FinalResult,
        }

        public IntermediateBuffer ShowBuffer
        {
            get { return showBuffer; }
            set { showBuffer = value; }
        }

        IntermediateBuffer showBuffer = IntermediateBuffer.FinalResult;

        public Bloom(Game game)
            : base(game)
        {


        }


        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            bloomCombine = Game.Content.Load<Effect>("CombineBloom");
            bloomEffect = Game.Content.Load<Effect>("BloomEffect");
            blurEffect = Game.Content.Load<Effect>("Blur");

            PresentationParameters pp = GraphicsDevice.PresentationParameters;
            SurfaceFormat format = pp.BackBufferFormat;

            int width = pp.BackBufferWidth;
            int height = pp.BackBufferHeight;


            sceneRenderT = new RenderTarget2D(GraphicsDevice, width, height, false,
                format, pp.DepthStencilFormat, pp.MultiSampleCount,
                RenderTargetUsage.DiscardContents);

            width /= 2;
            height /= 2;

            renderT1 = new RenderTarget2D(GraphicsDevice, width, height, false, format, DepthFormat.None);
            renderT2 = new RenderTarget2D(GraphicsDevice, width, height, false, format, DepthFormat.None);


        }


        public override void Initialize()
        {
            base.Initialize();

        }

        protected override void UnloadContent()
        {
            sceneRenderT.Dispose();
            renderT1.Dispose();
            renderT2.Dispose();
        }

        public void BeginDraw()
        {
            if (Visible)
            {
                GraphicsDevice.SetRenderTarget(sceneRenderT);
            }
        }


        private void DrawFullScreenQuad(Texture2D tex, RenderTarget2D target, Effect effect, IntermediateBuffer buffer)
        {

            GraphicsDevice.SetRenderTarget(target);

            DrawFullscreenQuad(tex, target.Width, target.Height, effect, buffer);
        }

        private void DrawFullscreenQuad(Texture2D texture, int width, int height,
                                 Effect effect, IntermediateBuffer currentBuffer)
        {

            if (showBuffer < currentBuffer)
            {
                effect = null;
            }

            spriteBatch.Begin(0, BlendState.Opaque, null, null, null, effect);
            spriteBatch.Draw(texture, new Rectangle(0, 0, width, height), Color.White);
            spriteBatch.End();

        }

        public override void Draw(GameTime gameTime)
        {
            //bloomEffect.Parameters["bloomThreshold"].SetValue(BloomSettings.bloomThreshold);
            DrawFullScreenQuad(sceneRenderT, renderT1, bloomEffect, IntermediateBuffer.PreBloom);

            SetBlurParameters(1.0f / (float)renderT1.Width, 0);

            DrawFullScreenQuad(renderT1, renderT2,
                               blurEffect,
                               IntermediateBuffer.BlurredHorizontally);

            SetBlurParameters(0, 1.0f / (float)renderT2.Height);

            DrawFullScreenQuad(renderT2, renderT1,
                         blurEffect,
                         IntermediateBuffer.BlurredBothWays);

            GraphicsDevice.SetRenderTarget(null);

            EffectParameterCollection parameters = bloomCombine.Parameters;
            parameters["bloomInt"].SetValue(BloomSettings.baseIntensity);
            parameters["baseInt"].SetValue(BloomSettings.baseIntensity);

            parameters["bloomSat"].SetValue(BloomSettings.bloomSaturation);

            parameters["baseSat"].SetValue(BloomSettings.baseSaturation);
            
            GraphicsDevice.Textures[1] = sceneRenderT;

            Viewport viewport = GraphicsDevice.Viewport;

            DrawFullscreenQuad(renderT1,
                               viewport.Width, viewport.Height,
                               bloomCombine,
                               IntermediateBuffer.FinalResult);

            base.Draw(gameTime);
        }

        private void SetBlurParameters(float x, float y)
        {
            EffectParameter weight, offset;

            weight = blurEffect.Parameters["weights"];
            offset = blurEffect.Parameters["offsetForPixels"];

            int sampleCount = 15;
            float[] sampleWeight = new float[sampleCount];
            Vector2[] sampleOffsets = new Vector2[sampleCount];
            sampleWeight[0] = ComputeGaussian(0);
            sampleOffsets[0] = new Vector2(0);

            float totalWeights = sampleWeight[0];

            for (int i = 0; i < sampleCount/2; i++)
            {
                float gWeight = ComputeGaussian(i + 1);
                sampleWeight[i * 2 + 1] = gWeight;
                sampleWeight[i * 2 + 2] = gWeight;

                totalWeights += gWeight * 2;
                float sampleOffset = i * 2 + 1.5f;
                Vector2 delta = new Vector2(x, y) * sampleOffset;
                sampleOffsets[i * 2 + 1] = delta;
                sampleOffsets[i * 2 + 2] = -delta;

            }

            for (int i = 0; i < sampleWeight.Length; i++)
            {
                sampleWeight[i] /= totalWeights;
            }

            weight.SetValue(sampleWeight);
            offset.SetValue(sampleOffsets);

        }

       private float ComputeGaussian(float n)
        {
            float theta = BloomSettings.blurAmount;

            return (float)((1.0 / Math.Sqrt(2 * Math.PI * theta)) *
                           Math.Exp(-(n * n) / (2 * theta * theta)));
        }





    }
}
