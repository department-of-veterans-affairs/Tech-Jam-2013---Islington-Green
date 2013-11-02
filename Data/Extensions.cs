using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IG.Data
{
    public static class Extensions
    {
        public static string Clean(this string s)
        {
            if (s == null) return string.Empty;
            s = s.Replace("/", "").Trim();
            s = s.Replace(" ", "");
            return s;

        }

    }
}
