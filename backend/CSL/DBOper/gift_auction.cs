using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Util;
using System.Data;
using MySql.Data.MySqlClient;

namespace DBOper
{
    public class gift_auction
    {
        public static string Insert(string gift_id, string title, string description, string min_bid_amount, string start_datetime, string end_datetime, string USER, string TOKEN)
        {
            if (!AccessToken.Read(USER, TOKEN)) return "登录超时";

            IDictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("gift_id", gift_id);
            dict.Add("title", title);
            dict.Add("description", description);
            dict.Add("min_bid_amount", min_bid_amount);
            dict.Add("start_datetime", start_datetime);
            dict.Add("end_datetime", end_datetime);
            DBHelper.Insert("gift_auction", dict);
            return string.Empty;
        }
        public static string Update(string id, string title, string description, string min_bid_amount, string start_datetime, string end_datetime, string delete_flag, string USER, string TOKEN)
        {
            if (!AccessToken.Read(USER, TOKEN)) return "登录超时";

            IDictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("title", title);
            dict.Add("description", description);
            dict.Add("min_bid_amount", min_bid_amount);
            dict.Add("start_datetime", start_datetime);
            dict.Add("end_datetime", end_datetime);
            dict.Add("delete_flag", delete_flag);
            IDictionary<string, string> fdict = new Dictionary<string, string>();
            fdict.Add("id", id);
            DBHelper.Update("gift_auction", dict, fdict, "and");
            return string.Empty;
        }
        public static string Delete(string id, string USER, string TOKEN)
        {
            if (!AccessToken.Read(USER, TOKEN)) return "登录超时";

            IDictionary<string, string> fdict = new Dictionary<string, string>();
            fdict.Add("id", id);
            DBHelper.Delete("gift_auction", fdict, "and");
            return string.Empty;
        }
        public static DataSet GetAll(string gift_id, string title, string pageSize, string pageIndex, string USER, string TOKEN)
        {
            if (!AccessToken.Read(USER, TOKEN)) return sys_dict.GetEmptyDs();

            IDictionary<string, string> fdict = new Dictionary<string, string>();
            if (gift_id.Length > 0) fdict.Add("gift_id", gift_id);
            if (title.Length > 0) fdict.Add("title", "%" + title + "%");
            return DBHelper.SelectPager("gift_auction_view", "*", string.Empty, fdict, "and", pageSize, pageIndex);
        }
        public static DataSet GetOne(string id, string USER, string TOKEN)
        {
            if (!AccessToken.Read(USER, TOKEN)) return sys_dict.GetEmptyDs();

            IDictionary<string, string> fdict = new Dictionary<string, string>();
            fdict.Add("id", id);
            return DBHelper.Select("gift_auction_view", "*", string.Empty, fdict, "and");
        }
        public static string End(string id, string USER, string TOKEN)
        {
            if (!AccessToken.Read(USER, TOKEN)) return "登录超时";

            IDictionary<string, string> fdict = new Dictionary<string, string>();
            fdict.Add("gift_auction_id", id);
            fdict.Add("gift_bid_status_id", "1");
            DataSet ds = DBHelper.Select("gift_auction_sys_user_bid_view", "id,sys_user_id,amount,auction_title", "amount desc", fdict, "and");

            using (MySqlConnection dbConnection = new MySqlConnection(DBHelper.strConnection))
            {
                dbConnection.Open();
                using (MySqlTransaction trans = dbConnection.BeginTransaction())
                {
                    IDictionary<string, string> dict = new Dictionary<string, string>();
                    dict.Add("end_datetime", DateTime.Now.ToString());
                    fdict = new Dictionary<string, string>();
                    fdict.Add("id", id);
                    DBHelper.Update("gift_auction", dict, fdict, "and", dbConnection, trans);

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        if (i == 0)
                        {
                            dict = new Dictionary<string, string>();
                            dict.Add("gift_bid_status_id", "2");
                            fdict = new Dictionary<string, string>();
                            fdict.Add("id", ds.Tables[0].Rows[i]["id"].ToString());
                            DBHelper.Update("gift_auction_sys_user_bid", dict, fdict, "and", dbConnection, trans);

                            sys_user_balance_change.Insert(ds.Tables[0].Rows[i]["sys_user_id"].ToString(), "6", "-" + ds.Tables[0].Rows[i]["amount"], "竞拍“" + ds.Tables[0].Rows[i]["auction_title"] + "”", "gift_auction_sys_user_bid", ds.Tables[0].Rows[i]["id"].ToString(), dbConnection, trans);
                        }
                        else
                        {
                            dict = new Dictionary<string, string>();
                            dict.Add("gift_bid_status_id", "4");
                            fdict = new Dictionary<string, string>();
                            fdict.Add("id", ds.Tables[0].Rows[i]["id"].ToString());
                            DBHelper.Update("gift_auction_sys_user_bid", dict, fdict, "and", dbConnection, trans);

                            dict = new Dictionary<string, string>();
                            dict.Add("balance", "数字相加+" + ds.Tables[0].Rows[i]["amount"]);
                            fdict = new Dictionary<string, string>();
                            fdict.Add("id", ds.Tables[0].Rows[i]["sys_user_id"].ToString());
                            DBHelper.Update("sys_user", dict, fdict, "and", dbConnection, trans);
                        }
                    }

                    trans.Commit();
                }
            }
            return string.Empty;
        }
    }
}
