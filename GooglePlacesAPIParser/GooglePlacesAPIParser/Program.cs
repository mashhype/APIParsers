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
using System.Threading;


namespace GooglePlacesAPIParser
{
    class Program
    {
        static void Main(string[] args)
        {
            //sql_query = "your db query goes here"
            //List<string> responseList = new List<string>();
            //List<string> resendList = new List<string>();
            //responseList = FileMethods.getResponseList(@"your path to output folder goes here");
            //resendList = FileMethods.getResendList(responseList);

            //List<Club> clubsInput = new List<Club>();
            //clubsInput = DBMethods.readTableInClause(sql_resend_query, "Server=server name here", "database=db name here", resendList);

            //so you can send the request over HTTP
            WebClient webClient = new WebClient();

            //Your API variables to send the request
            string ws_base_url = "https://maps.googleapis.com/maps/api/place/textsearch/json?query=";
            string ws_text_search = null;
            //string ws_competitor_name = null;
            string ws_api_key = " ";
            string ws_full_query = null;

            //SQL connection string params
            //string sql_query = " "

            ////fetch the data needed to send to the web service and put it in a list

            //clubsInput = DBMethods.readTable(sql_query, "Server= ", "database= ");
            ////DBMethods.writeTableTest("INSERT INTO dbo.Test (ID, Name, Address, City, State) VALUES(@ID, @Name, @Address, @City, @State)", clubsInput, @"Server= ", "database= ");

            //foreach (var club in clubsInput)
            //{
            //    Console.WriteLine("Club ID: " + club.id + "Club Description: " + club.name + "Club Address: " + club.address + "Club City: " + club.city + "Club State: " + club.state);
            //}

            // Read the excel file and place the required columns into a List creating one object per row
            //List<Club> clubsInput = new List<Club>();
            //MyExcel.InitializeExcel();
            //clubsInput = MyExcel.ReadMyExcel();
            ////close the Excel file else the program holds on to it and doesnt allow it to be opened by the user
            //MyExcel.CloseExcel();

            //foreach (var club in clubsInput)
            //{
            //    //here we want to assemble the request sent to the Google Maps API
            //    //ws_competitor_name = " ";
            //    ws_text_search = club.name + club.city + club.state + "&";
            //    //pause for 2 seconds to make sure we stay under rate limit imposed by Google
            //    Thread.Sleep(2000);
            //    ws_full_query = webClient.DownloadString(ws_base_url + ws_text_search + ws_api_key);
            //    dynamic json = JsonConvert.DeserializeObject(ws_full_query);
            //    string fileName = club.name + "_" + club.id + ".txt";

            //    //place each response into a separate file with the filename as the competitor key found in the input source
            //    System.IO.File.WriteAllText(@"your path to output folder goes here" + fileName, json.ToString());
            //}

            //loop through the files in the folder you just dropped off the json response files into:
            string[] directory = { "folder names go here" };
            for (int i = 0; i < directory.Length; i++ )
            {
                string[] files = Directory.GetFiles(@"your path to output folder goes here" + directory[i]);
                List<Club> clubsOutput = new List<Club>();
                string sourceFileName;
                string destFileName;
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
                            // based on the status we want to put in a separate folder for resend
                            JToken jtoken_status = results["status"];

                            if (jtoken_status.ToString().Equals("ZERO_RESULTS") || jtoken_status.ToString().Equals("REQUEST_DENIED"))
                            {
                                //move the file to the resend folder
                                sourceFileName = @"your path to folder goes here" + directory[i] + @"\" + Path.GetFileName(file);
                                destFileName = @"your path to folder goes here" + directory[i] + @"\resend\" + Path.GetFileName(file);
                                sr.Close();
                                File.Move(sourceFileName, destFileName);
                            }
                            else
                            {
                                foreach (var result in results["results"])
                                {
                                    //we only want to grab the json values that matches a particular competitor name
                                    JToken jtoken_name = result["name"];
                                    JToken jtoken_perm_closed = result["permanently_closed"];
                                    JToken jtoken_rating = result["rating"];
                                    if (jtoken_name.ToString().Equals(directory[i]))
                                    {
                                        if (jtoken_perm_closed == null)
                                        {
                                            jtoken_perm_closed = "open";
                                        }
                                        else
                                        {
                                            jtoken_perm_closed = "closed";
                                        }
                                        //need to validate whether the "rating" property exists in the result object
                                        if (jtoken_rating == null)
                                        {
                                            jtoken_rating = "N/A";
                                        }
                                        else
                                        {
                                            jtoken_rating = result["rating"];
                                        }

                                        clubsOutput.Add(new Club
                                       {
                                           id = Path.GetFileName(file).Replace(".txt", ""),
                                           name = result["name"].ToString(),
                                           address = result["formatted_address"].ToString(),
                                           latitude = result["geometry"]["location"]["lat"].ToString(),
                                           longitude = result["geometry"]["location"]["lng"].ToString(),
                                           rating = jtoken_rating.ToString(),
                                           status = jtoken_perm_closed.ToString()
                                       });
                                    }
                                }
                            }

                            //write clubsOutput to a table in SQL Server
                            DBMethods.writeTable("INSERT INTO dbo.Ratings2(ClubID, Rating, Address, ClubName, ClubStatus, ClubLat, ClubLong) VALUES(@ClubID, @Rating, @Address, @Name, @Status, @ClubLat, @ClubLong)", clubsOutput, @"Server=", "database=");

                            //using (System.IO.StreamWriter output =
                            //        new System.IO.StreamWriter(@"your path to output folder goes here" + directory[i] + ".txt", true))
                            //{
                            //    //here we need to iterate over the clubsOutput List and output each club object
                            //    foreach (var club in clubsOutput)
                            //    {
                            //        output.WriteLine("{0};{1};{2};{3};{4};{5};{6}", club.id, club.name, club.address, club.latitude, club.longitude, club.rating, club.status);
                            //    }
                            //}
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
}
