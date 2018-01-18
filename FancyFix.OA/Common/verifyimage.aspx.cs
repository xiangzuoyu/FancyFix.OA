
using FancyFix.Tools.Tool;
using System;
using System.Drawing;
using System.Drawing.Imaging;
namespace FancyFix.OA.Common
{
    public partial class verifyimage : System.Web.UI.Page
    {

        // 验证码长度
        private int codeLen = 4;
        // 图片宽度
        private int imgWidth = 80;
        // 图片高度
        private int imgHeight = 35;
        /// <summary>
        /// 验证码长度
        /// </summary>
        public int CodeLen
        {
            get
            {
                return codeLen;
            }
            set
            {
                codeLen = value;
            }
        }
        /// <summary>
        /// 图片宽度
        /// </summary>
        public int ImgWidth
        {
            get
            {
                return imgWidth;
            }
            set
            {
                imgWidth = value;
            }
        }
        /// <summary>
        /// 图片高度
        /// </summary>
        public int ImgHeight
        {
            get
            {
                return imgHeight;
            }
            set
            {
                imgHeight = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            // 规定验证码长度
            if (this.CodeLen < 4 || this.CodeLen > 16)
                throw new Exception("验证码长度必须在4到16之间");

            // 获取图片宽度
            if (this.ImgWidth < 16 || ImgWidth > 480)
                throw new Exception("图片宽度必须在16到480之间");

            // 获取图片高度
            if (ImgHeight < 16 || ImgHeight > 320)
                throw new Exception("图片高度必须在16到320之间");

            string validateCode = CreateValidateCode(this.Context);
            VerifyImage verifyimg = new VerifyImage(validateCode, imgWidth, imgHeight);

            Bitmap image = verifyimg.Image;

            Response.ContentType = "image/pjpeg";

            //MemoryStream ms = new MemoryStream();
            image.Save(Response.OutputStream, ImageFormat.Jpeg);
        }
        /// <summary>
        /// 随机生成验证码并生成Session
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string CreateValidateCode(System.Web.HttpContext context)
        {
            string validateCode = "";

            // 随机数对象
            Random random = new Random();
            for (int i = 0; i < codeLen; i++)
            {
                // 26: a - z
                int n = random.Next(26);

                // 将数字转换成大写字母
                validateCode += (char)(n + 65);
            }

            //生成Session["ImageCode"]
            context.Session.Add("ImageCode", validateCode);
            return validateCode;
        }
    }
}


