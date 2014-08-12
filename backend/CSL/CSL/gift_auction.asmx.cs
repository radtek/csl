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
    /// gift_auction 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    // [System.Web.Script.Services.ScriptService]
    public class gift_auction : System.Web.Services.WebService
    {
        [WebMethod]
        public void Insert(string gift_id, string title, string description, string min_bid_amount, string start_datetime, string end_datetime, string USER, string TOKEN)
        {
            string msg = DBOper.gift_auction.Insert(gift_id, title, description, min_bid_amount, start_datetime, end_datetime, USER, TOKEN);
            if (msg.Length == 0) Helper.WebServiceResponse(string.Empty);
            else Helper.WebServiceResponse(Helper.GetErrJson(msg));
        }
        [WebMethod]
        public void Update(string id, string title, string description, string min_bid_amount, string start_datetime, string end_datetime, string delete_flag, string USER, string TOKEN)
        {
            string msg = DBOper.gift_auction.Update(id, title, description, min_bid_amount, start_datetime, end_datetime, delete_flag, USER, TOKEN);
            if (msg.Length == 0) Helper.WebServiceResponse(string.Empty);
            else Helper.WebServiceResponse(Helper.GetErrJson(msg));
        }
        [WebMethod]
        public void Delete(string id, string USER, string TOKEN)
        {
            string msg = DBOper.gift_auction.Delete(id, USER, TOKEN);
            if (msg.Length == 0) Helper.WebServiceResponse(string.Empty);
            else Helper.WebServiceResponse(Helper.GetErrJson(msg));
        }
        [WebMethod]
        public void GetAll(string gift_id, string title, string pageSize, string pageIndex, string USER, string TOKEN)
        {
            DataSet ds = DBOper.gift_auction.GetAll(gift_id, title, pageSize, pageIndex, USER, TOKEN);
            Helper.WebServiceResponse(JsonHelper.GetJsonBase64(ds.Tables[0]));
        }
        [WebMethod]
        public void GetOne(string id, string USER, string TOKEN)
        {
            DataSet ds = DBOper.gift_auction.GetOne(id, USER, TOKEN);
            Helper.WebServiceResponse(JsonHelper.GetJsonBase64(ds.Tables[0]));
        }
        [WebMethod]
        public void End(string id, string USER, string TOKEN)
        {
            string msg = DBOper.gift_auction.End(id, USER, TOKEN);
            if (msg.Length == 0) Helper.WebServiceResponse(string.Empty);
            else Helper.WebServiceResponse(Helper.GetErrJson(msg));
        }
    }
}
