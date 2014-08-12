using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Util;
using System.Data;
using MySql.Data.MySqlClient;

namespace DBOper
{
    public class football_round_chain_vote_invite
    {
        public static string Update(string id, string sys_user_id, string match_result_id, string USER, string TOKEN)
        {
            if (!AccessToken.Read(USER, TOKEN)) return "登录超时";

            IDictionary<string, string> fdict = new Dictionary<string, string>();
            fdict.Add("id", id);
            DataSet ds = DBHelper.Select("football_round_chain_vote_invite", "sys_user_id,football_round_chain_vote_id", string.Empty, fdict, "and");
            if (ds.Tables[0].Rows.Count == 0) return "找不到接龙记录";
            if (ds.Tables[0].Rows[0][0].ToString().Length > 0 && (!ds.Tables[0].Rows[0][0].ToString().Equals(sys_user_id))) return "别人已接龙";
            string football_round_chain_vote_id = ds.Tables[0].Rows[0][1].ToString();

            using (MySqlConnection dbConnection = new MySqlConnection(DBHelper.strConnection))
            {
                dbConnection.Open();
                using (MySqlTransaction trans = dbConnection.BeginTransaction())
                {
                    IDictionary<string, string> dict = new Dictionary<string, string>();
                    dict.Add("sys_user_id", sys_user_id);
                    dict.Add("match_result_id", match_result_id);
                    dict.Add("vote_datetime", DateTime.Now.ToString());
                    fdict = new Dictionary<string, string>();
                    fdict.Add("id", id);
                    DBHelper.Update("football_round_chain_vote_invite", dict, fdict, "and", dbConnection, trans);

                    dict = new Dictionary<string, string>();
                    dict.Add("participant_need", "数字相减-1");
                    fdict = new Dictionary<string, string>();
                    fdict.Add("id", football_round_chain_vote_id);
                    DBHelper.Update("football_round_chain_vote", dict, fdict, "and", dbConnection, trans);

                    trans.Commit();
                }
            }
            return string.Empty;
        }
        public static DataSet GetAll(string football_round_chain_vote_id, string sys_user_id, string pageSize, string pageIndex, string USER, string TOKEN)
        {
            if (!AccessToken.Read(USER, TOKEN)) return sys_dict.GetEmptyDs();

            IDictionary<string, string> fdict = new Dictionary<string, string>();
            if (football_round_chain_vote_id.Length > 0) fdict.Add("football_round_chain_vote_id", football_round_chain_vote_id);
            if (sys_user_id.Length > 0) fdict.Add("sys_user_id", sys_user_id);
            if (pageSize.Length == 0) return DBHelper.Select("football_round_chain_vote_invite_view", "*", string.Empty, fdict, "and");
            return DBHelper.SelectPager("football_round_chain_vote_invite_view", "*", string.Empty, fdict, "and", pageSize, pageIndex);
        }
        public static DataSet GetOne(string id, string USER, string TOKEN)
        {
            if (!AccessToken.Read(USER, TOKEN)) return sys_dict.GetEmptyDs();

            IDictionary<string, string> fdict = new Dictionary<string, string>();
            fdict.Add("id", id);
            return DBHelper.Select("football_round_chain_vote_invite_view", "*", string.Empty, fdict, "and");
        }
    }
}
