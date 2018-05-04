using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FancyFix.Tools.Tool
{
    public class Tables
    {
        private int colnum = 0;
        private StringBuilder table = new StringBuilder();
        private StringBuilder temp = new StringBuilder();

        public Tables()
        {
            this.table.AppendLine("     <table class=\"layui-table\" >");
        }

        public void AddHead(List<string> heads)
        {
            this.temp.AppendFormat($"       <thead>");
            foreach (var item in heads)
            {
                this.temp.AppendFormat($"     <th >{item}</th>\n");
            }
            this.temp.AppendFormat($"     </thead>");
            this.AddRow();
        }

        public void AddHeadCol(string value, string style = "")
        {
            this.temp.Append($"     <th");
            if (!string.IsNullOrEmpty(style))
                this.temp.Append($" style=\"{style}\" ");
            this.temp.Append($"  \">{value}</th>\n");

            this.colnum++;
        }
        public void AddHeadRow()
        {
            this.table.Append("     <thead>");
            this.table.Append(this.temp.ToString());
            this.table.Append("     </thead>");

            this.temp.Remove(0, this.temp.Length);
        }

        public void AddCol(string value, string style = "")
        {
            this.temp.Append($"     <td");
            if (!string.IsNullOrEmpty(style))
                this.temp.Append($" style=\"{style}\" ");
            this.temp.Append($"  \">{(string.IsNullOrEmpty(value) ? "&nbsp;" : value)}</td>\n");
        }

        public void AddRow()
        {
            this.table.AppendLine("  <tr>");
            this.table.AppendLine(this.temp.ToString());
            this.table.AppendLine("  </tr>");
            this.temp.Remove(0, this.temp.Length);
        }
        public string GetTable()
        {
            this.table.AppendLine("    </table>");
            string tmp = this.table.ToString();
            this.temp = null;
            this.table = null;
            return tmp;
        }

        /// <summary>
        /// 合并行
        /// </summary>
        /// <param name="value"></param>
        /// <param name="colNum"></param>
        public void AddSpanCol(string value, int colNum)
        {
            this.temp.AppendFormat("    <td colspan='{0}'>{1}</td>\n", colNum, string.IsNullOrEmpty(value) ? "&nbsp" : value);
        }
    }
}
