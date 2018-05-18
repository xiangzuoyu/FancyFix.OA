using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FancyFix.OA.Model
{
    public partial class Supplier_VendorInfo
    {
        public string Code { set; get; }
        public string Name { set; get; }
        public string MainContactor { set; get; }
        public string TelephoneNumber { set; get; }
        public string FinanceContact { set; get; }
        public string FinanceContactPhone { set; get; }
        public string FinanceContactEmail { set; get; }
        public string AddressOfRegisteredOffice { set; get; }
        public string CompanyWebsiteAddress { set; get; }
    }
}
