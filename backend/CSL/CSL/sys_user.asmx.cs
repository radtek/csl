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
    /// sys_user 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    // [System.Web.Script.Services.ScriptService]
    public class sys_user : System.Web.Services.WebService
    {
        [WebMethod]
        public void Login(string login_name, string login_pwd)
        {
            DataSet ds = DBOper.sys_user.Login(login_name, login_pwd, this.Context.Request.UserHostAddress);
            Helper.WebServiceResponse(JsonHelper.GetJsonBase64(ds.Tables[0]));
        }
        [WebMethod]
        public void Register(string login_name, string login_pwd, string reference_sys_user_id, string reference_sys_user_login_name)
        {
            string msg = DBOper.sys_user.Register(login_name, login_pwd, reference_sys_user_id, reference_sys_user_login_name);
            if (msg.Length == 0) Helper.WebServiceResponse(string.Empty);
            else Helper.WebServiceResponse(Helper.GetErrJson(msg));
        }
        [WebMethod]
        public void UpdateInfo(string id, string name, string phone, string address, string email, string qq, string weixin, string USER, string TOKEN)
        {
            string msg = DBOper.sys_user.UpdateInfo(id, name, phone, address, email, qq, weixin, USER, TOKEN);
            if (msg.Length == 0) Helper.WebServiceResponse(string.Empty);
            else Helper.WebServiceResponse(Helper.GetErrJson(msg));
        }
        [WebMethod]
        public void Insert(string login_name, string login_pwd, string role_id, string name, string phone, string address, string email, string qq, string weixin, string reference_sys_user_login_name, string USER, string TOKEN)
        {
            string msg = DBOper.sys_user.Insert(login_name, login_pwd, role_id, name, phone, address, email, qq, weixin, reference_sys_user_login_name, USER, TOKEN);
            if (msg.Length == 0) Helper.WebServiceResponse(string.Empty);
            else Helper.WebServiceResponse(Helper.GetErrJson(msg));
        }
        [WebMethod]
        public void Update(string id, string role_id, string name, string phone, string address, string email, string qq, string weixin, string reference_sys_user_login_name, string delete_flag, string USER, string TOKEN)
        {
            string msg = DBOper.sys_user.Update(id, role_id, name, phone, address, email, qq, weixin, reference_sys_user_login_name, delete_flag, USER, TOKEN);
            if (msg.Length == 0) Helper.WebServiceResponse(string.Empty);
            else Helper.WebServiceResponse(Helper.GetErrJson(msg));
        }
        [WebMethod]
        public void UpdatePwd(string id, string login_pwd_old, string login_pwd, string USER, string TOKEN)
        {
            string msg = DBOper.sys_user.UpdatePwd(id, login_pwd_old, login_pwd, USER, TOKEN);
            if (msg.Length == 0) Helper.WebServiceResponse(string.Empty);
            else Helper.WebServiceResponse(Helper.GetErrJson(msg));
        }
        [WebMethod]
        public void Delete(string id, string USER, string TOKEN)
        {
            string msg = DBOper.sys_user.Delete(id, USER, TOKEN);
            if (msg.Length == 0) Helper.WebServiceResponse(string.Empty);
            else Helper.WebServiceResponse(Helper.GetErrJson(msg));
        }
        [WebMethod]
        public void GetAll(string login_name, string role_id, string reference_sys_user_id, string delete_flag, string register_dates, string register_datee, string pageSize, string pageIndex, string USER, string TOKEN)
        {
            DataSet ds = DBOper.sys_user.GetAll(login_name, role_id, reference_sys_user_id, delete_flag, register_dates, register_datee, pageSize, pageIndex, USER, TOKEN);
            Helper.WebServiceResponse(JsonHelper.GetJsonBase64(ds.Tables[0]));
        }
        [WebMethod]
        public void GetOne(string id, string USER, string TOKEN)
        {
            DataSet ds = DBOper.sys_user.GetOne(id, USER, TOKEN);
            Helper.WebServiceResponse(JsonHelper.GetJsonBase64(ds.Tables[0]));
        }
    }
}
