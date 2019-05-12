using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SL.Utils
{
    public class SerialTool
    {
        public static string ToSerial(int index)
        {
            if (index / 10 < 1)
            {
                return $"000{index}";
            }
            else if (index / 10 >= 1 && index / 100 < 1)
            {
                return $"00{index}";
            }
            else if(index / 100 >= 1 && index / 1000 < 1)
            {
                return $"0{index}";
            }
            else
            {
                return index.ToString(); 
            }
        }

        public static string NewID()
        {
            return Guid.NewGuid().ToString().Replace("-","");
        }
    }
}
