using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace TMCS.Server
{
    public static class Common
    {
        public static bool HasProperty(this object obj, string propName)
        {
            return obj.GetType().GetProperties().Any(prop => prop.Name.Equals(propName));
        }
    }
}
