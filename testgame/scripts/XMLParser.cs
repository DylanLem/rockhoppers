using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Rockhoppers.scripts
{
    public static class XMLParser
    {
        public static string defaultPath = "C:/Users/Dilly/source/repos/testgame/testgame/XML/";

        public static UIElement LoadUIItem(string XMLname)
        {
            UIElement item = new UIElement();

            XmlTextReader reader = new XmlTextReader(defaultPath + XMLname);
           

            reader.Read();

            while (reader.Read())
            {
                if (reader.IsStartElement())
                {
                    if(reader.Name == "sprite")
                    {
                        item.SpritePath = reader.ReadString();
                        System.Diagnostics.Debug.WriteLine(item.SpritePath);
                    }
                    

                    if (reader.Depth > 0 && reader.HasValue)
                    {
                        System.Diagnostics.Debug.WriteLine(reader.ReadString());
                    }

                }   
                


            }


            return item;
        }


        
    }
}
