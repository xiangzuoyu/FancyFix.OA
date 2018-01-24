using Dos.DataAccess.Base;
using FancyFix.OA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FancyFix.OA.Bll
{
    public class BllDetailType
    {
        
        public static IEnumerable<DetailType> GetList()
        {
            var list = Db.Context.From<DetailType>()
                .Where(o => o.Display != 2)
                .ToList();

            return list;
        }
    }
}
