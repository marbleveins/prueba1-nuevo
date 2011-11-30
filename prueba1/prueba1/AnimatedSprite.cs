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

    public class AnimatedSprite 
    {
        //la textura...
        
        float timer = 0f; //mide el tiempo transcurrido
        float interval = 50f; //el tiempo que debe transcurrir para cambiar de frame.
        int currentFrame = 0;
        //el ancho y el alto del rectangulo que se toma para cada cuadro de animacion
        int spriteWidth = 32;
        int spriteHeight = 48;
        Texture2D spriteTexture;
        Rectangle sourceRect;
        Vector2 position;
        Vector2 origin;
        string state;
        public string State
        {
            get { return state; }
            set { state = value; }
        }
        public Vector2 Position
	    {
	        get { return position; }
	        set { position = value; }
	    }
	 
	    public Vector2 Origin
	    {
	        get { return origin; }
	        set { origin = value; }
	    }
	 
	    public Texture2D Texture
	    {
            get { return spriteTexture; }
	        set { spriteTexture = value; }
	    }
	 
	    public Rectangle SourceRect
	    {
	        get { return sourceRect; }
	        set { sourceRect = value; }
	    }
     
  
        public AnimatedSprite(Texture2D texture, int currentFrame, int spriteWidth, int spriteHeight)
	    {
	        this.spriteTexture = texture;
	        this.currentFrame = currentFrame;
	        this.spriteWidth = spriteWidth;
	        this.spriteHeight = spriteHeight;
    	}

        public void HandleSpriteMovement(GameTime gametime)
        {
            //acá se determina cuál animacián hay que reproducir. se llama a una función para cada animación.

            sourceRect = new Rectangle(currentFrame * spriteWidth, 0, spriteWidth, spriteHeight);
            if(state== "Idle")
                AnimateStand(gametime);
            if (state == "Running")
                AnimateRun(gametime);

            origin = new Vector2(sourceRect.Width / 2, sourceRect.Height);
        }

        public void AnimateStand(GameTime gametime)
        {
            timer += (float)gametime.ElapsedGameTime.TotalMilliseconds; //cuánto tiempo pasó desde el último update.
            if (currentFrame > 1) { currentFrame = 0; }
            if (timer > interval*8)
            {
                currentFrame++;
                if (currentFrame > 1)
                {
                    currentFrame = 0;
                }
                timer = 0f; //resetea el timer.
            }
        }
        public void AnimateRun(GameTime gametime)
        {
            timer += (float)gametime.ElapsedGameTime.TotalMilliseconds; //cuánto tiempo pasó desde el último update.
            if (currentFrame > 9 || currentFrame < 2) { currentFrame = 2; }
            if (timer > interval * 2)
            {
                currentFrame++;
                if (currentFrame > 9)
                {
                    currentFrame = 2;
                }
                timer = 0f; //resetea el timer.
            }
        }

    }
}
