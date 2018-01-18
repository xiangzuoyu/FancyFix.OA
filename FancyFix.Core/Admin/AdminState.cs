using FancyFix.OA.Bll;
using FancyFix.OA.Model;
using System.Web;

namespace FancyFix.Core.Admin
{
    public class AdminState
    {
        HttpContextBase _context = null;

        public AdminState(HttpContextBase context)
        {
            _context = context;
        }

        public Mng_User GetUserInfo()
        {
            Mng_User model = null;
            int adminId = 0;
            object adminInfo = null;
            //执行登录检测
            if (Tools.Utility.Admin.GetAdminSession(ref adminId, ref adminInfo) && adminId > 0)
            {
                model = adminInfo as Mng_User;
                if (model == null)
                {
                    model = BllMng_User.First(o => o.Id == adminId);
                    Tools.Utility.Admin.SetSession(adminId, model);
                }
            }
            return model;
        }
    }
}
