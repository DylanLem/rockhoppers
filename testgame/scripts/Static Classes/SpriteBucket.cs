using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Graphics.SpriteFont;

namespace Rockhoppers.scripts
{
    public static class SpriteBucket
    {

        public static Dictionary<string,Texture2D> Sprites = new Dictionary<string, Texture2D>();
        public static List<string> sprite_paths = new List<string>
        {
            "ship",
            "ship2",
            "bullet",
            "cursor",
            "stars",
            "UIsaffronpurple",
            "arrow",
            "compass",
            "radar",
            "radarblip",
            "radarframe",
            "missile",
            "uistatus",
            "explosion",
            "buttonswitch",
            "buttonframe",
            "uilight",
            "uimissile"
        };

        public static List<string> spriteFont_paths = new List<string>
        {
            "fontMain"
        };
        public static Dictionary<string, SpriteFont> spriteFonts = new Dictionary<string, SpriteFont>();

        

    }
}
