using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Util;
using System.Data;
using MySql.Data.MySqlClient;

namespace DBOper
{
    public class check_in
    {
        const string prize = "1";
        public static string Insert(string sys_user_id, string USER, string TOKEN)
        {
            if (!AccessToken.Read(USER, TOKEN)) return "登录超时";

            IDictionary<string, string> fdict = new Dictionary<string, string>();
            fdict.Add("sys_user_id", sys_user_id);
            fdict.Add("check_in_date", DateTime.Today.ToString("yyyy-MM-dd"));
            if (DBHelper.SelectRowCount("check_in", fdict, "and") > 0) return "请不要重复签到";

            using (MySqlConnection dbConnection = new MySqlConnection(DBHelper.strConnection))
            {
                dbConnection.Open();
                using (MySqlTransaction trans = dbConnection.BeginTransaction())
                {
                    IDictionary<string, string> dict = new Dictionary<string, string>();
                    dict.Add("sys_user_id", sys_user_id);
                    dict.Add("check_in_date", DateTime.Today.ToString("yyyy-MM-dd"));
                    DBHelper.Insert("check_in", dict, dbConnection, trans);

                    dict = new Dictionary<string, string>();
                    dict.Add("balance", "数字相加+1");
                    fdict = new Dictionary<string, string>();
                    fdict.Add("id", sys_user_id);
                    DBHelper.Update("sys_user", dict, fdict, "and", dbConnection, trans);

                    sys_user_balance_change.Insert(sys_user_id, "5", "1", "签到赢1爱心", string.Empty, string.Empty, dbConnection, trans);

                    trans.Commit();
                }
            }
            return string.Empty;
        }
        public static DataSet GetAll(string sys_user_id, string check_in_dates, string check_in_datee, string pageSize, string pageIndex, string USER, string TOKEN)
        {
            if (!AccessToken.Read(USER, TOKEN)) return sys_dict.GetEmptyDs();

            IDictionary<string, string> fdict = new Dictionary<string, string>();
            if (sys_user_id.Length > 0) fdict.Add("sys_user_id", sys_user_id);
            if (check_in_dates.Length > 0) fdict.Add("check_in_date", ">=" + check_in_dates);
            if (check_in_datee.Length > 0) fdict.Add("重复字段A重复字段check_in_date", "<=" + check_in_datee + " 23:59:59");
            return DBHelper.SelectPager("check_in_view", "*", "id desc", fdict, "and", pageSize, pageIndex);
        }
    }
}
