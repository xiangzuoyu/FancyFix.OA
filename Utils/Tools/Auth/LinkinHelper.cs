using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FancyFix.Tools.Auth
{
    public class LinkinHelper : AuthHelper
    {
        public static Tools.Entity.LoginExt GetXmlData(string xml)
        {
            try
            {
                Tools.Entity.LoginExt entity = new Entity.LoginExt();
                XmlNode noteList = Utility.XMLHelper.GetXmlNodeByXpathFromStr(xml, "//person");
                string notePerson = noteList.InnerXml;
                entity.Id = noteList.ChildNodes[0].InnerText;
                entity.FirstName = noteList.ChildNodes[1].InnerText;
                entity.LastName = noteList.ChildNodes[2].InnerText;
                entity.HeadLine = noteList.ChildNodes[3].InnerText;
                entity.Link = noteList.ChildNodes[4].InnerText;
                return entity;
            }
            catch (Exception ex)
            {
                Tools.Tool.LogHelper.WriteLog(ex);
            }
            return null;

        }

        /// <summary>
        /// API获取令牌
        /// by:willian date:2016-11-18
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static AuthToken GetToken(string code, string returnUrl = LinkinConfig.ReturnUrl)
        {
            return GetToken(code, LinkinConfig.TokenUrl, LinkinConfig.ClientId, LinkinConfig.ClientSecret, returnUrl);
        }

        /// <summary>
        /// 获取个人资料
        /// by:willian date:2016-11-18
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public static Entity.LoginExt GetProFile(string accessToken)
        {
            var profile = GetProFileAuth(accessToken, LinkinConfig.ProfileResourceUrl);
            Entity.LoginExt entity = GetXmlData(profile);
            return entity;
        }
    }
}
