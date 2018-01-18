using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FancyFix.Tools.Json.Question
{
    [Serializable]
    public class Reply
    {
        /// <summary>
        /// 题目Id
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 题目名称
        /// </summary>
        public string t { get; set; }
        /// <summary>
        /// 答案
        /// </summary>
        public string a { get; set; }
    }
}
