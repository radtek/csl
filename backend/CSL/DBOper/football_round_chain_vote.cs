using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Util;
using System.Data;
using MySql.Data.MySqlClient;

namespace DBOper
{
    public class football_round_chain_vote
    {
        public static string Insert(string football_round_chain_id, string sys_user_id, string football_match_id, string match_result_id, string football_match_ids, string USER, string TOKEN)
        {
            if (!AccessToken.Read(USER, TOKEN)) return "登录超时";

            IDictionary<string, string> fdict = new Dictionary<string, string>();
            fdict.Add("id", football_round_chain_id);
            fdict.Add("delete_flag", "IS NULL");
            DataSet ds = DBHelper.Select("football_round_chain", "participant_amount,name", string.Empty, fdict, "and");
            if (ds.Tables[0].Rows.Count == 0) return "竞猜不存在";
            string participant_amount = ds.Tables[0].Rows[0]["participant_amount"].ToString();
            string football_round_chain_name = ds.Tables[0].Rows[0]["name"].ToString();
            string[] football_match_idarr = football_match_ids.Split(',');
            if (football_match_idarr.Length != Convert.ToInt32(participant_amount)) return "接龙长度异常";

            fdict = new Dictionary<string, string>();
            fdict.Add("id", sys_user_id);
            fdict.Add("delete_flag", "IS NULL");
            ds = DBHelper.Select("sys_user", "balance", string.Empty, fdict, "and");
            if (ds.Tables[0].Rows.Count == 0) return "用户不存在";
            if (Convert.ToInt32(ds.Tables[0].Rows[0]["balance"]) < 6) return "爱心不足6元";

            using (MySqlConnection dbConnection = new MySqlConnection(DBHelper.strConnection))
            {
                dbConnection.Open();
                using (MySqlTransaction trans = dbConnection.BeginTransaction())
                {
                    IDictionary<string, string> dict = new Dictionary<string, string>();
                    dict.Add("football_round_chain_id", football_round_chain_id);
                    dict.Add("sys_user_id", sys_user_id);
                    dict.Add("participant_need", (Convert.ToInt32(participant_amount) - 1).ToString());
                    dict.Add("chain_vote_status_id", "1");
                    dict.Add("vote_datetime", DateTime.Now.ToString());
                    DBHelper.Insert("football_round_chain_vote", dict, dbConnection, trans);

                    string football_round_chain_vote_id = DBHelper.SelectNewId(dbConnection, trans);

                    dict = new Dictionary<string, string>();
                    dict.Add("football_round_chain_vote_id", football_round_chain_vote_id);
                    dict.Add("sys_user_id", sys_user_id);
                    dict.Add("football_match_id", football_match_id);
                    dict.Add("match_result_id", match_result_id);
                    dict.Add("vote_datetime", DateTime.Now.ToString());
                    DBHelper.Insert("football_round_chain_vote_invite", dict, dbConnection, trans);

                    foreach (string football_match_idi in football_match_idarr)
                    {
                        if (football_match_idi.Equals(football_match_id)) continue;

                        dict = new Dictionary<string, string>();
                        dict.Add("football_round_chain_vote_id", football_round_chain_vote_id);
                        dict.Add("football_match_id", football_match_idi);
                        DBHelper.Insert("football_round_chain_vote_invite", dict, dbConnection, trans);
                    }

                    dict = new Dictionary<string, string>();
                    dict.Add("balance", "数字相减-6");
                    fdict = new Dictionary<string, string>();
                    fdict.Add("id", sys_user_id);
                    DBHelper.Update("sys_user", dict, fdict, "and", dbConnection, trans);

                    sys_user_balance_change.Insert(sys_user_id, "2", "-6", "发起接龙“" + football_round_chain_name + "”", "football_round_chain_vote", football_round_chain_vote_id, dbConnection, trans);


                    trans.Commit();
                }
            }
            return string.Empty;
        }
        public static string Delete(string id, string USER, string TOKEN)
        {
            if (!AccessToken.Read(USER, TOKEN)) return "登录超时";

            IDictionary<string, string> fdict = new Dictionary<string, string>();
            fdict.Add("id", id);
            DBHelper.Delete("football_round_chain_vote", fdict, "and");
            return string.Empty;
        }
        public static DataSet GetAll(string football_round_chain_id, string sys_user_id, string chain_vote_status_id, string pageSize, string pageIndex, string USER, string TOKEN)
        {
            if (!AccessToken.Read(USER, TOKEN)) return sys_dict.GetEmptyDs();

            IDictionary<string, string> fdict = new Dictionary<string, string>();
            if (football_round_chain_id.Length > 0) fdict.Add("football_round_chain_id", football_round_chain_id);
            if (sys_user_id.Length > 0) fdict.Add("sys_user_id", sys_user_id);
            if (chain_vote_status_id.Length > 0) fdict.Add("chain_vote_status_id", chain_vote_status_id);
            if (pageSize.Length == 0) return DBHelper.Select("football_round_chain_vote_view", "*", string.Empty, fdict, "and");
            return DBHelper.SelectPager("football_round_chain_vote_view", "*", string.Empty, fdict, "and", pageSize, pageIndex);
        }
        public static DataSet GetOne(string id, string USER, string TOKEN)
        {
            if (!AccessToken.Read(USER, TOKEN)) return sys_dict.GetEmptyDs();

            IDictionary<string, string> fdict = new Dictionary<string, string>();
            fdict.Add("id", id);
            return DBHelper.Select("football_round_chain_vote", "*", string.Empty, fdict, "and");
        }
        public static string End(string id, string USER, string TOKEN)
        {
            if (!AccessToken.Read(USER, TOKEN)) return "登录超时";

            IDictionary<string, string> fdict = new Dictionary<string, string>();
            fdict.Add("football_round_chain_vote_id", id);
            DataSet ds = DBHelper.Select("football_round_chain_vote_invite_view", "*", string.Empty, fdict, "and");
            if (ds.Tables[0].Rows.Count == 0) return "找不到该记录";
            string price = ds.Tables[0].Rows[0]["price"].ToString();
            string title = ds.Tables[0].Rows[0]["football_round_name"] + " " + ds.Tables[0].Rows[0]["football_round_chain_name"];

            string chain_vote_status_id = "2";
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (dr["vote_sys_user_id"].ToString().Length == 0) return "接龙未完成";
                if (dr["match_result_id"].ToString().Length == 0) return "仍有比赛未完成";
                if (!dr["match_result_id"].Equals(dr["vote_match_result_id"])) chain_vote_status_id = "3";
            }

            using (MySqlConnection dbConnection = new MySqlConnection(DBHelper.strConnection))
            {
                dbConnection.Open();
                using (MySqlTransaction trans = dbConnection.BeginTransaction())
                {
                    IDictionary<string, string> dict = new Dictionary<string, string>();
                    dict.Add("chain_vote_status_id", chain_vote_status_id);
                    fdict = new Dictionary<string, string>();
                    fdict.Add("id", id);
                    DBHelper.Update("football_round_chain_vote", dict, fdict, "and", dbConnection, trans);

                    if (chain_vote_status_id.Equals("2") && (price.Length > 0))
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            dict = new Dictionary<string, string>();
                            dict.Add("balance", "数字相加+" + price);
                            fdict = new Dictionary<string, string>();
                            fdict.Add("id", dr["vote_sys_user_id"].ToString());
                            DBHelper.Update("sys_user", dict, fdict, "and", dbConnection, trans);

                            sys_user_balance_change.Insert(dr["vote_sys_user_id"].ToString(), "2", price, title, "football_round_chain_vote", id, dbConnection, trans);
                        }
                    }

                    trans.Commit();
                }
            }
            return string.Empty;
        }
    }
}
