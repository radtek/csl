using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Util;
using System.Data;
using MySql.Data.MySqlClient;

namespace DBOper
{
    public class football_match
    {
        const int sys_percent = 20;
        public static string Insert(string football_round_id, string start_datetime, string home_football_team_id, string away_football_team_id, string vote_deadline, string USER, string TOKEN)
        {
            if (!AccessToken.Read(USER, TOKEN)) return "登录超时";

            IDictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("football_round_id", football_round_id);
            dict.Add("start_datetime", start_datetime);
            dict.Add("home_football_team_id", home_football_team_id);
            dict.Add("away_football_team_id", away_football_team_id);
            dict.Add("vote_deadline", vote_deadline);
            DBHelper.Insert("football_match", dict);
            return string.Empty;
        }
        public static string Update(string id, string football_round_id, string start_datetime, string home_football_team_id, string away_football_team_id, string vote_deadline, string USER, string TOKEN)
        {
            if (!AccessToken.Read(USER, TOKEN)) return "登录超时";

            IDictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("football_round_id", football_round_id);
            dict.Add("start_datetime", start_datetime);
            dict.Add("home_football_team_id", home_football_team_id);
            dict.Add("away_football_team_id", away_football_team_id);
            dict.Add("vote_deadline", vote_deadline);
            IDictionary<string, string> fdict = new Dictionary<string, string>();
            fdict.Add("id", id);
            DBHelper.Update("football_match", dict, fdict, "and");
            return string.Empty;
        }
        public static string Delete(string id, string USER, string TOKEN)
        {
            if (!AccessToken.Read(USER, TOKEN)) return "登录超时";

            IDictionary<string, string> fdict = new Dictionary<string, string>();
            fdict.Add("id", id);
            DBHelper.Delete("football_match", fdict, "and");
            return string.Empty;
        }
        public static DataSet GetAll(string football_round_id, string pageSize, string pageIndex, string USER, string TOKEN)
        {
            if (!AccessToken.Read(USER, TOKEN)) return sys_dict.GetEmptyDs();

            IDictionary<string, string> fdict = new Dictionary<string, string>();
            if (football_round_id.Length > 0) fdict.Add("football_round_id", football_round_id);
            if (pageSize.Length == 0) return DBHelper.Select("football_match", "*", string.Empty, fdict, "and");
            return DBHelper.SelectPager("football_match_view", "*", string.Empty, fdict, "and", pageSize, pageIndex);
        }
        public static DataSet GetOne(string id, string USER, string TOKEN)
        {
            if (!AccessToken.Read(USER, TOKEN)) return sys_dict.GetEmptyDs();

            IDictionary<string, string> fdict = new Dictionary<string, string>();
            fdict.Add("id", id);
            return DBHelper.Select("football_match_view", "*", string.Empty, fdict, "and");
        }
        public static string End(string id, string end_datetime, string home_score, string away_score, string final_home_score, string final_away_score, string match_result_id, string USER, string TOKEN)
        {
            if (!AccessToken.Read(USER, TOKEN)) return "登录超时";

            IDictionary<string, string> fdict = new Dictionary<string, string>();
            fdict.Add("id", id);
            DataSet ds = DBHelper.Select("football_match_view", "*", string.Empty, fdict, "and");
            if (ds.Tables[0].Rows.Count == 0) return "找不到该赛事";
            if (ds.Tables[0].Rows[0]["end_datetime"].ToString().Length > 0) return "赛事已结束";
            string title = ds.Tables[0].Rows[0]["football_round_name"] + " " + ds.Tables[0].Rows[0]["home_football_team_name"] + "VS" + ds.Tables[0].Rows[0]["away_football_team_name"];

            fdict = new Dictionary<string, string>();
            fdict.Add("football_match_id", id);
            DataSet vds = DBHelper.Select("football_match_vote", "*", string.Empty, fdict, "and");

            string match_score_type_id = string.Empty;
            if ((Convert.ToInt32(final_home_score) + Convert.ToInt32(final_away_score)) % 2 > 0) match_score_type_id = "1";
            else match_score_type_id = "2";
            fdict = new Dictionary<string, string>();
            fdict.Add("football_match_id", id);
            DataSet sds = DBHelper.Select("football_match_score", "*", string.Empty, fdict, "and");

            IDictionary<string, double> pdict = new Dictionary<string, double>();
            IDictionary<string, double> pvdict = new Dictionary<string, double>();
            IDictionary<string, double> psdict = new Dictionary<string, double>();

            IDictionary<string, double> cdict = new Dictionary<string, double>();
            double total = 0;
            double vtotal = 0;
            foreach (DataRow dr in vds.Tables[0].Rows)
            {
                total += Convert.ToInt32(dr["amount"]);
                if (dr["match_result_id"].ToString().Equals(match_result_id))
                {
                    vtotal += Convert.ToInt32(dr["amount"]);
                    cdict.Add(dr["sys_user_id"].ToString(), Convert.ToInt32(dr["amount"]));
                }
            }
            foreach (string sys_user_id in cdict.Keys)
            {
                pdict.Add(sys_user_id, cdict[sys_user_id] * total / vtotal);
                pvdict.Add(sys_user_id, cdict[sys_user_id] * total / vtotal);
            }

            cdict = new Dictionary<string, double>();
            total = 0;
            vtotal = 0;
            foreach (DataRow dr in sds.Tables[0].Rows)
            {
                total += Convert.ToInt32(dr["amount"]);
                if (dr["match_score_type_id"].ToString().Equals(match_score_type_id))
                {
                    vtotal += Convert.ToInt32(dr["amount"]);
                    cdict.Add(dr["sys_user_id"].ToString(), Convert.ToInt32(dr["amount"]));
                }
            }
            foreach (string sys_user_id in cdict.Keys)
            {
                if (pdict.ContainsKey(sys_user_id)) pdict[sys_user_id] = pdict[sys_user_id] + cdict[sys_user_id] * total / vtotal;
                else pdict.Add(sys_user_id, cdict[sys_user_id] * total / vtotal);
                psdict.Add(sys_user_id, cdict[sys_user_id] * total / vtotal);
            }

            using (MySqlConnection dbConnection = new MySqlConnection(DBHelper.strConnection))
            {
                dbConnection.Open();
                using (MySqlTransaction trans = dbConnection.BeginTransaction())
                {
                    IDictionary<string, string> dict = new Dictionary<string, string>();
                    dict.Add("end_datetime", end_datetime);
                    dict.Add("home_score", home_score);
                    dict.Add("away_score", away_score);
                    dict.Add("final_home_score", final_home_score);
                    dict.Add("final_away_score", final_away_score);
                    dict.Add("match_result_id", match_result_id);
                    fdict = new Dictionary<string, string>();
                    fdict.Add("id", id);
                    DBHelper.Update("football_match", dict, fdict, "and", dbConnection, trans);

                    foreach (string sys_user_id in pdict.Keys)
                    {
                        dict = new Dictionary<string, string>();
                        dict.Add("balance", "数字相加+" + (pdict[sys_user_id] * (100.0 - sys_percent) / 100.0).ToString());
                        fdict = new Dictionary<string, string>();
                        fdict.Add("id", sys_user_id);
                        DBHelper.Update("sys_user", dict, fdict, "and", dbConnection, trans);
                    }
                    foreach (string sys_user_id in pvdict.Keys)
                    {
                        sys_user_balance_change.Insert(sys_user_id, "3", (pvdict[sys_user_id] * (100.0 - sys_percent) / 100.0).ToString(), title + " " + dict_match_result.GetName(match_result_id), "football_match", id, dbConnection, trans);
                    }
                    foreach (string sys_user_id in psdict.Keys)
                    {
                        sys_user_balance_change.Insert(sys_user_id, "7", (psdict[sys_user_id] * (100.0 - sys_percent) / 100.0).ToString(), title + " " + dict_match_score_type.GetName(match_score_type_id), "football_match", id, dbConnection, trans);
                    }

                    trans.Commit();
                }
            }
            return string.Empty;
        }
    }
}
