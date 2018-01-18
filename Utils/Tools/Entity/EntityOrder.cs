using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FancyFix.Tools.Entity
{
    public class EntityOrder
    {

    }

    public class UploadCopy
    {
        public int payId { get; set; }
        public string payer { get; set; }
        public decimal amount { get; set; }
        public byte paymethod { get; set; }
        public DateTime paydate { get; set; }
        public string file { get; set; }
        public string localfile { get; set; }
    }

    public class EntityAddress
    {
        public int Id { get; set; }
        public int MbId { get; set; }
        public string ContactName { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }
        public string ProvinceName { get; set; }
        public string ZipCode { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Tel { get; set; }
        public bool IsDefault { get; set; }
    }
}
