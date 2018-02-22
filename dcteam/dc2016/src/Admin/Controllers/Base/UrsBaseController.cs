using DC2016.Admin.Configs;
using DC2016.Admin.Controllers.Common;
using Microsoft.Extensions.Logging;
using System;
using System.Drawing;
using System.Net;


namespace DC2016.Admin.Controllers.Base
{
    /// <summary>
    /// 业务控制器基类，同一继承
    /// </summary>
    public class UrsBaseController<T> : BaseController<T>
    {
        static readonly string WORDS = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghjkmnpqrstuvwxyz23456789";
        public UrsBaseController(ILogger<T> logger) : base(logger) { }

        protected bool IsMobile(string mobile)
        {
            return isnum(mobile) && mobile != null && mobile.Length == 11 && mobile[0] == '1';
        }

        protected bool isvpass(string pass)
        {
            return isvpass(pass, false);
        }

        protected bool isvpass(string pass, bool ismd5)
        {
            return ismd5 ? pass.Length == 32 : (pass.Length >= 6 && pass.Length <= 16);
        }

        protected bool isvname(string name)
        {
            return name.Length >= 2 && name.Length <= 14;
        }

        protected bool isemail(string email)
        {
            return email.IndexOf("@") != -1 || email.ToLower().EndsWith("@mobile");
        }

        protected bool isvgate(string gate)
        {
            return gate.Length >= 2 && gate.Length <= 8;
        }

        protected bool isidcard(string idcard)
        {
            IDCard idc = new IDCard(idcard);
            return idc.Valid;
        }

        protected bool isnum(string strck)
        {
            foreach (char c in strck)
                if (c < '0' || c > '9')
                    return false;
            return true;
        }

    
        protected int ip2long(string ip)
        {
            try
            {
                return BitConverter.ToInt16(IPAddress.Parse(ip).GetAddressBytes(), 0);
            }
            catch
            {
                return 0;
            }
        }

        protected void checkpara_string(string arg, int lengthmin, int lengthmax)
        {
            arg = arg.Replace("_", "");
            bool isnumorlit = true;
            if (isnumorlit == false || string.IsNullOrEmpty(arg) || arg.Length < lengthmin || arg.Length > lengthmax)
            {
                throw new ArgumentException(string.Format("[{0}]字符串格式不正确", arg));
            }
        }

        protected bool IsVIP()
        {
            return DC2Conf.VipIPs.Contains(this.IP);
        }

        /// <summary>
        ///  生成验证码的字节流
        /// </summary>
        /// <returns></returns>
        public static byte[] createVCode(out string code)
        {
            Random Rand = new Random(DateTime.Now.Millisecond);
            int len = WORDS.Length;
            //4位验证码
            code = WORDS.Substring(Rand.Next(len), 1) + WORDS.Substring(Rand.Next(len), 1) +
                WORDS.Substring(Rand.Next(len), 1) + WORDS.Substring(Rand.Next(len), 1);
            Bitmap image = new Bitmap((int)Math.Ceiling((code.Length * 13.5)), 25);
            Graphics g = Graphics.FromImage(image);

            try
            {
                //生成随机生成器
                Random random = new Random();

                //清空图片背景色
                g.Clear(Color.White);
                Color[] fontColor ={
                Color.FromArgb(0, 134, 16),
                Color.FromArgb(84, 137, 3),
                Color.FromArgb(0, 0, 0),
                Color.FromArgb(10, 36, 106),
                Color.FromArgb(0,98,35),
                Color.FromArgb(56,0,83),
                Color.FromArgb(0, 0, 0),
                Color.FromArgb(0,70,69),
                Color.FromArgb(91,120,68),
                Color.FromArgb(0,0,0),
                Color.FromArgb(0, 0, 0),

            };
                char[] TempChar = code.ToCharArray();
                for (int i = 0; i < TempChar.Length; i++)
                {
                    string fonttext = getFont(random);
                    Font font = new Font(fonttext, random.Next(14, 18), randFontStyle(random, fonttext));
                    //System.Drawing.Drawing2D.LinearGradientBrush brush = new System.Drawing.Drawing2D.LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), Color.Green, Color.Green, 1.5f);
                    Color t = fontColor[random.Next(fontColor.Length)];
                    System.Drawing.Drawing2D.HatchBrush brush = new System.Drawing.Drawing2D.HatchBrush(System.Drawing.Drawing2D.HatchStyle.SolidDiamond, t, t);

                    g.DrawString(TempChar[i].ToString(), font, brush, i * 12, random.Next(3));

                }
                //画图片的前景噪音点
                for (int i = 0; i < 100; i++)
                {
                    int x = random.Next(image.Width);
                    int y = random.Next(image.Height);
                    image.SetPixel(x, y, Color.FromArgb(random.Next(255), random.Next(255), random.Next(255)));
                }

                //画图片的边框线
                g.DrawRectangle(new Pen(Color.Black), 0, 0, image.Width - 1, image.Height - 1);

                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                return ms.ToArray();
            }
            finally
            {
                g.Dispose();
                image.Dispose();
            }
        }

        static string getFont(Random rand)
        {
            string[] fonds = { /*"Gautami", "GulimChe", */"宋体"/*, "MS PGothic", "Trebuchet MS", "Verdana", "Latha"*/ };

            return fonds[rand.Next(0, fonds.Length)];
        }

        static FontStyle randFontStyle(Random rand, string font)
        {
            object[] fondstyles = { FontStyle.Bold, FontStyle.Italic, FontStyle.Strikeout, FontStyle.Underline };
            int styleind = rand.Next(1, 4);
            FontFamily ff = new FontFamily(font);
            FontStyle fs = FontStyle.Regular;

            if (!ff.IsStyleAvailable(FontStyle.Regular))
            {
                foreach (object obj in fondstyles)
                {
                    if (ff.IsStyleAvailable((FontStyle)obj))
                        return (FontStyle)obj;
                }
            }

            for (int i = 0; i < styleind; i++)
            {
                int styleno = rand.Next(0, 4);
                if (fondstyles[styleno] == null)
                {
                    i--;
                    continue;
                }
                if (ff.IsStyleAvailable((FontStyle)fondstyles[styleno]))
                    fs = fs | (FontStyle)fondstyles[styleno];
            }

            return fs;
        }
    }
}