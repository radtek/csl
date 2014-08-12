using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Util;
using System.Data;
using MySql.Data.MySqlClient;

namespace DBOper
{
    public class gift_history
    {
        public static string Insert(string gift_id, string sys_user_id, string USER, string TOKEN)
        {
            if (!AccessToken.Read(USER, TOKEN)) return "登录超时";

            IDictionary<string, string> fdict = new Dictionary<string, string>();
            fdict.Add("id", gift_id);
            fdict.Add("delete_flag", "IS NULL");
            DataSet ds = DBHelper.Select("gift", "title,price,inventory,off_shelf_datetime", string.Empty, fdict, "and");
            if (ds.Tables[0].Rows.Count == 0) return "礼品不存在";
            string title = ds.Tables[0].Rows[0]["title"].ToString();
            string price = ds.Tables[0].Rows[0]["price"].ToString();
            string inventory = ds.Tables[0].Rows[0]["inventory"].ToString();
            string off_shelf_datetime = ds.Tables[0].Rows[0]["off_shelf_datetime"].ToString();
            if (inventory.Length > 0 && (Convert.ToInt32(inventory) < 1)) return "礼品库存不足";
            if (off_shelf_datetime.Length > 0 && (Convert.ToDateTime(off_shelf_datetime) < DateTime.Today)) return "礼品已下架";

            fdict = new Dictionary<string, string>();
            fdict.Add("id", sys_user_id);
            fdict.Add("delete_flag", "IS NULL");
            ds = DBHelper.Select("sys_user", "balance", string.Empty, fdict, "and");
            if (ds.Tables[0].Rows.Count == 0) return "用户不存在";
            if (Convert.ToInt32(ds.Tables[0].Rows[0]["balance"]) < Convert.ToInt32(price)) return "爱心不足";

            using (MySqlConnection dbConnection = new MySqlConnection(DBHelper.strConnection))
            {
                dbConnection.Open();
                using (MySqlTransaction trans = dbConnection.BeginTransaction())
                {
                    IDictionary<string, string> dict = new Dictionary<string, string>();
                    dict.Add("gift_id", gift_id);
                    dict.Add("sys_user_id", sys_user_id);
                    dict.Add("buy_datetime", DateTime.Now.ToString());
                    DBHelper.Insert("gift_history", dict, dbConnection, trans);

                    dict = new Dictionary<string, string>();
                    dict.Add("balance", "数字相减-" + price);
                    fdict = new Dictionary<string, string>();
                    fdict.Add("id", sys_user_id);
                    DBHelper.Update("sys_user", dict, fdict, "and", dbConnection, trans);

                    sys_user_balance_change.Insert(sys_user_id, "4", "-" + price, "兑换“" + title + "”", "gift", gift_id, dbConnection, trans);

                    if (inventory.Length > 0)
                    {
                        dict = new Dictionary<string, string>();
                        dict.Add("inventory", "数字相减-1");
                        fdict = new Dictionary<string, string>();
                        fdict.Add("id", gift_id);
                        DBHelper.Update("gift", dict, fdict, "and", dbConnection, trans);
                    }

                    trans.Commit();
                }
            }
            return string.Empty;
        }
        public static DataSet GetAll(string gift_id, string sys_user_id, string pageSize, string pageIndex, string USER, string TOKEN)
        {
            if (!AccessToken.Read(USER, TOKEN)) return sys_dict.GetEmptyDs();

            IDictionary<string, string> fdict = new Dictionary<string, string>();
            if (gift_id.Length > 0) fdict.Add("gift_id", gift_id);
            if (sys_user_id.Length > 0) fdict.Add("sys_user_id", sys_user_id);
            return DBHelper.SelectPager("gift_history_view", "*", string.Empty, fdict, "and", pageSize, pageIndex);
        }
        public static DataSet GetOne(string id, string USER, string TOKEN)
        {
            if (!AccessToken.Read(USER, TOKEN)) return sys_dict.GetEmptyDs();

            IDictionary<string, string> fdict = new Dictionary<string, string>();
            fdict.Add("id", id);
            return DBHelper.Select("gift_history", "*", string.Empty, fdict, "and");
        }
    }
}
