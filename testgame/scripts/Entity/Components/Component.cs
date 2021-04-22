using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rockhoppers.scripts
{
    public abstract class Component : Entity
    {
        bool isFunctional { get; set; }
        public UIElement uiDisplay { get; set; }
        public Ship ParentShip { get; set; }

        public Vector2 localOffset { get; set; }

       

        enum PowerType
        {
            Electrical,
            Combustion,
            Alien
        }

        public Component(string _spritePath) : base(_spritePath)
        {
            ParentShip = null;
        }

        public Component(string _spritePath, Ship parentShip) : base(_spritePath)
        {
            ParentShip = parentShip;
        }

        public override void Update(GameTime gameTime)
        {

            

            if (! (ParentShip is null))
            {
                this.WorldPosition = ParentShip.WorldPosition;
            }
            
            else if (ParentShip is null)
            {
                if (UtilityFunctions.CheckCollision(this, SceneManager.players[0].playerEntity))
                {
                    
                    Ship s = SceneManager.players[0].playerEntity as Ship;
                    s.InstallComponent(this);
                }
            }

            base.Update(gameTime);
            if (!isFunctional)
                return;
        }

        public void CheckStatus()
        {

        }

        public void SetParent(Ship ship)
        {
            if(ship is Ship)
            {
                this.ParentShip = ship;
                this.spriteDepth = ParentShip.spriteDepth - 0.000001f;
            }
        }
    }
}
