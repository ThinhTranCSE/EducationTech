using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationTech.Shared.Extensions
{
    public static class StringConverterExtensions
    {
        public static T ConvertTo<T>(this string str)
        {
            return (T)Convert.ChangeType(str, typeof(T));
        }

        public static T GetValue<T>(this string str)
        {
            return (T)Convert.ChangeType(str, typeof(T));
        }

    }
}
