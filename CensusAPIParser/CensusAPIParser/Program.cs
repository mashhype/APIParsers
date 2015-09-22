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

namespace CensusAPIParser
{
    class Program
    {
        static void Main(string[] args)
        {
            //WebClient c = new WebClient();
            //string key = "put your api key here";
            //var data = c.DownloadString("http://api.census.gov/data/2013/acs5/profile?get=NAME,DP02_0065E&for=zip+code+tabulation+area:90815&" + key);
            
            //deserialize the JSON "data" above
            //dynamic json = JsonConvert.DeserializeObject(data);

            try
            {
                // Create an instance of StreamReader to read from a file.
                // The using statement also closes the StreamReader.
                using (StreamReader sr = new StreamReader("data_profile_variables.json"))
                {
                    String json = sr.ReadToEnd();
                    Dictionary<string, JObject> data = JsonConvert.DeserializeObject<Dictionary<string, JObject>>(json);
                    String csv = String.Join(Environment.NewLine, data.Select(d => d.Key + ";" + d.Value["label"] + ";"));
                    System.IO.File.WriteAllText("json_parsed.txt", csv);
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
