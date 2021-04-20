using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace testgame.scripts
{
    class UIButton : UIElement
    {


        public UIButton(string frameSprite ="buttonframe",string switchSprite="buttonswitch", Color color = default)
        {
            UIElement buttonFrame = new UIElement(frameSprite);
            UIElement buttonSwitch = new UIElement(switchSprite);
            this.color = color;

            this.Add_Child("Frame",buttonFrame);
            this.Add_Child("Switch",buttonSwitch);
        }

    }
}
