using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Rockhoppers.scripts
{
    public class Tracker : Entity
    {

        public Ship ParentShip { get; set; }
        UIElement uiDisplay { get; }

        public Tracker(string xmlElement=null) : base(null)
        {
            uiDisplay = XMLParser.LoadUIItem("UIhuditems","tracker");
            uiDisplay.child_dict["ShipDisplay"].color = Color.Lime;
        }





        public override void Update(GameTime gameTime)
        {
            if (this.ParentShip == null || uiDisplay == null)
            {
                System.Diagnostics.Debug.WriteLine("parent; " + ParentShip);
                return;
            }


            if (Input.kbState.IsKeyDown(Keys.Tab) && ParentShip.trackedShips.Count > 0 && Input.TryInput(Keys.Tab))
            {

                if (ParentShip.targetIndex < ParentShip.trackedShips.Count - 1)
                    ParentShip.targetIndex += 1;
                else
                    ParentShip.targetIndex = 0;
                ParentShip.TargetShip(ParentShip.trackedShips[ParentShip.targetIndex]);


            }

            

            UIElement display = this.uiDisplay;
            Ship target = this.ParentShip.target as Ship;

            

           

            UIText vtText = (UIText)display.child_dict["TargetVelocity"];

            if (target != null)
                vtText.text = (int)(target.velocity - ParentShip.velocity).Length() + " dm/s";
            else
                vtText.text = "waiting";




            if (ParentShip.target == null)
                return;

            else
            {
                display.child_dict["ShipDisplay"].SpritePath = target.SpritePath;
                display.child_dict["ShipDisplay"].orientation = target.orientation;
            }


            base.Update(gameTime);

        }
    }
}
