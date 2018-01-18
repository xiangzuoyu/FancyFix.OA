using System;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Security.Cryptography;

namespace FancyFix.Tools.Tool
{
    /// <summary>
    /// 验证码图片类
    /// </summary>
    public class VerifyImage
    {
        /// <summary>
        /// 要显示的文字
        /// </summary>
        public string Text
        {
            get { return this.text; }
        }
        /// <summary>
        /// 图片
        /// </summary>
        public Bitmap Image
        {
            get { return this.image; }
        }
        /// <summary>
        /// 宽度
        /// </summary>
        public int Width
        {
            get { return this.width; }
        }
        /// <summary>
        /// 高度
        /// </summary>
        public int Height
        {
            get { return this.height; }
        }

        private string text;
        private int width;
        private int height;
        private Bitmap image;

        private static byte[] randb = new byte[4];
        private static RNGCryptoServiceProvider rand = new RNGCryptoServiceProvider();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="code">要显示的验证码</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        public VerifyImage(string code, int width, int height)
        {
            this.text = code;
            this.width = width;
            this.height = height;
            this.GenerateImage();

        }

        ~VerifyImage()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            this.Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                this.image.Dispose();
        }
        private FontFamily[] fonts = {
										 new FontFamily("Times New Roman"),
										 new FontFamily("Georgia"),
										 new FontFamily("Arial"),
										 new FontFamily("Comic Sans MS")
									 };

        /// <summary>
        /// 获得下一个随机数
        /// </summary>
        /// <returns></returns>
        public static int Next()
        {
            rand.GetBytes(randb);
            int value = BitConverter.ToInt32(randb, 0);
            if (value < 0) value = -value;
            return value;
        }

        /// <summary>
        /// 获得下一个随机数
        /// </summary>
        /// <param name="max">最大值</param>
        /// <returns></returns>
        public static int Next(int max)
        {
            rand.GetBytes(randb);
            int value = BitConverter.ToInt32(randb, 0);
            value = value % (max + 1);
            if (value < 0) value = -value;
            return value;
        }

        /// <summary>
        /// 获得下一个随机数
        /// </summary>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <returns></returns>
        public static int Next(int min, int max)
        {
            int value = Next(max - min) + min;
            return value;
        }


        /// <summary>
        /// 生成验证码图片
        /// </summary>
        private void GenerateImage()
        {
            Bitmap bitmap = new Bitmap(this.width, this.height, PixelFormat.Format32bppArgb);

            Graphics g = Graphics.FromImage(bitmap);
            Rectangle rect = new Rectangle(0, 0, this.width, this.height);
            g.SmoothingMode = SmoothingMode.AntiAlias;

            g.Clear(Color.White);

            int emSize = Next(3) + 15;//(int)((this.width - 20) * 2 / text.Length);
            FontFamily family = fonts[Next(fonts.Length - 1)];
            Font font = new Font(family, emSize, FontStyle.Bold);

            SizeF measured = new SizeF(0, 0);
            SizeF workingSize = new SizeF(this.width, this.height);
            while (emSize > 2 && (measured = g.MeasureString(text, font)).Width > workingSize.Width || measured.Height > workingSize.Height)
            {
                font.Dispose();
                font = new Font(family, emSize -= 2);
            }

            SolidBrush drawBrush = new SolidBrush(Color.FromArgb(Next(1), Next(1), Next(1)));
            for (int x = 0; x < 3; x++)
            {
                Pen linePen = new Pen(Color.FromArgb(Next(150), Next(150), Next(150)), 1);
                g.DrawLine(linePen, new PointF(0.0F + Next(1), 0.0F + Next(this.height)), new PointF(0.0F + Next(this.width), 0.0F + Next(this.height)));
            }

            for (int x = 0; x < this.text.Length; x++)
            {
                drawBrush.Color = Color.FromArgb(Next(50) + 200, Next(0) + 0, Next(0) + 0);
                PointF drawPoint = new PointF(0.0F + Next(4) + x * 15, 8.0F + Next(4));
                g.DrawString(this.text[x].ToString(), font, drawBrush, drawPoint);
            }

            double distort = Next(5, 10) * (Next(10) == 1 ? 1 : -1);

            using (Bitmap copy = (Bitmap)bitmap.Clone())
            {
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        int newX = (int)(x + (distort * Math.Sin(Math.PI * y / 84.0)));
                        int newY = (int)(y + (distort * Math.Cos(Math.PI * x / 54.0)));
                        if (newX < 0 || newX >= width) newX = 0;
                        if (newY < 0 || newY >= height) newY = 0;
                        bitmap.SetPixel(x, y, copy.GetPixel(newX, newY));
                    }
                }
            }


            //g.DrawRectangle(new Pen(Color.Silver), 0, 0, bitmap.Width - 1, bitmap.Height - 1);

            font.Dispose();
            drawBrush.Dispose();
            g.Dispose();

            this.image = bitmap;
        }

    }
}
