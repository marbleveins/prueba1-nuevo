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

        public AnimatedSprite Tex;

        public Vector2 Pos;
        public string State;
        public float Gravity;
        public float GravityForce;
        public float MaxVelocity;
        public float Velocity;
        public float Acceleration;
        public float JumpPower;
        private int _jumpTimer = 0;

        public void None()
        {
        }
        public void MoveLeft()
        {
            // hay que ver si los nombres van a ser Step o Move, 
            // porque no se si se ejecutaran una vez por frame.... etc.
        }
        public void MoveRight()
        {
        }
        public void Jump()
        {
        }
        public void MoveAtWill()//provisorio para guardar lo del keyinput.cs (Andando)
        {
            var collision = new Collision(); // así existe sólo mientras el metodo esta corriendo.
            _jumpTimer -= 1;

            //ECUACIONES M.R.U.V. REALES PARA APLICAR ALGUN DIA
            //Xf = X0 + v * t + 1/2@ * t2
            //Vf = V0 + @ * t
            //player.Pos.X = player.Pos.X + (player.Velocity * (1 / 60)) + (player.Acceleration / 2) * 2.7f;
            //-------------------------------

            // SECCION DE MOVIMIENTO VERTICAL
            if (Keyboard.GetState().IsKeyDown(Keys.Up) && Gravity == 0 && _jumpTimer < 1)
            {
                Gravity -= JumpPower;
                State = "jumping";
            }

            Gravity += GravityForce;
            Pos.Y += (int)Gravity;

            if (Gravity > 0)
            {
                if (Gravity > GravityForce * 6 || !collision.EstaColisionando(Pos, Game1.backgroundCollisionTex, Color.Black, 0, (int)(MaxVelocity * 3)))
                {
                    State = "falling";
                    _jumpTimer = 15;
                }
            }


            while (collision.EstaColisionando(Pos, Game1.backgroundCollisionTex, Color.Black, 0, 0))
            {
                State = "idle";
                Pos.Y--;
                Gravity = 0;
            }
            //-------------------------------

            // SECCION DE MOVIMIENTO HORIZONTAL
            if (State == "idle" || State == "running")
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                {
                    State = "running";
                    if (Velocity <= MaxVelocity)
                        Velocity += Acceleration;
                    else
                        Velocity = MaxVelocity;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Left))
                {
                    State = "running";
                    if (Velocity >= -MaxVelocity)
                        Velocity -= Acceleration;
                    else
                        Velocity = -MaxVelocity;
                }
                else
                {
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
            else
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                {
                    if (Velocity <= MaxVelocity)
                        Velocity += Acceleration / 2;
                    else
                        Velocity = MaxVelocity;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Left))
                {
                    if (Velocity >= -MaxVelocity)
                        Velocity -= Acceleration / 2;
                    else
                        Velocity = -MaxVelocity;
                }
            }

            Pos.X += Velocity;
            //---------------------------------

            if (Pos.X > Game1.screenWidth / Game1.scaleFactor)
                Pos.X = 5;
            if (Pos.X < 0)
                Pos.X = 180;
        }
    }
}
