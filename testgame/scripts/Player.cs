using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Rockhoppers.scripts
{
    public class Player
    {
        public UIElement playerUINav, playerUIRadar, playerUIHUD;

        public Entity playerEntity;

        public Vector2 playerPos { get => playerEntity.WorldPosition; }

       

        public Player()
        {
            Create_UI();
        }



        public void Create_UI()
        {
            

            playerUINav = UIBuilder.BuildUINav();

            playerUIRadar = UIBuilder.BuildUIRadar();

            playerUIHUD = UIBuilder.BuildUIHUD();

            playerUIHUD.Hide();

            playerUIHUD.SetScreenPos(Game1.ScreenSize - new Vector2(playerUIHUD.textureSize.X/2,  playerUIHUD.textureSize.Y * 1.5f));
            playerUINav.SetScreenPos(new Vector2(playerUINav.textureSize.X/1.5f, Game1.ScreenSize.Y - playerUINav.textureSize.Y/1.5f));
            playerUIRadar.SetScreenPos(Game1.ScreenSize - playerUIRadar.textureSize/1.5f);
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
            

           UpdateRadar();
           UpdateHUD();
            
        }

        private void UpdateRadar()
        {
            Ship ship = (Ship)playerEntity;
            UIElement radar = playerUIRadar;


            foreach (Ship bogey in ship.trackedShips)
            {
                if (radar.child_dict.ContainsKey(Convert.ToString(bogey.uniqueID)))
                {
                    if (bogey == ship.target)
                    {
                        radar.child_dict[Convert.ToString(bogey.uniqueID)].color = Color.OrangeRed;
                    }
                    else
                    {
                        radar.child_dict[Convert.ToString(bogey.uniqueID)].color = Color.White;
                    }
                    continue;
                }

                UIElement UIBlip = new UIElement("radarblip");
                
 
                UIBlip.textureScale = new Vector2(1f, 1f);
                UIBlip.spriteDepth -= 0.005f;

                Vector2 localPos = Vector2.Normalize(bogey.WorldPosition - ship.WorldPosition);



                System.Diagnostics.Debug.WriteLine(localPos);


                UIBlip.localVector = -localPos;

                radar.Add_Child(Convert.ToString(bogey.uniqueID), UIBlip);
            }

            foreach(KeyValuePair<string,UIElement> blip in radar.child_dict)
            {
                if (blip.Key.Contains("Frame"))
                    continue;
                else
                {

                    if(Math.Abs(blip.Value.localVector.X) > 1 || Math.Abs(blip.Value.localVector.Y) > 1)
                    {
                        radar.Remove_Child(blip.Value);
                    }
                    else
                    {
                        Ship bogey = SceneManager.GetEntity(blip.Key) as Ship;

                        Vector2 localPos = (playerEntity.WorldPosition - bogey.WorldPosition) / (ship.radarRange * 10000);
                        blip.Value.localVector = -localPos;
                    }
                }
            }  
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

