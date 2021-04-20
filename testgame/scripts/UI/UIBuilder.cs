using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Rockhoppers.scripts
{
    public static class UIBuilder 
    {

        public static UIElement BuildUINav()
        {
            

            UIElement UICompass = new UIElement("compass");

            UIElement UIArrow = new UIElement("arrow");

            UICompass.textureScale *= 1.5f;
            UIArrow.textureScale *= 1.5f;


            UICompass.Add_Child("Compass", UICompass);
            UICompass.Add_Child("Arrow",UIArrow);
            UIArrow.spriteDepth -= 0.01f;

      

            
            UIArrow.color = Color.PeachPuff;

            return UICompass;
        }

        public static UIElement BuildUIRadar()
        {
            
            UIElement UIRadar = new UIElement("radar");
            UIElement UIRadarFrame = new UIElement("radarframe");

            UIRadar.textureScale = new Vector2(10, 10);
            UIRadarFrame.textureScale = new Vector2(10, 10);

            UIRadar.Add_Child("Frame", UIRadarFrame);

            UIRadarFrame.spriteDepth -= 0.02f;
            

            return UIRadar;
        }

        public static UIElement BuildUIHUD()
        {
            UIElement UIBackGround = new UIElement("uistatus");
            UIElement UIShip = new UIElement();
            UIText UIplayerVelocity = new UIText("fontMain");
            UIText UItargetVelocity = new UIText("fontMain");

            UIElement UILight = new UIElement("uilight");

            UIBackGround.spriteDepth += 0.01f;

            UIShip.color = Color.Lime;
            UIShip.textureScale *= 0.35f;

            UIplayerVelocity.text = "m/s";
            UIplayerVelocity.color = Color.AntiqueWhite;

            UItargetVelocity.text = "m/s";
            UItargetVelocity.color = Color.Lime;

            UItargetVelocity.textureScale = Vector2.One/2;
            UIplayerVelocity.textureScale = Vector2.One * 0.75f;
            UILight.textureScale *= 0.5f;


            UIBackGround.Add_Child("Ship",UIShip);
            UIBackGround.Add_Child("playerVelocity", UIplayerVelocity);
            UIBackGround.Add_Child("targetVelocity", UItargetVelocity);
            UIBackGround.Add_Child("Light", UILight);

            UIplayerVelocity.localVector = new Vector2(.48f, -.38f);
            UItargetVelocity.localVector = new Vector2(.30f, 0.67f);
            UIShip.localVector = new Vector2(-.25f, 0.67f);
            UILight.localVector = Vector2.UnitY - (UILight.textureSize/UIBackGround.textureSize);
            

            return UIBackGround;
        }

    }
}
