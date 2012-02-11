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
        private string _direction = "right";
        //PUBLIC (PROPIEDADES DEL OBJETO)
        public AnimatedSprite Tex;
        public Vector2 Pos;
        public string State;
        public float JumpPower;
        public float Gravity;
        public float GravityForce;
        public float MaxVelocity;
        public double Velocity;
        public float Acceleration;
        public SpriteEffects Flipping = SpriteEffects.None;

        public void MoveNew()
        {
            switch (State)
            {
                case "idle":
                    Idle();
                    break;
                case "running":
                    Running();
                    break;
                case "jumping":
                    Jumping();
                    break;
                case "idleJumping":
                    IdleJumping();
                    break;
                case "falling":
                    Falling();
                    break;
                case "midAir":
                    MidAir();
                    break;
                case "preparingForJump":
                    PreparingForJump();
                    break;
                case "crouching":
                    Crouching();
                    break;
                case "rolling":
                    Rolling();
                    break;
            }
            //TEMP- WARP EN LAS PAREDES PARA PROBAR SIN ROMPERNOS LOS HOVOS
            if (Pos.X > (int)(MainClass.screenWidth / MainClass.scaleFactor))
                Pos.X = 5;
            if (Pos.X < 0)
                Pos.X = MainClass.screenWidth / MainClass.scaleFactor - 5;
            //-------------------------------------------------------------

            MoveX();
            MoveY();
        }
        public void MoveViejo()
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

            if (State == "idle" || State == "running" || State == "crouching" || State == "rolling")
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
                    Accelerate();
                    _direction = "right";
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Left))
                {
                    if (State == "crouching")
                        State = "rolling";
                    else
                        State = "running";
                    Flipping = SpriteEffects.FlipHorizontally;
                    Accelerate();
                    _direction = "left";
                }
                else
                {//FRENO AUTOMATICO
                    if (Velocity > Acceleration)
                        Velocity -= Acceleration;
                    else
                    {
                        Velocity = 0;
                    }
                }

            }
            if (State == "idleJumping" || State == "runningMidAir" || State == "runFalling" || State == "jumping" || State == "falling" || State == "midAir") //EN EL AIRE
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                {
                    Accelerate(5);
                    _direction = "right";
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Left))
                {
                    Accelerate(5);
                    _direction = "left";
                }
            }
            else if (State == "idleJumping")
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                {
                    Accelerate(5);
                    _direction = "right";
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Left))
                {
                    Accelerate(5);
                    _direction = "left";
                }
            }

            if (_direction == "right")
                MoveRight();
            else
                MoveLeft();
            //---------------------------------

            //TEMP- WRAP EN LAS PAREDES PARA PROBAR SIN ROMPERNOS LAS BOLAS
            if (Pos.X > MainClass.screenWidth / MainClass.scaleFactor)
                Pos.X = 5;
            if (Pos.X < 0)
                Pos.X = MainClass.screenWidth / MainClass.scaleFactor - 5;
            //---------------------------------
        }


        private void Idle()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Up) && Keyboard.GetState().IsKeyDown(Keys.Right) && !Keyboard.GetState().IsKeyDown(Keys.Down) && !Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                State = "idleJumping";
                Jump();
                _direction = "right";
                Flipping = SpriteEffects.None;
                Accelerate();
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Up) && Keyboard.GetState().IsKeyDown(Keys.Left) && !Keyboard.GetState().IsKeyDown(Keys.Down) && !Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                State = "idleJumping";
                Jump();
                _direction = "left";
                Flipping = SpriteEffects.FlipHorizontally;
                Accelerate();
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Down) && Keyboard.GetState().IsKeyDown(Keys.Right) && !Keyboard.GetState().IsKeyDown(Keys.Up) && !Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                State = "rolling";
                _direction = "right";
                Flipping = SpriteEffects.None;
                Accelerate();
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Down) && Keyboard.GetState().IsKeyDown(Keys.Left) && !Keyboard.GetState().IsKeyDown(Keys.Down) && !Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                State = "rolling";
                _direction = "left";
                Flipping = SpriteEffects.FlipHorizontally;
                Accelerate();
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Up) && !Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                State = "idleJumping";
                Jump();
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Down) && !Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                State = "crouching";
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Right) && !Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                State = "running";
                _direction = "right";
                Flipping = SpriteEffects.None;
                Accelerate();
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Left) && !Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                State = "running";
                _direction = "left";
                Flipping = SpriteEffects.FlipHorizontally;
                Accelerate();
            }
        }
        private void Running()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Up) && Keyboard.GetState().IsKeyDown(Keys.Right) && !Keyboard.GetState().IsKeyDown(Keys.Down) && !Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                State = "jumping";
                Jump();
                _direction = "right";
                Flipping = SpriteEffects.None;
                Accelerate();
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Up) && Keyboard.GetState().IsKeyDown(Keys.Left) && !Keyboard.GetState().IsKeyDown(Keys.Down) && !Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                State = "jumping";
                Jump();
                _direction = "left";
                Flipping = SpriteEffects.FlipHorizontally;
                Accelerate();
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Down) && Keyboard.GetState().IsKeyDown(Keys.Right) && !Keyboard.GetState().IsKeyDown(Keys.Up) && !Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                State = "rolling";
                _direction = "right";
                Flipping = SpriteEffects.None;
                Accelerate();
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Down) && Keyboard.GetState().IsKeyDown(Keys.Left) && !Keyboard.GetState().IsKeyDown(Keys.Down) && !Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                State = "rolling";
                _direction = "left";
                Flipping = SpriteEffects.FlipHorizontally;
                Accelerate();
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Up) && !Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                State = "jumping";
                Jump();
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Down) && !Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                State = "rolling";
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Right) && !Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                _direction = "right";
                Flipping = SpriteEffects.None;
                Accelerate();
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Left) && !Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                _direction = "left";
                Flipping = SpriteEffects.FlipHorizontally;
                Accelerate();
            }
        }
        private void Jumping()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Right) && !Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                _direction = "right";
                Flipping = SpriteEffects.None;
                Accelerate();
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Left) && !Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                _direction = "left";
                Flipping = SpriteEffects.FlipHorizontally;
                Accelerate();
            }
            if (Gravity > -GravityForce * ((int)(MaxVelocity * 4)) && Gravity < GravityForce * ((int)(MaxVelocity * 4)))
            {//CAMBIO DE jumping O idleJumping A midAir
                State = "midAir";
            }
        }
        private void IdleJumping()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Right) && !Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                _direction = "right";
                Flipping = SpriteEffects.None;
                Accelerate();
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Left) && !Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                _direction = "left";
                Flipping = SpriteEffects.FlipHorizontally;
                Accelerate();
            }
            if (Gravity > -GravityForce * ((int)(MaxVelocity * 4)) && Gravity < GravityForce * ((int)(MaxVelocity * 4)))
            {//CAMBIO DE jumping O idleJumping A midAir
                State = "midAir";
            }
        }
        private void Falling()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Right) && !Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                _direction = "right";
                Flipping = SpriteEffects.None;
                Accelerate();
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Left) && !Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                _direction = "left";
                Flipping = SpriteEffects.FlipHorizontally;
                Accelerate();
            }
        }
        private void PreparingForJump()
        {

        }
        private void MidAir()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Right) && !Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                _direction = "right";
                Flipping = SpriteEffects.None;
                Accelerate();
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Left) && !Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                _direction = "left";
                Flipping = SpriteEffects.FlipHorizontally;
                Accelerate();
            }
            if (Gravity < -GravityForce * ((int)(MaxVelocity * 4)) || Gravity > GravityForce * ((int)(MaxVelocity * 4)))
            {//CAMBIO DE jumping O idleJumping A midAir
                State = "falling";
            }
        }
        private void Crouching()
        {
            if (!Keyboard.GetState().IsKeyDown(Keys.Down) && !Keyboard.GetState().IsKeyDown(Keys.Right) && !Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                State = "idle";
            }
            else if (!Keyboard.GetState().IsKeyDown(Keys.Down) && Keyboard.GetState().IsKeyDown(Keys.Right) && !Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                State = "running";
                _direction = "right";
                Flipping = SpriteEffects.None;
                Accelerate();
            }
            else if (!Keyboard.GetState().IsKeyDown(Keys.Down) && Keyboard.GetState().IsKeyDown(Keys.Left) && !Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                State = "running";
                _direction = "left";
                Flipping = SpriteEffects.FlipHorizontally;
                Accelerate();
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Down) && Keyboard.GetState().IsKeyDown(Keys.Right) && !Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                State = "rolling";
                _direction = "right";
                Flipping = SpriteEffects.None;
                Accelerate();
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Down) && Keyboard.GetState().IsKeyDown(Keys.Left) && !Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                State = "rolling";
                _direction = "left";
                Flipping = SpriteEffects.FlipHorizontally;
                Accelerate();
            }
        }
        private void Rolling()
        {
            if (!Keyboard.GetState().IsKeyDown(Keys.Down) && !Keyboard.GetState().IsKeyDown(Keys.Right) && !Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                State = "idle";
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Down) && !Keyboard.GetState().IsKeyDown(Keys.Left) && !Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                State = "crouching";
            }
            else if (!Keyboard.GetState().IsKeyDown(Keys.Down) && Keyboard.GetState().IsKeyDown(Keys.Right) && !Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                State = "running";
                _direction = "right";
                Flipping = SpriteEffects.None;
                Accelerate();
            }
            else if (!Keyboard.GetState().IsKeyDown(Keys.Down) && Keyboard.GetState().IsKeyDown(Keys.Left) && !Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                State = "running";
                _direction = "left";
                Flipping = SpriteEffects.FlipHorizontally;
                Accelerate();
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Down) && Keyboard.GetState().IsKeyDown(Keys.Right) && !Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                _direction = "right";
                Flipping = SpriteEffects.None;
                Accelerate();
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Down) && Keyboard.GetState().IsKeyDown(Keys.Left) && !Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                _direction = "left";
                Flipping = SpriteEffects.FlipHorizontally;
                Accelerate();
            }
        }


        //SECCION DE METODOS PRIVADOS
        private void Accelerate(int accReducer = 1)
        {
            if (Velocity < MaxVelocity)
                Velocity += Acceleration / accReducer;
            else
                Velocity = MaxVelocity;
        }
        private void MoveRight()
        {
            Pos.X += (float)Math.Ceiling(Velocity);
        }
        private void MoveLeft()
        {
            Pos.X -= (float)Math.Ceiling(Velocity);
        }
        private void Jump()
        {
            Gravity -= JumpPower;
        }
        private void MoveX()
        {
            if (_direction == "right")
                MoveRight();
            else
                MoveLeft();
            if (!Keyboard.GetState().IsKeyDown(Keys.Right) && !Keyboard.GetState().IsKeyDown(Keys.Left))
            {//FRENO AUTOMATICO
                if (Velocity > Acceleration)
                    Velocity -= Acceleration;
                else
                {
                    Velocity = 0;
                }
            }
        }
        private void MoveY()
        {
            Gravity += GravityForce;
            Pos.Y += (int)Gravity;

            var collision = new Collision(); // así existe sólo dentro del método.
            while (collision.EstaColisionando(Pos, MainClass.backgroundCollisionTex, Color.Black, 0, 0))
            {//PISANDO
                Pos.Y--;
                Gravity = 0;
                if (!Keyboard.GetState().IsKeyDown(Keys.Up) && !Keyboard.GetState().IsKeyDown(Keys.Down) && !Keyboard.GetState().IsKeyDown(Keys.Left) && !Keyboard.GetState().IsKeyDown(Keys.Right))
                {
                    State = "idle";
                }
                else if (!Keyboard.GetState().IsKeyDown(Keys.Up) && !Keyboard.GetState().IsKeyDown(Keys.Down) && !Keyboard.GetState().IsKeyDown(Keys.Left) && Keyboard.GetState().IsKeyDown(Keys.Right))
                {
                    State = "running";
                    _direction = "right";
                    Flipping = SpriteEffects.None;
                    Accelerate();
                }
                else if (!Keyboard.GetState().IsKeyDown(Keys.Up) && !Keyboard.GetState().IsKeyDown(Keys.Down) && Keyboard.GetState().IsKeyDown(Keys.Left) && !Keyboard.GetState().IsKeyDown(Keys.Right))
                {
                    State = "running";
                    _direction = "left";
                    Flipping = SpriteEffects.FlipHorizontally;
                    Accelerate();
                }
                else if (!Keyboard.GetState().IsKeyDown(Keys.Up) && Keyboard.GetState().IsKeyDown(Keys.Down) && !Keyboard.GetState().IsKeyDown(Keys.Left) && !Keyboard.GetState().IsKeyDown(Keys.Right))
                {
                    State = "crouching";
                }
                else if (!Keyboard.GetState().IsKeyDown(Keys.Up) && Keyboard.GetState().IsKeyDown(Keys.Down) && !Keyboard.GetState().IsKeyDown(Keys.Left) && Keyboard.GetState().IsKeyDown(Keys.Right))
                {
                    State = "rolling";
                    _direction = "right";
                    Flipping = SpriteEffects.None;
                    Accelerate();
                }
                else if (!Keyboard.GetState().IsKeyDown(Keys.Up) && Keyboard.GetState().IsKeyDown(Keys.Down) && Keyboard.GetState().IsKeyDown(Keys.Left) && !Keyboard.GetState().IsKeyDown(Keys.Right))
                {
                    State = "rolling";
                    _direction = "left";
                    Flipping = SpriteEffects.FlipHorizontally;
                    Accelerate();
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Up) && !Keyboard.GetState().IsKeyDown(Keys.Down) && !Keyboard.GetState().IsKeyDown(Keys.Left) && !Keyboard.GetState().IsKeyDown(Keys.Right))
                {
                    State = "jumping";
                    Jump();
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Up) && !Keyboard.GetState().IsKeyDown(Keys.Down) && !Keyboard.GetState().IsKeyDown(Keys.Left) && Keyboard.GetState().IsKeyDown(Keys.Right))
                {
                    State = "jumping";
                    Jump();
                    _direction = "right";
                    Flipping = SpriteEffects.None;
                    Accelerate();
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Up) && !Keyboard.GetState().IsKeyDown(Keys.Down) && Keyboard.GetState().IsKeyDown(Keys.Left) && !Keyboard.GetState().IsKeyDown(Keys.Right))
                {
                    State = "jumping";
                    Jump();
                    _direction = "left";
                    Flipping = SpriteEffects.FlipHorizontally;
                    Accelerate();
                }
            }
        }
        private void JumpTimers()
        {
            if (_preparingJumpTimer >= 2)
            {
                _preparingJumpTimer++;
            }

            if (_jumpWaitTimer < JumpWaitMaxTime)
                _jumpWaitTimer++;

            if (_preparingJumpTimer > 0)
                _preparingJumpTimer++;
        }
        //-------------------------------------------

    }
}