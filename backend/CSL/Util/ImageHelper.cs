using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;

namespace Util
{
    public class ImageHelper
    {
        //static void SaveImageThumb(Bitmap mg, string dfile, Size newSize)
        //{
        //    double ratio = 0d;
        //    double myThumbWidth = 0d;
        //    double myThumbHeight = 0d;
        //    int x = 0;
        //    int y = 0;
        //    Bitmap bp;
        //    if ((mg.Width / Convert.ToDouble(newSize.Width)) > (mg.Height /
        //    Convert.ToDouble(newSize.Height)))
        //        ratio = Convert.ToDouble(mg.Width) / Convert.ToDouble(newSize.Width);
        //    else
        //        ratio = Convert.ToDouble(mg.Height) / Convert.ToDouble(newSize.Height);
        //    myThumbHeight = Math.Ceiling(mg.Height / ratio);
        //    myThumbWidth = Math.Ceiling(mg.Width / ratio);
        //    Size thumbSize = new Size((int)newSize.Width, (int)newSize.Height);
        //    bp = new Bitmap(newSize.Width, newSize.Height);
        //    x = (newSize.Width - thumbSize.Width) / 2;
        //    y = (newSize.Height - thumbSize.Height);
        //    System.Drawing.Graphics g = Graphics.FromImage(bp);
        //    g.SmoothingMode = SmoothingMode.HighQuality;
        //    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
        //    g.PixelOffsetMode = PixelOffsetMode.HighQuality;
        //    Rectangle rect = new Rectangle(x, y, thumbSize.Width, thumbSize.Height);
        //    g.DrawImage(mg, rect, 0, 0, mg.Width, mg.Height, GraphicsUnit.Pixel);

        //    bp.Save(dfile);
        //    bp.Dispose();
        //    bp = null;
        //}
        static void SaveImageThumb(Bitmap originalImage, string dfile, Size newSize)
        {
            int width = newSize.Width;
            int height = newSize.Height;
            int towidth = newSize.Width;
            int toheight = newSize.Height;

            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;

            string mode = "HW";
            switch (mode)
            {
                case "HW"://指定高宽缩放（可能变形）                
                    break;
                case "W"://指定宽，高按比例                    
                    toheight = originalImage.Height * width / originalImage.Width;
                    break;
                case "H"://指定高，宽按比例
                    towidth = originalImage.Width * height / originalImage.Height;
                    break;
                case "Cut"://指定高宽裁减（不变形）                
                    if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                    {
                        oh = originalImage.Height;
                        ow = originalImage.Height * towidth / toheight;
                        y = 0;
                        x = (originalImage.Width - ow) / 2;
                    }
                    else
                    {
                        ow = originalImage.Width;
                        oh = originalImage.Width * height / towidth;
                        x = 0;
                        y = (originalImage.Height - oh) / 2;
                    }
                    break;
                default:
                    break;
            }

            //新建一个bmp图片
            Image bitmap = new System.Drawing.Bitmap(towidth, toheight);

            //新建一个画板
            Graphics g = System.Drawing.Graphics.FromImage(bitmap);

            //设置高质量插值法
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            //设置高质量,低速度呈现平滑程度
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //清空画布并以透明背景色填充
            g.Clear(Color.Transparent);

            //在指定位置并且按指定大小绘制原图片的指定部分
            g.DrawImage(originalImage, new Rectangle(0, 0, towidth, toheight),
                new Rectangle(x, y, ow, oh),
                GraphicsUnit.Pixel);

            //以jpg格式保存缩略图
            bitmap.Save(dfile, System.Drawing.Imaging.ImageFormat.Jpeg);
            bitmap.Dispose();
            g.Dispose();
        }
        public static void SaveShopImg(string sfile, string dfile)
        {
            IDictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("yyzz", "营业执照");
            dict.Add("swdjz", "税务登记证");
            dict.Add("zzjgdmz", "组织机构代码证");
            dict.Add("scsfz", "手持身份证");
            dict.Add("sfzzm", "身份证正面");
            dict.Add("sfzfm", "身份证反面");
            dict.Add("yhkhxkz", "银行开户许可证");
            dict.Add("yhkzm", "银行卡正面");
            dict.Add("yhkfm", "银行卡反面");
            dict.Add("tyshfwxy", "特约商户POS服务协议");
            dict.Add("shzcdjb", "商户注册登记表");
            dict.Add("yycsmpbz", "营业场所门牌标识");
            dict.Add("sytzp", "收银台照片");
            dict.Add("yycsnbzp1", "营业场所内部照片1");
            dict.Add("yycsnbzp2", "营业场所内部照片2");
            dict.Add("yycsnbzp3", "营业场所内部照片3");
            dict.Add("yycsnbzp4", "营业场所内部照片4");
            dict.Add("frjsk", "法人结算卡");
            dict.Add("dxhzxy1", "电信合作协议1");
            dict.Add("dxhzxy2", "电信合作协议2");
            dict.Add("dxhzxy3", "电信合作协议3");
            dict.Add("qtby1", "其它备用1");
            dict.Add("qtby2", "其它备用2");
            dict.Add("qtby3", "其它备用3");
            dict.Add("qtby4", "其它备用4");
            dict.Add("qtby5", "其它备用5");

            foreach (string key in dict.Keys)
            {
                dfile = dfile.Replace(key, dict[key]);
            }

            Bitmap mg = new Bitmap(sfile);
            int width = 0;
            int height = 0;
            if (mg.Size.Height < 680 || mg.Size.Width < 680)
            {
                File.Copy(sfile, dfile, true);
            }
            else
            {
                if (mg.Size.Height > mg.Size.Width)
                {
                    width = 680;
                    height = 850;
                }
                else
                {
                    width = 850;
                    height = 680;
                }
                Size newSize = new Size(width, height);
                SaveImageThumb(mg, dfile, newSize);
            }
            mg.Dispose();
            mg = null;
            File.Delete(sfile);
        }
        public static void SaveHeadImg(string sfile, string dfile)
        {
            Bitmap mg = new Bitmap(sfile);
            if (mg.Size.Height < 120 || mg.Size.Width < 120)
            {
                File.Copy(sfile, dfile, true);
            }
            else
            {
                int width = 120;
                int height = 160;
                Size newSize = new Size(width, height);
                SaveImageThumb(mg, dfile, newSize);
            }
            mg.Dispose();
            mg = null;
            File.Delete(sfile);
        }
    }
}
