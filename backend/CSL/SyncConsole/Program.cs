using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBOper;
using Util;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace SyncConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            football_round_chain.GetRandomMatch("1", "2011", "2011-2-1", "2", "", "");

            Console.WriteLine("finish");
            Console.Read();
        }
        public static string GetMd5(string str)
        {
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] bytesSrc = Encoding.Default.GetBytes(str);
            byte[] btyresult = md5.ComputeHash(bytesSrc);
            return Convert.ToBase64String(btyresult);
        }
    }
}
