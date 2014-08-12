using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;
using System.Configuration;
using System.Data;
using System.Text.RegularExpressions;

namespace Util
{
    public class Helper
    {
        public static string WebDisk = ConfigurationManager.AppSettings["WebDisk"];
        public static string WebAddr = ConfigurationManager.AppSettings["WebAddr"];
        #region 通用帮助
        public static string GetGuid()
        {
            return Guid.NewGuid().ToString();
        }
        public static DataSet MakeEmptyDataSet()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable(); ds.Tables.Add(dt);
            return ds;
        }
        public static string GetIdJson(string content)
        {
            return "{\"id\":\"" + content + "\"}";
        }
        public static string GetErrJson(string content)
        {
            return "{\"error\":\"" + content + "\"}";
        }
        public static void WebServiceResponse(string content)
        {
            if (content.Length > 0) content = ("(" + content + ")");
            else content = "({\"success\":\"success\"})";
            string callback = HttpContext.Current.Request["jsoncallback"];
            HttpContext.Current.Response.Write(callback + content);
            HttpContext.Current.Response.End();
        }
        public static void WebServiceResponseCheckMsg(string msg)
        {
            if (msg.Length == 0) WebServiceResponse(string.Empty);
            else WebServiceResponse(Helper.GetErrJson(msg));
        }
        public static void CheckDir(string dir)
        {
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
        }
        public static string Encode64(string Message)
        {
            char[] Base64Code = new char[]
          {
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N',
              'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', 'a', 'b',
              'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p',
              'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', '0', '1', '2', '3',
              '4', '5', '6', '7', '8', '9', '+', '/', '='
          };
            byte empty = (byte)0;
            System.Collections.ArrayList byteMessage = new
              System.Collections.ArrayList(System.Text.Encoding.UTF8.GetBytes
              (Message));
            System.Text.StringBuilder outmessage;
            int messageLen = byteMessage.Count;
            int page = messageLen / 3;
            int use = 0;
            if ((use = messageLen % 3) > 0)
            {
                for (int i = 0; i < 3 - use; i++)
                    byteMessage.Add(empty);
                page++;
            }
            outmessage = new System.Text.StringBuilder(page * 4);
            for (int i = 0; i < page; i++)
            {
                byte[] instr = new byte[3];
                instr[0] = (byte)byteMessage[i * 3];
                instr[1] = (byte)byteMessage[i * 3 + 1];
                instr[2] = (byte)byteMessage[i * 3 + 2];
                int[] outstr = new int[4];
                outstr[0] = instr[0] >> 2;
                outstr[1] = ((instr[0] & 0x03) << 4) ^ (instr[1] >> 4);
                if (!instr[1].Equals(empty))
                    outstr[2] = ((instr[1] & 0x0f) << 2) ^ (instr[2] >> 6);
                else
                    outstr[2] = 64;
                if (!instr[2].Equals(empty))
                    outstr[3] = (instr[2] & 0x3f);
                else
                    outstr[3] = 64;
                outmessage.Append(Base64Code[outstr[0]]);
                outmessage.Append(Base64Code[outstr[1]]);
                outmessage.Append(Base64Code[outstr[2]]);
                outmessage.Append(Base64Code[outstr[3]]);
            }
            return outmessage.ToString();
        }
        public static string Decode64(string Message)
        {
            string Base64Code = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";
            int page = Message.Length / 4;
            System.Collections.ArrayList outMessage = new System.Collections.ArrayList(page * 3);
            char[] message = Message.ToCharArray();
            for (int i = 0; i < page; i++)
            {
                byte[] instr = new byte[4];
                instr[0] = (byte)Base64Code.IndexOf(message[i * 4]);
                instr[1] = (byte)Base64Code.IndexOf(message[i * 4 + 1]);
                instr[2] = (byte)Base64Code.IndexOf(message[i * 4 + 2]);
                instr[3] = (byte)Base64Code.IndexOf(message[i * 4 + 3]);
                byte[] outstr = new byte[3];
                outstr[0] = (byte)((instr[0] << 2) ^ ((instr[1] & 0x30) >> 4));
                if (instr[2] != 64)
                {
                    outstr[1] = (byte)((instr[1] << 4) ^ ((instr[2] & 0x3c) >> 2));
                }
                else
                {
                    outstr[2] = 0;
                }
                if (instr[3] != 64)
                {
                    outstr[2] = (byte)((instr[2] << 6) ^ instr[3]);
                }
                else
                {
                    outstr[2] = 0;
                }
                outMessage.Add(outstr[0]);
                if (outstr[1] != 0)
                    outMessage.Add(outstr[1]);
                if (outstr[2] != 0)
                    outMessage.Add(outstr[2]);
            }
            byte[] outbyte = (byte[])outMessage.ToArray(Type.GetType("System.Byte"));
            return System.Text.Encoding.Default.GetString(outbyte);
        }
        public static string GetMd5(string str)
        {
            byte[] textBytes = System.Text.Encoding.Default.GetBytes(str);
            System.Security.Cryptography.MD5CryptoServiceProvider cryptHandler;
            cryptHandler = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] hash = cryptHandler.ComputeHash(textBytes);
            string ret = "";
            foreach (byte a in hash)
            {
                if (a < 16)
                    ret += "0" + a.ToString("x");
                else
                    ret += a.ToString("x");
            }
            return ret;
        }
        public static string MakeXmlForExcel(DataSet ds, IDictionary<string, string> cols)
        {
            string top = "<?xml version=\"1.0\"?>" +
"<?mso-application progid=\"Excel.Sheet\"?>" +
"<Workbook xmlns=\"urn:schemas-microsoft-com:office:spreadsheet\"" +
" xmlns:o=\"urn:schemas-microsoft-com:office:office\"" +
" xmlns:x=\"urn:schemas-microsoft-com:office:excel\"" +
" xmlns:ss=\"urn:schemas-microsoft-com:office:spreadsheet\"" +
" xmlns:html=\"http://www.w3.org/TR/REC-html40\">" +
" <Worksheet ss:Name=\"Sheet1\">" +
"  <Table>";
            string bottom =
"  </Table>" +
" </Worksheet>" +
"</Workbook>";

            string head = "<Row>";
            foreach (string c in cols.Keys) head += "<Cell><Data ss:Type=\"String\">" + c + "</Data></Cell>";
            head += "</Row>";

            string content = string.Empty;
            int i = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                //if (i == 1000) break; i++;
                content += "<Row>";
                foreach (string c in cols.Keys)
                {
                    if (ds.Tables[0].Columns.Contains(cols[c]))
                        content += "<Cell><Data ss:Type=\"String\">" + GetForDateVal(dr[cols[c]].ToString(), cols[c]) + "</Data></Cell>";
                    else
                        content += "<Cell><Data ss:Type=\"String\"></Data></Cell>";
                }
                content += "</Row>";
            }

            return MakeXml(top + head + content + bottom);
        }
        public static string MakeXml(string content)
        {
            CheckDir(WebDisk + "Tmp\\");
            string filename = DateTime.Now.ToString("yyyyMMdd hhmmss") + ".xls";
            FileStream myFs = new FileStream(WebDisk + "Tmp\\" + filename, FileMode.Create);
            StreamWriter mySw = new StreamWriter(myFs);
            mySw.Write(content);
            mySw.Close();
            myFs.Close();

            return WebAddr + "Tmp/" + filename;
        }
        static string GetForDateVal(string data, string col)
        {
            if (col.Contains("时间") || col.Contains("日期") || col.Contains("_DATE"))
            {
                if (data.Length > 0)
                {
                    return Convert.ToDateTime(data).ToString("yyyy-MM-dd");
                }
            }
            return data;
        }
        public static bool HasInvalidStr(string val)
        {
            val = val.ToLower();

            IDictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("`", string.Empty); dict.Add("~", string.Empty); dict.Add("!", string.Empty); dict.Add("@", string.Empty);
            dict.Add("#", string.Empty); dict.Add("$", string.Empty); dict.Add("%", string.Empty); dict.Add("^", string.Empty);
            dict.Add("&", string.Empty); dict.Add("*", string.Empty); dict.Add("(", string.Empty); dict.Add(")", string.Empty);
            dict.Add("-", string.Empty); dict.Add("=", string.Empty); dict.Add("_", string.Empty); dict.Add("+", string.Empty);
            dict.Add("{", string.Empty); dict.Add("}", string.Empty); dict.Add("[", string.Empty); dict.Add("]", string.Empty);
            dict.Add(";", string.Empty); dict.Add(":", string.Empty); dict.Add("'", string.Empty); dict.Add("\"", string.Empty);
            dict.Add("\\", string.Empty); dict.Add("|", string.Empty); dict.Add("<", string.Empty); dict.Add(",", string.Empty);
            dict.Add(">", string.Empty); dict.Add(".", string.Empty); dict.Add("/", string.Empty); dict.Add("?", string.Empty);
            dict.Add("and ", string.Empty);
            dict.Add("or ", string.Empty);
            dict.Add("exec ", string.Empty);
            dict.Add("execute ", string.Empty);
            dict.Add("insert ", string.Empty);
            dict.Add("select ", string.Empty);
            dict.Add("delete ", string.Empty);
            dict.Add("update ", string.Empty);
            dict.Add("alter ", string.Empty);
            dict.Add("create ", string.Empty);
            dict.Add("drop ", string.Empty);
            dict.Add("truncate ", string.Empty);
            dict.Add("declare ", string.Empty);
            dict.Add("xp_cmdshell", string.Empty);
            dict.Add("restore ", string.Empty);
            dict.Add("backup ", string.Empty);
            dict.Add("net ", string.Empty);

            foreach (string key in dict.Keys)
            {
                if (val.Contains(key)) return true;
            }
            return false;
        }
        public static string PhpBase64Encode(string AStr)
        {
            return Convert.ToBase64String(ASCIIEncoding.Default.GetBytes(AStr));
        }
        public static string PhpBase64Decode(string ABase64)
        {
            return ASCIIEncoding.Default.GetString(Convert.FromBase64String(ABase64));
        }
        #endregion
        #region 额外帮助
        #endregion
    }
}
