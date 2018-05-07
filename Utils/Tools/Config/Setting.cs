using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace FancyFix.Tools.Config
{
    public class Setting : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get
            {
                return this["name"] as string;
            }
        }


        /// <summary>
        /// 文件路径
        /// </summary>
        [ConfigurationProperty("filePath", IsRequired = true)]
        public string FilePath
        {
            get
            {
                return this["filePath"] as string;
            }
        }



        /// <summary>
        /// (针对编辑器有效webeditor)上传文件后生成的路径前缀
        /// </summary>
        [ConfigurationProperty("urlFilePath", IsRequired = true)]
        public string UrlFilePath
        {
            get
            {
                return this["urlFilePath"] as string;
            }
        }


        /// <summary>
        /// 允许上传文件,例：jpg|jpeg|gif
        /// </summary>
        [ConfigurationProperty("allowUpload", IsRequired = true)]
        public string AllowUpload
        {
            get
            {
                return this["allowUpload"] as string;
            }
        }

        /// <summary>
        /// 允许最大上载文件
        /// </summary>
        [ConfigurationProperty("maxFileSize", IsRequired = true)]
        public int MaxFileSize
        {
            get
            {
                return int.Parse(this["maxFileSize"].ToString());
            }
        }

        /// <summary>
        /// 是否生成小图
        /// </summary>
        [ConfigurationProperty("createMinPic", IsRequired = false)]
        public bool CreateMinPic
        {
            get
            {
                return bool.Parse(this["createMinPic"].ToString());
            }
        }

        /// <summary>
        /// 是否生成小图
        /// </summary>
        [ConfigurationProperty("createSmallPic", IsRequired = true)]
        public bool CreateSmallPic
        {
            get
            {
                return bool.Parse(this["createSmallPic"].ToString());
            }
        }

        /// <summary>
        /// 是否生成大图
        /// </summary>
        [ConfigurationProperty("createBigPic", IsRequired = false)]
        public bool CreateBigPic
        {
            get
            {
                return bool.Parse(this["createBigPic"].ToString());
            }
        }

        /// <summary>
        /// 是否生成中图
        /// </summary>
        [ConfigurationProperty("createMiddlePic", IsRequired = false)]
        public bool CreateMiddlePic
        {
            get
            {
                return bool.Parse(this["createMiddlePic"].ToString());
            }
        }
        /// <summary>
        /// 生成小图的宽
        /// </summary>
        [ConfigurationProperty("width", IsRequired = true)]
        public int Width
        {
            get
            {
                return int.Parse(this["width"].ToString());
            }
        }

        /// <summary>
        /// 生成小图的高
        /// </summary>
        [ConfigurationProperty("height", IsRequired = true)]
        public int Height
        {
            get
            {
                return int.Parse(this["height"].ToString());
            }
        }

        /// <summary>
        /// 生成大图的宽
        /// </summary>
        [ConfigurationProperty("bigWidth", IsRequired = false)]
        public int BigWidth
        {
            get
            {
                return int.Parse(this["bigWidth"].ToString());
            }
        }

        /// <summary>
        /// 生成大图的高
        /// </summary>
        [ConfigurationProperty("bigHeight", IsRequired = false)]
        public int BigHeight
        {
            get
            {
                return int.Parse(this["bigHeight"].ToString());
            }
        }

        /// <summary>
        /// 生成中图的宽
        /// </summary>
        [ConfigurationProperty("middleWidth", IsRequired = false)]
        public int MiddleWidth
        {
            get
            {
                return int.Parse(this["middleWidth"].ToString());
            }
        }

        /// <summary>
        /// 生成中图的高
        /// </summary>
        [ConfigurationProperty("middleHeight", IsRequired = false)]
        public int MiddleHeight
        {
            get
            {
                return int.Parse(this["middleHeight"].ToString());
            }
        }

        /// <summary>
        /// 生成缩略图的宽
        /// </summary>
        [ConfigurationProperty("minWidth", IsRequired = false)]
        public int MinWidth
        {
            get
            {
                return int.Parse(this["minWidth"].ToString());
            }
        }

        /// <summary>
        /// 生成缩略图的高
        /// </summary>
        [ConfigurationProperty("minHeight", IsRequired = false)]
        public int MinHeight
        {
            get
            {
                return int.Parse(this["minHeight"].ToString());
            }
        }

        /// <summary>
        /// 上传图片最大宽度
        /// </summary>
        [ConfigurationProperty("maxWidth", IsRequired = false)]
        public int MaxWidth
        {
            get
            {
                return int.Parse(this["maxWidth"].ToString());
            }
        }

        /// <summary>
        /// 上传图片最大高度
        /// </summary>
        [ConfigurationProperty("maxHeight", IsRequired = false)]
        public int MaxHeight
        {
            get
            {
                return int.Parse(this["maxHeight"].ToString());
            }
        } 

        /// <summary>
        /// 是否打水印
        /// </summary>
        [ConfigurationProperty("addWaterMark", IsRequired = true)]
        public bool AddWaterMark
        {
            get
            {
                return bool.Parse(this["addWaterMark"].ToString());
            }
        }


        /// <summary>
        /// 水印类型
        /// </summary>
        [ConfigurationProperty("waterMarkType", IsRequired = true)]
        public string WaterMarkType
        {
            get
            {
                return this["waterMarkType"] as string;
            }
        }

        /// <summary>
        /// 水印类型
        /// </summary>
        [ConfigurationProperty("waterMarkImgOrTxt", IsRequired = true)]
        public string WaterMarkImgOrTxt
        {
            get
            {
                return this["waterMarkImgOrTxt"] as string;
            }
        }

        /// <summary>
        /// 水印透明度
        /// </summary>
        [ConfigurationProperty("transparency", IsRequired = true)]
        public float Transparency
        {
            get
            {
                return float.Parse(this["transparency"].ToString());
            }
        }



    }
}
