﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Rockhoppers.scripts
{
    public abstract class Entity
    {

        public int uniqueID = 0;

        public string Name { get; set; }
        public string SpritePath { get; set; }
        public virtual Texture2D Texture { get => SpritePath != null ? SpriteBucket.Sprites[SpritePath] : null; }

        public float spriteDepth;
        public Vector2 TextureScale { get => textureScale;  set { textureScale = value; } }

        private Vector2 textureScale;
        public virtual Vector2 textureSize { get => Texture != null ? new Vector2(Texture.Width * textureScale.X, Texture.Height * textureScale.Y) : Vector2.Zero; }


        public Circle hitbubble;


        public Vector2 ScreenPosition { get; set; }
        public Vector2 WorldPosition { get; set; }




        public Color color = Color.White;

        

        public bool IsPlayer { get; set; }
        public bool IsOnScreen { get; set; }
        public bool deleteQueued { get; set; }


        public float deleteTimer { get; set; }
        public float deleteDelay { get; set; }




        public float orientation;

        public Vector2 velocity;
        public float accelerationMagnitude { get; set; }
        public float velocityMagnitude { get => Math.Abs(Vector2.Distance(Vector2.Zero, velocity)); }


        public Entity(string sprite_path)
        {
            Random ran = new Random();

            

            while(uniqueID == 0)
            {
                int num = ran.Next(1, 999999999);
                foreach(Entity e in SceneManager.entityList)
                {
                    if (e.uniqueID == num)
                        goto skip;
                    
                }

                uniqueID = num;
                break;

            skip:
                continue;
                    
            }

            SpritePath = sprite_path;
            velocity = Vector2.Zero;

            //textureScale = new Vector2(4);
          

            SceneManager.QueueEntity(this);

            IsOnScreen = true;
            IsPlayer = false;
            deleteQueued = false;

            deleteTimer = 0;
        }



        public void SetScreenPos(Vector2 pos)
        {
            ScreenPosition = pos;
        }

        public void SetWorldPos(Vector2 pos)
        {
            if (pos.X > 0 && pos.Y > 0)
                WorldPosition = pos;
            else
                WorldPosition = Vector2.Zero;
        }




        public virtual void Update(GameTime gameTime)
        {

            this.hitbubble = new Circle(textureSize.X/ 2);
            if (deleteQueued)
            {
                deleteTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if(deleteTimer >= deleteDelay)
                {
                    Delete();
                }
            }

            if (IsPlayer)
            {
                Update_As_Player(gameTime);
            }

            if (this.IsPlayer != true && (GetType() != typeof(Background) && GetType() != typeof(UIElement)) && GetType() != typeof(UIText))
            {

                if (Math.Abs(WorldPosition.X - SceneManager.camPos.X) < Game1.ScreenSize.X / 2 && Math.Abs(WorldPosition.Y - SceneManager.camPos.Y) < Game1.ScreenSize.X / 2)
                {
                    IsOnScreen = true;
                    ScreenPosition = WorldPosition - SceneManager.camPos + Game1.ScreenSize / 2;
                }
                else
                {
                    ScreenPosition = Game1.ScreenSize * 5;
                    IsOnScreen = false;
                }
            }
           

        }

        public virtual void Update_As_Player(GameTime gameTime)
        {
            if(! IsPlayer)
                return;
        }


        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (Texture == null || ! IsOnScreen)
            {
                return;
            }


            spriteBatch.Draw(Texture, ScreenPosition, null, color, orientation, new Vector2(textureSize.X / (2*textureScale.X), textureSize.Y / (2*textureScale.Y)), textureScale, 0, spriteDepth);
            Game1.counter += 1;
        }

        public void AlignOrientationToMouse()
        {
            orientation = Input.GetMouseDirectionFromEntity(this) + (float)Math.PI / 2;
        }

        public void Delete()
        {
            SceneManager.RemoveEntity(this);
        }


    }
}
 