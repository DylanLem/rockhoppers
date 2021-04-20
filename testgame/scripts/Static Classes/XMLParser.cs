using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Microsoft.Xna.Framework;

namespace Rockhoppers.scripts
{
    public static class XMLParser
    {
        public static string defaultPath = "C:/Users/Dilly/source/repos/testgame/testgame/XML/";

        public static UIElement LoadUIItem(string fileName, string objectName)
        {
            UIElement UIbase = new UIElement();

            XmlTextReader reader = new XmlTextReader(defaultPath + fileName + ".xml");


            while(! reader.EOF)
            {
               if(reader.Name != "base")
                    reader.Read();

                if (reader.GetAttribute("name") == objectName)
                    break;

                reader.Read();
                reader.MoveToElement();
            }
            



            while (! reader.EOF)
            {
                if(reader.IsStartElement())
                {
                    if (reader.Name == "base")
                    {
                        UIbase = Build_Child(reader);
                    }


                    if (reader.Name == "child")
                    {
                        UIbase.Add_Child(reader.GetAttribute("name"), Build_Child(reader));
                    }
                }    
                

                reader.Read();
                reader.MoveToElement();

            }

            System.Diagnostics.Debug.WriteLine(UIbase.SpritePath);
            return UIbase;
        }

        private static UIElement Build_Child(XmlTextReader reader)
        {

            if (reader.GetAttribute("type") == "text")
            {
                reader.Read();
                reader.ReadStartElement("font");
                string font = reader.ReadString();

                UIText uiText = new UIText(font);
                reader.ReadEndElement();


                reader.ReadStartElement("vector");
                string[] vector = reader.ReadString().Split(',');
                uiText.localVector = new Vector2((float)Convert.ToDecimal(vector[0]), (float)Convert.ToDecimal(vector[1]));
                reader.ReadEndElement();


                reader.ReadStartElement("scaling");
                string[] scaling = reader.ReadString().Split(',');
                uiText.TextureScale  = new Vector2((float)Convert.ToDecimal(scaling[0]), (float)Convert.ToDecimal(scaling[1]));
                reader.ReadEndElement();

                return uiText;
            }

            else if (reader.GetAttribute("type") == "sprite")
            {
                reader.Read();
                reader.ReadStartElement("sprite");

                UIElement uiSprite = new UIElement(reader.ReadString());
                reader.ReadEndElement();

                reader.ReadStartElement("vector");
                string[] vector = reader.ReadString().Split(',');
                uiSprite.localVector = new Vector2((float)Convert.ToDecimal(vector[0]), (float)Convert.ToDecimal(vector[1]));
                reader.ReadEndElement();

                uiSprite.Set_Local_Position(uiSprite.localVector);


                reader.ReadStartElement("scaling");
                string[] scaling = reader.ReadString().Split(',');
                uiSprite.TextureScale = new Vector2((float)Convert.ToDecimal(scaling[0]), (float)Convert.ToDecimal(scaling[1]));
                reader.ReadEndElement();

                uiSprite.Set_Local_Position(uiSprite.localVector);

                return uiSprite;

            }

            return new UIElement();
        }


        
    }
}
