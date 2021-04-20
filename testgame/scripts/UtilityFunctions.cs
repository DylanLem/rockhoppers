using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace testgame.scripts
{
    static class UtilityFunctions
    {

        public static float GetVectorMagnitude(Vector2 vector)
        {
            float magnitude = Vector2.Distance(Vector2.Zero, vector);

            return magnitude;
        }

        public static Vector2 GetVectorFromAngle(float angle)
        {
            
            Vector2 unit_vector = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));

            return unit_vector;
        }

        public static float GetAngleFromVector(Vector2 vector)
        {
            return (float)Math.Atan2(vector.Y, vector.X);
        }


        public static bool CheckCollision(Circle c1, Circle c2)
        {
            if (Vector2.Distance(c1.Position, c2.Position) < c1.Radius + c2.Radius) 
            {
                return true;
            }

            return false;
        }

        public static bool CheckCollision(Entity e1, Entity e2)
        {
            if (Vector2.Distance(e1.WorldPosition,e2.WorldPosition) < e1.hitbubble.Radius + e2.hitbubble.Radius)
                return true;
            
            else
                return false;
        }


    }
}
