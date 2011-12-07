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
        private const int PreparingJumpDuration = 8;
        //PUBLIC (PROPIEDADES DEL OBJETO)
        public AnimatedSprite Tex;
        public Vector2 Pos;
        public string State;
        public float JumpPower;
        public float Gravity;
        public float GravityForce;
        public float MaxVelocity;
        public float Velocity;
        public float Acceleration;

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
            {
                _preparingJumpTimer++;
                State = "preparingForJump";
            }

            if (_preparingJumpTimer >= PreparingJumpDuration)
            {
                if (State != "falling")
                {
                    if ((int)Velocity != 0)
                        State = "jumping";
                    else
                        State = "idleJumping";
                    Jump();
                }
                _preparingJumpTimer = 0;
            }
            //-------------------------------

            Gravity += GravityForce;
            Pos.Y += (int)Gravity;

            if (State == "jumping" || State == "idleJumping" || State == "falling")
            {
                if (Gravity > -GravityForce * ((int)(MaxVelocity * 4)) && Gravity < GravityForce * ((int)(MaxVelocity * 4)))
                {//CAMBIO DE jumping O idleJumping A midAir
                    if ((int)Velocity != 0)
                        State = "runningMidAir";
                    else
                        State = "midAir";
                }
            }


            if (Gravity > GravityForce * ((int)(MaxVelocity * 4)))
            {
                if (Gravity > GravityForce * (int)(MaxVelocity * 10) || !collision.EstaColisionando(Pos, MainClass.backgroundCollisionTex, Color.Black, 0, (int)(MaxVelocity * 3)))
                {//CAMBIO DE jumping O idleJumping A falling
                    if ((int)Velocity != 0)
                        State = "runFalling";
                    else
                        State = "falling";
                    _jumpWaitTimer = 0;
                }
            }


            while (collision.EstaColisionando(Pos, MainClass.backgroundCollisionTex, Color.Black, 0, 0))
            {//PISANDO
                if (State != "preparingForJump" && State != "crouching")
                    State = "idle";
                Pos.Y--;
                Gravity = 0;
            }
            //-------------------------------

            // SECCION DE MOVIMIENTO HORIZONTAL

            if (State == "idle" || State == "running" || State == "crouching")
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Down))
                    State = "crouching";
                else
                    State = "idle";

                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                {
                    if (State == "crouching")
                        State = "rolling";
                    else
                        State = "running";
                    Flipping = SpriteEffects.None;
                    MoveRight();
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Left))
                {
                    if (State == "crouching")
                        State = "rolling";
                    else
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
            if (State=="idleJumping" || State == "runningMidAir" || State == "runFalling" || State == "jumping" || State == "falling" || State == "midAir") //EN EL AIRE
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                {
                    MoveRight(3);
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Left))
                {
                    MoveLeft(3);
                }
            }
            
            

            Pos.X += (int)Velocity;
            //---------------------------------

            //TEMP- WRAP EN LAS PAREDES PARA PROBAR SIN ROMPERNOS LAS BOLAS
            if (Pos.X > MainClass.screenWidth / MainClass.scaleFactor)
                Pos.X = 5;
            if (Pos.X < 0)
                Pos.X = MainClass.screenWidth / MainClass.scaleFactor - 5;
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