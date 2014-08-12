using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Util;
using MySql.Data.MySqlClient;
using System.Data;

namespace DBOper
{
    public class sys_user_balance_change
    {
        public static void Insert(string sys_user_id, string balance_change_type_id, string amount, string remark, string ref_table, string ref_id, MySqlConnection dbConnection, MySqlTransaction trans)
        {
            IDictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("sys_user_id", sys_user_id);
            dict.Add("balance_change_type_id", balance_change_type_id);
            dict.Add("amount", amount);
            dict.Add("remark", remark);
            dict.Add("ref_table", ref_table);
            dict.Add("ref_id", ref_id);
            dict.Add("balance_change_datetime", DateTime.Now.ToString());
            DBHelper.Insert("sys_user_balance_change", dict, dbConnection, trans);
        }
        public static DataSet GetAll(string sys_user_id, string balance_change_datetimes, string balance_change_datetimee, string pageSize, string pageIndex, string USER, string TOKEN)
        {
            if (!AccessToken.Read(USER, TOKEN)) return sys_dict.GetEmptyDs();

            IDictionary<string, string> fdict = new Dictionary<string, string>();
            if (sys_user_id.Length > 0) fdict.Add("sys_user_id", sys_user_id);
            if (balance_change_datetimes.Length > 0) fdict.Add("balance_change_datetime", ">=" + balance_change_datetimes);
            if (balance_change_datetimee.Length > 0) fdict.Add("重复字段A重复字段balance_change_datetime", "<=" + balance_change_datetimee + " 23:59:59");
            return DBHelper.SelectPager("sys_user_balance_change_view", "*", "id desc", fdict, "and", pageSize, pageIndex);
        }
    }
}
