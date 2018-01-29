using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FancyFix.Tools.Enums
{
    public class Site
    {
        public enum InputType : byte
        {
            [Description("单选")]
            Radio = 1,

            [Description("文本")]
            Text = 2,

            [Description("多选")]
            CheckBox = 3
        }
    }
}
