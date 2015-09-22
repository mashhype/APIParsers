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

namespace GoogleMapsAPIReverseGeoCoder
{
    class Program
    {
        static void Main(string[] args)
        {
            //so you can send the request over HTTP
            WebClient webClient = new WebClient();

            //Your API variables to send the request
            string base_url = "https://maps.googleapis.com/maps/api/geocode/json?";
            string latlng = null;
            string api_key = "your key here";
            string full_query = null;

            ////read the excel file and place the required columns into a List creating one object per row
            //List<Club> clubs = new List<Club>();
            //MyExcel.InitializeExcel();
            //clubs = MyExcel.ReadMyExcel();
           
            //foreach (var club in clubs)
            //{
            //    //here we want to assemble the request sent to the Google Maps API
            //    latlng = "latlng=" + club.Lat + "," + club.Long + "&";
            //    full_query = webClient.DownloadString(base_url + latlng + api_key);
            //    dynamic json = JsonConvert.DeserializeObject(full_query);
            //    string fileName = club.Competitor_Key + ".txt";

            //    //place each response into a separate file with the filename as the competitor key found in the excel input file
            //    System.IO.File.WriteAllText(@"your path here" + fileName, json.ToString());
            //}

            //loop through the files in the folder you just dropped off the json response files into:
            string[] files = Directory.GetFiles(@"your path here");
            string competitorKey;
            foreach (string file in files)
            {
                try
                {   //open a Streamreader to read the file from beginning to end
                    using (StreamReader sr = new StreamReader(file))
                    {
                        // read the file to the very end and store it as a string
                        String json_response = sr.ReadToEnd();
                        // parse the response into a json object
                        JObject results = JObject.Parse(json_response);
                        // create a json array to store the tokens found in the 'results' object
                        JArray competitor_address = new JArray();
                        // create a string to store the instance of the json array element we want
                        string fullAddress = null;
                        // loop through the different tokens putting each token into 'competitor_address' jarray
                        // assign the address we want to the 'fullAddress' string variable
                        foreach (var result in results["results"])
                        {  
                            competitor_address.Add(result["formatted_address"]);
                            fullAddress = (string) competitor_address[0];
                        }
                        //write 'fullAddress' to a file
                        //the 'using' statement allows you to append to the same file
                        //clear the 'competitor_address' jarray to ensure you grab the
                        //full address of each file only
                        using (System.IO.StreamWriter output =
                                new System.IO.StreamWriter(@"your path here", true))
                        {
                            competitorKey = Path.GetFileName(file);
                            output.WriteLine(competitorKey + "; " + fullAddress + ";");
                            competitor_address.Clear();
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
        }
    }
}
