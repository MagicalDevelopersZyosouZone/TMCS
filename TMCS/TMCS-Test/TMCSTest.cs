using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TMCS_Test
{
    public class TMCSTest
    {
        public static Random rand = new Random();
        public static string RandomSalt(long length)
        {
            var salt = "";
            for(var i=0;i<length;i++)
            {
                salt += rand.Next(16).ToString("X");
            }
            return salt;
        }
    }
}
