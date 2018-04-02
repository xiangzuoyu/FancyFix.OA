using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FancyFix.Tools.Tool
{
    public class CheckFilesRealFormat
    {
        /// <summary>
        /// 验证上传文件真实格式
        /// </summary>
        /// <param name="fileUpload"></param>
        /// <returns></returns>
        public static bool ValidationFile(HttpPostedFileBase fileUpload)
        {
            HttpPostedFileBase file = fileUpload;
            BinaryReader r = new BinaryReader(fileUpload.InputStream);
            string bx = " ";
            byte buffer;
            try
            {
                buffer = r.ReadByte();
                bx = buffer.ToString();
                buffer = r.ReadByte();
                bx += buffer.ToString();
            }
            catch (Exception)
            {
                return false;
            }

            bool isok = false;
            foreach (int item in Enum.GetValues(typeof(FileExtension)))
            {
                if (item.ToString() == bx)
                {
                    isok = true;
                    break;
                }
            }

            return isok;
        }

        /// <summary>
        /// 上传文件的真实格式
        /// </summary>
        public enum FileExtension
        {
            JPG = 255216,
            GIF = 7173,
            BMP = 6677,
            PNG = 13780,
            //COM = 7790,
            //EXE = 7790,
            //DLL = 7790,
            RAR = 8297,
            //ZIP = 8075,
            //XML = 6063,
            //HTML = 6033,
            //ASPX = 239187,
            //CS = 117115,
            //JS = 119105,
            TXT = 9292,
            //SQL = 255254,
            //BAT = 64101,
            //BTSEED = 10056,
            //RDP = 255254,
            //PSD = 5666,
            PDF = 3780,
            //CHM = 7384,
            //LOG = 70105,
            //REG = 8269,
            //HLP = 6395,
            XLSX = 8075,
            DOC = 8075,
            XLS = 208207,
            DOCX = 208207,
        }
    }
}
