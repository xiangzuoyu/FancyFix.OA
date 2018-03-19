using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FancyFix.OA.Model;

namespace FancyFix.OA.Bll
{
    public class BllSupplier_RawMaterialPrice:ServiceBase<Supplier_RawMaterialPrice>
    {
        public static BllSupplier_RawMaterialPrice Instance()
        {
            return new BllSupplier_RawMaterialPrice();
        }

    }
}
