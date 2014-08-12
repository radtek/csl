using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Util;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            if (Request.UrlReferrer != null)
                ViewState["lastPage"] = Request.UrlReferrer.ToString();
    }
    protected void back(object sender, EventArgs e)
    {
        if (ViewState["lastPage"] != null)
            Response.Redirect(ViewState["lastPage"].ToString());
    }

    protected void btnFileUpload_Click(object sender, EventArgs e)
    {
        if (FileUpLoad1.HasFile)
        {
            string type = Request.QueryString["type"];
            string path = Server.MapPath("~/File/" + type + "/");
            Helper.CheckDir(path);

            string ext = (Request.QueryString["ext"] == null ? string.Empty : ("." + Request.QueryString["ext"]));
            if (ext.Length == 0)
            {
                switch (type)
                {
                    case "avatar":
                    case "icon": ext = ".jpg"; break;
                    case "apk": ext = ".apk"; break;
                    case "ipa": ext = ".ipa"; break;
                    default:
                        lblMessage.Text = "权限异常!";
                        return;
                }
            }

            string fnm = (Request.QueryString["fn"] == null ? DateTime.Now.ToString("yyyyMMddhhmmss") : Request.QueryString["fn"]);
            FileUpLoad1.PostedFile.SaveAs(path + fnm + ext);
            lblMessage.Text = "上传成功!";
            back(null, null);
        }
        else
        {
            lblMessage.Text = "尚未选择文件!";
        }
    }
}