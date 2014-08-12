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
    /// football_round_chain 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    // [System.Web.Script.Services.ScriptService]
    public class football_round_chain : System.Web.Services.WebService
    {
        [WebMethod]
        public void Insert(string football_round_id, string name, string participant_amount, string gift_id, string price, string vote_deadline, string odr, string USER, string TOKEN)
        {
            string msg = DBOper.football_round_chain.Insert(football_round_id, name, participant_amount, gift_id, price, vote_deadline, odr, USER, TOKEN);
            if (msg.Length == 0) Helper.WebServiceResponse(string.Empty);
            else Helper.WebServiceResponse(Helper.GetErrJson(msg));
        }
        [WebMethod]
        public void Update(string id, string football_round_id, string name, string participant_amount, string gift_id, string price, string vote_deadline, string odr, string delete_flag, string USER, string TOKEN)
        {
            string msg = DBOper.football_round_chain.Update(id, football_round_id, name, participant_amount, gift_id, price, vote_deadline, odr, delete_flag, USER, TOKEN);
            if (msg.Length == 0) Helper.WebServiceResponse(string.Empty);
            else Helper.WebServiceResponse(Helper.GetErrJson(msg));
        }
        [WebMethod]
        public void Delete(string id, string USER, string TOKEN)
        {
            string msg = DBOper.football_round_chain.Delete(id, USER, TOKEN);
            if (msg.Length == 0) Helper.WebServiceResponse(string.Empty);
            else Helper.WebServiceResponse(Helper.GetErrJson(msg));
        }
        [WebMethod]
        public void GetAll(string football_round_id, string pageSize, string pageIndex, string USER, string TOKEN)
        {
            DataSet ds = DBOper.football_round_chain.GetAll(football_round_id, pageSize, pageIndex, USER, TOKEN);
            Helper.WebServiceResponse(JsonHelper.GetJsonBase64(ds.Tables[0]));
        }
        [WebMethod]
        public void GetOne(string id, string USER, string TOKEN)
        {
            DataSet ds = DBOper.football_round_chain.GetOne(id, USER, TOKEN);
            Helper.WebServiceResponse(JsonHelper.GetJsonBase64(ds.Tables[0]));
        }
        [WebMethod]
        public void GetRandomMatch(string football_tournament_id, string year, string today, string amount, string USER, string TOKEN)
        {
            DataSet ds = DBOper.football_round_chain.GetRandomMatch(football_tournament_id, year, today, amount, USER, TOKEN);
            Helper.WebServiceResponse(JsonHelper.GetJsonBase64(ds.Tables[0]));
        }
    }
}
