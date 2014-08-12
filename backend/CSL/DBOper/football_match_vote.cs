using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Util;
using MySql.Data.MySqlClient;

namespace DBOper
{
    public class football_match_vote
    {
        public static string Insert(string football_match_id, string sys_user_id, string match_result_id, string amount, string USER, string TOKEN)
        {
            if (!AccessToken.Read(USER, TOKEN)) return "登录超时";

            if (amount.Length == 0 || Convert.ToInt32(amount) <= 0) return "竞拍额异常";

            IDictionary<string, string> fdict = new Dictionary<string, string>();
            fdict.Add("id", sys_user_id);
            fdict.Add("delete_flag", "IS NULL");
            DataSet ds = DBHelper.Select("sys_user", "balance", string.Empty, fdict, "and");
            if (ds.Tables[0].Rows.Count == 0) return "用户不存在";
            if (Convert.ToInt32(ds.Tables[0].Rows[0]["balance"]) < Convert.ToInt32(amount)) return "爱心不足";

            fdict = new Dictionary<string, string>();
            fdict.Add("id", football_match_id);
            fdict.Add("vote_deadline", ">='" + DateTime.Now.ToString() + "'");
            ds = DBHelper.Select("football_match_view", "id,football_round_name,home_football_team_name,away_football_team_name", string.Empty, fdict, "and");
            if (ds.Tables[0].Rows.Count == 0) return "竞猜已结束";
            string round_name = ds.Tables[0].Rows[0]["football_round_name"].ToString();
            string home_football_team_name = ds.Tables[0].Rows[0]["home_football_team_name"].ToString();
            string away_football_team_name = ds.Tables[0].Rows[0]["away_football_team_name"].ToString();

            fdict = new Dictionary<string, string>();
            fdict.Add("football_match_id", football_match_id);
            fdict.Add("sys_user_id", sys_user_id);
            ds = DBHelper.Select("football_match_vote", "id,match_result_id,amount", string.Empty, fdict, "and");

            using (MySqlConnection dbConnection = new MySqlConnection(DBHelper.strConnection))
            {
                dbConnection.Open();
                using (MySqlTransaction trans = dbConnection.BeginTransaction())
                {
                    IDictionary<string, string> dict = new Dictionary<string, string>();
                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        dict.Add("football_match_id", football_match_id);
                        dict.Add("sys_user_id", sys_user_id);
                        dict.Add("match_result_id", match_result_id);
                        dict.Add("amount", amount);
                        dict.Add("vote_datetime", DateTime.Now.ToString());
                        DBHelper.Insert("football_match_vote", dict, dbConnection, trans);
                    }
                    else
                    {
                        if (amount.Equals(ds.Tables[0].Rows[0]["amount"].ToString()) && match_result_id.Equals(ds.Tables[0].Rows[0]["match_result_id"].ToString())) return string.Empty;

                        dict.Add("match_result_id", match_result_id);
                        dict.Add("amount", amount);
                        dict.Add("vote_datetime", DateTime.Now.ToString());
                        fdict = new Dictionary<string, string>();
                        fdict.Add("id", ds.Tables[0].Rows[0]["id"].ToString());
                        DBHelper.Update("football_match_vote", dict, fdict, "and", dbConnection, trans);
                        amount = (Convert.ToInt32(amount) - Convert.ToInt32(ds.Tables[0].Rows[0]["amount"])).ToString();
                    }

                    dict = new Dictionary<string, string>();
                    dict.Add("balance", "数字相减-" + amount);
                    fdict = new Dictionary<string, string>();
                    fdict.Add("id", sys_user_id);
                    DBHelper.Update("sys_user", dict, fdict, "and", dbConnection, trans);

                    sys_user_balance_change.Insert(sys_user_id, "3", "-" + amount, round_name + " " + home_football_team_name + "VS" + away_football_team_name + " " + dict_match_result.GetName(match_result_id), "football_match", football_match_id, dbConnection, trans);

                    trans.Commit();
                }
            }
            return string.Empty;
        }
        public static DataSet GetAll(string football_match_id, string sys_user_id, string match_result_id, string pageSize, string pageIndex, string USER, string TOKEN)
        {
            if (!AccessToken.Read(USER, TOKEN)) return sys_dict.GetEmptyDs();

            IDictionary<string, string> fdict = new Dictionary<string, string>();
            if (football_match_id.Length > 0) fdict.Add("football_match_id", football_match_id);
            if (sys_user_id.Length > 0) fdict.Add("sys_user_id", sys_user_id);
            if (match_result_id.Length > 0) fdict.Add("match_result_id", match_result_id);
            return DBHelper.SelectPager("football_match_vote_view", "*", string.Empty, fdict, "and", pageSize, pageIndex);
        }
        public static DataSet GetOne(string id, string USER, string TOKEN)
        {
            if (!AccessToken.Read(USER, TOKEN)) return sys_dict.GetEmptyDs();

            IDictionary<string, string> fdict = new Dictionary<string, string>();
            fdict.Add("id", id);
            return DBHelper.Select("football_match_vote_view", "*", string.Empty, fdict, "and");
        }
    }
}
