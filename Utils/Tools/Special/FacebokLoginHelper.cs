using Facebook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FancyFix.Tools.Special
{
    public class FacebokLoginHelper
    {
        public static Entity.LoginExt AuthGetInfomation(string accessToken)
        {
            try
            {
                if (string.IsNullOrEmpty(accessToken))
                    return null;

                var client = new FacebookClient(accessToken);
                var me = client.Get("me") as IDictionary<string, object>;
                // AddLoginLog(me, 0, "");
                Entity.LoginExt entity = new Entity.LoginExt();
                entity.Id = me["id"].ToString();
                entity.FirstName = !me.ContainsKey("first_name") ? "" : me["first_name"].ToString();
                entity.LastName = !me.ContainsKey("last_name") ? "" : me["last_name"].ToString();
                entity.Sex = !me.ContainsKey("gender") ? "" : me["gender"].ToString();
                entity.Link = !me.ContainsKey("link") ? "" : me["link"].ToString();
                entity.Name = !me.ContainsKey("name") ? "" : me["name"].ToString();
                entity.Email = !me.ContainsKey("email") ? "" : me["email"].ToString();

                return entity;
            }
            catch (Exception ex)
            {
                Tools.Tool.LogHelper.WriteLog(ex);
                return null;
            }
        }
    }
}
