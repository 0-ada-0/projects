using DC2016.Admin.Controllers.Base;
using DC2016.Admin.Controllers.Common;
using DC2016.Admin.Controllers.Sms;
using DC2016.BLL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace DC2016.Admin.Controllers.GraphCode
{
    [Route("dc2/common")]
    public class GraphCodeControler : BaseController<SmsController>
    {
        static readonly string WORDS = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghjkmnpqrstuvwxyz23456789";
        public GraphCodeControler(ILogger<SmsController> logger) : base(logger) { }

        /// <summary>
        /// 生成验证码，并和加密变量vregval一起返回，如下三种情况被调用
        /// 验证码失效；超过一小时限定次数，直接获取验证码
        /// </summary>
        [HttpGet(@"getimgcode")]
        public IActionResult createVCode(string regip)
        {
            string code = string.Empty;
            byte[] buffer = createVCode(out code);
            if (buffer != null && code.Length == 4)
            {
                string imgbytes = Convert.ToBase64String(buffer);
                Hashtable cls = new Hashtable();
                string key = Guid.NewGuid().ToString("N");
                RedisHelper.Set(key, code, 5 * 60);
                cls["imgbytes"] = imgbytes;
                cls["vregval"] = key;

                return this.FuncResult(new APIReturn(0, "验证码", cls));
            }
            else
            {
                return this.FuncResult(new APIReturn(10100, "生成验证码失败"));
            }
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
