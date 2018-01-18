using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;

namespace FancyFix.Tools.Tool
{

    /**/
    /// <summary>
    /// 水印的类型
    /// </summary>
    public enum WaterMarkType
    {
        /**/
        /// <summary>
        /// 文字水印
        /// </summary>
        TextMark,
        /**/
        /// <summary>
        /// 图片水印
        /// </summary>
        ImageMark
    };

    /**/
    /// <summary>
    /// 水印的位置
    /// </summary>
    public enum WaterMarkPosition
    {
        /**/
        /// <summary>
        /// 左上角
        /// </summary>
        Left_Top,
        /**/
        /// <summary>
        /// 左下角
        /// </summary>
        Left_Bottom,
        /**/
        /// <summary>
        /// 右上角
        /// </summary>
        Right_Top,
        /**/
        /// <summary>
        /// 右下角
        /// </summary>
        Right_Bottom,
        /// <summary>
        /// 中间
        /// </summary>
        Middle
    };

    public class ImageTools
    {



        #region 水印

        /// <summary>
        /// 生成图片或文字水印
        /// </summary>
        /// <param name="imgPath">原图绝对路径</param>
        /// <param name="savePath">生成图路径</param>
        /// <param name="waterMarkPathOrmarkText">水印图片路径或文字水印内容</param>
        /// <param name="wmType">水印类型，文字或者图片</param>
        /// <param name="wmPosition">水印位置</param>
        /// <param name="transparency">透明度</param>
        /// <returns>是否成功</returns>
        public static bool AddWaterMark(string imgPath, string savePath, string waterMarkPathOrmarkText, WaterMarkType wmType, WaterMarkPosition wmPosition, float transparency)
        {
            switch (wmType)
            {
                case WaterMarkType.ImageMark:
                    return AddImageWaterMark(imgPath, savePath, waterMarkPathOrmarkText, wmPosition, transparency);
                case WaterMarkType.TextMark:
                    return AddTextWaterMark(imgPath, savePath, waterMarkPathOrmarkText, wmPosition, transparency, 0f);
                default:
                    return false;
            }


        }

        /// <summary>
        /// 文字水印
        /// </summary>
        /// <param name="imgPath">原图绝对路径</param>
        /// <param name="savePath">生成图绝对路径</param>
        /// <param name="markText">水印文字</param> 
        /// <param name="wmPosition">水印位置</param>
        /// <param name="transparency">透明度(0-1)</param>
        /// <param name="fontSize">文字大小,0 则随图片自动调整大小</param>
        /// <returns></returns>
        public static bool AddTextWaterMark(string imgPath, string savePath, string markText, WaterMarkPosition wmPosition, float transparency, float fontSize)
        {
            Bitmap image = null;
            Graphics g = null;
            try
            {

                //创建一个图片对象用来装载要被添加水印的图片
                Image image1 = Image.FromStream(ByteToStream(SetImageToByteArray(imgPath)));
                image = new Bitmap(image1);
                image1.Dispose();
                g = Graphics.FromImage(image);
                //设置高质量插值法
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                //设置高质量,低速度呈现平滑程度
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;


                //************* 文字水印 *****************
                int imgWidth = image.Width;
                int imgHeight = image.Height;


                int[] sizes = new int[] { 48, 36, 28, 24, 16, 14, 12, 10 };

                Font font = null;
                SizeF crSize = new SizeF();

                if (fontSize == 0)
                {

                    //利用一个循环语句来选择我们要添加文字的型号
                    //直到它的长度比图片的宽度小
                    for (int i = 0; i < sizes.Length; i++)
                    {
                        font = new Font("宋体", sizes[i], FontStyle.Bold);

                        //测量用指定的 Font 对象绘制并用指定的 StringFormat 对象格式化的指定字符串。
                        crSize = g.MeasureString(markText, font);

                        // ushort 关键字表示一种整数数据类型
                        if ((ushort)crSize.Width < (ushort)imgWidth * 0.5)
                            break;
                    }

                }
                else
                {
                    font = new Font("宋体", fontSize, FontStyle.Bold); //定义字体 ;
                    crSize = g.MeasureString(markText, font);
                }


                int xpos = 0;
                int ypos = 0;

                switch (wmPosition)
                {
                    case WaterMarkPosition.Left_Top:
                        xpos = (int)(image.Width * (float).01);
                        ypos = (int)(image.Height * (float).01);
                        break;

                    case WaterMarkPosition.Right_Top:
                        xpos = (int)((image.Width * (float).99) - (crSize.Width));
                        ypos = (int)(image.Height * (float).01);
                        break;

                    case WaterMarkPosition.Middle:
                        xpos = (int)((image.Width * (float).50) - (crSize.Width / 2));
                        ypos = (int)((image.Height * (float).50) - (crSize.Height / 2));
                        break;

                    case WaterMarkPosition.Left_Bottom:
                        xpos = (int)(image.Width * (float).01);
                        ypos = (int)((image.Height * (float).99) - crSize.Height);
                        break;

                    case WaterMarkPosition.Right_Bottom:
                        xpos = (int)((image.Width * (float).99) - (crSize.Width));
                        ypos = (int)((image.Height * (float).99) - crSize.Height);
                        break;
                }

                g.DrawString(markText, font, new SolidBrush(Color.FromArgb((int)(transparency * 255), 0, 0, 0)), xpos + 1, ypos + 1);
                g.DrawString(markText, font, new SolidBrush(Color.FromArgb((int)(transparency * 255), 255, 255, 255)), xpos, ypos);


                ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo ici = null;
                foreach (ImageCodecInfo codec in codecs)
                {
                    if (codec.MimeType.IndexOf("jpeg") > -1)
                    {
                        ici = codec;
                    }
                }
                EncoderParameters encoderParams = new EncoderParameters();
                long[] qualityParam = new long[1];

                qualityParam[0] = 80;

                EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qualityParam);
                encoderParams.Param[0] = encoderParam;

                if (ici != null)
                {
                    image.Save(savePath, ici, encoderParams);
                }
                else
                {
                    image.Save(savePath);
                }

                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                g.Dispose();
                image.Dispose();
            }



        }




        /// <summary>
        /// 图片水印
        /// </summary>
        /// <param name="imgPath">原图绝对路径</param>
        /// <param name="savePath">生成图绝对路径</param>
        /// <param name="waterMarkPath">水印图片路径</param>
        /// <param name="wmPosition">水印位置</param>
        /// <param name="transparency">透明度(0-1)</param>
        /// <returns></returns>
        public static bool AddImageWaterMark(string imgPath, string savePath, string waterMarkPath, WaterMarkPosition wmPosition, float transparency)
        {

            Bitmap image = null;
            Graphics g = null;
            Image watermark = null;
            ImageAttributes imageAttributes = null;

            try
            {

                //创建一个图片对象用来装载要被添加水印的图片
                Image image1 = Image.FromStream(ByteToStream(SetImageToByteArray(imgPath)));
                image = new Bitmap(image1);
                image1.Dispose();
                g = Graphics.FromImage(image);
                //设置高质量插值法
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                //设置高质量,低速度呈现平滑程度
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;


                watermark = new Bitmap(waterMarkPath);
                //if (watermark.Height >= image.Height || watermark.Width >= image.Width)
                //{
                //    //如果水印图片高宽超过原始图片则返回

                //    g.Dispose();
                //    image.Dispose();
                //    watermark.Dispose();
                //    return false;
                //}

                imageAttributes = new ImageAttributes();
                ColorMap colorMap = new ColorMap();

                colorMap.OldColor = Color.FromArgb(255, 0, 255, 0);
                colorMap.NewColor = Color.FromArgb(0, 0, 0, 0);
                ColorMap[] remapTable = { colorMap };

                imageAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);



                float[][] colorMatrixElements = {
                                            new float[] {1.0f,  0.0f,  0.0f,  0.0f, 0.0f},
                                            new float[] {0.0f,  1.0f,  0.0f,  0.0f, 0.0f},
                                            new float[] {0.0f,  0.0f,  1.0f,  0.0f, 0.0f},
                                            new float[] {0.0f,  0.0f,  0.0f,  transparency, 0.0f},
                                            new float[] {0.0f,  0.0f,  0.0f,  0.0f, 1.0f}
                                        };

                ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);

                imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                int xpos = 0;
                int ypos = 0;

                switch (wmPosition)
                {
                    case WaterMarkPosition.Left_Top:
                        xpos = (int)(image.Width * (float).01);
                        ypos = (int)(image.Height * (float).01);
                        break;

                    case WaterMarkPosition.Right_Top:
                        xpos = (int)((image.Width * (float).99) - (watermark.Width));
                        ypos = (int)(image.Height * (float).01);
                        break;

                    case WaterMarkPosition.Middle:
                        xpos = (int)((image.Width * (float).50) - (watermark.Width / 2));
                        ypos = (int)((image.Height * (float).50) - (watermark.Height / 2));
                        break;

                    case WaterMarkPosition.Left_Bottom:
                        xpos = (int)(image.Width * (float).01);
                        ypos = (int)((image.Height * (float).99) - watermark.Height);
                        break;

                    case WaterMarkPosition.Right_Bottom:
                        xpos = (int)((image.Width * (float).99) - (watermark.Width));
                        ypos = (int)((image.Height * (float).99) - watermark.Height);
                        break;
                }

                g.DrawImage(watermark, new Rectangle(xpos, ypos, watermark.Width, watermark.Height), 0, 0, watermark.Width, watermark.Height, GraphicsUnit.Pixel, imageAttributes);





                ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo ici = null;
                foreach (ImageCodecInfo codec in codecs)
                {
                    if (codec.MimeType.IndexOf("jpeg") > -1)
                    {
                        ici = codec;
                    }
                }
                EncoderParameters encoderParams = new EncoderParameters();
                long[] qualityParam = new long[1];

                qualityParam[0] = 80;

                EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qualityParam);
                encoderParams.Param[0] = encoderParam;

                if (ici != null)
                {
                    image.Save(savePath, ici, encoderParams);
                }
                else
                {
                    image.Save(savePath);
                }

                g.Dispose();
                image.Dispose();
                watermark.Dispose();
                imageAttributes.Dispose();


                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (g != null)
                    g.Dispose();
                if (image != null)
                    image.Dispose();
                if (watermark != null)
                    watermark.Dispose();
                if (imageAttributes != null)
                    imageAttributes.Dispose();
            }


        }
        #endregion


        #region 缩略图

        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="imgPath">原图路径</param>
        /// <param name="savePath">生成图路径</param>
        /// <param name="width">缩略图宽</param>
        /// <param name="height">缩略图高</param>
        /// <returns></returns>
        public static bool CreateSmallImage(string imgPath, string savePath, int width, int height)
        {
            Image image = null;
            Bitmap bmp = null;
            Graphics g = null;
            try
            {
                //创建一个图片对象用来装载要被添加水印的图片
                image = Image.FromFile(imgPath);

                int oldWidth = image.Width;
                int oldHeight = image.Height;
                int newWidth = oldWidth;
                int newHeight = oldHeight;

                if (width > oldWidth && height > oldHeight)
                {
                    //如果原图偏小,如果生成路径需要覆盖原图，则直接返回
                    if (imgPath == savePath)
                    {
                        image.Dispose();
                        return true;
                    }

                    newWidth = oldWidth;
                    newHeight = oldHeight;
                }
                else if (width == 0)
                {
                    if (oldHeight > height)
                    {
                        newHeight = height;
                        newWidth = oldWidth * height / oldHeight;
                    }
                }
                else if (height == 0)
                {
                    if (oldWidth > width)
                    {
                        newWidth = width;
                        newHeight = oldHeight * width / oldWidth;

                    }
                }
                else
                {
                    if (oldWidth / oldHeight >= width / height)
                    {
                        newWidth = width;
                        newHeight = width * oldHeight / oldWidth;
                    }
                    else
                    {
                        newHeight = height;
                        newWidth = oldWidth * height / oldHeight;
                    }
                }



                bmp = new Bitmap(newWidth, newHeight);
                g = Graphics.FromImage(bmp);

                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                //设置高质量,低速度呈现平滑程度
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.Clear(Color.Transparent);
                g.DrawImage(image, new Rectangle(0, 0, newWidth, newHeight), 0, 0, oldWidth, oldHeight, GraphicsUnit.Pixel);
                image.Dispose();
                bmp.Save(savePath, ImageFormat.Jpeg);

                return true;
            }
            catch
            {
                return false;
            }
            finally
            {

                if (g != null)
                    g.Dispose();
                if (image != null)
                    image.Dispose();
                if (bmp != null)
                    bmp.Dispose();
            }
        }

        #endregion

        /// <summary>
        /// 获取图片对象
        /// </summary>
        /// <param name="picName"></param>
        /// <returns></returns>
        public static System.Drawing.Image GetObjImg(string picName)
        {
            Image objImg = null;
            byte[] imgStreams = SetImageToByteArray(picName);
            if (imgStreams != null && imgStreams.Length > 0)
            {
                objImg = SetByteToImage(imgStreams);
            }

            return objImg;
        }

        /// <summary>
        /// 限制图片最大尺寸
        /// </summary>
        /// <param name="path"></param>
        /// <param name="maxWidth"></param>
        /// <param name="maxHeight"></param>
        /// <returns></returns>
        public static Boolean SetImgSize(string path, int maxWidth, int maxHeight)
        {
            int flag = 0;
            Image objImg = Image.FromFile(path);
            int width = objImg.Width;
            int height = objImg.Height;
            if (width > maxWidth)
            {
                width = maxWidth;
                flag++;
            }
            if (height > maxHeight)
            {
                height = maxHeight;
                flag++;
            }
            if (objImg != null)
            {
                objImg.Dispose();
            }
            if (flag > 0)
            {
                return CreateSmallImage(path, path, width, height);
            }

            return true;

        }

        /// <summary>
        /// 按比例缩放图片尺寸
        /// </summary>
        /// <param name="path"></param>
        /// <param name="savePath"></param>
        /// <param name="percent"></param>
        /// <param name="maxWidth"></param>
        /// <param name="maxHeight"></param>
        /// <returns></returns>
        public static Boolean SetImgSize(string path, string savePath, float percent, int maxWidth, int maxHeight)
        {
            int flag = 0;
            Image objImg = Image.FromFile(path);
            int width = objImg.Width;
            int height = objImg.Height;
            if (width > 0 && height > 0)
            {
                flag = 1;

                //按比例缩放
                width = (int)Math.Ceiling(width * percent);
                height = (int)Math.Ceiling(height * percent);

                //限制图片最大尺寸
                if (width > maxWidth)
                    width = maxWidth;
                if (height > maxHeight)
                    height = maxHeight;
            }
            if (objImg != null)
            {
                objImg.Dispose();
            }
            if (flag > 0)
            {
                return CreateSmallImage(path, savePath, width, height);
            }
            return true;

        }

        #region 将文件转换成流
        //public byte[] SetImageToByteArray(string fileName, ref string fileSize)
        /**/
        /// <summary>
        /// 将文件转换成流
        /// </summary>
        /// <param name="fileName">文件全路径</param>
        /// <returns></returns>
        private static byte[] SetImageToByteArray(string fileName)
        {
            byte[] image = null;
            try
            {
                FileStream fs = new FileStream(fileName, FileMode.Open);
                FileInfo fileInfo = new FileInfo(fileName);
                //fileSize = Convert.ToDecimal(fileInfo.Length / 1024).ToString("f2") + " K";
                int streamLength = (int)fs.Length;
                image = new byte[streamLength];
                fs.Read(image, 0, streamLength);
                fs.Close();
                return image;
            }
            catch
            {
                return image;
            }
        }
        #endregion


        #region 将byte转换成MemoryStream类型
        /**/
        /// <summary>
        /// 将byte转换成MemoryStream类型
        /// </summary>
        /// <param name="mybyte">byte[]变量</param>
        /// <returns></returns>
        private static MemoryStream ByteToStream(byte[] mybyte)
        {
            MemoryStream mymemorystream = new MemoryStream(mybyte, 0, mybyte.Length);
            return mymemorystream;
        }
        #endregion


        #region 将byte转换成Image文件
        /**/
        /// <summary>
        /// 将byte转换成Image文件
        /// </summary>
        /// <param name="mybyte">byte[]变量</param>
        /// <returns></returns>
        private static System.Drawing.Image SetByteToImage(byte[] mybyte)
        {
            System.Drawing.Image image;
            MemoryStream mymemorystream = new MemoryStream(mybyte, 0, mybyte.Length);
            image = System.Drawing.Image.FromStream(mymemorystream);
            return image;
        }
        #endregion

    }
}





