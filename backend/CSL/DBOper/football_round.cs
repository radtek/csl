using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Util;
using System.Data;

namespace DBOper
{
    public class football_round
    {
        public static string Insert(string football_tournament_id, string year, string name, string start_datetime, string end_datetime, string USER, string TOKEN)
        {
            if (!AccessToken.Read(USER, TOKEN)) return "登录超时";

            IDictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("football_tournament_id", football_tournament_id);
            dict.Add("year", year);
            dict.Add("name", name);
            dict.Add("start_datetime", start_datetime);
            dict.Add("end_datetime", end_datetime);
            DBHelper.Insert("football_round", dict);
            return string.Empty;
        }
        public static string Update(string id, string football_tournament_id, string year, string name, string start_datetime, string end_datetime, string USER, string TOKEN)
        {
            if (!AccessToken.Read(USER, TOKEN)) return "登录超时";

            IDictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("football_tournament_id", football_tournament_id);
            dict.Add("year", year);
            dict.Add("name", name);
            dict.Add("start_datetime", start_datetime);
            dict.Add("end_datetime", end_datetime);
            IDictionary<string, string> fdict = new Dictionary<string, string>();
            fdict.Add("id", id);
            DBHelper.Update("football_round", dict, fdict, "and");
            return string.Empty;
        }
        public static string Delete(string id, string USER, string TOKEN)
        {
            if (!AccessToken.Read(USER, TOKEN)) return "登录超时";

            IDictionary<string, string> fdict = new Dictionary<string, string>();
            fdict.Add("id", id);
            DBHelper.Delete("football_round", fdict, "and");
            return string.Empty;
        }
        public static DataSet GetAll(string football_tournament_id, string year, string pageSize, string pageIndex, string USER, string TOKEN)
        {
            if (!AccessToken.Read(USER, TOKEN)) return sys_dict.GetEmptyDs();

            IDictionary<string, string> fdict = new Dictionary<string, string>();
            if (football_tournament_id.Length > 0) fdict.Add("football_tournament_id", football_tournament_id);
            if (year.Length > 0) fdict.Add("year", year);
            if (pageSize.Length == 0) return DBHelper.Select("football_round", "*", string.Empty, fdict, "and");
            return DBHelper.SelectPager("football_round", "*", string.Empty, fdict, "and", pageSize, pageIndex);
        }
        public static DataSet GetOne(string id, string USER, string TOKEN)
        {
            if (!AccessToken.Read(USER, TOKEN)) return sys_dict.GetEmptyDs();

            IDictionary<string, string> fdict = new Dictionary<string, string>();
            fdict.Add("id", id);
            return DBHelper.Select("football_round", "*", string.Empty, fdict, "and");
        }
    }
}
