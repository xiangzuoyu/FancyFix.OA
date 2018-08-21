using FancyFix.Tools.Config;
using FancyFix.Tools.Tool;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace FancyFix.OA.Helper
{
    public class ProdcutImgQueue
    {
        public readonly static ProdcutImgQueue ProdcutImgQueueInstance = new ProdcutImgQueue();
        private ConcurrentQueue<ProductImgQueueInfo> queueList = new ConcurrentQueue<ProductImgQueueInfo>();
        private static Tools.Config.UploadConfig config = UploadProvice.Instance();
        private static SiteOption option = null;
        private static Setting productConfig =null;
        private static string dirPath = string.Empty;
        public void AddQueue(ProductImgQueueInfo queueInfo) //入列
        {
            if (queueInfo != null && !string.IsNullOrWhiteSpace(queueInfo.ImgUrl) && !string.IsNullOrWhiteSpace(queueInfo.ImgPath))
            {
                queueList.Enqueue(queueInfo);
            }
        }
        public void Start()//启动
        {
            option = config.SiteOptions["files"];
            productConfig = config.Settings["product"];
            if (option != null && productConfig != null)
            {
                dirPath = option.Folder.TrimEnd('\\') + productConfig.FilePath;
            }
            Thread thread = new Thread(threadStart);
            thread.Start();
        }

        private void threadStart()
        {

            while (true)
            {
                if (!queueList.IsEmpty && queueList.Count>0)
                {
                    try
                    {
                        while (queueList.Count > 0)
                        {
                            try
                            {
                                ProductImgQueueInfo queueInfo = null;
                                //从队列中取出
                                 queueList.TryDequeue(out queueInfo);
                                if (queueInfo == null)
                                {
                                    LogHelper.Info("队列取出东西为nul");
                                    continue;
                                }
                
                                var ImgPath = queueInfo.ImgPath;
                                var imgurl = queueInfo.ImgUrl;
                                //生成缩略图
                                if (productConfig.CreateMinPic)
                                {
                                    string minPic = string.Concat(dirPath, Tools.Usual.Utils.GetMinPic(imgurl));
                                    ImageTools.CreateSmallImage(ImgPath, minPic, productConfig.MinWidth, productConfig.MinHeight);
                                }
                                //生成小图
                                if (productConfig.CreateSmallPic)
                                {
                                    string smallPic = string.Concat(dirPath, Tools.Usual.Utils.GetSmallPic(imgurl));
                                    ImageTools.CreateSmallImage(ImgPath, smallPic, productConfig.Width, productConfig.Height);
                                }
                                //生成中图
                                if (productConfig.CreateMiddlePic)
                                {
                                    string middlePic = string.Concat(dirPath, Tools.Usual.Utils.GetMiddlePic(imgurl));
                                    ImageTools.CreateSmallImage(ImgPath, middlePic, productConfig.MiddleWidth, productConfig.MiddleHeight);
                                }
                                //生成大图
                                if (productConfig.CreateBigPic)
                                {
                                    string bigPic = string.Concat(dirPath, Tools.Usual.Utils.GetBigPic(imgurl));
                                    ImageTools.CreateSmallImage(ImgPath, bigPic, productConfig.BigWidth, productConfig.BigHeight);
                                }

                            }
                            catch (Exception ex)
                            {
                                LogHelper.Info($"threadStart出问题了,队列数据:{JsonConvert.SerializeObject(queueList)}",ex);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Info($"threadStart出问题了,队列数据:{JsonConvert.SerializeObject(queueList)}", ex);
                    }
                }
                else
                {
                  
                    //没有任务，休息3秒钟
                    Thread.Sleep(3000);
                }
            }
        }

    }
    public class ProductImgQueueInfo
    {

        /// <summary>
        /// 源文件路径
        /// </summary>
        public string ImgPath { get; set; }
        /// <summary>
        /// excel存的地址
        /// </summary>
        public string ImgUrl { get; set; }
    }
}
