using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Configuration;
using System.Collections;
using System.IO;
using MySql.Data.MySqlClient;

namespace Util
{
    /// <summary>
    /// 数据库帮助类
    /// </summary>
    public class DBHelper
    {
        /// <summary>
        /// 连接字符串
        /// </summary>
        public static string strConnection = "SERVER=localhost;USER ID=root;PASSWORD=Iloveyou!;DATABASE=csl;POOLING=false;CHARSET=utf8";
        #region 一般操作
        static void ExecuteSql(string strSql, MySqlParameter[] dd)
        {
            if (strSql.Length > 0)
            {
                using (MySqlConnection dbConnection = new MySqlConnection(strConnection))
                {
                    dbConnection.Open();
                    using (MySqlCommand objCommand = new MySqlCommand(strSql, dbConnection))
                    {
                        using (MySqlTransaction trans = dbConnection.BeginTransaction())
                        {
                            //objCommand.CommandTimeout = 0;
                            objCommand.Transaction = trans;
                            objCommand.Parameters.AddRange(dd);
                            objCommand.ExecuteNonQuery();
                            trans.Commit();
                        }
                    }
                }
            }
        }
        static void ExecuteSql(string strSql, MySqlParameter[] dd, MySqlConnection dbConnection, MySqlTransaction trans)
        {
            if (strSql.Length > 0)
            {
                using (MySqlCommand objCommand = new MySqlCommand(strSql, dbConnection))
                {
                    objCommand.Transaction = trans;
                    objCommand.Parameters.AddRange(dd);
                    objCommand.ExecuteNonQuery();
                }
            }
        }
        static DataSet Query(string strSql, MySqlParameter[] ss)
        {
            DataSet retds = new DataSet();
            using (MySqlConnection dbConnection = new MySqlConnection(strConnection))
            {
                dbConnection.Open();
                using (MySqlDataAdapter objSDA = new MySqlDataAdapter())
                {
                    objSDA.SelectCommand = new MySqlCommand(strSql, dbConnection);
                    objSDA.SelectCommand.Parameters.AddRange(ss);
                    objSDA.Fill(retds, "tableName");
                }
            }
            return retds;
        }
        static DataSet Query(string strSql, MySqlParameter[] ss, MySqlConnection dbConnection, MySqlTransaction trans)
        {
            DataSet retds = new DataSet();
            using (MySqlDataAdapter objSDA = new MySqlDataAdapter())
            {
                objSDA.SelectCommand = new MySqlCommand(strSql, dbConnection, trans);
                objSDA.SelectCommand.Parameters.AddRange(ss);
                objSDA.Fill(retds, "tableName");
            }
            return retds;
        }
        static int QueryRowCount(string strSql, MySqlParameter[] ss)
        {
            int nums = 0;
            using (MySqlConnection dbConnection = new MySqlConnection(strConnection))
            {
                dbConnection.Open();
                using (MySqlCommand objCommand = new MySqlCommand(strSql, dbConnection))
                {
                    objCommand.Parameters.AddRange(ss);
                    nums = Convert.ToInt32(objCommand.ExecuteScalar());
                }
            }
            return nums;
        }
        #endregion
        #region 额外的帮助
        public static string SafeDbVal(string val)
        {
            IDictionary<string, string> dict = new Dictionary<string, string>();
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
            foreach (string s in dict.Keys)
            {
                if (val.ToLower().Contains(s)) return string.Empty;
            }
            return val.Trim();
        }
        static object GetDBValue(object val)
        {
            string ret = SafeDbVal(val.ToString());

            if ((val == null) || (ret.Length == 0))
            {
                return System.DBNull.Value;
            }
            return ret;
        }
        static string RemoveDupCol(string col)
        {
            string[] colarr = col.Split(new string[] { "重复字段" }, StringSplitOptions.RemoveEmptyEntries);
            if (colarr.Length == 2) return colarr[1];
            return col;
        }
        #endregion
        #region 模版
        public static void Insert(string table, IDictionary<string, string> dict)
        {
            if (dict.Count > 0)
            {
                int counter = 1;
                IList<MySqlParameter> paraList = new List<MySqlParameter>();
                string cols = string.Empty;
                string vals = string.Empty;

                foreach (string key in dict.Keys)
                {
                    cols += key;
                    cols += (counter == dict.Count ? ")" : ",");

                    vals += "@" + key;
                    vals += (counter == dict.Count ? ")" : ",");

                    paraList.Add(new MySqlParameter(key, GetDBValue(dict[key])));

                    counter++;
                }

                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into " + table + "(");
                strSql.Append(cols);
                strSql.Append(" values (");
                strSql.Append(vals);

                MySqlParameter[] parameters = new MySqlParameter[paraList.Count];
                for (int i = 0; i < paraList.Count; i++)
                {
                    parameters[i] = paraList[i];
                }
                ExecuteSql(strSql.ToString(), parameters);
            }
        }
        public static void Insert(string table, IDictionary<string, string> dict, MySqlConnection dbConnection, MySqlTransaction trans)
        {
            if (dict.Count > 0)
            {
                int counter = 1;
                IList<MySqlParameter> paraList = new List<MySqlParameter>();
                string cols = string.Empty;
                string vals = string.Empty;

                foreach (string key in dict.Keys)
                {
                    cols += key;
                    cols += (counter == dict.Count ? ")" : ",");

                    vals += "@" + key;
                    vals += (counter == dict.Count ? ")" : ",");

                    paraList.Add(new MySqlParameter(key, GetDBValue(dict[key])));

                    counter++;
                }

                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into " + table + "(");
                strSql.Append(cols);
                strSql.Append(" values (");
                strSql.Append(vals);

                MySqlParameter[] parameters = new MySqlParameter[paraList.Count];
                for (int i = 0; i < paraList.Count; i++)
                {
                    parameters[i] = paraList[i];
                }
                ExecuteSql(strSql.ToString(), parameters, dbConnection, trans);
            }
        }
        public static void Update(string table, IDictionary<string, string> dict, IDictionary<string, string> fdict, string andor)
        {
            if (dict.Count > 0 && fdict.Count > 0)
            {
                int counter = 1;
                IList<MySqlParameter> paraList = new List<MySqlParameter>();

                StringBuilder strSql = new StringBuilder();
                strSql.Append("update " + table + " set ");

                foreach (string key in dict.Keys)
                {
                    if (dict[key].StartsWith("数字相加+") || dict[key].StartsWith("数字相减-"))
                    {
                        strSql.Append(key + "=" + key + dict[key].Substring(4, 1) + "@" + key);
                        paraList.Add(new MySqlParameter(key, GetDBValue(dict[key].Substring(5))));
                    }
                    else
                    {
                        strSql.Append(key + "=@" + key);
                        paraList.Add(new MySqlParameter(key, GetDBValue(dict[key])));
                    }
                    strSql.Append(counter == dict.Count ? string.Empty : ",");

                    counter++;
                }

                string filter = string.Empty;
                foreach (string key in fdict.Keys)
                {
                    if (fdict[key].ToUpper().Equals("IS NULL"))
                    {
                        filter += (" " + andor + " " + RemoveDupCol(key) + " IS NULL");
                    }
                    else if (fdict[key].ToUpper().Equals("IS NOT NULL"))
                    {
                        filter += (" " + andor + " " + RemoveDupCol(key) + " IS NOT NULL");
                    }
                    else if (fdict[key].StartsWith("<>") || fdict[key].StartsWith(">=") || fdict[key].StartsWith("<="))
                    {
                        filter += (" " + andor + " " + RemoveDupCol(key) + " " + fdict[key].Substring(0, 2) + " @" + key);
                        paraList.Add(new MySqlParameter(key, GetDBValue(fdict[key].Substring(2))));
                    }
                    else if (fdict[key].StartsWith(">") || fdict[key].StartsWith("<"))
                    {
                        filter += (" " + andor + " " + RemoveDupCol(key) + " " + fdict[key].Substring(0, 1) + " @" + key);
                        paraList.Add(new MySqlParameter(key, GetDBValue(fdict[key].Substring(1))));
                    }
                    else if (fdict[key].StartsWith("%") || fdict[key].EndsWith("%"))
                    {
                        filter += (" " + andor + " " + RemoveDupCol(key) + " like @" + key);
                        paraList.Add(new MySqlParameter("@" + key, GetDBValue(fdict[key])));
                    }
                    else
                    {
                        filter += (" " + andor + " " + RemoveDupCol(key) + "=@" + key);
                        paraList.Add(new MySqlParameter(key, GetDBValue(fdict[key])));
                    }
                }
                strSql.Append(" where " + filter.Substring(andor.Length + 2));

                MySqlParameter[] parameters = new MySqlParameter[paraList.Count];
                for (int i = 0; i < paraList.Count; i++)
                {
                    parameters[i] = paraList[i];
                }
                ExecuteSql(strSql.ToString(), parameters);
            }
        }
        public static void Update(string table, IDictionary<string, string> dict, IDictionary<string, string> fdict, string andor, MySqlConnection dbConnection, MySqlTransaction trans)
        {
            if (dict.Count > 0 && fdict.Count > 0)
            {
                int counter = 1;
                IList<MySqlParameter> paraList = new List<MySqlParameter>();

                StringBuilder strSql = new StringBuilder();
                strSql.Append("update " + table + " set ");

                foreach (string key in dict.Keys)
                {
                    if (dict[key].StartsWith("数字相加+") || dict[key].StartsWith("数字相减-"))
                    {
                        strSql.Append(key + "=" + key + dict[key].Substring(4, 1) + "@" + key);
                        paraList.Add(new MySqlParameter(key, GetDBValue(dict[key].Substring(5))));
                    }
                    else
                    {
                        strSql.Append(key + "=@" + key);
                        paraList.Add(new MySqlParameter(key, GetDBValue(dict[key])));
                    }
                    strSql.Append(counter == dict.Count ? string.Empty : ",");

                    counter++;
                }

                string filter = string.Empty;
                foreach (string key in fdict.Keys)
                {
                    if (fdict[key].ToUpper().Equals("IS NULL"))
                    {
                        filter += (" " + andor + " " + RemoveDupCol(key) + " IS NULL");
                    }
                    else if (fdict[key].ToUpper().Equals("IS NOT NULL"))
                    {
                        filter += (" " + andor + " " + RemoveDupCol(key) + " IS NOT NULL");
                    }
                    else if (fdict[key].StartsWith("<>") || fdict[key].StartsWith(">=") || fdict[key].StartsWith("<="))
                    {
                        filter += (" " + andor + " " + RemoveDupCol(key) + " " + fdict[key].Substring(0, 2) + " @" + key);
                        paraList.Add(new MySqlParameter(key, GetDBValue(fdict[key].Substring(2))));
                    }
                    else if (fdict[key].StartsWith(">") || fdict[key].StartsWith("<"))
                    {
                        filter += (" " + andor + " " + RemoveDupCol(key) + " " + fdict[key].Substring(0, 1) + " @" + key);
                        paraList.Add(new MySqlParameter(key, GetDBValue(fdict[key].Substring(1))));
                    }
                    else if (fdict[key].StartsWith("%") || fdict[key].EndsWith("%"))
                    {
                        filter += (" " + andor + " " + RemoveDupCol(key) + " like @" + key);
                        paraList.Add(new MySqlParameter("@" + key, GetDBValue(fdict[key])));
                    }
                    else
                    {
                        filter += (" " + andor + " " + RemoveDupCol(key) + "=@" + key);
                        paraList.Add(new MySqlParameter(key, GetDBValue(fdict[key])));
                    }
                }
                strSql.Append(" where " + filter.Substring(andor.Length + 2));

                MySqlParameter[] parameters = new MySqlParameter[paraList.Count];
                for (int i = 0; i < paraList.Count; i++)
                {
                    parameters[i] = paraList[i];
                }
                ExecuteSql(strSql.ToString(), parameters, dbConnection, trans);
            }
        }
        public static void Delete(string table, IDictionary<string, string> fdict, string andor)
        {
            if (fdict.Count > 0)
            {
                IList<MySqlParameter> paraList = new List<MySqlParameter>();

                StringBuilder strSql = new StringBuilder();
                strSql.Append("delete from " + table);

                string filter = string.Empty;
                foreach (string key in fdict.Keys)
                {
                    if (fdict[key].ToUpper().Equals("IS NULL"))
                    {
                        filter += (" " + andor + " " + RemoveDupCol(key) + " IS NULL");
                    }
                    else if (fdict[key].ToUpper().Equals("IS NOT NULL"))
                    {
                        filter += (" " + andor + " " + RemoveDupCol(key) + " IS NOT NULL");
                    }
                    else if (fdict[key].StartsWith("<>") || fdict[key].StartsWith(">=") || fdict[key].StartsWith("<="))
                    {
                        filter += (" " + andor + " " + RemoveDupCol(key) + " " + fdict[key].Substring(0, 2) + " @" + key);
                        paraList.Add(new MySqlParameter(key, GetDBValue(fdict[key].Substring(2))));
                    }
                    else if (fdict[key].StartsWith(">") || fdict[key].StartsWith("<"))
                    {
                        filter += (" " + andor + " " + RemoveDupCol(key) + " " + fdict[key].Substring(0, 1) + " @" + key);
                        paraList.Add(new MySqlParameter(key, GetDBValue(fdict[key].Substring(1))));
                    }
                    else if (fdict[key].StartsWith("%") || fdict[key].EndsWith("%"))
                    {
                        filter += (" " + andor + " " + RemoveDupCol(key) + " like @" + key);
                        paraList.Add(new MySqlParameter("@" + key, GetDBValue(fdict[key])));
                    }
                    else
                    {
                        filter += (" " + andor + " " + RemoveDupCol(key) + "=@" + key);
                        paraList.Add(new MySqlParameter(key, GetDBValue(fdict[key])));
                    }
                }
                strSql.Append(" where " + filter.Substring(andor.Length + 2));

                MySqlParameter[] parameters = new MySqlParameter[paraList.Count];
                for (int i = 0; i < paraList.Count; i++)
                {
                    parameters[i] = paraList[i];
                }

                ExecuteSql(strSql.ToString(), parameters);
            }
        }
        public static void Delete(string table, IDictionary<string, string> fdict, string andor, MySqlConnection dbConnection, MySqlTransaction trans)
        {
            if (fdict.Count > 0)
            {
                IList<MySqlParameter> paraList = new List<MySqlParameter>();

                StringBuilder strSql = new StringBuilder();
                strSql.Append("delete from " + table);

                string filter = string.Empty;
                foreach (string key in fdict.Keys)
                {
                    if (fdict[key].ToUpper().Equals("IS NULL"))
                    {
                        filter += (" " + andor + " " + RemoveDupCol(key) + " IS NULL");
                    }
                    else if (fdict[key].ToUpper().Equals("IS NOT NULL"))
                    {
                        filter += (" " + andor + " " + RemoveDupCol(key) + " IS NOT NULL");
                    }
                    else if (fdict[key].StartsWith("<>") || fdict[key].StartsWith(">=") || fdict[key].StartsWith("<="))
                    {
                        filter += (" " + andor + " " + RemoveDupCol(key) + " " + fdict[key].Substring(0, 2) + " @" + key);
                        paraList.Add(new MySqlParameter(key, GetDBValue(fdict[key].Substring(2))));
                    }
                    else if (fdict[key].StartsWith(">") || fdict[key].StartsWith("<"))
                    {
                        filter += (" " + andor + " " + RemoveDupCol(key) + " " + fdict[key].Substring(0, 1) + " @" + key);
                        paraList.Add(new MySqlParameter(key, GetDBValue(fdict[key].Substring(1))));
                    }
                    else if (fdict[key].StartsWith("%") || fdict[key].EndsWith("%"))
                    {
                        filter += (" " + andor + " " + RemoveDupCol(key) + " like @" + key);
                        paraList.Add(new MySqlParameter("@" + key, GetDBValue(fdict[key])));
                    }
                    else
                    {
                        filter += (" " + andor + " " + RemoveDupCol(key) + "=@" + key);
                        paraList.Add(new MySqlParameter(key, GetDBValue(fdict[key])));
                    }
                }
                strSql.Append(" where " + filter.Substring(andor.Length + 2));

                MySqlParameter[] parameters = new MySqlParameter[paraList.Count];
                for (int i = 0; i < paraList.Count; i++)
                {
                    parameters[i] = paraList[i];
                }

                ExecuteSql(strSql.ToString(), parameters, dbConnection, trans);
            }
        }
        public static DataSet SelectFree(string sql, IDictionary<string, string> fdict)
        {
            IList<MySqlParameter> paraList = new List<MySqlParameter>();
            foreach (string key in fdict.Keys)
            {
                paraList.Add(new MySqlParameter(key, GetDBValue(fdict[key])));
            }
            MySqlParameter[] parameters = new MySqlParameter[paraList.Count];
            for (int i = 0; i < paraList.Count; i++)
            {
                parameters[i] = paraList[i];
            }
            return Query(sql, parameters);
        }
        public static DataSet SelectFree(string sql, IDictionary<string, string> fdict, MySqlConnection dbConnection, MySqlTransaction trans)
        {
            IList<MySqlParameter> paraList = new List<MySqlParameter>();
            foreach (string key in fdict.Keys)
            {
                paraList.Add(new MySqlParameter(key, GetDBValue(fdict[key])));
            }
            MySqlParameter[] parameters = new MySqlParameter[paraList.Count];
            for (int i = 0; i < paraList.Count; i++)
            {
                parameters[i] = paraList[i];
            }
            return Query(sql, parameters, dbConnection, trans);
        }
        public static DataSet Select(string table, string getter, string order, IDictionary<string, string> fdict, string andor)
        {
            IList<MySqlParameter> paraList = new List<MySqlParameter>();

            string sql = "select " + getter + " from " + table;

            string filter = string.Empty;
            foreach (string key in fdict.Keys)
            {
                if (fdict[key].ToUpper().Equals("IS NULL"))
                {
                    filter += (" " + andor + " " + RemoveDupCol(key) + " IS NULL");
                }
                else if (fdict[key].ToUpper().Equals("IS NOT NULL"))
                {
                    filter += (" " + andor + " " + RemoveDupCol(key) + " IS NOT NULL");
                }
                else if (fdict[key].StartsWith("<>") || fdict[key].StartsWith(">=") || fdict[key].StartsWith("<="))
                {
                    filter += (" " + andor + " " + RemoveDupCol(key) + " " + fdict[key].Substring(0, 2) + " @" + key);
                    paraList.Add(new MySqlParameter(key, GetDBValue(fdict[key].Substring(2))));
                }
                else if (fdict[key].StartsWith(">") || fdict[key].StartsWith("<"))
                {
                    filter += (" " + andor + " " + RemoveDupCol(key) + " " + fdict[key].Substring(0, 1) + " @" + key);
                    paraList.Add(new MySqlParameter(key, GetDBValue(fdict[key].Substring(1))));
                }
                else if (fdict[key].StartsWith("%") || fdict[key].EndsWith("%"))
                {
                    filter += (" " + andor + " " + RemoveDupCol(key) + " like @" + key);
                    paraList.Add(new MySqlParameter("@" + key, GetDBValue(fdict[key])));
                }
                else
                {
                    filter += (" " + andor + " " + RemoveDupCol(key) + "=@" + key);
                    paraList.Add(new MySqlParameter(key, GetDBValue(fdict[key])));
                }
            }
            if (fdict.Count > 0) sql += " where " + filter.Substring(andor.Length + 2);
            if (order.Length > 0) sql += " order by " + order;

            MySqlParameter[] parameters = new MySqlParameter[paraList.Count];
            for (int i = 0; i < paraList.Count; i++)
            {
                parameters[i] = paraList[i];
            }

            return Query(sql, parameters);
        }
        public static int SelectRowCount(string table, IDictionary<string, string> fdict, string andor)
        {
            IList<MySqlParameter> paraList = new List<MySqlParameter>();

            string sql = "select count(*) from " + table;

            string filter = string.Empty;
            foreach (string key in fdict.Keys)
            {
                if (fdict[key].ToUpper().Equals("IS NULL"))
                {
                    filter += (" " + andor + " " + RemoveDupCol(key) + " IS NULL");
                }
                else if (fdict[key].ToUpper().Equals("IS NOT NULL"))
                {
                    filter += (" " + andor + " " + RemoveDupCol(key) + " IS NOT NULL");
                }
                else if (fdict[key].StartsWith("<>") || fdict[key].StartsWith(">=") || fdict[key].StartsWith("<="))
                {
                    filter += (" " + andor + " " + RemoveDupCol(key) + " " + fdict[key].Substring(0, 2) + " @" + key);
                    paraList.Add(new MySqlParameter(key, GetDBValue(fdict[key].Substring(2))));
                }
                else if (fdict[key].StartsWith(">") || fdict[key].StartsWith("<"))
                {
                    filter += (" " + andor + " " + RemoveDupCol(key) + " " + fdict[key].Substring(0, 1) + " @" + key);
                    paraList.Add(new MySqlParameter(key, GetDBValue(fdict[key].Substring(1))));
                }
                else if (fdict[key].StartsWith("%") || fdict[key].EndsWith("%"))
                {
                    filter += (" " + andor + " " + RemoveDupCol(key) + " like @" + key);
                    paraList.Add(new MySqlParameter("@" + key, GetDBValue(fdict[key])));
                }
                else
                {
                    filter += (" " + andor + " " + RemoveDupCol(key) + "=@" + key);
                    paraList.Add(new MySqlParameter(key, GetDBValue(fdict[key])));
                }
            }
            if (fdict.Count > 0) sql += " where " + filter.Substring(andor.Length + 2);
            MySqlParameter[] parameters = new MySqlParameter[paraList.Count];
            for (int i = 0; i < paraList.Count; i++)
            {
                parameters[i] = paraList[i];
            }

            return QueryRowCount(sql, parameters);
        }
        public static string SelectNewId(MySqlConnection dbConnection, MySqlTransaction trans)
        {
            return Query("select LAST_INSERT_ID()", new MySqlParameter[0], dbConnection, trans).Tables[0].Rows[0][0].ToString();
        }
        public static DataSet SelectPager(string table, string getter, string order, IDictionary<string, string> fdict, string andor, string pageSize, string pageIndex)
        {
            IList<MySqlParameter> paraList = new List<MySqlParameter>();

            string sql = "select " + getter + " from " + table;

            string filter = string.Empty;
            foreach (string key in fdict.Keys)
            {
                if (fdict[key].ToUpper().Equals("IS NULL"))
                {
                    filter += (" " + andor + " " + RemoveDupCol(key) + " IS NULL");
                }
                else if (fdict[key].ToUpper().Equals("IS NOT NULL"))
                {
                    filter += (" " + andor + " " + RemoveDupCol(key) + " IS NOT NULL");
                }
                else if (fdict[key].StartsWith("<>") || fdict[key].StartsWith(">=") || fdict[key].StartsWith("<="))
                {
                    filter += (" " + andor + " " + RemoveDupCol(key) + " " + fdict[key].Substring(0, 2) + " @" + key);
                    paraList.Add(new MySqlParameter(key, GetDBValue(fdict[key].Substring(2))));
                }
                else if (fdict[key].StartsWith(">") || fdict[key].StartsWith("<"))
                {
                    filter += (" " + andor + " " + RemoveDupCol(key) + " " + fdict[key].Substring(0, 1) + " @" + key);
                    paraList.Add(new MySqlParameter(key, GetDBValue(fdict[key].Substring(1))));
                }
                else if (fdict[key].StartsWith("%") || fdict[key].EndsWith("%"))
                {
                    filter += (" " + andor + " " + RemoveDupCol(key) + " like @" + key);
                    paraList.Add(new MySqlParameter("@" + key, GetDBValue(fdict[key])));
                }
                else
                {
                    filter += (" " + andor + " " + RemoveDupCol(key) + "=@" + key);
                    paraList.Add(new MySqlParameter(key, GetDBValue(fdict[key])));
                }
            }
            if (fdict.Count > 0) sql += " where " + filter.Substring(andor.Length + 2);
            if (order.Length > 0) sql += " order by " + order;
            sql += " limit " + (Convert.ToInt32(pageIndex) * Convert.ToInt32(pageSize)).ToString() + "," + pageSize;

            MySqlParameter[] parameters = new MySqlParameter[paraList.Count];
            for (int i = 0; i < paraList.Count; i++)
            {
                parameters[i] = paraList[i];
            }

            DataSet ds = Query(sql, parameters);
            DataColumn dc = new DataColumn("RowCount", typeof(string));
            dc.DefaultValue = SelectRowCount(table, fdict, andor);
            ds.Tables[0].Columns.Add(dc);

            return ds;
        }
        #endregion
    }
}
