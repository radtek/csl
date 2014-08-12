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
    /// gift_auction_sys_user_bid 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    // [System.Web.Script.Services.ScriptService]
    public class gift_auction_sys_user_bid : System.Web.Services.WebService
    {
        [WebMethod]
        public void Insert(string gift_auction_id, string sys_user_id, string amount, string USER, string TOKEN)
        {
            string msg = DBOper.gift_auction_sys_user_bid.Insert(gift_auction_id, sys_user_id, amount, USER, TOKEN);
            if (msg.Length == 0) Helper.WebServiceResponse(string.Empty);
            else Helper.WebServiceResponse(Helper.GetErrJson(msg));
        }
        [WebMethod]
        public void GetAll(string gift_auction_id, string sys_user_id, string gift_bid_status_id, string pageSize, string pageIndex, string USER, string TOKEN)
        {
            DataSet ds = DBOper.gift_auction_sys_user_bid.GetAll(gift_auction_id, sys_user_id, gift_bid_status_id, pageSize, pageIndex, USER, TOKEN);
            Helper.WebServiceResponse(JsonHelper.GetJsonBase64(ds.Tables[0]));
        }
        [WebMethod]
        public void GetOne(string id, string USER, string TOKEN)
        {
            DataSet ds = DBOper.gift_auction_sys_user_bid.GetOne(id, USER, TOKEN);
            Helper.WebServiceResponse(JsonHelper.GetJsonBase64(ds.Tables[0]));
        }
    }
}
