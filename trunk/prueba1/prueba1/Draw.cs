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
    class Draw
    {

        public void Background(SpriteBatch spriteBatch, Texture2D backgroundViewTex)
        {
            spriteBatch.Draw(backgroundViewTex, new Vector2(0, 0), Color.White);
        }

        public void Player(SpriteBatch spriteBatch, Player player)
        {
            spriteBatch.Draw(player.Tex.Texture, player.Pos, player.Tex.SourceRect, Color.White, 0f, player.Tex.Origin, 1.0f, player.Flipping, 0);
        }

        public void Text(SpriteBatch spriteBatch, Player player, SpriteFont font)
        {
            spriteBatch.DrawString(font, player.State, new Vector2(10, 10), Color.Red);
        }
    }
}
