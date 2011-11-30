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

namespace prueba1
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {

        readonly GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        SpriteFont font;
        private GraphicsDevice device;
        public static int screenWidth;
        public static int screenHeight;
        public static Texture2D backgroundViewTex;
        public static Texture2D backgroundCollisionTex;
        public static int scaleFactor = 2;


        private Player player;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        protected override void Initialize()
        {


            graphics.PreferredBackBufferWidth = 184 * scaleFactor;
            graphics.PreferredBackBufferHeight = 145 * scaleFactor;
            graphics.IsFullScreen = true;

            graphics.ApplyChanges();
            Window.Title = "Prueba1";

            player = new Player
            {
                Pos = new Vector2(20, 20),
                State = "Idle",
                GravityForce = .5f,
                Gravity = 0,
                JumpForce = -2,
                jumpDecr = .75f,
                Velocity = 0,
                MaxVelocity = 1,
                Acceleration = 1,
                jump = 0,
                jumpKeyPressed = 0,
                Tex = new AnimatedSprite(Content.Load<Texture2D>("playeranim"), 0, 16, 32),


            };

            base.Initialize();
        }
        protected override void LoadContent()
        { // TODO: use this.Content to load your game content here
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("font");
            device = graphics.GraphicsDevice;

            backgroundViewTex = Content.Load<Texture2D>("bg1texture");
            backgroundCollisionTex = Content.Load<Texture2D>("bg1collision");
            screenWidth = device.PresentationParameters.BackBufferWidth;
            screenHeight = device.PresentationParameters.BackBufferHeight;


        }
        protected override void UnloadContent()
        {// TODO: Unload any non ContentManager content here
        }
        protected override void Update(GameTime gameTime)
        {

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();
            if (Keyboard.GetState().IsKeyDown(Keys.Tab))
                player.Pos.X = 20;
            KeyInput.Player(player);

            player.Tex.State = player.State;

            player.Tex.HandleSpriteMovement(gameTime);
            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            Matrix globalTransformation = Matrix.CreateScale(scaleFactor, scaleFactor, 1);

            globalTransformation.Translation = new Vector3((screenWidth - backgroundViewTex.Width * scaleFactor) / 2, (screenHeight - backgroundViewTex.Height * scaleFactor) / 2, 0);
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, globalTransformation);

            DrawBackground();
            DrawPlayer();
            DrawText();

            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void DrawText()
        {//LO DEJO DE GUIA POR SI NECESITAMOS ESCRIBIR TEXTO :p
            spriteBatch.DrawString(font, "State: " + player.State, new Vector2(10, (int)(screenHeight / 6)), Color.Red);
        }

        private void DrawBackground()
        {
            Vector2 bgpos = new Vector2(0, 0);
            //var screenRectangle = new Rectangle(0, 0, screenWidth, screenHeight);
            spriteBatch.Draw(backgroundViewTex, bgpos, Color.White);
        }

        private void DrawPlayer()
        {
            //spriteBatch.Draw(player.Tex.Texture, player.Pos, player.Tex.SourceRect, Color.White, 0f, player.Tex.Origin, 1.0f, SpriteEffects.None, 0);
            spriteBatch.Draw(player.Tex.Texture, player.Pos, player.Tex.SourceRect, Color.White, 0f, player.Tex.Origin, 1.0f, SpriteEffects.None, 0);
        }
    }
}