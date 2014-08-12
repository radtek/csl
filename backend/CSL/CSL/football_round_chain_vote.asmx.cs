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
    /// football_round_chain_vote 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    // [System.Web.Script.Services.ScriptService]
    public class football_round_chain_vote : System.Web.Services.WebService
    {
        [WebMethod]
        public void Insert(string football_round_chain_id, string sys_user_id, string football_match_id, string match_result_id, string football_match_ids, string USER, string TOKEN)
        {
            string msg = DBOper.football_round_chain_vote.Insert(football_round_chain_id, sys_user_id, football_match_id, match_result_id, football_match_ids, USER, TOKEN);
            if (msg.Length == 0) Helper.WebServiceResponse(string.Empty);
            else Helper.WebServiceResponse(Helper.GetErrJson(msg));
        }
        [WebMethod]
        public void Delete(string id, string USER, string TOKEN)
        {
            string msg = DBOper.football_round_chain_vote.Delete(id, USER, TOKEN);
            if (msg.Length == 0) Helper.WebServiceResponse(string.Empty);
            else Helper.WebServiceResponse(Helper.GetErrJson(msg));
        }
        [WebMethod]
        public void GetAll(string football_round_chain_id, string sys_user_id, string chain_vote_status_id, string pageSize, string pageIndex, string USER, string TOKEN)
        {
            DataSet ds = DBOper.football_round_chain_vote.GetAll(football_round_chain_id, sys_user_id, chain_vote_status_id, pageSize, pageIndex, USER, TOKEN);
            Helper.WebServiceResponse(JsonHelper.GetJsonBase64(ds.Tables[0]));
        }
        [WebMethod]
        public void GetOne(string id, string USER, string TOKEN)
        {
            DataSet ds = DBOper.football_round_chain_vote.GetOne(id, USER, TOKEN);
            Helper.WebServiceResponse(JsonHelper.GetJsonBase64(ds.Tables[0]));
        }
        [WebMethod]
        public void End(string id, string USER, string TOKEN)
        {
            string msg = DBOper.football_round_chain_vote.End(id, USER, TOKEN);
            if (msg.Length == 0) Helper.WebServiceResponse(string.Empty);
            else Helper.WebServiceResponse(Helper.GetErrJson(msg));
        }
    }
}
