using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Excel = Microsoft.Office.Interop.Excel;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;

namespace OracleXMLEditor
{
    internal class FindLastValue
    {

        public int totalRows { get; set; }
        public int totalColumns { get; set; } = 2;


        public int rows(Excel.Worksheet wks)
        {
            // Find the last real row
            var lastUsedRow = wks.Cells.Find("*", System.Reflection.Missing.Value,
                                           System.Reflection.Missing.Value, System.Reflection.Missing.Value,
                                           Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlPrevious,
                                           false, System.Reflection.Missing.Value, System.Reflection.Missing.Value).Row;


            return lastUsedRow;
        }

        public int collums(Excel.Worksheet wks)
        {
            // Find the last real row
            var lastUsedColumn = wks.Cells.Find("*", System.Reflection.Missing.Value,
                               System.Reflection.Missing.Value, System.Reflection.Missing.Value,
                               Excel.XlSearchOrder.xlByColumns, Excel.XlSearchDirection.xlPrevious,
                               false, System.Reflection.Missing.Value, System.Reflection.Missing.Value).Column;


            return lastUsedColumn;
        }


    }
}
