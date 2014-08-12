<%@ WebHandler Language="C#" Class="Handler" %>

using System;
using System.Web;
using Util;

public class Handler : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        HttpFileCollection files = context.Request.Files;
        if (files.Count > 0)
        {
            HttpPostedFile file = files[0];
            if (file.ContentLength > 0)
            {
                try
                {
                    string type = context.Request.QueryString["type"];
                    string path = context.Server.MapPath("~/File/" + type + "/");
                    Helper.CheckDir(path);

                    string ext = (context.Request.QueryString["ext"] == null ? string.Empty : ("." + context.Request.QueryString["ext"]));
                    if (ext.Length == 0)
                    {
                        switch (type)
                        {
                            case "gift": ext = ".jpg"; break;
                            default:
                                context.Response.Write("权限异常!");
                                context.Response.End();
                                return;
                        }
                    }

                    string fnm = (context.Request.QueryString["fn"] == null ? DateTime.Now.ToString("yyyyMMddhhmmss") : context.Request.QueryString["fn"]);
                    file.SaveAs(path + fnm + ext);
                }
                catch
                {
                    context.Response.Write("失败，请重试");
                    context.Response.End();
                    return;
                }
            }
        }
        context.Response.End();
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}