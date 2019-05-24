using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbFillingTool
{
    public static class Extensions
    {
        public static ExcelFileRow[] DeleteNulls(this List<ExcelFileRow> list)
        {
            return list.Where(r => r.Name != null).ToArray();
        }
    }
}
