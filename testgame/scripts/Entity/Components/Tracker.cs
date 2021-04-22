using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Rockhoppers.scripts
{
    public class Tracker : Component
    {

        

        public Tracker(string xmlElement=null) : base(null)
        {
            uiDisplay = XMLParser.LoadUIItem("UIhuditems","tracker");
            uiDisplay.child_dict["ShipDisplay"].color = Color.Lime;
        }





        public override void Update(GameTime gameTime)
        {
            if (this.ParentShip == null || uiDisplay == null)
            {
                return;
            }


            if (Input.kbState.IsKeyDown(Keys.Tab) && ParentShip.trackedShips.Count > 0 && Input.TryInput(Keys.Tab))
            {
                
                if (ParentShip.targetIndex < ParentShip.trackedShips.Count - 1)
                    ParentShip.targetIndex += 1;
                else
                {
                    ParentShip.targetIndex = -1;
                    ParentShip.target = null;
                    return;
                }
                ParentShip.TargetShip(ParentShip.trackedShips[ParentShip.targetIndex]);


            }

            

            UIElement display = this.uiDisplay;
            Ship target = this.ParentShip.target as Ship;

            

           

            UIText vtText = (UIText)display.child_dict["TargetVelocity"];
            UIText dtText = (UIText)display.child_dict["TargetDistance"];
            UIText hpText = (UIText)display.child_dict["TargetHealth"];

            if (target != null)
            {
                float vAngle = Math.Abs(UtilityFunctions.GetAngleFromVector( ParentShip.velocity - target.velocity));
                float dAngle = Math.Abs(UtilityFunctions.GetAngleFromVector(ParentShip.WorldPosition - target.WorldPosition));
                float fAngle = vAngle - dAngle;
                float vMag = (float)Math.Cos(fAngle) * target.velocityMagnitude;

                vtText.text = "v: "  + (int)vMag + " dm/s";
                dtText.text = "d: " + Math.Floor(Vector2.Distance(target.WorldPosition, ParentShip.WorldPosition) / 100) + "Km";
                hpText.text = "Hull Status: " + (int)target.Health;
            }
                
            else
            {
                vtText.text = "waiting";
                dtText.text = "waiting";
            }
                


            if (ParentShip.target == null)
                display.child_dict["ShipDisplay"].SpritePath = "radarblip";

            else
            {
                display.child_dict["ShipDisplay"].SpritePath = target.SpritePath;
                display.child_dict["ShipDisplay"].orientation = target.orientation;
            }


            base.Update(gameTime);

        }
    }
}
