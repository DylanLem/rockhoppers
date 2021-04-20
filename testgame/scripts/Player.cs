using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Rockhoppers.scripts
{
    public class Player
    {
        public UIElement playerUINav, playerUIHUD;

        public Radar playerRadar;


        public Entity playerEntity;

        public Vector2 playerPos { get => playerEntity.WorldPosition; }

       

        public Player()
        {
            Create_UI();
        }



        public void Create_UI()
        {
            

            playerUINav = UIBuilder.BuildUINav();

            playerRadar = new Radar("radar");


            playerUIHUD = UIBuilder.BuildUIHUD();

            playerUIHUD.Hide();

            playerUIHUD.SetScreenPos(Game1.ScreenSize - new Vector2(playerUIHUD.textureSize.X/2,  playerUIHUD.textureSize.Y * 1.5f));
            playerUINav.SetScreenPos(new Vector2(playerUINav.textureSize.X/1.5f, Game1.ScreenSize.Y - playerUINav.textureSize.Y/1.5f));
            playerRadar.SetScreenPos(Game1.ScreenSize - playerRadar.textureSize/1.5f);

            UIElement u = XMLParser.LoadUIItem("UIweapondata", "missile");
            u.ScreenPosition = Game1.ScreenSize - u.textureSize;
            UIText t = u.child_dict["TextCount"] as UIText;
            t.text = "1119";
            t.color = Color.PowderBlue;
        }

        public void Set_Entity(Entity e)
        {
            playerEntity = e;
            e.IsPlayer = true;
            e.IsOnScreen = true;
        }

        public void Update(GameTime gameTime)
        {
            //System.Diagnostics.Debug.WriteLine(playerEntity.SpritePath);
            

            if (playerUINav.child_dict.ContainsKey("Arrow"))
            {
                playerUINav.child_dict["Arrow"].orientation = (UtilityFunctions.GetAngleFromVector(playerEntity.velocity) + (float)Math.PI/2);
            }
            

           
           UpdateHUD();
            
        }



        public void UpdateHUD()
        {

            Ship playerShip = (Ship)playerEntity;

            Ship target = playerShip.target as Ship;

            UIText vText = (UIText)playerUIHUD.child_dict["playerVelocity"];
            vText.text = (int)playerShip.velocityMagnitude + " dm/s";

            UIText vtText = (UIText)playerUIHUD.child_dict["targetVelocity"];

            if (target != null)
                vtText.text = (int)(target.velocity - playerShip.velocity).Length() + " dm/s";
            else
                vtText.text = "waiting";

            if (playerShip.shootTimer >= playerShip.shotDelay)
            {
                playerUIHUD.child_dict["Light"].color = Color.Lime;
            }
            else
            {
                playerUIHUD.child_dict["Light"].color = Color.OrangeRed;
            }




            if (playerShip.target == null)
                return;
            
            else
            {
                playerUIHUD.child_dict["Ship"].SpritePath = target.SpritePath;
                playerUIHUD.child_dict["Ship"].orientation = target.orientation;
            }

            

        }

    }
}

