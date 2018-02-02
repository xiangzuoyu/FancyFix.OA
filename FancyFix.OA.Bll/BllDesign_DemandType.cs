using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dos.DataAccess.Base;
using FancyFix.OA.Model;

namespace FancyFix.OA.Bll
{
    public class BllDesign_DemandType : ServiceBase<Design_DemandType>
    {
        public static BllDesign_DemandType Instance()
        {
            return new BllDesign_DemandType();
        }
         
        public static IEnumerable<Design_DemandType> GetList()
        {
            var list = Db.Context.From<Design_DemandType>()
                .Where(o => o.Display !=2)
                .ToList();

            return list;
        }
    }
}
