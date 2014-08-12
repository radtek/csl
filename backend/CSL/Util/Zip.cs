using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ionic.Zip;

namespace Util
{
    public class Zip
    {
        public static string Create(string DirectoryToZip, string ZipDirToCreate, string zipfile)
        {
            Helper.CheckDir(DirectoryToZip);
            Helper.CheckDir(ZipDirToCreate);

            zipfile = zipfile + ".zip";
            using (ZipFile zip = new ZipFile(Encoding.UTF8))
            {
                String[] filenames = System.IO.Directory.GetFiles(DirectoryToZip);
                foreach (String filename in filenames)
                {
                    zip.AddFile(filename);
                }
                zip.Save(ZipDirToCreate + zipfile);
            }
            return zipfile;
        }
        public static string CreateMulti(IDictionary<string, string> DirectoryToZips, string ZipDirToCreate)
        {
            string tmpdir = ZipDirToCreate + Helper.GetGuid() + "\\";

            foreach (string key in DirectoryToZips.Keys)
            {
                Create(key, tmpdir, DirectoryToZips[key]);
            }
            return Create(tmpdir, ZipDirToCreate, DateTime.Now.ToString("yyyyMMddhhmmss"));
        }
    }
}
