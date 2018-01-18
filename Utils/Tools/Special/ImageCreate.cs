using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using FancyFix.Tools.Tool;

namespace FancyFix.Tools.Special
{
    /// <summary>
    /// 该方法提供生成小图方法
    /// 后期img域切换服务器时需要调整，临时方法 维护需要注意
    /// by:willian date:2015-12-11
    /// </summary>
    public class ImageCreate
    {
        public static void CreateSpiderProImgs(string pics)
        {
            foreach (string t in pics.Split(',').ToList<string>())
            {
                CreateSpiderProImg(t);
            }
        }

        public static void CreateSpiderProImg(string pic)
        {
            string imgDir = Common.GetImgSpiderImagDir();
            string imgUrl = Common.GetImgSpiderUrl();
            string waterImgPath = Common.GetWaterImg();

            string fileExt = Path.GetExtension(pic).ToLower();
            string saveFile = pic.Replace(imgUrl, imgDir);

            string newSmallFileName = Utility.Web.GetSmallPic(saveFile);
            string newBigFileName = Utility.Web.GetBigPic(saveFile);
            string newMiddleFileName = Utility.Web.GetMiddlePic(saveFile);

            //生成对应小图
            Tool.ImageTools.CreateSmallImage(saveFile, saveFile, 800, 600);
            Tool.ImageTools.CreateSmallImage(saveFile, newSmallFileName, 260, 240);
            Tool.ImageTools.CreateSmallImage(saveFile, newBigFileName, 580, 480);
            Tool.ImageTools.CreateSmallImage(saveFile, newMiddleFileName, 328, 260);

            //打上水印
            WaterMarkType wmtype = WaterMarkType.ImageMark;
            Tools.Tool.ImageTools.AddWaterMark(saveFile, saveFile, waterImgPath, wmtype, WaterMarkPosition.Right_Bottom, 1);
            Tools.Tool.ImageTools.AddWaterMark(newBigFileName, newBigFileName, waterImgPath, wmtype, WaterMarkPosition.Right_Bottom, 1);

        }
    }
}
