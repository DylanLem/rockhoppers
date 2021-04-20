using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rockhoppers.scripts
{
    public class Bullet : Entity
    {
        public new bool IsPlayer { get => false; }

        public float speed;
        public Vector2 realVelocity { get => inertialVelocity + (vectorAngle * speed); }

        public Vector2 inertialVelocity;
        public Vector2 vectorAngle;

        public Bullet(string spritePath, Entity parent) : base(spritePath)
        {
            IsOnScreen = true;

            spriteDepth = 0.6f;

            orientation = parent.orientation;
            vectorAngle = -UtilityFunctions.GetVectorFromAngle(orientation + ((float)Math.PI / 2));

           //Set_Inertial_Velocity(parent.velocity);
            speed = 1000f;

            ScreenPosition = parent.ScreenPosition;

            textureScale = new Vector2(2);
        }


        private void Set_Inertial_Velocity(Vector2 velocity)
        {
            Vector2 v = new Vector2(velocity.X,velocity.Y);

            inertialVelocity = v;
        }
        public override void Update(GameTime gameTime)
        {
            Vector2 deltaPos = realVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds;



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

            base.Update(gameTime);
        }



        



    }
}
