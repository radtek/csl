using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Util;
using System.Data;

namespace DBOper
{
    public class dict_match_result
    {
        static IDictionary<string, string> dict;
        public static string GetName(string id)
        {
            if (dict == null)
            {
                DataSet ds = DBHelper.Select("dict_match_result", "*", string.Empty, new Dictionary<string, string>(), "and");
                dict = new Dictionary<string, string>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    dict.Add(dr["id"].ToString(), dr["name"].ToString());
                }
            }
            return dict[id];
        }
    }
}
