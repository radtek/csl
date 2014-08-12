using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Util;

namespace DBOper
{
    public class sys_dict
    {
        public static DataSet GetAll(string table, string val)
        {
            IDictionary<string, string> fdict = new Dictionary<string, string>();
            DataSet ds = DBHelper.Select(table, "*", string.Empty, fdict, "and");

            if (val.Length > 0)
            {
                DataColumn dc = new DataColumn("VAL", typeof(string));
                dc.DefaultValue = val;
                ds.Tables[0].Columns.Add(dc);
            }
            return ds;
        }
        public static DataSet GetEmptyDs()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable(); ds.Tables.Add(dt);
            return ds;
        }
    }
}
