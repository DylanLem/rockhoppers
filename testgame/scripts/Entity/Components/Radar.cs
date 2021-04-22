using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rockhoppers.scripts
{
    public class Radar : Component
    {


        public Radar(string xmlElement=null) : base(null)
        {
            uiDisplay = XMLParser.LoadUIItem("UIhuditems",xmlElement);
            uiDisplay.child_dict["Frame"].spriteDepth -= 0.02f;
        }





        public override void Update(GameTime gameTime)
        {

            UIElement display = this.uiDisplay;

            if (this.ParentShip == null || uiDisplay == null)
                return;


            foreach (Ship bogey in ParentShip.trackedShips)
            {
                if (display.child_dict.ContainsKey(Convert.ToString(bogey.uniqueID)))
                {
                    if (bogey == ParentShip.target)
                    {
                        display.child_dict[Convert.ToString(bogey.uniqueID)].color = Color.OrangeRed;
                    }
                    else
                    {
                        display.child_dict[Convert.ToString(bogey.uniqueID)].color = Color.White;
                    }
                    continue;
                }
                else
                {
                    UIElement UIBlip = new UIElement("radarblip")
                    {
                        TextureScale = new Vector2(1f, 1f)
                    };

                    UIBlip.spriteDepth -= 0.08f;
                    Vector2 localPos = (bogey.WorldPosition - ParentShip.WorldPosition) / (ParentShip.radarRange * 10000);

                    UIBlip.localVector = localPos;

                    display.Add_Child(Convert.ToString(bogey.uniqueID), UIBlip);
                }
                

                
            }

            foreach (KeyValuePair<string, UIElement> blip in display.child_dict)
            {


                if (blip.Key.Contains("Frame"))
                {
                    continue;
                }
                    
                if (SceneManager.GetEntity(blip.Key) == null)
                    display.Remove_Child(blip.Key);
                else
                {

                    if (Math.Abs(blip.Value.localVector.X) >= 0.90 || Math.Abs(blip.Value.localVector.Y) > .90)
                    {
                        uiDisplay.Remove_Child(blip.Value);
                        blip.Value.Delete();
                    }
                    else
                    {
                        Ship bogey = SceneManager.GetEntity(blip.Key) as Ship;

                        Vector2 localPos = 0.9f * ( bogey.WorldPosition - ParentShip.WorldPosition ) / (ParentShip.radarRange * 10000);
                        blip.Value.localVector = localPos;
                    }
                }
            }

            base.Update(gameTime);

        }
    }
}
