using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Tools.Tool;

namespace Tools.Tool
{
    public static class SmsHelper
    {
        /// <summary>
        /// 添加发送短信
        /// </summary>
        public static bool Send(string note, string tel)
        {
            try
            {
                if (IsCanSendMobile(tel))
                {

                    WKSms.smsSoapClient sms = new WKSms.smsSoapClient();
                    note += "【捷配网】";
                    bool isSend = false;
                    string key = "]2GBW6w[w#z-]YV$%]:uxz{>mc&7dao#uY1Z.8";
                    byte projectType = 220;
                    byte smsType = 1;
                    sms.Open();
                    isSend = sms.AddSms(key, tel, note, projectType, smsType);
                    sms.Close();
                    return isSend;
                }
                return false;
            }
            catch (Exception ex)
            {
                Log.WritePurWeb(ex.ToString());
                return false;
            }
        }
        public static bool IsCanSendMobile(string checkStr)
        {
            if (string.IsNullOrEmpty(checkStr)) { return false; }
            return Regex.IsMatch(checkStr, @"^1\d{10}(\/1\d{10}){0,2}$");
        }
    }
}
