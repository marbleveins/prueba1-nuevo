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
    }
}
