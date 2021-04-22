using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Text;

namespace Rockhoppers.scripts
{
    static class Input
    {
        public static KeyboardState kbState;

        public static MouseState mouseState;
        public static Vector2 mousePosition { get => new Vector2(mouseState.X, mouseState.Y);}

        
        public static Dictionary<Keys, float> lockedInputs = new Dictionary<Keys, float>();
        public static float inputDelay = 0.25f;


        public static void Update(GameTime gameTime)
        {
            kbState = Keyboard.GetState();
            mouseState = Mouse.GetState();


            
            foreach(KeyValuePair<Keys,float> kvp in lockedInputs)
            {
                if(kvp.Value > inputDelay)
                {
                    lockedInputs.Remove(kvp.Key);
                    return;
                }
                else
                {
                    lockedInputs[kvp.Key] += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    return;
                }
            }
           
        }


        public static Vector2 GetMovementDirection()
        {
            Vector2 direction = Vector2.Zero;

            if(kbState.IsKeyDown(Keys.A))
                direction.X -= 1;
            if (kbState.IsKeyDown(Keys.D))
                direction.X += 1;
            if (kbState.IsKeyDown(Keys.W))
                direction.Y -= 1;
            if (kbState.IsKeyDown(Keys.S))
                direction.Y += 1;

            return direction;
        }


        public static Vector2 GetMouseDistanceFromEntity(Entity entity)
        {
            Vector2 distance = mousePosition - entity.ScreenPosition;

            return distance;
        }

        public static float GetMouseDirectionFromEntity(Entity entity)
        {
            Vector2 directionVector = Vector2.Normalize(GetMouseDistanceFromEntity(entity));

            double angle = Math.Atan2(directionVector.Y, directionVector.X);


            return (float)angle;
        }

        public static bool TryInput(Keys key)
        {
            if ( ! lockedInputs.ContainsKey(key))
            {
                lockedInputs.TryAdd(key, 0f);
                return true;
            }
                

            return false;
        }
    }
}
