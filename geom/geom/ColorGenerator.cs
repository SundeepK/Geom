using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace geom
{
   public static class ColorGenerator
    {
       public static Color getRanColor()
       {

           Random ran = new Random();
           int colPicker = ran.Next(1, 6);

           switch (colPicker)
           {
               case 1:
                   return new Color(165, 252, 255);//blue
               case 2:
                   return new Color(254, 129, 253);//purple
               case 3:
                   return new Color(255, 120, 120);//red
               case 4:
                   return new Color(148, 252, 209);//aqua
               case 5:
                   return new Color(252, 226, 148);//orange
               case 6:
                   return new Color(215, 145, 190);//pink
               default:
                   return new Color(37, 248, 97);
           }

       }

    }
}
