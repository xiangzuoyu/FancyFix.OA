using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Text.RegularExpressions;

namespace FancyFix.Tools
{
    /// <summary>
    /// 安全的在页面中输出各种内容
    /// 包括过滤xss脚本
    /// </summary>
    public class SafeEcho
    {
        /// <summary>
        /// 输出input的value
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public static string Input(string value)
        {
            return Attr(value);
        }

        /// <summary>
        /// 输出img的src
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static string Img(string src)
        {
            return Attr(src);
        }

        /// <summary>
        /// 在textarea中输出内容
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string TextArea(string content)
        {
            return HttpUtility.HtmlEncode(content);
        }

        /// <summary>
        /// 去除2个以上的空格
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string RemoveMoreSpace(string content)
        {
            if (string.IsNullOrEmpty(content)) return "";
            Regex r = new Regex(@"\s{2,}", RegexOptions.Multiline);
            return r.Replace(content, " ");
        }
        /// <summary>
        /// 在页面输出文本内容
        /// 会将\n\r转换成br,\t转换成4个nbsp
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Text(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }
            StringBuilder builder1 = new StringBuilder();
            builder1.Append(str);
            //builder1.Replace("&", "&amp;");
            builder1.Replace("<", "&lt;");
            builder1.Replace(">", "&gt;");
            builder1.Replace("\"", "&quot;");
            builder1.Replace("\n", "<br/>");
            //builder1.Replace(" ", "&nbsp;");
            builder1.Replace("\t", "&nbsp;&nbsp;&nbsp;&nbsp;");
            return builder1.ToString();
        }

        /// <summary>
        /// 输出动态html代码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Html(string str)
        {
            str = FilterXss2(str);
            return str;
        }

        /// <summary>
        /// 输出url
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string Url(string url)
        {
            url = HttpUtility.UrlEncode(url) ;
            return url;
        }

        /// <summary>
        /// 输出Title
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Title(string str)
        {
            return Meta(str);
        }

        /// <summary>
        /// 输出Title
        /// </summary>
        /// <param name="str"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string Title(string str,int len)
        {
            return Meta(str,len);
        }

        /// <summary>
        /// 输出Meta
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Meta(string str)
        {
            str = str.Replace("\r", " ");
            str = str.Replace("\n", " ");
            str = str.Replace("\t", " ");
            str = str.Replace("\"", "");
            str = str.Replace("&nbsp;", " ");
            str = RemoveMoreSpace(str);
            return FancyFix.Tools.Usual.Common.RemoveHtml(str);
        }

        /// <summary>
        /// 输出Meta
        /// </summary>
        /// <param name="str"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string Meta(string str,int len)
        {
            str = Meta(str);
            return FancyFix.Tools.Usual.Common.CutString(str, len);
        }

        /// <summary>
        /// 输出js脚本
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        //public static string Js(string code)
        //{
        //    return code;
        //}

        private static string Attr(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return "";
            }
            value = value.Replace("\r", "");
            value = value.Replace("\n", "");
            value = HttpUtility.HtmlEncode(value);
            return value;
        }

        #region xss
        private static string FilterXss(string html)
        {
            if (string.IsNullOrEmpty(html)) return string.Empty;

            //把可能的16进制转回来
            Regex r16 = new Regex("&#[x|X]0{0,}(\\d*);?", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            string ret = r16.Replace(html, new MatchEvaluator(R16CapText));

            //去掉标签中的换行
            //Regex reg = new Regex("=[\\s]*?('|\")(.*?)(\\1)", RegexOptions.IgnoreCase|RegexOptions.Singleline);
            Regex reg = new Regex("<[^>]*?>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            Match m = reg.Match(ret);
            ret = reg.Replace(ret, new MatchEvaluator(CapText));

            // CR(0a) ，LF(0b) ，TAB(9) 除外，过滤掉所有的不打印出来字符.    
            // 目的防止这样形式的入侵 ＜java\0script＞    
            // 注意：\n, \r,  \t 可能需要单独处理，因为可能会要用到    
            ret = Regex.Replace(ret, "([\x00-\x08][\x0b-\x0c][\x0e-\x20])", string.Empty);

            //替换属性中的

            //过滤\t, \n, \r构建的恶意代码  
            string[] keywords = {"javascript", "vbscript", "expression",
                "applet", "meta", "xml", "blink", "link", "style",
                "script", "embed", "object", "iframe", "frame",
                "frameset", "ilayer", "layer", "bgsound", "title",
                "base" ,"onabort", "onactivate", "onafterprint",
                "onafterupdate", "onbeforeactivate", "onbeforecopy",
                "onbeforecut", "onbeforedeactivate", "onbeforeeditfocus",
                "onbeforepaste", "onbeforeprint", "onbeforeunload",
                "onbeforeupdate", "onblur", "onbounce", "oncellchange",
                "onchange", "onclick", "oncontextmenu", "oncontrolselect",
                "oncopy", "oncut", "ondataavailable", "ondatasetchanged",
                "ondatasetcomplete", "ondblclick", "ondeactivate",
                "ondrag", "ondragend", "ondragenter", "ondragleave",
                "ondragover", "ondragstart", "ondrop", "onerror",
                "onerrorupdate", "onfilterchange", "onfinish",
                "onfocus", "onfocusin", "onfocusout", "onhelp",
                "onkeydown", "onkeypress", "onkeyup", "onlayoutcomplete",
                "onload", "onlosecapture", "onmousedown", "onmouseenter",
                "onmouseleave", "onmousemove", "onmouseout", "onmouseover",
                "onmouseup", "onmousewheel", "onmove", "onmoveend",
                "onmovestart", "onpaste", "onpropertychange",
                "onreadystatechange", "onreset", "onresize",
                "onresizeend", "onresizestart", "onrowenter",
                "onrowexit", "onrowsdelete", "onrowsinserted",
                "onscroll", "onselect", "onselectionchange",
                "onselectstart", "onstart", "onstop", "onsubmit",
                "onunload"};

            bool found = true;
            while (found)
            {
                var retBefore = ret;
                for (int i = 0; i < keywords.Length; i++)
                {
                    string pattern = "/";
                    for (int j = 0; j < keywords[i].Length; j++)
                    {
                        if (j > 0)
                            pattern = string.Concat(pattern,
                                '(', "(&#[x|X]0{0,8}([9][a][b]);?)?",
                                "|(&#0{0,8}([9][10][13]);?)?",
                                ")?");
                        pattern = string.Concat(pattern, keywords[i][j]);
                    }
                    string replacement =
                        string.Concat(keywords[i].Substring(0, 2),
                            "xx", keywords[i].Substring(2));
                    ret =
                        System.Text.RegularExpressions.Regex.Replace(ret,
                            pattern, replacement,
                            System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                    if (ret == retBefore)
                        found = false;
                }

            }
            return ret;
        }

        private static string FilterXss2(string html)
        {
            if (string.IsNullOrEmpty(html)) return string.Empty;

            //把可能的16进制转回来
            Regex r16 = new Regex("&#[x|X]0{0,}(\\d*);?", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            string ret = r16.Replace(html, new MatchEvaluator(R16CapText));

            //去掉标签中的换行
            //Regex reg = new Regex("=[\\s]*?('|\")(.*?)(\\1)", RegexOptions.IgnoreCase|RegexOptions.Singleline);
            Regex reg = new Regex("<[^>]*?>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            Match m = reg.Match(ret);
            ret = reg.Replace(ret, new MatchEvaluator(CapText));

            // CR(0a) ，LF(0b) ，TAB(9) 除外，过滤掉所有的不打印出来字符.    
            // 目的防止这样形式的入侵 ＜java\0script＞    
            // 注意：\n, \r,  \t 可能需要单独处理，因为可能会要用到    
            ret = Regex.Replace(ret, "([\x00-\x08][\x0b-\x0c][\x0e-\x20])", string.Empty);

            string[] tags = { "script", "link", "style", "meta", "frameset", "iframe", "frame", "embed", "object", "xml", "html", "body" };
            string[] props = { "javascript", "vbscript", "expression",
                "applet", "blink", 
                "script", "ilayer", "layer", "bgsound", "title",
                "base" ,"onabort", "onactivate", "onafterprint",
                "onafterupdate", "onbeforeactivate", "onbeforecopy",
                "onbeforecut", "onbeforedeactivate", "onbeforeeditfocus",
                "onbeforepaste", "onbeforeprint", "onbeforeunload",
                "onbeforeupdate", "onblur", "onbounce", "oncellchange",
                "onchange", "onclick", "oncontextmenu", "oncontrolselect",
                "oncopy", "oncut", "ondataavailable", "ondatasetchanged",
                "ondatasetcomplete", "ondblclick", "ondeactivate",
                "ondrag", "ondragend", "ondragenter", "ondragleave",
                "ondragover", "ondragstart", "ondrop", "onerror",
                "onerrorupdate", "onfilterchange", "onfinish",
                "onfocus", "onfocusin", "onfocusout", "onhelp",
                "onkeydown", "onkeypress", "onkeyup", "onlayoutcomplete",
                "onload", "onlosecapture", "onmousedown", "onmouseenter",
                "onmouseleave", "onmousemove", "onmouseout", "onmouseover",
                "onmouseup", "onmousewheel", "onmove", "onmoveend",
                "onmovestart", "onpaste", "onpropertychange",
                "onreadystatechange", "onreset", "onresize",
                "onresizeend", "onresizestart", "onrowenter",
                "onrowexit", "onrowsdelete", "onrowsinserted",
                "onscroll", "onselect", "onselectionchange",
                "onselectstart", "onstart", "onstop", "onsubmit",
                "onunload"};

            //remove validation tags
            foreach (string tag in tags)
            {
                Regex regTag = new Regex("<("+tag+")[^>]*?>[\\s\\S]*?</\\1>", RegexOptions.IgnoreCase);
                ret= regTag.Replace(ret, "");
            }

            foreach (string prop in props)
            {
                ret = ret.Replace(prop, prop + "_");
            }
            return ret;
        }

        private static string CapText(Match m)
        {
            return new Regex("[\r\n\t]", RegexOptions.IgnoreCase | RegexOptions.Singleline).Replace(m.Groups[0].Value, "");
        }

        private static string R16CapText(Match m)
        {
            return ((char)Convert.ToInt32(m.Groups[1].Value)).ToString();
        }
        #endregion
    }
}
