using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Rockhoppers.scripts
{
    public class Background : Entity
    {
        private bool is_wrapping;

        Rectangle viewRect { get => new Rectangle((int)ScreenPosition.X, (int)ScreenPosition.Y, (int)Game1.ScreenSize.X, (int)Game1.ScreenSize.Y); }

        public float degree;

        public Player parent_player = null;

        public Background(bool isWrapping, string spritePath) : base(spritePath)
        {

            IsOnScreen = true;
            is_wrapping = isWrapping;
            spriteDepth = 0.99f;
            degree = 0.5f;
        }


        public void ApplyParallax()
        {

            velocity = parent_player.playerEntity.velocity * degree;
        }

        public override void Update(GameTime gameTime)
        {

            ApplyParallax();

            Vector2 deltaPos = velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            ScreenPosition += (deltaPos / textureScale);



            if (ScreenPosition.X < 0)
            {
                ScreenPosition = new Vector2(Texture.Width, ScreenPosition.Y);
            }
            if (ScreenPosition.X > Texture.Width)
            {
                ScreenPosition = new Vector2(0, ScreenPosition.Y);
            }
            if (ScreenPosition.Y < 0)
            {
                ScreenPosition = new Vector2(ScreenPosition.X, Texture.Height);
            }
            if (ScreenPosition.Y > Texture.Height)
            {
                ScreenPosition = new Vector2(ScreenPosition.X, 0);
            }

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 offset = Vector2.Zero;


            /*
             at any moment we will have to consider four rectangles.
             |A|B|
             |C|D|
             
             Our default rectangle will be A, which will be the only one drawn if there is no wrapping.
             The positioning of A B C D relative to one another on the camera is fixed. A will always be top-left and so on.
             Because of this, if there is a D square, it will always draw from the origin of the texture.

             The cool part is that we can transform this by simply checking the sign of our offset.x and offset.y respectively.

             now we have to do 3 things
             
            1. Determine where on the screen each rect will be drawn.
                    - Remember A is our default rect, so if there's no offset nothing changes.

            2. Determine the dimensions of the rects

            3. Determine where on the texture the rects will draw from0

            

            //The Offset determines how much, if at all, we have to wrap the background texture. 
            // Because we need the idea of our square positioning to be universal,
            // if the offset of X is negative (left wrap), we need our previous A square to become a B square
            // if the offset of Y is negative (top wrap), we need our previous A square to become a C square
            // D is also positioned based on sign of the offset

              :::::
              ::A:: (if this guy moves up and left far enough for the texture to wrap, it will cause a negative x and y offset)
              :::::
                |
                V
              |A|B::| (The A square becomes the D square as the camera moves past the origin)
              |C|D::|
              |:|:::|                       
             */

            //BASICALLY NEGATIVE OFFSET TURNS INTO REALLY LARGE POSITIVE OFFSET
            //THIS LETS ME AVOID MAKING MANY MANY EDGE CASES :)
            if (ScreenPosition.X < 0)
            {

                offset.X = (Game1.ScreenSize.X / textureScale.X) + ScreenPosition.X;
            }
            if (ScreenPosition.Y < 0)
            {
                offset.Y = (Game1.ScreenSize.Y / textureScale.Y) + ScreenPosition.Y;
            }

            if (ScreenPosition.X > Texture.Width - (viewRect.Width / textureScale.X))
            {
                offset.X = ScreenPosition.X - (Texture.Width - (viewRect.Width / textureScale.X));
            }

            if (ScreenPosition.Y > Texture.Height - (viewRect.Height / textureScale.Y))
            {
                offset.Y = ScreenPosition.Y - (Texture.Height - viewRect.Height / textureScale.Y);
            }


            List<Rectangle> rects = new List<Rectangle>();

            List<Vector2> rect_screen_positions = new List<Vector2>()
            {
                //initializes our aRect cam position 
                Vector2.Zero
            };


            //First we shall determine the dimensions and texture positions of our fair squares (rectangles)
            Rectangle aRect = new Rectangle((int)ScreenPosition.X, (int)ScreenPosition.Y, (int)((viewRect.Width / textureScale.X) - (int)offset.X), (int)((viewRect.Height / textureScale.Y) - (int)offset.Y));
            rects.Add(aRect);



            Rectangle bRect = new Rectangle(0, (int)ScreenPosition.Y, (int)offset.X, (int)((viewRect.Height / textureScale.Y) - (int)offset.Y));
            if (bRect.Width * bRect.Height != 0)
            {
                rects.Add(bRect);
                rect_screen_positions.Add(new Vector2((int)Game1.ScreenSize.X - (int)((int)offset.X * textureScale.X), 0));
            }


            Rectangle cRect = new Rectangle((int)ScreenPosition.X, 0, (int)((viewRect.Width / textureScale.X) - (int)offset.X), (int)offset.Y);
            if (cRect.Width * cRect.Height != 0)
            {
                rects.Add(cRect);
                rect_screen_positions.Add(new Vector2(0, (int)Game1.ScreenSize.Y - (int)((int)offset.Y * textureScale.Y)));
            }


            Rectangle dRect = new Rectangle(0, 0, (int)offset.X, (int)offset.Y);
            if (dRect.Width * dRect.Height != 0)
            {
                rects.Add(dRect);
                rect_screen_positions.Add(new Vector2((int)Game1.ScreenSize.X - (int)((int)offset.X * textureScale.X), ((int)Game1.ScreenSize.Y - (int)((int)offset.Y * textureScale.Y))));
            }



            for (int i = 0; i < rects.Count; i++)
            {
                spriteBatch.Draw(Texture, rect_screen_positions[i], rects[i], Color.White, orientation, Vector2.Zero, textureScale, SpriteEffects.None, 0.9f);
            }

            return;

        }



    }


}