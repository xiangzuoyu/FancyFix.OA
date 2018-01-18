using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;
using System.Drawing.Imaging;

namespace Tools.Tool
{
    public class PDFTool
    {
        /// <summary>
        /// 添加文字水印
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="toPath"></param>
        /// <param name="txt"></param>
        /// <param name="url"></param>
        /// <param name="threadNum"></param>
        /// <returns></returns>
        public static int AddTextWaterByMutiThread(string filePath, string toPath, string txt, string url, int threadNum)
        {
            if (!File.Exists(filePath))
                return 2;

            //string fileFloder = Path.GetDirectoryName(filePath);
            string fileName = Path.GetFileName(filePath);
            //string midFloder = fileFloder + "\\Thread" + threadNum.ToString();
            //FileHelper.FolderCreate(midFloder);
            
            //文字水印
            string midFloder =  "PDFTemp\\Thread" + threadNum.ToString();
            if (!Directory.Exists(midFloder))
            {
                Directory.CreateDirectory(midFloder);
            }
            string minFilePath = midFloder + "\\" + fileName;

            int retVal = 0;
            try
            {
                //打水印生成中间目录
                ConvertPDFToPDFByText2(filePath, minFilePath, txt, url);

                //转移文件
                if (PDFCopyToPath(minFilePath, toPath))
                    retVal = 1;
                else
                    retVal = 4; //转移出错
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("PdfReader not opened with owner password"))
                {
                    retVal = 3;
                }
                else
                {
                    retVal = 5;
                }
            }
          
            //删除中间文件
            FileInfo fi = new FileInfo(minFilePath);
            fi.Delete();

            return retVal;
        }
        /// <summary>
        /// 转移源PDF文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="toPath"></param>
        /// <returns></returns>
        public static Boolean PDFCopyToPath(string path, string toPath)
        {
            string toPathName = Path.GetDirectoryName(toPath);
            Tools.Tool.FileHelper.FolderCreate(toPathName);
            if (Tools.Tool.FileHelper.FileCoppy2(path, toPath))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 文字和图片水印一起
        /// 一起全部加上水印
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="toPath"></param>
        /// <param name="txt">查询型号供应商</param>
        /// <param name="url"></param>
        public static void ConvertPDFAll(string filePath, string toPath, string txt, string url)
        {
            //文字水印
            PdfReader pdfReader = new PdfReader(filePath);
            int n = pdfReader.NumberOfPages;
            iTextSharp.text.Rectangle psize = pdfReader.GetPageSize(1);
            float width = psize.Width;
            float height = psize.Height;
            Document document = new Document(psize, 50f, 50f, 50f, 50f);
            FileStream fileStream = new FileStream(toPath, FileMode.OpenOrCreate);
            PdfWriter writer = PdfWriter.GetInstance(document, fileStream);
            document.Open();
            PdfContentByte cb = writer.DirectContentUnder;
            int i = 0;
            int p = 0;
            while (i < n)
            {
                document.NewPage();
                p++;
                i++;

                Paragraph paragraph = new Paragraph("");
                BaseFont.AddToResourceSearch("iTextAsian.dll");
                BaseFont.AddToResourceSearch("iTextAsianCmaps.dll");
                Font font = new Font(BaseFont.CreateFont("STSong-Light", "UniGB-UCS2-H", true), 12f, 4, new Color(0, 0, 0xff));
                Anchor anchor1 = new Anchor(txt, font);
                anchor1.Reference = url;
                paragraph.Add(anchor1);
                paragraph.IndentationLeft = 10f;
                paragraph.IndentationRight = 10f;
                document.Add(paragraph);

                PdfImportedPage page1 = writer.GetImportedPage(pdfReader, i);
                cb.AddTemplate(page1, 0f, 0f);
            }
            document.Close();
            pdfReader.Close();
            fileStream.Close();

            //图片水印
            string fileName = Path.GetFileNameWithoutExtension(toPath) + ".pdf";
            PdfReader reader = new PdfReader(toPath);
            FileStream fs = new FileStream(@"PDFUpload\" + fileName, FileMode.Create);
            PdfStamper stamp = null;
            try
            {
                stamp = new PdfStamper(reader, fs);
                //aku
                System.Drawing.Image img = new System.Drawing.Bitmap("aku.gif");
                System.Drawing.Bitmap waterBitmap = new System.Drawing.Bitmap(img.Width, img.Height);
                System.Drawing.Graphics gr = System.Drawing.Graphics.FromImage(waterBitmap);
                float[][] ptsArray ={ 
	                new float[] {1, 0, 0, 0, 0},
	                new float[] {0, 1, 0, 0, 0},
	                new float[] {0, 0, 1, 0, 0},
                    new float[] {0, 0, 0, 0.6f, 0},
                    new float[] {0, 0, 0, 0, 1}};
                ColorMatrix clrMatrix = new ColorMatrix(ptsArray);
                ImageAttributes imgAttributes = new ImageAttributes();
                //设置图像的颜色属性
                imgAttributes.SetColorMatrix(clrMatrix, ColorMatrixFlag.Default,
                ColorAdjustType.Bitmap);
                //画图像
                gr.DrawImage(img, new System.Drawing.Rectangle(0, 0, img.Width, img.Height),
                    0, 0, img.Width, img.Height,
                    System.Drawing.GraphicsUnit.Pixel, imgAttributes);

                Image mm = Image.GetInstance(waterBitmap, System.Drawing.Imaging.ImageFormat.Png);

                mm.SetAbsolutePosition(5, 5);
                mm.ScaleAbsolute(108, 63);
                mm.Annotation = new Annotation(0f, 0f, 0f, 0f, "http://pdf.dzsc.com/");

                stamp.GetOverContent(1).AddImage(mm);

                //wku
                System.Drawing.Image imgW = new System.Drawing.Bitmap("wku.jpg");
                System.Drawing.Bitmap waterBitmapW = new System.Drawing.Bitmap(imgW.Width, imgW.Height);
                System.Drawing.Graphics grW = System.Drawing.Graphics.FromImage(waterBitmapW);
                float[][] ptsArrayW ={ 
	                new float[] {1, 0, 0, 0, 0},
	                new float[] {0, 1, 0, 0, 0},
	                new float[] {0, 0, 1, 0, 0},
                    new float[] {0, 0, 0, 0.1f, 0}, 
                    new float[] {0, 0, 0, 0, 1}};
                ColorMatrix clrMatrixW = new ColorMatrix(ptsArrayW);
                ImageAttributes imgAttributesW = new ImageAttributes();
                //设置图像的颜色属性
                imgAttributesW.SetColorMatrix(clrMatrixW, ColorMatrixFlag.Default,
                ColorAdjustType.Bitmap);
                //画图像
                grW.DrawImage(imgW, new System.Drawing.Rectangle(0, 0, imgW.Width, imgW.Height),
                    0, 0, imgW.Width, imgW.Height,
                    System.Drawing.GraphicsUnit.Pixel, imgAttributesW);

                Image mmW = Image.GetInstance(waterBitmapW, System.Drawing.Imaging.ImageFormat.Png);

                mmW.SetAbsolutePosition(48, 65);
                mmW.ScaleAbsolute(500, 680);
                stamp.GetOverContent(1).AddImage(mmW);

                stamp.Close();
                reader.Close();
                gr.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                fs.Dispose();
                fs.Close();
            }
        }

        /// <summary>
        /// 文字水印和图片水印
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="toPath"></param>
        /// <param name="txt"></param>
        /// <param name="url"></param>
        public static void ConvertPDFAll2(string filePath, string toPath, string txt, string url)
        {
            //文字水印
            if (!Directory.Exists("PDFTemp"))
            {
                Directory.CreateDirectory("PDFTemp");
            }
            string tempPath = @"PDFTemp\"+Path.GetFileNameWithoutExtension(toPath) + ".pdf";
            PdfReader pdfReader = new PdfReader(filePath);
            int n = pdfReader.NumberOfPages;
            iTextSharp.text.Rectangle psize = pdfReader.GetPageSize(1);
            float width = psize.Width;
            float height = psize.Height;
            Document document = new Document(psize, 50f, 50f, 50f, 50f);
            FileStream fileStream = new FileStream(tempPath, FileMode.OpenOrCreate);
            PdfWriter writer = PdfWriter.GetInstance(document, fileStream);
            document.Open();
            PdfContentByte cb = writer.DirectContentUnder;
            int i = 0;
            int p = 0;
            while(i < n)
            {
                document.NewPage();
                p++;
                i++;

                //Paragraph paragraph = new Paragraph("");
                iTextSharp.text.Table table = new Table(2);
                table.Border = 0;
                table.Cellpadding = 3;
                table.Cellspacing = 0;
                table.Alignment = Element.ALIGN_MIDDLE;
              
                BaseFont.AddToResourceSearch("iTextAsian.dll");
                BaseFont.AddToResourceSearch("iTextAsianCmaps.dll");
                if (i == 1) // 就第一页加链接地址
                {
                    Font font = new Font(BaseFont.CreateFont("STSong-Light", "UniGB-UCS2-H", true), 9f, 4, new Color(0, 0, 0xff));
                    iTextSharp.text.Cell cell = new Cell();
                    cell.Border = 0;
                    Anchor anchor1 = new Anchor(txt, font);
                    anchor1.Reference = url;
                    //paragraph.Add(anchor1);
                    cell.Add(anchor1);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;

                    table.AddCell(cell);
           
                    cell = new Cell();
                    cell.Border = 0;
                    anchor1 = new Anchor("捷多邦，专业PCB打样工厂，24小时加急出货", font);
                    anchor1.Reference = "http://www.jdbpcb.com/J/";
                    cell.Add(anchor1);
                    cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    table.AddCell(cell);

                    //paragraph.Add(anchor1);
                }
                //paragraph.IndentationLeft = 10f;
                //paragraph.IndentationRight = 10f;
                //paragraph.Alignment = 3;
                //document.Add(paragraph);
                document.Add(table);

                PdfImportedPage page1 = writer.GetImportedPage(pdfReader, i);
                cb.AddTemplate(page1, 0f, -40f);
            }
            document.Close();
            pdfReader.Close();
            fileStream.Close();

            //图片水印
            PdfReader reader = new PdfReader(tempPath);
            //获取保存目录名
            string toPathName = Path.GetDirectoryName(toPath);
            if (!Directory.Exists(toPathName))
            {
                Directory.CreateDirectory(toPathName);
            }
            FileStream fs = new FileStream(toPath, FileMode.Create);
            PdfStamper stamp = null;
            try
            {
                stamp = new PdfStamper(reader,fs);
                //aku
                System.Drawing.Image img = new System.Drawing.Bitmap("aku.gif");
                System.Drawing.Bitmap waterBitmap = new System.Drawing.Bitmap(img.Width, img.Height);
                System.Drawing.Graphics gr = System.Drawing.Graphics.FromImage(waterBitmap);
                float[][] ptsArray ={ 
	                new float[] {1, 0, 0, 0, 0},
	                new float[] {0, 1, 0, 0, 0},
	                new float[] {0, 0, 1, 0, 0},
                    new float[] {0, 0, 0, 0.6f, 0},
                    new float[] {0, 0, 0, 0, 1}};
                ColorMatrix clrMatrix = new ColorMatrix(ptsArray);
                ImageAttributes imgAttributes = new ImageAttributes();
                //设置图像的颜色属性
                imgAttributes.SetColorMatrix(clrMatrix, ColorMatrixFlag.Default,
                ColorAdjustType.Bitmap);
                //画图像
                gr.DrawImage(img, new System.Drawing.Rectangle(0, 0, img.Width, img.Height),
                    0, 0, img.Width, img.Height,
                    System.Drawing.GraphicsUnit.Pixel, imgAttributes);

                Image mm = Image.GetInstance(waterBitmap, System.Drawing.Imaging.ImageFormat.Png);

                mm.SetAbsolutePosition(5, 5);
                mm.ScaleAbsolute(108, 63);
                mm.Annotation = new Annotation(0f, 0f, 0f, 0f, "http://pdf.dzsc.com/");

                stamp.GetOverContent(1).AddImage(mm);

                //wku
                System.Drawing.Image imgW = new System.Drawing.Bitmap("wku.jpg");
                System.Drawing.Bitmap waterBitmapW = new System.Drawing.Bitmap(imgW.Width, imgW.Height);
                System.Drawing.Graphics grW = System.Drawing.Graphics.FromImage(waterBitmapW);
                float[][] ptsArrayW ={ 
	                new float[] {1, 0, 0, 0, 0},
	                new float[] {0, 1, 0, 0, 0},
	                new float[] {0, 0, 1, 0, 0},
                    new float[] {0, 0, 0, 0.1f, 0}, 
                    new float[] {0, 0, 0, 0, 1}};
                ColorMatrix clrMatrixW = new ColorMatrix(ptsArrayW);
                ImageAttributes imgAttributesW = new ImageAttributes();
                //设置图像的颜色属性
                imgAttributesW.SetColorMatrix(clrMatrixW, ColorMatrixFlag.Default,
                ColorAdjustType.Bitmap);
                //画图像
                grW.DrawImage(imgW, new System.Drawing.Rectangle(0, 0, imgW.Width, imgW.Height),
                    0, 0, imgW.Width, imgW.Height,
                    System.Drawing.GraphicsUnit.Pixel, imgAttributesW);

                Image mmW = Image.GetInstance(waterBitmapW, System.Drawing.Imaging.ImageFormat.Png);

                mmW.SetAbsolutePosition(48, 65);
                mmW.ScaleAbsolute(500, 680);
                stamp.GetOverContent(1).AddImage(mmW);

                stamp.Close();
                reader.Close();
                gr.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                fs.Dispose();
                fs.Close();

                FileInfo fi = new FileInfo(tempPath);
                fi.Delete();
            }
        }

        /// <summary>
        /// 文字水印和图片水印 获取页数
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="toPath"></param>
        /// <param name="txt"></param>
        /// <param name="url"></param>
        /// <param name="pages"></param>
        public static void ConvertPDFAll2(string filePath, string toPath, string txt, string url,ref int pages)
        {
            //文字水印
            if (!Directory.Exists("PDFTemp"))
            {
                Directory.CreateDirectory("PDFTemp");
            }
            string tempPath = @"PDFTemp\" + Path.GetFileNameWithoutExtension(toPath) + ".pdf";
            PdfReader pdfReader = new PdfReader(filePath);
            int n = pdfReader.NumberOfPages;
            pages = n;
            iTextSharp.text.Rectangle psize = pdfReader.GetPageSize(1);
            float width = psize.Width;
            float height = psize.Height;
            Document document = new Document(psize, 50f, 50f, 50f, 50f);
            FileStream fileStream = new FileStream(tempPath, FileMode.OpenOrCreate);
            PdfWriter writer = PdfWriter.GetInstance(document, fileStream);
            document.Open();
            PdfContentByte cb = writer.DirectContentUnder;
            int i = 0;
            int p = 0;
            while (i < n)
            {
                document.NewPage();
                p++;
                i++;

                Paragraph paragraph = new Paragraph("");
                BaseFont.AddToResourceSearch("iTextAsian.dll");
                BaseFont.AddToResourceSearch("iTextAsianCmaps.dll");
                if (i == 1) // 就第一页加链接地址
                {
                    Font font = new Font(BaseFont.CreateFont("STSong-Light", "UniGB-UCS2-H", true), 12f, 4, new Color(0, 0, 0xff));
                    Anchor anchor1 = new Anchor(txt, font);
                    anchor1.Reference = url;
                    paragraph.Add(anchor1);
                }
                paragraph.IndentationLeft = 10f;
                paragraph.IndentationRight = 10f;
                document.Add(paragraph);

                PdfImportedPage page1 = writer.GetImportedPage(pdfReader, i);
                cb.AddTemplate(page1, 0f, -40f);
            }
            document.Close();
            pdfReader.Close();
            fileStream.Close();

            //图片水印
            PdfReader reader = new PdfReader(tempPath);
            //获取保存目录名
            string toPathName = Path.GetDirectoryName(toPath);
            if (!Directory.Exists(toPathName))
            {
                Directory.CreateDirectory(toPathName);
            }
            FileStream fs = new FileStream(toPath, FileMode.Create);
            PdfStamper stamp = null;
            try
            {
                stamp = new PdfStamper(reader, fs);
                //aku
                System.Drawing.Image img = new System.Drawing.Bitmap("aku.gif");
                System.Drawing.Bitmap waterBitmap = new System.Drawing.Bitmap(img.Width, img.Height);
                System.Drawing.Graphics gr = System.Drawing.Graphics.FromImage(waterBitmap);
                float[][] ptsArray ={ 
	                new float[] {1, 0, 0, 0, 0},
	                new float[] {0, 1, 0, 0, 0},
	                new float[] {0, 0, 1, 0, 0},
                    new float[] {0, 0, 0, 0.6f, 0},
                    new float[] {0, 0, 0, 0, 1}};
                ColorMatrix clrMatrix = new ColorMatrix(ptsArray);
                ImageAttributes imgAttributes = new ImageAttributes();
                //设置图像的颜色属性
                imgAttributes.SetColorMatrix(clrMatrix, ColorMatrixFlag.Default,
                ColorAdjustType.Bitmap);
                //画图像
                gr.DrawImage(img, new System.Drawing.Rectangle(0, 0, img.Width, img.Height),
                    0, 0, img.Width, img.Height,
                    System.Drawing.GraphicsUnit.Pixel, imgAttributes);

                Image mm = Image.GetInstance(waterBitmap, System.Drawing.Imaging.ImageFormat.Png);

                mm.SetAbsolutePosition(5, 5);
                mm.ScaleAbsolute(108, 63);
                mm.Annotation = new Annotation(0f, 0f, 0f, 0f, "http://pdf.dzsc.com/");

                stamp.GetOverContent(1).AddImage(mm);

                //wku
                System.Drawing.Image imgW = new System.Drawing.Bitmap("wku.jpg");
                System.Drawing.Bitmap waterBitmapW = new System.Drawing.Bitmap(imgW.Width, imgW.Height);
                System.Drawing.Graphics grW = System.Drawing.Graphics.FromImage(waterBitmapW);
                float[][] ptsArrayW ={ 
	                new float[] {1, 0, 0, 0, 0},
	                new float[] {0, 1, 0, 0, 0},
	                new float[] {0, 0, 1, 0, 0},
                    new float[] {0, 0, 0, 0.1f, 0}, 
                    new float[] {0, 0, 0, 0, 1}};
                ColorMatrix clrMatrixW = new ColorMatrix(ptsArrayW);
                ImageAttributes imgAttributesW = new ImageAttributes();
                //设置图像的颜色属性
                imgAttributesW.SetColorMatrix(clrMatrixW, ColorMatrixFlag.Default,
                ColorAdjustType.Bitmap);
                //画图像
                grW.DrawImage(imgW, new System.Drawing.Rectangle(0, 0, imgW.Width, imgW.Height),
                    0, 0, imgW.Width, imgW.Height,
                    System.Drawing.GraphicsUnit.Pixel, imgAttributesW);

                Image mmW = Image.GetInstance(waterBitmapW, System.Drawing.Imaging.ImageFormat.Png);

                mmW.SetAbsolutePosition(48, 65);
                mmW.ScaleAbsolute(500, 680);
                stamp.GetOverContent(1).AddImage(mmW);

                stamp.Close();
                reader.Close();
                gr.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                fs.Dispose();
                fs.Close();

                FileInfo fi = new FileInfo(tempPath);
                fi.Delete();
            }
        }


        /// <summary>
        /// 文字图片水印
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="toPath"></param>
        /// <param name="showText">显示文字</param>
        /// <param name="url">链接地址</param>
        public static void ConvertPDFToPDFByText(string filePath, string toPath, string txt, float absoluteX, float absoluteY, string url)
        {
            System.Drawing.Image img = GetImageForString(txt);
            ConvertPDFToPDFByImg(filePath, toPath, img, absoluteX, absoluteY, url);
        }

        /// <summary>
        /// 转化PDF格式加文字水印
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="toPath"></param>
        /// <param name="txt"></param>
        /// <param name="url"></param>
        public static void ConvertPDFToPDFByText2(string filePath, string toPath, string txt, string url)
        {
            ////文字水印
            PdfReader pdfReader = new PdfReader(filePath);
            int n = pdfReader.NumberOfPages;
            iTextSharp.text.Rectangle psize = pdfReader.GetPageSize(1);
            float width = psize.Width;
            float height = psize.Height;
            Document document = new Document(psize, 50f, 50f, 0f, 50f);
            FileStream fileStream = new FileStream(toPath, FileMode.OpenOrCreate);
            PdfWriter writer = PdfWriter.GetInstance(document, fileStream);
            document.Open();
            try
            {
                PdfContentByte cb = writer.DirectContentUnder;
                int i = 0;
                int p = 0;
                while (i < n)
                {
                    document.NewPage();
                    p++;
                    i++;
                    if (i == 1) // 就第一页加链接地址
                    {
                        Paragraph paragraph = new Paragraph("");
                        BaseFont.AddToResourceSearch("iTextAsian.dll");
                        BaseFont.AddToResourceSearch("iTextAsianCmaps.dll");
                    
                        Font font = new Font(BaseFont.CreateFont("STSong-Light", "UniGB-UCS2-H", true), 8f, 4, new Color(0, 0, 0xff));
                        Anchor anchor1 = new Anchor(txt, font);
                        anchor1.Reference = url;
                        
                        paragraph.Add(anchor1);
                        paragraph.IndentationLeft = 10f;
                        paragraph.IndentationRight = 10f;
                        paragraph.Alignment = 2; //右对齐
                        document.Add(paragraph);
                    }

                    PdfImportedPage page1 = writer.GetImportedPage(pdfReader, i);
                    cb.AddTemplate(page1, 0f, 0f);
                }
                document.Close();
                pdfReader.Close();
                fileStream.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                fileStream.Dispose();
                fileStream.Close();
            }
        }

        /// <summary>
        /// 加图片水印  不带url
        /// </summary>
        /// <param name="filePath">包含文件名</param>
        /// <param name="toPath">包含文件名</param>
        /// <param name="imgName">图片名称(在应用程序目录下)</param>
        /// <param name="type">1:左下角logo  2:背景图片</param>
        public static void CovertPDFByPicWater(string filePath, string toPath, string imgName, int type)
        {
            //图片水印
            string fileName = filePath; //Path.GetFileNameWithoutExtension(toPath) + ".pdf";
            string picName = AppDomain.CurrentDomain.BaseDirectory + imgName; // "dzsc.gif";
            PdfReader reader = new PdfReader(fileName);
            FileStream fs = new FileStream(toPath, FileMode.Create);//@"PDFUpload\" + fileName
            PdfStamper stamp = null;
            try
            {
                if (type == 1)
                {
                    stamp = new PdfStamper(reader, fs);

                    // 设置logo
                    //Application.CommonAppDataPath() + "dzsc.gif";
                    System.Drawing.Image img = new System.Drawing.Bitmap(picName);
                    System.Drawing.Bitmap waterBitmap = new System.Drawing.Bitmap(img.Width, img.Height);
                    System.Drawing.Graphics gr = System.Drawing.Graphics.FromImage(waterBitmap);
                    float[][] ptsArray ={ 
                    new float[] {1, 0, 0, 0, 0},
                    new float[] {0, 1, 0, 0, 0},
                    new float[] {0, 0, 1, 0, 0},
                    new float[] {0, 0, 0, 0.6f, 0},
                    new float[] {0, 0, 0, 0, 1}};
                    ColorMatrix clrMatrix = new ColorMatrix(ptsArray);
                    ImageAttributes imgAttributes = new ImageAttributes();
                    //设置图像的颜色属性
                    imgAttributes.SetColorMatrix(clrMatrix, ColorMatrixFlag.Default,
                    ColorAdjustType.Bitmap);
                    //画图像
                    gr.DrawImage(img, new System.Drawing.Rectangle(0, 0, img.Width, img.Height),
                        0, 0, img.Width, img.Height,
                        System.Drawing.GraphicsUnit.Pixel, imgAttributes);

                    Image mm = Image.GetInstance(waterBitmap, System.Drawing.Imaging.ImageFormat.Png);

                    mm.SetAbsolutePosition(5, 5);
                    mm.ScaleAbsolute(108, 63);
                    mm.Annotation = new Annotation(0f, 0f, 0f, 0f, "http://www.dzsc.com/");

                    stamp.GetOverContent(1).AddImage(mm);
                    stamp.Close();
                    reader.Close();
                    gr.Dispose();
                }
                else if (type == 2)
                {
                    // 设置背景
                    System.Drawing.Image imgW = new System.Drawing.Bitmap(picName);
                    System.Drawing.Bitmap waterBitmapW = new System.Drawing.Bitmap(imgW.Width, imgW.Height);
                    System.Drawing.Graphics grW = System.Drawing.Graphics.FromImage(waterBitmapW);
                    float[][] ptsArrayW ={ 
                        new float[] {1, 0, 0, 0, 0},
                        new float[] {0, 1, 0, 0, 0},
                        new float[] {0, 0, 1, 0, 0},
                        new float[] {0, 0, 0, 0.1f, 0}, 
                        new float[] {0, 0, 0, 0, 1}};
                    ColorMatrix clrMatrixW = new ColorMatrix(ptsArrayW);
                    ImageAttributes imgAttributesW = new ImageAttributes();
                    //设置图像的颜色属性
                    imgAttributesW.SetColorMatrix(clrMatrixW, ColorMatrixFlag.Default,
                    ColorAdjustType.Bitmap);
                    //画图像
                    grW.DrawImage(imgW, new System.Drawing.Rectangle(0, 0, imgW.Width, imgW.Height),
                        0, 0, imgW.Width, imgW.Height,
                        System.Drawing.GraphicsUnit.Pixel, imgAttributesW);

                    Image mmW = Image.GetInstance(waterBitmapW, System.Drawing.Imaging.ImageFormat.Png);

                    mmW.SetAbsolutePosition(48, 65);
                    mmW.ScaleAbsolute(500, 680);
                    stamp.GetOverContent(1).AddImage(mmW);
                    stamp.Close();
                    reader.Close();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                fs.Dispose();
                fs.Close();
            }
        }

        /// <summary>
        /// 转化PDF加图片水印
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="toPath"></param>
        /// <param name="imgpath">图片全路径</param>
        /// <param name="url"></param>
        public static void ConvertPDFToPDFByImg(string filePath, string toPath, string imgpath, string url)
        {
            System.Drawing.Image img = System.Drawing.Image.FromFile(imgpath);
            ConvertPDFToPDFByImg(filePath, toPath, img, 50, 10, url);
        }

        /// <summary>
        /// 转化PDF加图片水印
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="toPath"></param>
        /// <param name="imgpath">图片全路径</param>
        /// <param name="url"></param>
        public static void ConvertPDFToPDFByImg(string filePath, string toPath, string imgpath, float absoluteX, float absoluteY, string url)
        {
            System.Drawing.Image img = System.Drawing.Image.FromFile(imgpath);
            ConvertPDFToPDFByImg(filePath, toPath, img, absoluteX, absoluteY, url);
        }


        /// <summary>
        /// 转化PDF加图片水印
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="toPath"></param>
        /// <param name="imgpath"></param>
        /// <param name="url"></param>
        public static void ConvertPDFToPDFByImg(string filePath, string toPath, System.Drawing.Image imgpath, string url)
        {
            ConvertPDFToPDFByImg(filePath, toPath, imgpath, 50, 10, url);
        }

        /// <summary>
        /// 转化PDF加图片水印
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="toPath"></param>
        /// <param name="imgpath"></param>
        /// <param name="absoluteX"></param>
        /// <param name="absoluteY"></param>
        /// <param name="fitWidth"></param>
        /// <param name="fitHeight"></param>
        /// <param name="url"></param>
        public static void ConvertPDFToPDFByImg(string filePath, string toPath, System.Drawing.Image imgpath, float absoluteX, float absoluteY, string url)
        {
            PdfReader reader = new PdfReader(filePath);
            FileStream fs = new FileStream(toPath, FileMode.Create);
            PdfStamper stamp = null;

            try
            {
                stamp = new PdfStamper(reader, fs);
                int i = 0;
                while (reader.NumberOfPages > i)
                {
                    i++;
                    Image mm = Image.GetInstance(imgpath, System.Drawing.Imaging.ImageFormat.Jpeg);
                    Rectangle pageSize = reader.GetPageSizeWithRotation(i);
                    mm.SetAbsolutePosition(absoluteX, absoluteY);
                    mm.ScaleAbsolute((float)imgpath.Width, (float)imgpath.Height);
                    mm.Annotation = new Annotation(0f, 0f, 0f, 0f, url);
                    stamp.GetOverContent(i).AddImage(mm);
                }
                stamp.Close();
                reader.Close();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                fs.Dispose();
                fs.Close();
            }
        }

        /// <summary>
        /// 添加链接到PDF
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="toPath"></param>
        /// <param name="url"></param>
        /// <param name="urlName"></param>
        /// <param name="absoluteX"></param>
        /// <param name="absoluteY"></param>
        /// <param name="fitWidth"></param>
        /// <param name="fitHeight"></param>
        public static void AddLink(string filePath, string toPath, string url, string urlName, float absoluteX, float absoluteY, float fitWidth, float fitHeight)
        {
            PdfReader reader = new PdfReader(filePath);
            FileStream fs = new FileStream(toPath, FileMode.Create);
            PdfStamper stamp = null;
            try
            {
                stamp = new PdfStamper(reader, fs);
                int i = 0;
                int n = reader.NumberOfPages;
                Anchor a = new Anchor(urlName, FontFactory.GetFont("Helvetica", 12f, 4, new iTextSharp.text.Color(0, 0, 0xff)));
                a.Reference = url;
                new PdfAction(new Uri(url));
                while (i < n)
                {
                    i++;
                    reader.GetPageSizeWithRotation(i);
                    stamp.GetOverContent(i);
                }
                stamp.Close();
                reader.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                fs.Dispose();
                fs.Close();
            }
        }

        /// <summary>
        /// 自动生成型号的图片对象
        /// </summary>
        /// <param name="checkCode"></param>
        /// <returns></returns>
        public static System.Drawing.Image GetImageForString(string checkCode)
        {
            if ((checkCode == null) || (checkCode.Trim() == string.Empty))
            {
                checkCode = "点击查找相关型号资料";
            }
            int strlen = Encoding.Default.GetBytes(checkCode).Length;
            System.Drawing.Bitmap image = new System.Drawing.Bitmap((int)Math.Ceiling(strlen * 12.1), 0x19);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(image);
            g.Clear(System.Drawing.Color.White);
            System.Drawing.Font font = new System.Drawing.Font("楷体_GB2312", 16f, System.Drawing.FontStyle.Underline);
            System.Drawing.Drawing2D.LinearGradientBrush brush = new System.Drawing.Drawing2D.LinearGradientBrush(new System.Drawing.Rectangle(0, 0, image.Width, image.Height), System.Drawing.Color.Blue, System.Drawing.Color.DarkRed, 1.2f, true);
            g.DrawString(checkCode, font, brush, (float)1f, (float)1f);
            return image;
        }
    }
}
