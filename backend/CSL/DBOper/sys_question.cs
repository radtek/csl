using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Util;
using System.Data;

namespace DBOper
{
    public class sys_question
    {
        public static string Insert(string title, string detail, string USER, string TOKEN)
        {
            if (!AccessToken.Read(USER, TOKEN)) return "登录超时";

            IDictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("title", title);
            dict.Add("detail", detail);
            DBHelper.Insert("sys_question", dict);
            return string.Empty;
        }
        public static string Update(string id, string title, string detail, string USER, string TOKEN)
        {
            if (!AccessToken.Read(USER, TOKEN)) return "登录超时";

            IDictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("title", title);
            dict.Add("detail", detail);
            IDictionary<string, string> fdict = new Dictionary<string, string>();
            fdict.Add("id", id);
            DBHelper.Update("sys_question", dict, fdict, "and");
            return string.Empty;
        }
        public static string Delete(string id, string USER, string TOKEN)
        {
            if (!AccessToken.Read(USER, TOKEN)) return "登录超时";

            IDictionary<string, string> fdict = new Dictionary<string, string>();
            fdict.Add("id", id);
            DBHelper.Delete("sys_question", fdict, "and");
            return string.Empty;
        }
        public static DataSet GetAll(string title, string pageSize, string pageIndex, string USER, string TOKEN)
        {
            if (!AccessToken.Read(USER, TOKEN)) return sys_dict.GetEmptyDs();

            IDictionary<string, string> fdict = new Dictionary<string, string>();
            if (title.Length > 0) fdict.Add("title", "%" + title + "%");
            return DBHelper.SelectPager("sys_question", "*", string.Empty, fdict, "and", pageSize, pageIndex);
        }
        public static DataSet GetOne(string id, string USER, string TOKEN)
        {
            if (!AccessToken.Read(USER, TOKEN)) return sys_dict.GetEmptyDs();

            IDictionary<string, string> fdict = new Dictionary<string, string>();
            fdict.Add("id", id);
            return DBHelper.Select("sys_question", "*", string.Empty, fdict, "and");
        }
    }
}