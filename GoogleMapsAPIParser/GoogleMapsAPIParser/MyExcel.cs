using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.ComponentModel;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;


namespace GoogleMapsAPIParser
{
    class MyExcel
    {
        public static string DB_PATH = @"your excel file path here";
        public static List<Clubs> ClubList = new List<Clubs>();
        private static Excel.Workbook MyBook = null;
        private static Excel.Application MyApp = null;
        private static Excel.Worksheet MySheet = null;
        private static int lastRow = 0;

        public MyExcel()
        {}
        public static void InitializeExcel()
        {
            MyApp = new Excel.Application();
            MyApp.Visible = false;
            MyBook = MyApp.Workbooks.Open(DB_PATH);
            MySheet = (Excel.Worksheet)MyBook.Sheets[1]; // Explict cast is not required here
            lastRow = MySheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell).Row;
        }

        public static List<Clubs> ReadMyExcel()
        {
            ClubList.Clear();
            for (int index = 2; index <= lastRow; index++)
            {
                System.Array MyValues = (System.Array) MySheet.get_Range("A" + index.ToString(), "D" + index.ToString()).Cells.Value;
                ClubList.Add(new Clubs
                {
                    ID = MyValues.GetValue(1, 1).ToString(),
                    Name = MyValues.GetValue(1, 2).ToString(),
                    Lat = MyValues.GetValue(1, 3).ToString(),
                    Long = MyValues.GetValue(1, 4).ToString()
                });
            }
            return ClubList;
        }


        public static void CloseExcel()
        {
            MyBook.Saved = true;
            MyApp.Quit();

        }
    
    
    
    
    
    
    
    
    
    
    }
}
