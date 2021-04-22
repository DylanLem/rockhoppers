using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Rockhoppers.scripts
{
    public class Ship : Entity, IDamageable
    {
        private float health;
        public float Health { get => health; set => health = value; }

        private bool isDestroyed;
        public bool IsDestroyed { get => isDestroyed; set => isDestroyed = value; }

        public int targetIndex = -1;

        //Delay in seconds
        public float shotDelay = 0.75f;
        public float shootTimer = 0.0f;

        public Dictionary<string, Component> loadOut = new Dictionary<string, Component>();


        // Measured in steps of 10,000 game units of distance
        public float radarRange = 1.0f;

        public bool is_boosting = false;

        public Entity target;

        public List<Ship> trackedShips = new List<Ship>();
        
       

        public Ship(string spritePath) : base(spritePath)
        {
            

            spriteDepth = 0.5f;
            accelerationMagnitude = 25f;
            TextureScale = new Vector2(4);

            health = 100;
        }




        public override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if(!trackedShips.Contains((Ship)target))
            {
                target = null;
            }

            if(! IsPlayer)
            {
                target = SceneManager.players[0].playerEntity;

                EntityBehaviours.FollowTarget(this, target, gameTime);

            }

            


            shootTimer += deltaTime;




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

            Vector2 moveDirection = Vector2.Zero;

            if(Input.mouseState.LeftButton == ButtonState.Pressed)
                 moveDirection = UtilityFunctions.GetVectorFromAngle(Input.GetMouseDirectionFromEntity(this));


            

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
                       if(this.loadOut.ContainsKey("Missile"))
                        {
                            Weapon missi = loadOut["Missile"] as Weapon;
                            missi.Fire();
                        }
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

        public void InstallComponent(Component item)
        {
            if(item is Weapon)
            {
                Weapon weapy = item as Weapon;

                this.loadOut[weapy.weaponType] = weapy;
                weapy.SetParent(this);
            }
        }

        public void NotifyHit(string ammo_name, float damage)
        {
            System.Diagnostics.Debug.WriteLine(ammo_name + " hit for " + damage);
        }
        
    }
}
    