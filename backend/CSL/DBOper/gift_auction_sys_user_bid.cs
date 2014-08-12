using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Util;
using System.Data;
using MySql.Data.MySqlClient;

namespace DBOper
{
    public class gift_auction_sys_user_bid
    {
        public static string Insert(string gift_auction_id, string sys_user_id, string amount, string USER, string TOKEN)
        {
            if (!AccessToken.Read(USER, TOKEN)) return "登录超时";

            if (amount.Length == 0 || Convert.ToInt32(amount) <= 0) return "竞拍额异常";

            IDictionary<string, string> fdict = new Dictionary<string, string>();
            fdict.Add("id", gift_auction_id);
            fdict.Add("end_datetime", ">='" + DateTime.Now.ToString() + "'");
            fdict.Add("delete_flag", "IS NULL");
            DataSet ds = DBHelper.Select("gift_auction", "min_bid_amount", string.Empty, fdict, "and");
            if (ds.Tables[0].Rows.Count == 0) return "该竞拍已结束";
            string min_bid_amount = ds.Tables[0].Rows[0]["min_bid_amount"].ToString();
            if (Convert.ToInt32(amount) < Convert.ToInt32(min_bid_amount)) return "不能低于最小竞拍额";

            fdict = new Dictionary<string, string>();
            fdict.Add("id", sys_user_id);
            fdict.Add("delete_flag", "IS NULL");
            ds = DBHelper.Select("sys_user", "balance", string.Empty, fdict, "and");
            if (ds.Tables[0].Rows.Count == 0) return "用户不存在";
            if (Convert.ToInt32(ds.Tables[0].Rows[0]["balance"]) < Convert.ToInt32(amount)) return "爱心不足";

            fdict = new Dictionary<string, string>();
            fdict.Add("gift_auction_id", gift_auction_id);
            fdict.Add("sys_user_id", sys_user_id);
            ds = DBHelper.Select("gift_auction_sys_user_bid", "id,amount", string.Empty, fdict, "and");

            using (MySqlConnection dbConnection = new MySqlConnection(DBHelper.strConnection))
            {
                dbConnection.Open();
                using (MySqlTransaction trans = dbConnection.BeginTransaction())
                {
                    IDictionary<string, string> dict = new Dictionary<string, string>();
                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        dict.Add("gift_auction_id", gift_auction_id);
                        dict.Add("sys_user_id", sys_user_id);
                        dict.Add("amount", amount);
                        dict.Add("gift_bid_status_id", "1");
                        dict.Add("bid_datetime", DateTime.Now.ToString());
                        DBHelper.Insert("gift_auction_sys_user_bid", dict, dbConnection, trans);
                    }
                    else
                    {
                        if (amount.Equals(ds.Tables[0].Rows[0]["amount"].ToString())) return string.Empty;

                        dict.Add("amount", amount);
                        dict.Add("bid_datetime", DateTime.Now.ToString());
                        fdict = new Dictionary<string, string>();
                        fdict.Add("id", ds.Tables[0].Rows[0]["id"].ToString());
                        DBHelper.Update("gift_auction_sys_user_bid", dict, fdict, "and", dbConnection, trans);
                        amount = (Convert.ToInt32(amount) - Convert.ToInt32(ds.Tables[0].Rows[0]["amount"])).ToString();
                    }
                    dict = new Dictionary<string, string>();
                    dict.Add("balance", "数字相减-" + amount);
                    fdict = new Dictionary<string, string>();
                    fdict.Add("id", sys_user_id);
                    DBHelper.Update("sys_user", dict, fdict, "and", dbConnection, trans);

                    trans.Commit();
                }
            }
            return string.Empty;
        }
        public static DataSet GetAll(string gift_auction_id, string sys_user_id, string gift_bid_status_id, string pageSize, string pageIndex, string USER, string TOKEN)
        {
            if (!AccessToken.Read(USER, TOKEN)) return sys_dict.GetEmptyDs();

            IDictionary<string, string> fdict = new Dictionary<string, string>();
            if (gift_auction_id.Length > 0) fdict.Add("gift_auction_id", gift_auction_id);
            if (sys_user_id.Length > 0) fdict.Add("sys_user_id", sys_user_id);
            if (gift_bid_status_id.Length > 0) fdict.Add("gift_bid_status_id", gift_bid_status_id);
            return DBHelper.SelectPager("gift_auction_sys_user_bid_view", "*", string.Empty, fdict, "and", pageSize, pageIndex);
        }
        public static DataSet GetOne(string id, string USER, string TOKEN)
        {
            if (!AccessToken.Read(USER, TOKEN)) return sys_dict.GetEmptyDs();

            IDictionary<string, string> fdict = new Dictionary<string, string>();
            fdict.Add("id", id);
            return DBHelper.Select("gift_auction_sys_user_bid_view", "*", string.Empty, fdict, "and");
        }
    }
}
