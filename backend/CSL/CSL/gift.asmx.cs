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
    /// gift 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    // [System.Web.Script.Services.ScriptService]
    public class gift : System.Web.Services.WebService
    {
        [WebMethod]
        public void Insert(string title, string description, string price, string inventory, string off_shelf_datetime, string odr, string USER, string TOKEN)
        {
            string msg = DBOper.gift.Insert(title, description, price, inventory, off_shelf_datetime, odr, USER, TOKEN);
            if (msg.Length == 0) Helper.WebServiceResponse(string.Empty);
            else Helper.WebServiceResponse(Helper.GetErrJson(msg));
        }
        [WebMethod]
        public void Update(string id, string title, string description, string price, string inventory, string off_shelf_datetime, string odr, string delete_flag, string USER, string TOKEN)
        {
            string msg = DBOper.gift.Update(id, title, description, price, inventory, off_shelf_datetime, odr, delete_flag, USER, TOKEN);
            if (msg.Length == 0) Helper.WebServiceResponse(string.Empty);
            else Helper.WebServiceResponse(Helper.GetErrJson(msg));
        }
        [WebMethod]
        public void Delete(string id, string USER, string TOKEN)
        {
            string msg = DBOper.gift.Delete(id, USER, TOKEN);
            if (msg.Length == 0) Helper.WebServiceResponse(string.Empty);
            else Helper.WebServiceResponse(Helper.GetErrJson(msg));
        }
        [WebMethod]
        public void GetAll(string title, string delete_flag, string pageSize, string pageIndex, string USER, string TOKEN)
        {
            DataSet ds = DBOper.gift.GetAll(title, delete_flag, pageSize, pageIndex, USER, TOKEN);
            Helper.WebServiceResponse(JsonHelper.GetJsonBase64(ds.Tables[0]));
        }
        [WebMethod]
        public void GetOne(string id, string USER, string TOKEN)
        {
            DataSet ds = DBOper.gift.GetOne(id, USER, TOKEN);
            Helper.WebServiceResponse(JsonHelper.GetJsonBase64(ds.Tables[0]));
        }
    }
}
