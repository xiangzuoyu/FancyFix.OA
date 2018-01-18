using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FancyFix.Tools.Entity
{
    public class ConfigUpload
    {
        public string FileExt { get; set; }
        public string MaxSize { get; set; }
        public int UploadNum { get; set; }
        public string Uptype { get; set; }
        public string Key { get; set; }
        public string CheckCode { get; set; }
        public string SuccessFun { get; set; }

        public string Title { get; set; }
    }
}
