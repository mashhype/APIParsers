using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GooglePlacesAPIParser
{
    class FileMethods
    {
        public static List<Club> ClubList = new List<Club>();
        public static List<string> fileList = new List<string>();
        public static List<string> db_tableList = new List<string>();
        // this is a list of the files that received a response
        public static List<string> getResponseList(string pathToFiles)
        {
            try
            {
                //iterate over a folder and return a list of the file names
                string[] files = Directory.GetFiles(pathToFiles);
                foreach (var file in files)
                {
                    fileList.Add(Path.GetFileName(file).Replace(".txt", ""));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return fileList;
        }

        //this will send out the list of requests that didnt get a response
        public static List<string> getResendList(List<string> responseList)
        {
            try
            {
                //take the list of responses and compare it to everything to return the difference
               ClubList = DBMethods.readTable("your sql query goes here", "Server= ", "database= ");

               foreach (var club in ClubList)
               {
                   db_tableList.Add(club.id);
                   //Console.WriteLine(id.ToString());
               }
              fileList = db_tableList.Except(responseList).ToList(); 
            }
            catch (Exception e) 
            {
                Console.WriteLine(e.ToString());
            }
            return fileList;
        }
    }
}
