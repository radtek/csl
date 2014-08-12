using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Util;
using System.Data;

namespace DBOper
{
    public class sys_message
    {
        public static string Insert(string title, string detail, string receive_sys_user_id, string send_sys_user_id, string USER, string TOKEN)
        {
            if (!AccessToken.Read(USER, TOKEN)) return "登录超时";

            IDictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("title", title);
            dict.Add("detail", detail);
            dict.Add("receive_sys_user_id", receive_sys_user_id);
            dict.Add("send_sys_user_id", send_sys_user_id);
            dict.Add("send_datetime", DateTime.Now.ToString());
            DBHelper.Insert("sys_message", dict);
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
            DBHelper.Update("sys_message", dict, fdict, "and");
            return string.Empty;
        }
        public static string Delete(string id, string USER, string TOKEN)
        {
            if (!AccessToken.Read(USER, TOKEN)) return "登录超时";

            IDictionary<string, string> fdict = new Dictionary<string, string>();
            fdict.Add("id", id);
            DBHelper.Delete("sys_message", fdict, "and");
            return string.Empty;
        }
        public static DataSet GetAll(string receive_sys_user_id, string title, string pageSize, string pageIndex, string USER, string TOKEN)
        {
            if (!AccessToken.Read(USER, TOKEN)) return sys_dict.GetEmptyDs();

            IDictionary<string, string> fdict = new Dictionary<string, string>();
            if (receive_sys_user_id.Length > 0) fdict.Add("receive_sys_user_id", receive_sys_user_id);
            if (title.Length > 0) fdict.Add("title", "%" + title + "%");
            return DBHelper.SelectPager("sys_message_view", "*", string.Empty, fdict, "and", pageSize, pageIndex);
        }
        public static DataSet GetOne(string id, string USER, string TOKEN)
        {
            if (!AccessToken.Read(USER, TOKEN)) return sys_dict.GetEmptyDs();

            IDictionary<string, string> fdict = new Dictionary<string, string>();
            fdict.Add("id", id);
            return DBHelper.Select("sys_message", "*", string.Empty, fdict, "and");
        }
    }
}
