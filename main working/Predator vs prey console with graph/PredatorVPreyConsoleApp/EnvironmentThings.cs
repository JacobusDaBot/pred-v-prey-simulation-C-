using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp
{
    internal class EnvironmentThings
    {
        public static readonly Random rand = new Random();
        internal class WaterPlace
        { 
            public int xpos, ypos;
            public string WaterASCII;        
            public WaterPlace(int formHeight, int formWidth, int placenum)
            {
                xpos = rand.Next(formWidth);
                ypos = rand.Next(formHeight);
                WaterASCII = "W";                
            }
        }
        internal class PlantThings
        {  
            public int xpos, ypos;
            public PlantThings(int formHeight, int formWidth)
            {
                xpos = rand.Next(formWidth);
                ypos = rand.Next(formHeight);              
            }
        }
    }
}
