using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rockhoppers.scripts
{
    class Missile : Entity
    {

        public bool isExploding = false;


        public float rangeRadius = 10000000f;
        public Entity target;

        public Circle explosionCirle = new Circle(50);

        public float fuel;

        public Missile(string spritePath, Ship parent) : base(spritePath)
        {
            IsOnScreen = true;

            spriteDepth = parent.spriteDepth + 0.01f;

            orientation = parent.orientation;
            


            velocity = parent.velocity;
            fuel = 5f;
            accelerationMagnitude = 950;
            

            WorldPosition = parent.WorldPosition;

            textureScale = new Vector2(2);

            target = parent.target;

            deleteDelay = 1.0f;
        }


        



        public override void Update(GameTime gameTime)
        {
            TrackTarget(target);

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if(isExploding)
            {
                textureScale *= (1 + deltaTime);
            }



            if(fuel >= 0)
            {            

                EntityBehaviours.Intercept(this, target, gameTime);
                
                fuel -= deltaTime;

            }       

            else
            {
                if(!deleteQueued)
                    deleteDelay = 5;

                deleteQueued = true;        
            }


           

            Vector2 deltaPos = velocity * deltaTime;


           

            WorldPosition += deltaPos;


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

        public void TrackTarget(Entity e)
        {
            if(Vector2.Distance(this.WorldPosition, e.WorldPosition) > rangeRadius)
            {
                Delete();
                return;
            }

            if (Vector2.Distance(this.WorldPosition, target.WorldPosition) < explosionCirle.Radius)
            {
                Detonate();
            }

        }

        public void Boost(Vector2 direction, GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Vector2 moment = direction * accelerationMagnitude * deltaTime;

            velocity += moment;

            orientation = UtilityFunctions.GetAngleFromVector(velocity) + (float)Math.PI / 2;
        }

        public void Detonate()
        {
            isExploding = true;
            deleteQueued = true;
            textureScale *= 1f;
            SpritePath = "explosion";
            velocity = target.velocity;
            spriteDepth = 0.2f;
        }

        



    }
}
