using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rockhoppers.scripts
{
    public class Radar : Entity
    {

        public Ship ParentShip { get; set; }
        UIElement uiDisplay { get; }

        public Radar(string xmlElement=null) : base(null)
        {
            uiDisplay = XMLParser.LoadUIItem("UIhuditems",xmlElement);
        }





        public override void Update(GameTime gameTime)
        {

            UIElement display = this.uiDisplay;

            if (this.ParentShip == null || uiDisplay == null)
                return;


            System.Diagnostics.Debug.WriteLine("hey bitch");


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

                UIElement UIBlip = new UIElement("radarblip");


                UIBlip.TextureScale = new Vector2(1f, 1f);
                UIBlip.spriteDepth -= 0.005f;

                Vector2 localPos = Vector2.Normalize(bogey.WorldPosition - ParentShip.WorldPosition);



                System.Diagnostics.Debug.WriteLine(localPos);


                UIBlip.localVector = -localPos;

                display.Add_Child(Convert.ToString(bogey.uniqueID), UIBlip);
            }

            foreach (KeyValuePair<string, UIElement> blip in display.child_dict)
            {
                if (blip.Key.Contains("Frame"))
                    continue;
                else
                {

                    if (Math.Abs(blip.Value.localVector.X) > 1 || Math.Abs(blip.Value.localVector.Y) > 1)
                    {
                        uiDisplay.Remove_Child(blip.Value);
                    }
                    else
                    {
                        Ship bogey = SceneManager.GetEntity(blip.Key) as Ship;

                        Vector2 localPos = (ParentShip.WorldPosition - bogey.WorldPosition) / (ParentShip.radarRange * 10000);
                        blip.Value.localVector = -localPos;
                    }
                }
            }

            base.Update(gameTime);

        }
    }
}
