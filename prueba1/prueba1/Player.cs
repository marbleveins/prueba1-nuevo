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
        public float JumpForce;
        public float MaxVelocity;
        public float Velocity;
        public float Acceleration;
        public float jump;
        public float jumpDecr;
        public float jumpKeyPressed;

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
        public void Move()//provisorio para guardar lo del keyinput.cs (Andando)
        {
            var collision = new Collision(); // así existe sólo mientras el metodo esta corriendo.
            //ECUACIONES M.R.U.V. REALES PARA APLICAR ALGUN DIA
            //Xf = X0 + v * t + 1/2@ * t2
            //Vf = V0 + @ * t
            //player.Pos.X = player.Pos.X + (player.Velocity * (1 / 60)) + (player.Acceleration / 2) * 2.7f;
            //-------------------------------
            // SECCION DE MOVIMIENTO VERTICAL

            if (Keyboard.GetState().IsKeyDown(Keys.Up) && jumpKeyPressed < 2 && State != "Falling")
            {
                Gravity += jump;
                jumpKeyPressed = 1;

            }
            if (!Keyboard.GetState().IsKeyDown(Keys.Up) && jumpKeyPressed == 1)
                jumpKeyPressed = 2;


            if (Gravity < 0)
            {
                State = "Jumping";
            }
            if (Gravity > 0)
            {
                State = "Falling";
            }


            jump *= jumpDecr;
            Pos.Y += (int)Gravity;

            while (collision.EstaColisionando(Pos, Game1.backgroundCollisionTex, Color.Black, 0, -1))
            {
                State = "Idle";
                Pos.Y--;
                Gravity = 0;
                State = "Idle";
                jumpKeyPressed = 0;
                jump = JumpForce;

            }
            Gravity += GravityForce;
            //-------------------------------

            // SECCION DE MOVIMIENTO HORIZONTAL
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                State = "Running";
                if (Velocity <= MaxVelocity)
                    Velocity += Acceleration;
                else
                    Velocity = MaxVelocity;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                State = "Running";
                if (Velocity >= -MaxVelocity)
                    Velocity -= Acceleration;
                else
                    Velocity = -MaxVelocity;
            }
            else
            {
                if (Velocity > 0)
                    Velocity -= 1;
                else if (Velocity < 0)
                    Velocity += 1;
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
