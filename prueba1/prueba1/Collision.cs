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
    public class Collision
    {
        public bool EstaColisionando(Vector2 position, Texture2D texture, Color color, int addX, int addY)
        {
            var textureDataBG = new Color[(texture.Width * texture.Height)];
            texture.GetData(textureDataBG);

            if (((int)position.Y + addY) * texture.Width + ((int)position.X + addX) <= textureDataBG.Length)
                if (textureDataBG[((int)position.Y + addY) * texture.Width + ((int)position.X + addX)] != color)
                    return false;



            return true;
        }
    }
}
