using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Util;
using System.Data;

namespace UUSchool
{
    /// <summary>
    /// football_match 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    // [System.Web.Script.Services.ScriptService]
    public class football_match : System.Web.Services.WebService
    {
        [WebMethod]
        public void Insert(string football_round_id, string start_datetime, string home_football_team_id, string away_football_team_id, string vote_deadline, string USER, string TOKEN)
        {
            string msg = DBOper.football_match.Insert(football_round_id, start_datetime, home_football_team_id, away_football_team_id, vote_deadline, USER, TOKEN);
            if (msg.Length == 0) Helper.WebServiceResponse(string.Empty);
            else Helper.WebServiceResponse(Helper.GetErrJson(msg));
        }
        [WebMethod]
        public void Update(string id, string football_round_id, string start_datetime, string home_football_team_id, string away_football_team_id, string vote_deadline, string USER, string TOKEN)
        {
            string msg = DBOper.football_match.Update(id, football_round_id, start_datetime, home_football_team_id, away_football_team_id, vote_deadline, USER, TOKEN);
            if (msg.Length == 0) Helper.WebServiceResponse(string.Empty);
            else Helper.WebServiceResponse(Helper.GetErrJson(msg));
        }
        [WebMethod]
        public void Delete(string id, string USER, string TOKEN)
        {
            string msg = DBOper.football_match.Delete(id, USER, TOKEN);
            if (msg.Length == 0) Helper.WebServiceResponse(string.Empty);
            else Helper.WebServiceResponse(Helper.GetErrJson(msg));
        }
        [WebMethod]
        public void GetAll(string football_round_id, string pageSize, string pageIndex, string USER, string TOKEN)
        {
            DataSet ds = DBOper.football_match.GetAll(football_round_id, pageSize, pageIndex, USER, TOKEN);
            Helper.WebServiceResponse(JsonHelper.GetJsonBase64(ds.Tables[0]));
        }
        [WebMethod]
        public void GetOne(string id, string USER, string TOKEN)
        {
            DataSet ds = DBOper.football_match.GetOne(id, USER, TOKEN);
            Helper.WebServiceResponse(JsonHelper.GetJsonBase64(ds.Tables[0]));
        }
        [WebMethod]
        public void End(string id, string end_datetime, string home_score, string away_score, string final_home_score, string final_away_score, string match_result_id, string USER, string TOKEN)
        {
            string msg = DBOper.football_match.End(id, end_datetime, home_score, away_score, final_home_score, final_away_score, match_result_id, USER, TOKEN);
            if (msg.Length == 0) Helper.WebServiceResponse(string.Empty);
            else Helper.WebServiceResponse(Helper.GetErrJson(msg));
        }
    }
}
