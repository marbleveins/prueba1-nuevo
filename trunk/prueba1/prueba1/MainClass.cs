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
    public class MainClass : Microsoft.Xna.Framework.Game
    {

        readonly GraphicsDeviceManager graphics;
        private SpriteBatch SpriteBatch;
        private SpriteFont font;
        private GraphicsDevice device;
        public static int screenWidth;
        public static int screenHeight;
        public static Texture2D backgroundViewTex;
        public static Texture2D backgroundCollisionTex;
        public static int scaleFactor = 2;
        private Draw draw;


        private Player player;

        public MainClass()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        protected override void Initialize()
        {


            graphics.PreferredBackBufferWidth = 184 * scaleFactor;
            graphics.PreferredBackBufferHeight = 145 * scaleFactor;
            graphics.IsFullScreen = false;

            graphics.ApplyChanges();
            Window.Title = "Prueba1";

            player = new Player
                         {
                             Pos = new Vector2(20, 20),
                             State = "falling",
                             JumpPower = 3f,
                             GravityForce = .15f,
                             Gravity = 0,
                             Velocity = 0,
                             MaxVelocity = 1.4f,
                             Acceleration = .2f,
                             Tex = new AnimatedSprite(Content.Load<Texture2D>("playeranim"), 0, 16, 32),


                         };
            draw = new Draw();

            base.Initialize();
        }
        protected override void LoadContent()
        { // TODO: use this.Content to load your game content here
            SpriteBatch = new SpriteBatch(GraphicsDevice);
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
            player.Move();

            player.Tex.State = player.State;

            player.Tex.HandleSpriteMovement(gameTime);
            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            Matrix globalTransformation = Matrix.CreateScale(scaleFactor, scaleFactor, 1);

            globalTransformation.Translation = new Vector3((screenWidth - backgroundViewTex.Width * scaleFactor) / 2, (screenHeight - backgroundViewTex.Height * scaleFactor) / 2, 0);
            SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, globalTransformation);
            
            draw.Background(SpriteBatch, backgroundViewTex);
            draw.Player(SpriteBatch, player);
            draw.Text(SpriteBatch, player, font);

            SpriteBatch.End();
            base.Draw(gameTime);
        }
    }
}