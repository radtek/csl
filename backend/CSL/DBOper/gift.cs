using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Util;
using System.Data;

namespace DBOper
{
    public class gift
    {
        public static string Insert(string title, string description, string price, string inventory, string off_shelf_datetime, string odr, string USER, string TOKEN)
        {
            if (!AccessToken.Read(USER, TOKEN)) return "登录超时";

            IDictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("title", title);
            dict.Add("description", description);
            dict.Add("price", price);
            dict.Add("inventory", inventory);
            dict.Add("off_shelf_datetime", off_shelf_datetime);
            dict.Add("odr", odr);
            DBHelper.Insert("gift", dict);
            return string.Empty;
        }
        public static string Update(string id, string title, string description, string price, string inventory, string off_shelf_datetime, string odr, string delete_flag, string USER, string TOKEN)
        {
            if (!AccessToken.Read(USER, TOKEN)) return "登录超时";

            IDictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("title", title);
            dict.Add("description", description);
            dict.Add("price", price);
            dict.Add("inventory", inventory);
            dict.Add("off_shelf_datetime", off_shelf_datetime);
            dict.Add("odr", odr);
            dict.Add("delete_flag", delete_flag);
            IDictionary<string, string> fdict = new Dictionary<string, string>();
            fdict.Add("id", id);
            DBHelper.Update("gift", dict, fdict, "and");
            return string.Empty;
        }
        public static string Delete(string id, string USER, string TOKEN)
        {
            if (!AccessToken.Read(USER, TOKEN)) return "登录超时";

            IDictionary<string, string> fdict = new Dictionary<string, string>();
            fdict.Add("id", id);
            DBHelper.Delete("gift", fdict, "and");
            return string.Empty;
        }
        public static DataSet GetAll(string title, string delete_flag, string pageSize, string pageIndex, string USER, string TOKEN)
        {
            if (!AccessToken.Read(USER, TOKEN)) return sys_dict.GetEmptyDs();

            IDictionary<string, string> fdict = new Dictionary<string, string>();
            if (title.Length > 0) fdict.Add("title", "%" + title + "%");
            if (delete_flag.Length > 0) fdict.Add("delete_flag", delete_flag);
            return DBHelper.SelectPager("gift", "*", "odr,id", fdict, "and", pageSize, pageIndex);
        }
        public static DataSet GetOne(string id, string USER, string TOKEN)
        {
            if (!AccessToken.Read(USER, TOKEN)) return sys_dict.GetEmptyDs();

            IDictionary<string, string> fdict = new Dictionary<string, string>();
            fdict.Add("id", id);
            return DBHelper.Select("gift", "*", string.Empty, fdict, "and");
        }
    }
}
