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
    public static class KeyInput
    {
        //private static int oldJumpAmount;

        public static void Player(Player player)
        {
            //ECUACIONES M.R.U.V. REALES PARA APLICAR ALGUN DIA
            //Xf = X0 + v * t + 1/2@ * t2
            //Vf = V0 + @ * t
            //player.Pos.X = player.Pos.X + (player.Velocity * (1 / 60)) + (player.Acceleration / 2) * 2.7f;
            //-------------------------------
            // SECCION DE MOVIMIENTO VERTICAL

            if (EstaPisando(player) && player.jumpKeyPressed < 2 && player.State != "Falling")
            {
                player.Gravity += player.jump;
                player.jumpKeyPressed = 1;

            }
            if (!EstaPisando(player) && player.jumpKeyPressed == 1)
            {
                player.jumpKeyPressed = 2;
            }


            if (player.Gravity < 0)
            {
                player.State = "Jumping";
            }
            if (player.Gravity > 0)
            {
                player.State = "Falling";
            }


            player.jump *= player.jumpDecr;
            player.Pos.Y += (int) player.Gravity;

            while (EstaPisando(player))
            {
                player.State = "Idle";
                player.Pos.Y--;
                player.Gravity = 0;
                player.State = "Idle";
                player.jumpKeyPressed = 0;
                player.jump = player.JumpForce;

            }
            player.Gravity += player.GravityForce;
            //-------------------------------

            // SECCION DE MOVIMIENTO HORIZONTAL
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                player.State = "Running";
                if (player.Velocity <= player.MaxVelocity)
                    player.Velocity += player.Acceleration;
                else
                    player.Velocity = player.MaxVelocity;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                player.State = "Running";
                if (player.Velocity >= -player.MaxVelocity)
                    player.Velocity -= player.Acceleration;
                else
                    player.Velocity = -player.MaxVelocity;
            }
            else
            {
                if (player.Velocity > 0)
                    player.Velocity -= 1;
                else if (player.Velocity < 0)
                    player.Velocity += 1;
            }

            player.Pos.X += player.Velocity;
            //---------------------------------

            if (player.Pos.X > Game1.screenWidth/Game1.scaleFactor)
                player.Pos.X = 5;
            if (player.Pos.X < 0)
                player.Pos.X = 180;
        }


        private static bool EstaPisando(Player player)
        {
            return Collision.EstaColisionando(player.Pos, Game1.backgroundCollisionTex, Color.Black, 0, -1);
        }
    }
}