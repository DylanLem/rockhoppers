using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace testgame.scripts
{
    public static class EntityBehaviours
    {

        public static void FollowTarget(Entity pursuant, Entity target, GameTime gameTime)
        {
            if (pursuant == null || target == null)
                return;


            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Vector2 destination = target.WorldPosition;

            //The target's velocity from the pursuant's frame of reference
            Vector2 apparentVelocity = target.velocity - pursuant.velocity;


            Vector2 targetLine = (destination - pursuant.WorldPosition) + (apparentVelocity * 2) ;

            Vector2 accelerationVector = Vector2.Normalize(targetLine);
            

            if (pursuant.GetType() == typeof(Ship))
            {
                Ship dude = (Ship)pursuant;
                dude.ApplyThrust(accelerationVector,gameTime);
                
            }
            else if(pursuant.GetType() == typeof(Missile))
            {
                
                Missile shooty = (Missile)pursuant;
                shooty.Boost(accelerationVector, gameTime);
                
            }
        }

        public static void Flee(Entity escapee, Entity pursuant, GameTime gameTime)
        {
            if (pursuant == null || escapee == null)
                return;


            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Vector2 displacement = pursuant.WorldPosition - escapee.WorldPosition;

            //The target's velocity from the pursuant's frame of reference
            Vector2 apparentVelocity = pursuant.velocity - escapee.velocity;

            Vector2 targetLine;

            if (displacement.Length() > pursuant.velocityMagnitude)
                targetLine = new Vector2(apparentVelocity.Y, apparentVelocity.X);
            else
                targetLine = -displacement;
           
            if(targetLine == Vector2.Zero)
            {
                targetLine = displacement;
            }

            Vector2 accelerationVector = Vector2.Normalize(targetLine);
            System.Diagnostics.Debug.WriteLine(accelerationVector);


            if (escapee.GetType() == typeof(Ship))
            {
                Ship dude = (Ship)escapee;
                dude.ApplyThrust(accelerationVector, gameTime);

            }
            else if (escapee.GetType() == typeof(Missile))
            {

                Missile shooty = (Missile)escapee;
                shooty.Boost(accelerationVector, gameTime);
            }

        }

        
        public static void Intercept(Entity pursuant, Entity target, GameTime gameTime)
        {
            if (pursuant == null || target == null)
                return;


            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Vector2 destination = target.WorldPosition;

            //The target's velocity from the pursuant's frame of reference
            Vector2 apparentVelocity = (target.velocity - pursuant.velocity);


            Vector2 targetLine = (destination - pursuant.WorldPosition) + (apparentVelocity * deltaTime);

            Vector2 accelerationVector = Vector2.Normalize(targetLine);


            if (pursuant.GetType() == typeof(Ship))
            {
                Ship dude = (Ship)pursuant;
                dude.ApplyThrust(accelerationVector, gameTime);

            }
            else if (pursuant.GetType() == typeof(Missile))
            {

                Missile shooty = (Missile)pursuant;
                shooty.Boost(accelerationVector, gameTime);

            }
        }
    }
}
