using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace GoogleMapsAPIReverseGeoCoder
{
    class MyExcel
    {
        public static string DB_PATH = @"C:\Users\2031361\Documents\Visual Studio 2013\Projects\GoogleMapsAPIReverseGeoCoder\GoogleMapsAPIReverseGeoCoder\bin\Debug\input\Competitors_ReverseGeocode.xlsm";
        public static List<Club> ClubList = new List<Club>();
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

        public static List<Club> ReadMyExcel()
        {
            ClubList.Clear();
            for (int index = 2; index <= lastRow; index++)
            {
                System.Array MyValues = (System.Array) MySheet.get_Range("A" + index.ToString(), "G" + index.ToString()).Cells.Value;
                ClubList.Add(new Club
                {
                    Club_ID = MyValues.GetValue(1, 1).ToString(),
                    Lat = MyValues.GetValue(1, 2).ToString(),
                    Long = MyValues.GetValue(1, 3).ToString(),
                    Competitor_Key = MyValues.GetValue(1, 7).ToString()
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
