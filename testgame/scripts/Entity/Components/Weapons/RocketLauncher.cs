using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rockhoppers.scripts
{
    public class RocketLauncher : Weapon
    {

        public Ship target;
        public RocketLauncher() : base("rocketlauncher")
        {
            this.TextureScale = new Microsoft.Xna.Framework.Vector2(2); 
            uiDisplay = XMLParser.LoadUIItem("UIweapondata", "missiles");
            SetAmmo(typeof(Missile));
            weaponType = "Missile";
        }


        public override void Update(GameTime gameTime)
        {
            
            base.Update(gameTime);
            
            if (ParentShip != null)
                target = (Ship)ParentShip.target;

            if(target != null)
            {
                orientation += (UtilityFunctions.GetAngleFromVector(target.WorldPosition - this.WorldPosition) + (float)Math.PI / 2 - this.orientation) / 4;
            }
            else if(ParentShip != null)
            {
                orientation = UtilityFunctions.GetAngleFromVector(Vector2.Lerp(UtilityFunctions.GetVectorFromAngle(ParentShip.orientation), UtilityFunctions.GetVectorFromAngle(this.orientation), 0.85f));
            }
        }

        public override void Fire()
        {
            base.Fire();
        }

    }
}
