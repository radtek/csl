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
    /// sys_dict 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    // [System.Web.Script.Services.ScriptService]
    public class sys_dict : System.Web.Services.WebService
    {
        [WebMethod]
        public void GetAll(string table, string val)
        {
            DataSet ds = DBOper.sys_dict.GetAll(table, val);
            Helper.WebServiceResponse(JsonHelper.GetJsonBase64(ds.Tables[0]));
        }
    }
}
