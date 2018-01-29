using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dos.DataAccess.Base;
using FancyFix.OA.Model;

namespace FancyFix.OA.Bll
{
    public class BllDemandType
    {
        public static BllArtTaskList Instance()
        {
            return new BllArtTaskList();
        }
         
        public static IEnumerable<DemandType> GetList()
        {
            var list = Db.Context.From<DemandType>()
                .Where(o => o.Display !=2)
                .ToList();

            return list;
        }
    }
}
