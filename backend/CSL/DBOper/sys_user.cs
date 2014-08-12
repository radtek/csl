using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Util;
using System.Data;
using System.Data.SqlClient;

namespace DBOper
{
    public class sys_user
    {
        static string MakePwd(string login_name, string login_pwd, string login_pwd_guid)
        {
            string login_pwd_code = "~!@#$%^&*()";
            return Helper.GetMd5(login_pwd_guid + login_name + login_pwd_code + login_pwd);
        }
        public static DataSet Login(string login_name, string login_pwd, string login_ip)
        {
            IDictionary<string, string> fdict = new Dictionary<string, string>();
            fdict.Add("login_name", login_name);
            fdict.Add("delete_flag", "IS NULL");
            DataSet ds = DBHelper.Select("sys_user", "*", string.Empty, fdict, "and");
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (!MakePwd(login_name, login_pwd, ds.Tables[0].Rows[0]["login_pwd_guid"].ToString()).Equals(ds.Tables[0].Rows[0]["login_pwd"].ToString())) return sys_dict.GetEmptyDs();

                string token = Helper.GetGuid();
                DataColumn dc = new DataColumn("TOKEN", typeof(string));
                dc.DefaultValue = token;
                ds.Tables[0].Columns.Add(dc);
                AccessToken.Add(ds.Tables[0].Rows[0][0].ToString(), token);

                IDictionary<string, string> dict = new Dictionary<string, string>();
                dict.Add("last_login_date", DateTime.Now.ToString());
                dict.Add("last_login_ip", login_ip);
                fdict = new Dictionary<string, string>();
                fdict.Add("id", ds.Tables[0].Rows[0]["id"].ToString());
                DBHelper.Update("sys_user", dict, fdict, "and");
            }
            return ds;
        }
        public static string Register(string login_name, string login_pwd, string reference_sys_user_id, string reference_sys_user_login_name)
        {
            if (Helper.HasInvalidStr(login_name)) return "用户名包含非法字符";

            IDictionary<string, string> fdict = new Dictionary<string, string>();
            fdict.Add("login_name", login_name);
            if (DBHelper.SelectRowCount("sys_user", fdict, "or") > 0) return "用户名已存在";

            if (reference_sys_user_id.Length == 0 && reference_sys_user_login_name.Length > 0)
            {
                fdict = new Dictionary<string, string>();
                fdict.Add("login_name", reference_sys_user_login_name);
                fdict.Add("delete_flag", "IS NULL");
                DataSet ds = DBHelper.Select("sys_user", "id", string.Empty, fdict, "and");
                if (ds.Tables[0].Rows.Count == 0) return "推荐人不存在";
                reference_sys_user_id = ds.Tables[0].Rows[0]["id"].ToString();
            }
            string login_pwd_guid = Helper.GetGuid().ToLower();
            IDictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("login_name", login_name);
            dict.Add("login_pwd", MakePwd(login_name, login_pwd, login_pwd_guid));
            dict.Add("login_pwd_guid", login_pwd_guid);
            dict.Add("role_id", "2");
            dict.Add("reference_sys_user_id", reference_sys_user_id);
            dict.Add("register_date", DateTime.Now.ToString());
            DBHelper.Insert("sys_user", dict);
            return string.Empty;
        }
        public static string UpdateInfo(string id, string name, string phone, string address, string email, string qq, string weixin, string USER, string TOKEN)
        {
            if (!AccessToken.Read(USER, TOKEN)) return "登录超时";

            IDictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("name", name);
            dict.Add("phone", phone);
            dict.Add("address", address);
            dict.Add("email", email);
            dict.Add("qq", qq);
            dict.Add("weixin", weixin);
            IDictionary<string, string> fdict = new Dictionary<string, string>();
            fdict.Add("id", id);
            DBHelper.Update("sys_user", dict, fdict, "and");
            return string.Empty;
        }
        public static string Insert(string login_name, string login_pwd, string role_id, string name, string phone, string address, string email, string qq, string weixin, string reference_sys_user_login_name, string USER, string TOKEN)
        {
            if (!AccessToken.Read(USER, TOKEN)) return "登录超时";

            if (Helper.HasInvalidStr(login_name)) return "用户名包含非法字符";

            IDictionary<string, string> fdict = new Dictionary<string, string>();
            fdict.Add("login_name", login_name);
            if (DBHelper.SelectRowCount("sys_user", fdict, "or") > 0) return "用户名已存在";

            string reference_sys_user_id = string.Empty;
            if (reference_sys_user_login_name.Length > 0)
            {
                fdict = new Dictionary<string, string>();
                fdict.Add("login_name", reference_sys_user_login_name);
                fdict.Add("delete_flag", "IS NULL");
                DataSet ds = DBHelper.Select("sys_user", "id", string.Empty, fdict, "and");
                if (ds.Tables[0].Rows.Count == 0) return "推荐人不存在";
                reference_sys_user_id = ds.Tables[0].Rows[0]["id"].ToString();
            }
            string login_pwd_guid = Helper.GetGuid().ToLower();
            IDictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("login_name", login_name);
            dict.Add("login_pwd", MakePwd(login_name, login_pwd, login_pwd_guid));
            dict.Add("login_pwd_guid", login_pwd_guid);
            dict.Add("role_id", role_id);
            dict.Add("name", name);
            dict.Add("phone", phone);
            dict.Add("address", address);
            dict.Add("email", email);
            dict.Add("qq", qq);
            dict.Add("weixin", weixin);
            dict.Add("reference_sys_user_id", reference_sys_user_id);
            dict.Add("register_date", DateTime.Now.ToString());
            DBHelper.Insert("sys_user", dict);
            return string.Empty;
        }
        public static string Update(string id, string role_id, string name, string phone, string address, string email, string qq, string weixin, string reference_sys_user_login_name, string delete_flag, string USER, string TOKEN)
        {
            if (!AccessToken.Read(USER, TOKEN)) return "登录超时";

            IDictionary<string, string> fdict = null;
            string reference_sys_user_id = string.Empty;
            if (reference_sys_user_login_name.Length > 0)
            {
                fdict = new Dictionary<string, string>();
                fdict.Add("login_name", reference_sys_user_login_name);
                fdict.Add("delete_flag", "IS NULL");
                DataSet ds = DBHelper.Select("sys_user", "id", string.Empty, fdict, "and");
                if (ds.Tables[0].Rows.Count == 0) return "推荐人不存在";
                reference_sys_user_id = ds.Tables[0].Rows[0]["id"].ToString();
            }
            IDictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("role_id", role_id);
            dict.Add("name", name);
            dict.Add("phone", phone);
            dict.Add("address", address);
            dict.Add("email", email);
            dict.Add("qq", qq);
            dict.Add("weixin", weixin);
            dict.Add("reference_sys_user_id", reference_sys_user_id);
            dict.Add("delete_flag", delete_flag);
            fdict = new Dictionary<string, string>();
            fdict.Add("id", id);
            DBHelper.Update("sys_user", dict, fdict, "and");
            return string.Empty;
        }
        public static string UpdatePwd(string id, string login_pwd_old, string login_pwd, string USER, string TOKEN)
        {
            if (!AccessToken.Read(USER, TOKEN)) return "登录超时";

            IDictionary<string, string> fdict = new Dictionary<string, string>();
            fdict.Add("id", id);
            DataSet ds = DBHelper.Select("sys_user", "*", string.Empty, fdict, "and");
            if (ds.Tables[0].Rows.Count == 0) return "找不到该用户";
            string login_name = ds.Tables[0].Rows[0]["login_name"].ToString();
            string login_pwd_guid = ds.Tables[0].Rows[0]["login_pwd_guid"].ToString();

            if (!login_pwd_old.Equals("aD0In"))
            {
                if (!MakePwd(login_name, login_pwd_old, login_pwd_guid).Equals(ds.Tables[0].Rows[0]["login_pwd"].ToString())) return "原密码错误";
            }
            IDictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("login_pwd", MakePwd(login_name, login_pwd, login_pwd_guid));
            fdict = new Dictionary<string, string>();
            fdict.Add("id", id);
            DBHelper.Update("sys_user", dict, fdict, "and");
            return string.Empty;
        }
        public static string Delete(string id, string USER, string TOKEN)
        {
            if (!AccessToken.Read(USER, TOKEN)) return "登录超时";

            IDictionary<string, string> fdict = new Dictionary<string, string>();
            fdict.Add("id", id);
            DBHelper.Delete("sys_user", fdict, "and");
            return string.Empty;
        }
        public static DataSet GetAll(string login_name, string role_id, string reference_sys_user_id, string delete_flag, string register_dates, string register_datee, string pageSize, string pageIndex, string USER, string TOKEN)
        {
            if (!AccessToken.Read(USER, TOKEN)) return sys_dict.GetEmptyDs();

            IDictionary<string, string> fdict = new Dictionary<string, string>();
            if (login_name.Length > 0) fdict.Add("login_name", login_name);
            if (role_id.Length > 0) fdict.Add("role_id", role_id);
            if (reference_sys_user_id.Length > 0) fdict.Add("reference_sys_user_id", reference_sys_user_id);
            if (delete_flag.Length > 0) fdict.Add("delete_flag", delete_flag);
            if (register_dates.Length > 0) fdict.Add("register_date", ">=" + register_dates);
            if (register_datee.Length > 0) fdict.Add("重复字段A重复字段register_date", "<=" + register_datee + " 23:59:59");
            return DBHelper.SelectPager("sys_user_view", "*", string.Empty, fdict, "and", pageSize, pageIndex);
        }
        public static DataSet GetOne(string id, string USER, string TOKEN)
        {
            if (!AccessToken.Read(USER, TOKEN)) return sys_dict.GetEmptyDs();

            IDictionary<string, string> fdict = new Dictionary<string, string>();
            fdict.Add("id", id);
            return DBHelper.Select("sys_user", "*", string.Empty, fdict, "and");
        }
    }
}
