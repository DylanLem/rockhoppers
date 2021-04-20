using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Rockhoppers.scripts
{
    public class UIElement : Entity
    {
        public Dictionary<string, UIElement> child_dict;

        bool is_child;
        bool is_localScale;

        public bool IsTemp = false;

        public Vector2 localVector;
        public Vector2 localScale;
        UIElement parent;


        public UIElement(string spritePath = null) : base(spritePath)
        {
            child_dict = new Dictionary<string, UIElement>();
            textureScale = new Vector2(8, 6);
            spriteDepth = 0.2f;
            IsOnScreen = true;
        }

        public void Set_Local_Position()
        {
            
            ScreenPosition = parent.ScreenPosition + (parent.textureSize * localVector / 2);      
        }

        public void Add_Child(string name, UIElement element)
        {
            child_dict[name] = element;
            element.parent = this;
            element.is_child = true;
            element.spriteDepth = this.spriteDepth - 0.01f;
            element.Set_Local_Position();
        }

        public UIElement Remove_Child(UIElement element)
        {
            foreach(KeyValuePair<string,UIElement> kvp in child_dict)
            {
                if(kvp.Value == element)
                {

                    child_dict.Remove(kvp.Key);
                    return kvp.Value;
                }
            }

            return null;
        }

        public UIElement Remove_Child(string name)
        {

            UIElement killed_element = null;

            if(child_dict.ContainsKey(name))
            {
                killed_element = child_dict[name];
                child_dict.Remove(name);
                killed_element.Delete();
            }

            return killed_element;
        }

        public void Kill_All_Children()
        {
            foreach(UIElement child in child_dict.Values)
            {
               child.Delete();
            }

            child_dict = new Dictionary<string, UIElement>();
        }

        public override void Update(GameTime gameTime)
        {
            if(is_child)
            {
                Set_Local_Position();
            }

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            base.Draw(spriteBatch);

            if (IsTemp)
            { 
                Delete();
                if (is_child)
                    parent.Remove_Child(this);
            }
        }

        public void Hide()
        {
            color.A = 128;

            if(child_dict.Count > 0)
            {
                foreach(UIElement child in child_dict.Values)
                {
                    child.Hide();
                }
            }
        }

    }
}
