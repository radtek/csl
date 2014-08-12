using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Util;
using System.Data;

namespace DBOper
{
    public class football_round_chain
    {
        public static string Insert(string football_round_id, string name, string participant_amount, string gift_id, string price, string vote_deadline, string odr, string USER, string TOKEN)
        {
            if (!AccessToken.Read(USER, TOKEN)) return "登录超时";

            IDictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("football_round_id", football_round_id);
            dict.Add("name", name);
            dict.Add("participant_amount", participant_amount);
            dict.Add("gift_id", gift_id);
            dict.Add("price", price);
            dict.Add("vote_deadline", vote_deadline);
            dict.Add("odr", odr);
            DBHelper.Insert("football_round_chain", dict);
            return string.Empty;
        }
        public static string Update(string id, string football_round_id, string name, string participant_amount, string gift_id, string price, string vote_deadline, string odr, string delete_flag, string USER, string TOKEN)
        {
            if (!AccessToken.Read(USER, TOKEN)) return "登录超时";

            IDictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("football_round_id", football_round_id);
            dict.Add("name", name);
            dict.Add("participant_amount", participant_amount);
            dict.Add("gift_id", gift_id);
            dict.Add("price", price);
            dict.Add("vote_deadline", vote_deadline);
            dict.Add("odr", odr);
            dict.Add("delete_flag", delete_flag);
            IDictionary<string, string> fdict = new Dictionary<string, string>();
            fdict.Add("id", id);
            DBHelper.Update("football_round_chain", dict, fdict, "and");
            return string.Empty;
        }
        public static string Delete(string id, string USER, string TOKEN)
        {
            if (!AccessToken.Read(USER, TOKEN)) return "登录超时";

            IDictionary<string, string> fdict = new Dictionary<string, string>();
            fdict.Add("id", id);
            DBHelper.Delete("football_round_chain", fdict, "and");
            return string.Empty;
        }
        public static DataSet GetAll(string football_round_id, string pageSize, string pageIndex, string USER, string TOKEN)
        {
            if (!AccessToken.Read(USER, TOKEN)) return sys_dict.GetEmptyDs();

            IDictionary<string, string> fdict = new Dictionary<string, string>();
            if (football_round_id.Length > 0) fdict.Add("football_round_id", football_round_id);
            if (pageSize.Length == 0) return DBHelper.Select("football_round", "*", "odr,id", fdict, "and");
            return DBHelper.SelectPager("football_round_chain", "*", "odr,id", fdict, "and", pageSize, pageIndex);
        }
        public static DataSet GetOne(string id, string USER, string TOKEN)
        {
            if (!AccessToken.Read(USER, TOKEN)) return sys_dict.GetEmptyDs();

            IDictionary<string, string> fdict = new Dictionary<string, string>();
            fdict.Add("id", id);
            return DBHelper.Select("football_round_chain", "*", string.Empty, fdict, "and");
        }
        public static DataSet GetRandomMatch(string football_tournament_id, string year, string today, string amount, string USER, string TOKEN)
        {
            if (!AccessToken.Read(USER, TOKEN)) return sys_dict.GetEmptyDs();

            IDictionary<string, string> fdict = new Dictionary<string, string>();
            fdict.Add("football_tournament_id", football_tournament_id);
            fdict.Add("year", year);
            fdict.Add("start_datetime", "<=" + today);
            fdict.Add("end_datetime", ">=" + today + " 23:59:59");
            DataSet ds = DBHelper.Select("football_round", "id", string.Empty, fdict, "and");
            if (ds.Tables[0].Rows.Count != 1) return sys_dict.GetEmptyDs();
            string football_round_id = ds.Tables[0].Rows[0][0].ToString();

            fdict = new Dictionary<string, string>();
            fdict.Add("football_round_id", football_round_id);
            fdict.Add("vote_deadline", ">'" + today + "'");
            ds = DBHelper.Select("football_match_view", "*", string.Empty, fdict, "and");

            if (ds.Tables[0].Rows.Count < Convert.ToInt32(amount)) return sys_dict.GetEmptyDs();
            for (int i = 0; i < ds.Tables[0].Rows.Count - Convert.ToInt32(amount); i++)
            {
                Random rd = new Random(DateTime.Now.Minute * DateTime.Now.Second * DateTime.Now.Millisecond);
                ds.Tables[0].Rows.RemoveAt(rd.Next(0, ds.Tables[0].Rows.Count - 1));
            }

            return ds;
        }
    }
}
