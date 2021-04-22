using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rockhoppers.scripts
{
    class Missile : Entity, IDamaging
    {

        public int Damage { get => damage; set => damage = value; }

        private int damage; 

        public bool isExploding = false;
        private float waitTime = 0.0f;

        public float rangeRadius = 10000000f;
        public Entity target;

        public Circle explosionCirle = new Circle(40);

        public float fuel;

        Weapon Parent { get; set; }

        public Missile( Weapon parent) : base("missile")
        {
            Name = "Missile";
            damage = 100;

            IsOnScreen = true;

            Parent = parent;

            spriteDepth = parent.spriteDepth + 0.0000001f;

            orientation = parent.orientation;
            


            velocity = parent.velocity;
            fuel = 15f;
            accelerationMagnitude = 450;
            

            WorldPosition = parent.WorldPosition;
            this.velocity = parent.ParentShip.velocity;

            TextureScale = new Vector2(2);

            target = parent.ParentShip.target;

            deleteDelay = 1.0f;
        }


        



        public override void Update(GameTime gameTime)
        {
            TrackTarget(target);

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if(isExploding)
            {
                TextureScale *= (1 + deltaTime);
            }

            if (waitTime > 0)
            {
                waitTime -= deltaTime;
                base.Update(gameTime);
                return;
            }

            if(fuel >= 0)
            {



                if (Vector2.Distance(this.WorldPosition, target.WorldPosition) < explosionCirle.Radius * 50)
                {
                    EntityBehaviours.Intercept(this, target, gameTime);
                }
                else
                {
                    EntityBehaviours.FollowTarget(this, target, gameTime);
                }
                
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





            base.Update(gameTime);
        }

        public void TrackTarget(Entity e)
        {
            if(Vector2.Distance(this.WorldPosition, e.WorldPosition) > rangeRadius)
            {
                Delete();
                return;
            }

            if (Vector2.Distance(this.WorldPosition, target.WorldPosition) < explosionCirle.Radius && ! isExploding)
            {
                Detonate();
            }

        }

        public void Boost(Vector2 direction, GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Vector2 moment = direction * accelerationMagnitude * deltaTime;

            velocity += moment;

            orientation = UtilityFunctions.GetAngleFromVector(moment) + (float)Math.PI / 2;
        }

        public void Detonate()
        {
            isExploding = true;
            deleteQueued = true;
            TextureScale *= 1f;
            SpritePath = "explosion";
            velocity = target.velocity;
            spriteDepth = 0.2f;

            EntityBehaviours.DealDamage(this, (IDamageable)target);

            Parent.ParentShip.NotifyHit(this.Name,damage);
            
        }

        



    }
}
