using NSoup;
using NSoup.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FancyFix.Tools.Nsoup
{
    public class NsoupHelper
    {
        Tools.Helper.HttpHelper httpHelper = new Tools.Helper.HttpHelper("UTF-8");

        public Document GetDocument(string url)
        {
            try
            {
                string html = GetHtml(url);
                if (string.IsNullOrEmpty(html))
                    return null;
                if (html.Contains("<title>509 unused</title>") || html.Contains("<h1>unused</h1>") || html.Contains("The server encountered an internal error or misconfiguration and was unable to complete your request."))
                    return null;
                return NSoupClient.Parse(html);
            }
#pragma warning disable CS0168 // 声明了变量“ex”，但从未使用过
            catch (Exception ex)
#pragma warning restore CS0168 // 声明了变量“ex”，但从未使用过
            {
                return null;
            }
        }

        public string GetHtml(string url)
        {
            try
            {
                return httpHelper.Get(url).ResultHtml;
            }
#pragma warning disable CS0168 // 声明了变量“ex”，但从未使用过
            catch (Exception ex)
#pragma warning restore CS0168 // 声明了变量“ex”，但从未使用过
            {
                return "";
            }
        }
    }
}
