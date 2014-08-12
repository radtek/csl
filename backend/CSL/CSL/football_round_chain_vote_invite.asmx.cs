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
    /// football_round_chain_vote_invite 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    // [System.Web.Script.Services.ScriptService]
    public class football_round_chain_vote_invite : System.Web.Services.WebService
    {
        [WebMethod]
        public void Update(string id, string sys_user_id, string match_result_id, string USER, string TOKEN)
        {
            string msg = DBOper.football_round_chain_vote_invite.Update(id, sys_user_id, match_result_id, USER, TOKEN);
            if (msg.Length == 0) Helper.WebServiceResponse(string.Empty);
            else Helper.WebServiceResponse(Helper.GetErrJson(msg));
        }
        [WebMethod]
        public void GetAll(string football_round_chain_vote_id, string sys_user_id, string pageSize, string pageIndex, string USER, string TOKEN)
        {
            DataSet ds = DBOper.football_round_chain_vote_invite.GetAll(football_round_chain_vote_id, sys_user_id, pageSize, pageIndex, USER, TOKEN);
            Helper.WebServiceResponse(JsonHelper.GetJsonBase64(ds.Tables[0]));
        }
        [WebMethod]
        public void GetOne(string id, string USER, string TOKEN)
        {
            DataSet ds = DBOper.football_round_chain_vote_invite.GetOne(id, USER, TOKEN);
            Helper.WebServiceResponse(JsonHelper.GetJsonBase64(ds.Tables[0]));
        }
    }
}
