using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FancyFix.Tools.Enums
{
    public class Attr
    {
        /// <summary>
        /// 产品颜色
        /// </summary>
        public enum Colors : byte
        {
            [Description("white")]
            white = 1,

            [Description("black")]
            black = 2,

            [Description("red")]
            red = 3,

            [Description("green")]
            green = 4,

            [Description("orange")]
            orange = 5,

            [Description("gray")]
            gray = 6,

            [Description("pink")]
            pink = 7,

            [Description("yellow")]
            yellow = 8,

            [Description("blue")]
            blue = 9,

            [Description("purple")]
            purple = 10,

            [Description("beige")]
            beige = 11,

            [Description("brown")]
            brown = 12
        }
    }
}
