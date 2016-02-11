using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;


namespace GooglePlacesAPIParser
{
    class MyExcel
    {
        public static string FILE_PATH = @"your file path goes here";
        public static List<Club> ClubList = new List<Club>();
        private static Excel.Workbook MyBook = null;
        private static Excel.Application MyApp = null;
        private static Excel.Worksheet MySheet = null;
        private static int lastRow = 0;

        //public MyExcel()
        //{ }
        public static void InitializeExcel()
        {
            MyApp = new Excel.Application();
            MyApp.Visible = false;
            MyBook = MyApp.Workbooks.Open(FILE_PATH);
            //Sheets[1] just grabs the first sheet going from left to right
            MySheet = (Excel.Worksheet)MyBook.Sheets[1]; // Explict cast is not required here
            lastRow = MySheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell).Row;
        }

        public static List<Club> ReadMyExcel()
        {
            ClubList.Clear();
            for (int index = 2; index <= lastRow; index++)
            {   //this can be modified based on the columns in your Excel
                System.Array MyValues = (System.Array)MySheet.get_Range("A" + index.ToString(), "D" + index.ToString()).Cells.Value;
                ClubList.Add(new Club
                {
                    name = MyValues.GetValue(1, 1).ToString(),
                    id = MyValues.GetValue(1, 2).ToString(),
                    city = MyValues.GetValue(1,3).ToString(),
                    state = MyValues.GetValue(1,4).ToString()
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
