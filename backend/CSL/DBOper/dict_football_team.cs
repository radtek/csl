using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Util;
using System.Data;

namespace DBOper
{
    public class dict_football_team
    {
        public static string Insert(string name, string odr, string USER, string TOKEN)
        {
            if (!AccessToken.Read(USER, TOKEN)) return "登录超时";

            IDictionary<string, string> fdict = new Dictionary<string, string>();
            fdict.Add("name", name);
            if (DBHelper.SelectRowCount("dict_football_team", fdict, "or") > 0) return "该球队已存在";

            IDictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("name", name);
            dict.Add("odr", odr);
            DBHelper.Insert("dict_football_team", dict);
            return string.Empty;
        }
        public static string Update(string id, string name, string odr, string delete_flag, string USER, string TOKEN)
        {
            if (!AccessToken.Read(USER, TOKEN)) return "登录超时";

            IDictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("name", name);
            dict.Add("odr", odr);
            dict.Add("delete_flag", delete_flag);
            IDictionary<string, string> fdict = new Dictionary<string, string>();
            fdict.Add("id", id);
            DBHelper.Update("dict_football_team", dict, fdict, "and");
            return string.Empty;
        }
        public static string Delete(string id, string USER, string TOKEN)
        {
            if (!AccessToken.Read(USER, TOKEN)) return "登录超时";

            IDictionary<string, string> fdict = new Dictionary<string, string>();
            fdict.Add("id", id);
            DBHelper.Delete("dict_football_team", fdict, "and");
            return string.Empty;
        }
        public static DataSet GetAll(string delete_flag, string pageSize, string pageIndex, string USER, string TOKEN)
        {
            if (!AccessToken.Read(USER, TOKEN)) return sys_dict.GetEmptyDs();

            IDictionary<string, string> fdict = new Dictionary<string, string>();
            if (delete_flag.Length > 0) fdict.Add("delete_flag", delete_flag);
            if (pageSize.Length == 0) return DBHelper.Select("dict_football_team", "*", "odr,id", fdict, "and");
            return DBHelper.SelectPager("dict_football_team", "*", "odr,id", fdict, "and", pageSize, pageIndex);
        }
        public static DataSet GetOne(string id, string USER, string TOKEN)
        {
            if (!AccessToken.Read(USER, TOKEN)) return sys_dict.GetEmptyDs();

            IDictionary<string, string> fdict = new Dictionary<string, string>();
            fdict.Add("id", id);
            return DBHelper.Select("dict_football_team", "*", string.Empty, fdict, "and");
        }
    }
}
