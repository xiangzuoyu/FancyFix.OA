using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FancyFix.Tools.Utility
{
    public class SSOJson
    {
        [Serializable]
        public class Attr
        {
            private string _mb_IsLogin = string.Empty;
            private string _mb_KeyInfo = string.Empty;
            private string _mb_User = string.Empty;

            public string Mb_IsLogin
            {
                set { _mb_IsLogin = value.Replace("\"", "&quot;"); }
                get { return _mb_IsLogin; }
            }


            public string Mb_KeyInfo
            {
                set { _mb_KeyInfo = value.Replace("\"", "&quot;"); }
                get { return _mb_KeyInfo; }
            }

            public string Mb_User
            {
                set { _mb_User = value.Replace("\"", "&quot;"); }
                get { return _mb_User; }
            }

          
        }

        /// <summary>
        /// 属性
        /// </summary>
        [Serializable]
        public class AttrList
        {
            public List<Attr> List { set; get; }
            public AttrList()
            {
                List = new List<Attr>();
            }
        }

    }
}
