using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Rockhoppers.scripts
{
    public class Player
    {
        public UIElement playerUINav, playerUIHUD;

        public Tracker tracker;

        public Radar playerRadar;


        public Entity playerEntity;

        public Vector2 playerPos { get => playerEntity.WorldPosition; }

       

        public Player()
        {
            
        }



        public void Create_UI()
        {
            

            playerUINav = UIBuilder.BuildUINav();

            playerRadar = new Radar("radar");


            tracker = new Tracker();
            tracker.ParentShip = (Ship)this.playerEntity;


            
            playerUINav.SetScreenPos(new Vector2(playerUINav.textureSize.X/1.5f, Game1.ScreenSize.Y - playerUINav.textureSize.Y/1.5f));
            tracker.SetScreenPos(Game1.ScreenSize - tracker.textureSize/2 - new Vector2(0,Game1.ScreenSize.Y/2));
            playerRadar.SetScreenPos(Game1.ScreenSize - playerRadar.textureSize/1.5f);

            
        }

        public void Set_Entity(Entity e)
        {
            playerEntity = e;
            e.IsPlayer = true;
            e.IsOnScreen = true;
        }

        public void Update(GameTime gameTime)
        {
            
            

            if (playerUINav.child_dict.ContainsKey("Arrow"))
            {
                playerUINav.child_dict["Arrow"].orientation = (UtilityFunctions.GetAngleFromVector(playerEntity.velocity) + (float)Math.PI/2);
            }
            

            
        }





    }
}

