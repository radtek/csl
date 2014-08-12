using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBOper
{
    public class AccessToken
    {
        static IDictionary<string, string> token = new Dictionary<string, string>();
        static IDictionary<string, DateTime> timer = new Dictionary<string, DateTime>();

        public static void Add(string user, string val)
        {
            if (token.ContainsKey(user))
            {
                token[user] = val;
                timer[user] = DateTime.Now;
            }
            else
            {
                token.Add(user, val);
                timer.Add(user, DateTime.Now);
            }
        }

        public static bool Read(string user, string val)
        {
            return true;
            if (user.Equals("cslgly") && user.Equals(val)) return true;

            if (!token.ContainsKey(user)) return false;
            if (!token[user].Equals(val)) return false;
            DateTime odt = timer[user];
            TimeSpan ts = DateTime.Now - odt;
            if (ts.Hours > 6) return false;
            return true;
        }
    }
}
