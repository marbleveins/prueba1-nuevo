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
    public class Player
    {
        //PRIVATE (INTERNAS DE LA CLASE)
        private int _jumpWaitTimer = 0;
        private const int JumpWaitMaxTime = 15;
        private int _preparingJumpTimer = 0;
        private const int PreparingJumpDuration = 10;
        //PUBLIC (PROPIEDADES DEL OBJETO)
        public AnimatedSprite Tex;
        public Vector2 Pos;
        public string State;
        public float Gravity;
        public float GravityForce;
        public float MaxVelocity;
        public float Velocity;
        public float Acceleration;
        public float JumpPower;
        public SpriteEffects Flipping = SpriteEffects.None;


        public void Move()
        {   //ECUACIONES M.R.U.V. REALES PARA APLICAR ALGUN DIA
            //Xf = X0 + v * t + 1/2@ * t2
            //Vf = V0 + @ * t
            //player.Pos.X = player.Pos.X + (player.Velocity * (1 / 60)) + (player.Acceleration / 2) * 2.7f;
            //-------------------------------

            var collision = new Collision(); // así existe sólo mientras el metodo esta corriendo.
            
            if (_jumpWaitTimer < JumpWaitMaxTime)
                _jumpWaitTimer++;

            if (_preparingJumpTimer > 0)
                _preparingJumpTimer++;

            // SECCION DE MOVIMIENTO VERTICAL 
            //SALTO
            if (Keyboard.GetState().IsKeyDown(Keys.Up) && Gravity == 0 && _jumpWaitTimer >= JumpWaitMaxTime)
                _preparingJumpTimer++;

            if (_preparingJumpTimer >= PreparingJumpDuration)
            {
                if (State != "falling")
                {
                    State = "jumping";
                    Jump();
                }
                _preparingJumpTimer = 0;
            }
            //-------------------------------

            Gravity += GravityForce;
            Pos.Y += (int)Gravity;

            if (Gravity > 0)
            {
                if (Gravity > GravityForce * 6 || !collision.EstaColisionando(Pos, Game1.backgroundCollisionTex, Color.Black, 0, (int)(MaxVelocity * 3)))
                {//CAMBIO DE jumping A falling
                    State = "falling";
                    _jumpWaitTimer = 0;
                }
            }

            
            while (collision.EstaColisionando(Pos, Game1.backgroundCollisionTex, Color.Black, 0, 0))
            {//PISANDO
                State = "idle";
                Pos.Y--;
                Gravity = 0;
                _jumpWaitTimer = JumpWaitMaxTime;
            }
            //-------------------------------

            // SECCION DE MOVIMIENTO HORIZONTAL
            if (State == "idle" || State == "running")
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                {
                    State = "running";
                    Flipping = SpriteEffects.None;
                    MoveRight();
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Left))
                {
                    State = "running";
                    Flipping = SpriteEffects.FlipHorizontally;
                    MoveLeft();
                }
                else
                {//FRENO AUTOMATICO
                    if (Velocity > Acceleration)
                        Velocity -= Acceleration;
                    else if (Velocity < -Acceleration)
                        Velocity += Acceleration;
                    else
                    {
                        Velocity = 0;
                    }
                }

            }
            if (State == "jumping" || State == "falling") //EN EL AIRE
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                {
                    MoveRight(2);
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Left))
                {
                    MoveLeft(2);
                }
            }

            Pos.X += Velocity;
            //---------------------------------

            //TEMP- WRAP EN LAS PAREDES PARA PROBAR SIN ROMPERNOS LAS BOLAS
            if (Pos.X > Game1.screenWidth / Game1.scaleFactor)
                Pos.X = 5;
            if (Pos.X < 0)
                Pos.X = Game1.screenWidth / Game1.scaleFactor - 5;
            //---------------------------------
        }

        //SECCION DE METODOS PRIVADOS
        private void Jump()
        {
            Gravity -= JumpPower;
        }
        private void MoveRight(int accReducer = 1)
        {
            if (Velocity <= MaxVelocity)
                Velocity += Acceleration / accReducer;
            else
                Velocity = MaxVelocity;

        }
        private void MoveLeft(int accReducer = 1)
        {
            if (Velocity >= -MaxVelocity)
                Velocity -= Acceleration / accReducer;
            else
                Velocity = -MaxVelocity;
        }
        //---------------------------------
    }
}