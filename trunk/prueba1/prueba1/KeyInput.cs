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
    public class KeyInput
    {
        public string PreviousUserWill;

        public string UsersWill()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                return "Jump";
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                return "MoveRight";
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                return "MoveLeft";
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                return "CouchOrRoll";
            }

            return "None";
        }
    }
}