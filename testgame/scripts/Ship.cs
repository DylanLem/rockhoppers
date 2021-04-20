using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace testgame.scripts
{
    public class Ship : Entity
    {
        private int targetIndex = 0;

        //Delay in seconds
        public float shotDelay = 5f;
        public float shootTimer = 0.0f;



        // Measured in steps of 10,000 game units of distance
        public float radarRange = 1.0f;

        public bool is_boosting = false;

        public Entity target;

        public List<Ship> trackedShips = new List<Ship>();
        

        public Ship(string spritePath) : base(spritePath)
        {
            spriteDepth = 0.5f;
            accelerationMagnitude = 25f;   
        }




        public override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if(! IsPlayer)
            {
                target = SceneManager.players[0].playerEntity;

                EntityBehaviours.FollowTarget(this, target, gameTime);

                if (Math.Abs(WorldPosition.X - SceneManager.camPos.X) < Game1.ScreenSize.X / 2 && Math.Abs(WorldPosition.Y - SceneManager.camPos.Y) < Game1.ScreenSize.X / 2)
                {
                    IsOnScreen = true;
                    ScreenPosition =  WorldPosition - SceneManager.camPos + Game1.ScreenSize/2;
                }
                else
                {
                    ScreenPosition = Game1.ScreenSize * 5;
                    IsOnScreen = false;
                }
            }

            


            shootTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;




            Move(velocity, gameTime);
            base.Update(gameTime);
        }

        public void ApplyThrust(Vector2 direction, GameTime gameTime)
        {
            Vector2 moment = direction * accelerationMagnitude * (float)gameTime.ElapsedGameTime.TotalSeconds;
            orientation = UtilityFunctions.GetAngleFromVector(moment) + (float)Math.PI / 2;


            if (is_boosting || Input.kbState.IsKeyDown(Keys.LeftShift) && IsPlayer)
            {
                velocity += 8 * moment;
            }
            else
            {
                velocity += moment;
            }

           
            is_boosting = false;
        }

        public void Move(Vector2 velocity, GameTime gameTime)
        {
            Vector2 deltaPos = velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            WorldPosition += deltaPos;

            
        }

        public override void Update_As_Player(GameTime gameTime)
        {

            shootTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            Input.Update(gameTime);

            

            Vector2 moveDirection = Input.GetMovementDirection();


            if(Input.kbState.IsKeyDown(Keys.Tab) && trackedShips.Count > 0 && Input.TryInput(Keys.Tab))
            {

                if (targetIndex < trackedShips.Count - 1)
                    targetIndex += 1;
                else
                    targetIndex = 0;
                TargetShip(trackedShips[targetIndex]);

                
            }

            if (Input.kbState.IsKeyDown(Keys.M) && target != null && shootTimer >= shotDelay)
            {
                Shoot("MISSILE");
                shootTimer = 0;
            }

            if (moveDirection != Vector2.Zero)
                  ApplyThrust(moveDirection, gameTime);

            ScanRadar();
            AlignOrientationToMouse();
        }


        public void Shoot(string weapon)
        {
            switch(weapon)
            {
                case ("MISSILE"):
                    {
                        Missile miss = new Missile("missile", this);
                        break;
                    }
            }
            
        }


        public void TargetShip(Ship targetShip)
        {
            if (!trackedShips.Contains(targetShip))
                return;

            target = targetShip;
        }


        public void ScanRadar()
        {
            trackedShips = new List<Ship>();

            foreach (Entity e in SceneManager.entityList)
            {
                if (e.uniqueID != this.uniqueID && e.GetType() == typeof(Ship))
                    if (Vector2.Distance(this.WorldPosition, e.WorldPosition) <= radarRange * 10000)
                    {
                        trackedShips.Add((Ship)e);
                    }

            }

        }

        public void MatchTargetVelocity(Entity target, GameTime gameTime)
        {
            Vector2 velocityDif = Vector2.Normalize(target.velocity - this.velocity);

            ApplyThrust(velocityDif, gameTime);
            
        }


        
    }
}
    