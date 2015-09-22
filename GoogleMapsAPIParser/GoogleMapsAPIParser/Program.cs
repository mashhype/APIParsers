using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Data;
using Excel = Microsoft.Office.Interop.Excel;


namespace GoogleMapsAPIParser
{
    class Program
    {
        static void Main(string[] args)
        {
            //so you can send the request over HTTP
            WebClient webClient = new WebClient();

            //Your API variables to send the request
            string base_url = "https://maps.googleapis.com/maps/api/place/nearbysearch/json?";
            string location = null;
            string radius = "radius=50000&";  //in meters
            string types = "types=gym&"; //this can be used for any TYPE of place

            string api_key = "your key here";
            string full_query = null;
            string clubid = null;

            //testing output to console below
            //Console.WriteLine("Google API Sample Response -------------------------------- ");
            //Console.WriteLine(base_url + location + radius + types + api_key);
            //Console.WriteLine(json);
            //write the resulting json response to a text file
            //System.IO.File.WriteAllText(@"your path here", json.ToString());

            List<Clubs> clubs = new List<Clubs>();
            MyExcel.InitializeExcel();
            clubs = MyExcel.ReadMyExcel();

            foreach (var club in clubs)
            {
                //here we want to assemble the request sent to the Google Maps API
                location = "location=" + club.Lat + "," + club.Long + "&";
                full_query = webClient.DownloadString(base_url + location + radius + types + api_key);

                //deserialize the response 
                dynamic json = JsonConvert.DeserializeObject(full_query);

                //save the filename as the clubid
                clubid = club.Club_ID + ".txt";
                //save each file to the designated directory path
                System.IO.File.WriteAllText(@"your path here" + clubid, json.ToString());

            }

            //loop through the files in the folder you just dropped off the json response files into:
            string[] files = Directory.GetFiles(@"your path here");
            string clubName;
            foreach (string fileName in files)
            {
                
                try
                {   //open a Streamreader to read the file from beginning to end
                    using (StreamReader sr = new StreamReader(fileName))
                    {
                        String json_response = sr.ReadToEnd();
                        JObject results = JObject.Parse(json_response);

                        //parse out the results array which hold the elements we want
                        foreach (var result in results["results"])
                        {   
                            //add the filename to each row to identify the one to many relationship between our club and competitor clubs
                            clubName = Path.GetFileName(fileName);
                            
                            //this line below allows us to append rows to the same file
                            using (System.IO.StreamWriter output = 
                                new System.IO.StreamWriter(@"your path here", true))
                            {
                                output.WriteLine(clubName + "; " + result["geometry"]["location"]["lat"] + "; " + 
                                    result["geometry"]["location"]["lng"] + "; " + result["name"] + "; " + result["vicinity"]);
                                
                            }
                            //make sure our output is correct
                            //Console.WriteLine("Location: " + result["geometry"]["location"]);
                            //Console.WriteLine("Competitor Name: " + result["name"]);
                            //Console.WriteLine("Address: " + result["vicinity"]);
                        }
                    }

                }
                catch (Exception e)
                {
                    // Let the user know what went wrong.
                    Console.WriteLine("The file could not be read:");
                    Console.WriteLine(e.Message);
                }
            }

            //try
            //{
            //    using (StreamReader sr = new StreamReader("test.txt"))
            //    {
            //        String json_response = sr.ReadToEnd();
            //        JObject results = JObject.Parse(json_response);

            //        foreach (var result in results["results"])
            //        {
            //            Console.WriteLine("Location: " + result["geometry"]["location"]);
            //            Console.WriteLine("Competitor Name: " + result["name"]);
            //            Console.WriteLine("Address: " + result["vicinity"]);
            //        }
            //    }

            //}
            //catch (Exception e)
            //{
            //    // Let the user know what went wrong.
            //    Console.WriteLine("The file could not be read:");
            //    Console.WriteLine(e.Message);
            //}
        }
    }
}
