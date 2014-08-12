using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Data;

namespace Util
{
    public class JsonHelper
    {
        static string GetJson<T>(T obj)
        {
            DataContractJsonSerializer json = new DataContractJsonSerializer(obj.GetType());
            using (MemoryStream stream = new MemoryStream())
            {
                json.WriteObject(stream, obj);
                string szJson = Encoding.UTF8.GetString(stream.ToArray());
                return szJson;
            }
        }
        static T ParseFromJson<T>(string szJson)
        {
            T obj = Activator.CreateInstance<T>();
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(szJson)))
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
                return (T)serializer.ReadObject(ms);
            }
        }
        public static string GetJson(DataTable dt)
        {
            StringBuilder JsonString = new StringBuilder();
            JsonString.Append("{ ");
            JsonString.Append("\"Head\":[ ");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                JsonString.Append("{ ");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (j < dt.Columns.Count - 1)
                    {
                        JsonString.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":" + "\"" + dt.Rows[i][j].ToString() + "\",");
                    }
                    else if (j == dt.Columns.Count - 1)
                    {
                        JsonString.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":" + "\"" + dt.Rows[i][j].ToString() + "\"");
                    }
                }
                /*end Of String*/
                if (i == dt.Rows.Count - 1)
                {
                    JsonString.Append("} ");
                }
                else
                {
                    JsonString.Append("}, ");
                }
            }
            JsonString.Append("]}");
            return JsonString.ToString().Replace("\r", "<br/>").Replace("\n", "<br/>");
        }
        public static string GetJsonBase64(DataTable dt)
        {
            StringBuilder JsonString = new StringBuilder();
            JsonString.Append("{ ");
            JsonString.Append("\"Head\":[ ");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                JsonString.Append("{ ");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (j < dt.Columns.Count - 1)
                    {
                        JsonString.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":" + "\"" + Helper.Encode64(dt.Rows[i][j].ToString()) + "\",");
                    }
                    else if (j == dt.Columns.Count - 1)
                    {
                        JsonString.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":" + "\"" + Helper.Encode64(dt.Rows[i][j].ToString()) + "\"");
                    }
                }
                /*end Of String*/
                if (i == dt.Rows.Count - 1)
                {
                    JsonString.Append("} ");
                }
                else
                {
                    JsonString.Append("}, ");
                }
            }
            JsonString.Append("]}");
            return JsonString.ToString().Replace("\r", "<br/>").Replace("\n", "<br/>");
        }
    }
}
