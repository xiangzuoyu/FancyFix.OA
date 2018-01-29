using Dos.DataAccess.Base;
using FancyFix.OA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FancyFix.OA.Bll
{
    public class BllDesign_DetailType : ServiceBase<Design_DetailType>
    {
        public static BllDesign_DetailType Instance()
        {
            return new BllDesign_DetailType();
        }

        public static IEnumerable<Design_DetailType> GetList()
        {
            var list = Db.Context.From<Design_DetailType>()
                .Where(o => o.Display != 2)
                .ToList();

            return list;
        }
    }
}
