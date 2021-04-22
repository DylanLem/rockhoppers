using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rockhoppers.scripts
{
    public abstract class Weapon : Component
    {
        Type ammo = null;
        int bulletCount = 10;

        public string weaponType;
        
        public Weapon(string _spritePath) : base(_spritePath)
        {
           spriteDepth = 0.2f;

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if(this.ParentShip == null && this.uiDisplay.isHidden == false)
                    this.uiDisplay.Hide(true);


            else if(this.ParentShip != null)
            {
                this.WorldPosition = ParentShip.WorldPosition;
                this.uiDisplay.Hide(false);

                this.ScreenPosition -= UtilityFunctions.GetVectorFromAngle(ParentShip.orientation - MathF.PI/2) * TextureScale * 4;
                this.WorldPosition -= UtilityFunctions.GetVectorFromAngle(ParentShip.orientation - MathF.PI / 2) * TextureScale * 4;
            }
            System.Diagnostics.Debug.WriteLine(this.uiDisplay.isHidden);
        }

        public virtual void Fire()
        {
            if (bulletCount <= 0)
                return;

            object bullet = Activator.CreateInstance(ammo,this);

        }

        public void SetAmmo(Type bulletType)
        {
            if (bulletType == typeof(Bullet))
                return;
            ammo = bulletType;
        }


    }
}
