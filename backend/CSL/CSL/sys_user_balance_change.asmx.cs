using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using Util;

namespace UUSchool
{
    /// <summary>
    /// sys_user_balance_change 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    // [System.Web.Script.Services.ScriptService]
    public class sys_user_balance_change : System.Web.Services.WebService
    {
        [WebMethod]
        public void GetAll(string sys_user_id, string balance_change_datetimes, string balance_change_datetimee, string pageSize, string pageIndex, string USER, string TOKEN)
        {
            DataSet ds = DBOper.sys_user_balance_change.GetAll(sys_user_id, balance_change_datetimes, balance_change_datetimee, pageSize, pageIndex, USER, TOKEN);
            Helper.WebServiceResponse(JsonHelper.GetJsonBase64(ds.Tables[0]));
        }
    }
}
