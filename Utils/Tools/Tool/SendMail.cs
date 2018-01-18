using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Net;

namespace FancyFix.Tools.Tool
{
    public class SendMail
    {
        private SmtpClient sc;
        private string receiveName; //接收方名称
        private string receiveMail; //接收方邮件地址
        private string fromName;    //发送方名称
        private string fromMail;    //发送方邮件地址
        private string subject;     //邮件标题
        private string body;        //邮件内容
        private bool isHtml = true; //邮件内容是否是HTML
        private Encoding encoding = Encoding.GetEncoding("gb2312");  //邮件编码



        /// <summary>
        /// 接收方名称
        /// </summary>
        public string ReceiveName
        {
            get { return receiveName; }
            set { receiveName = value; }
        }
        /// <summary>
        /// 接收方邮件地址
        /// </summary>
        public string ReceiveMail
        {
            get { return receiveMail; }
            set { receiveMail = value; }
        }
        /// <summary>
        /// 发送方名称
        /// </summary>
        public string FromName
        {
            get { return fromName; }
            set { fromName = value; }
        }
        /// <summary>
        /// 发送方邮件地址
        /// </summary>
        public string FromMail
        {
            get { return fromMail; }
            set { fromMail = value; }
        }
        /// <summary>
        /// 邮件标题
        /// </summary>
        public string Subject
        {
            get { return subject; }
            set { subject = value; }
        }
        /// <summary>
        /// 邮件正文
        /// </summary>
        public string Body
        {
            get { return body; }
            set { body = value; }
        }
        /// <summary>
        /// 是否是HTML格式 默认为是
        /// </summary>
        public bool IsHtml
        {
            get { return isHtml; }
            set { isHtml = value; }
        }

        /// <summary>
        /// 邮件编码 默认为GB2312
        /// </summary>
        public Encoding Encoding
        {
            get { return encoding; }
            set { encoding = value; }
        }


        public SendMail(string mailServerAddr, string mailServerUserName, string mailServerPassword)
        {
            sc = new SmtpClient(mailServerAddr);
            sc.Credentials = new NetworkCredential(mailServerUserName, mailServerPassword);
            //sc.DeliveryMethod = SmtpDeliveryMethod.Network;
            sc.DeliveryMethod = SmtpDeliveryMethod.Network;
            //sc.DeliveryMethod = SmtpDeliveryMethod.PickupDirectoryFromIis;
            sc.Timeout = 500000;
        }

        public string Send()
        {
            MailAddress mysendmail = new MailAddress(fromMail, fromName);
            MailAddress myrereivemail = new MailAddress(receiveMail, receiveName);

            MailMessage mm = new MailMessage(mysendmail, myrereivemail);
            mm.Subject = subject;
            mm.SubjectEncoding = encoding;
            mm.Body = body;
            mm.BodyEncoding = encoding;
            mm.IsBodyHtml = IsHtml;
            mm.Priority = MailPriority.Normal;

            try
            {
                sc.Send(mm);
                return "发送成功";
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }

        }


    }
}
