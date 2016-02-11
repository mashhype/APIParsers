using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace GooglePlacesAPIParser
{
    class DBMethods
    {
        public static List<Club> ClubList = new List<Club>();
        public static SqlConnection myConnection = null;
        public static SqlDataReader myReader = null;
        public static SqlCommand myCommand = null;
        public string serverName = null;
        public string databaseName = null;
     
        public static List<Club> readTable(string SqlQuery, string serverName, string databaseName)
        {
            myConnection = new SqlConnection(serverName + ";" + databaseName + ";" + "Integrated Security=true");

            try 
            {
                myConnection.Open();
                myCommand = new SqlCommand(SqlQuery, myConnection);
                myReader = myCommand.ExecuteReader();
                ClubList.Clear();
                while(myReader.Read()) {
                    //Console.WriteLine(myReader[0].ToString());
                    //code here to map query output to club objects and put into list
                    ClubList.Add(new Club
                    {
                        id = myReader[0].ToString(),
                        //name = myReader[1].ToString(),
                        //address = myReader[2].ToString(),
                        //city = myReader[3].ToString(),
                        //state = myReader[4].ToString()
                    });
                }
            } catch (Exception e) {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                myConnection.Close();
            }
            return ClubList;
        }

        public static List<Club> readTableInClause(string sql_query, string serverName, string databaseName, List<string> resendList)
        {
            myConnection = new SqlConnection(serverName + ";" + databaseName + ";" + "Integrated Security=true");

            try
            {
                myConnection.Open();
                myCommand = new SqlCommand(sql_query, myConnection);
                var idParameterList = new List<string>();
                var index = 0;

                foreach (var id in resendList)
                {
                    var paramName = "@id" + index;
                    myCommand.Parameters.AddWithValue(paramName, id);
                    idParameterList.Add(paramName);
                    index++;
                }

                //this stores the entire query in myCommand concatenating each parameter in the IN clause
                myCommand.CommandText = String.Format(sql_query, string.Join(",", idParameterList));
                myReader = myCommand.ExecuteReader();
                ClubList.Clear();

                while (myReader.Read())
                {
                    //Console.WriteLine(myReader[0].ToString());
                    //code here to map query output to club objects and put into list
                    ClubList.Add(new Club
                    {
                        id = myReader[0].ToString(),
                        name = myReader[1].ToString(),
                        address = myReader[2].ToString(),
                        city = myReader[3].ToString(),
                        state = myReader[4].ToString()
                    });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                myConnection.Close();
            }
            return ClubList;
        }

        //assumes table exists
        public static void writeTableTest(string SqlQuery, List<Club> result, string serverName, string databaseName)
        {
            myConnection = new SqlConnection(serverName + ";" + databaseName + ";" + "Integrated Security=true");
            
            try
            {
                myConnection.Open();
                myCommand = new SqlCommand(SqlQuery, myConnection);
                myCommand.CommandType = System.Data.CommandType.Text;
                //add in all the parameters of your SQL query for the insert along with their data types
                myCommand.Parameters.Add("@ID", SqlDbType.VarChar);
                myCommand.Parameters.Add("@Name", SqlDbType.VarChar);
                myCommand.Parameters.Add("@Address", SqlDbType.VarChar);
                myCommand.Parameters.Add("@City", SqlDbType.VarChar);
                myCommand.Parameters.Add("@State", SqlDbType.VarChar);

                foreach (var element in result)
                {
                    myCommand.Parameters["@ID"].Value = element.id;
                    myCommand.Parameters["@Name"].Value = element.name;
                    myCommand.Parameters["@Address"].Value = element.address;
                    myCommand.Parameters["@City"].Value = element.city;
                    myCommand.Parameters["@State"].Value = element.state;
                    myCommand.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                myConnection.Close();
            }
        }

        //assumes table exists
        public static void writeTable(string SqlQuery, List<Club> result, string serverName, string databaseName) 
        {
            myConnection = new SqlConnection(serverName + ";" + databaseName + ";" + "Integrated Security=true");
  
            try 
            {
                myConnection.Open();
                myCommand = new SqlCommand(SqlQuery, myConnection);
                myCommand.CommandType = System.Data.CommandType.Text;
                //add in all the parameters of your SQL query for the insert along with their data types
                myCommand.Parameters.Add("@ClubID", System.Data.SqlDbType.VarChar);
                myCommand.Parameters.Add("@Rating", System.Data.SqlDbType.VarChar);
                myCommand.Parameters.Add("@Address", System.Data.SqlDbType.VarChar);
                myCommand.Parameters.Add("@Name", System.Data.SqlDbType.VarChar);
                myCommand.Parameters.Add("@Status", System.Data.SqlDbType.VarChar);
                myCommand.Parameters.Add("@ClubLat", System.Data.SqlDbType.VarChar);
                myCommand.Parameters.Add("@ClubLong", System.Data.SqlDbType.VarChar);
                
                foreach (var element in result)
                {
                    myCommand.Parameters["@ClubID"].Value = element.id;
                    myCommand.Parameters["@Rating"].Value = element.rating;
                    myCommand.Parameters["@Address"].Value = element.address;
                    myCommand.Parameters["@Name"].Value = element.name;
                    myCommand.Parameters["@Status"].Value = element.status;
                    myCommand.Parameters["@ClubLat"].Value = element.latitude;
                    myCommand.Parameters["@ClubLong"].Value = element.longitude;
                    myCommand.ExecuteNonQuery();
                }
            } 
            catch (Exception e) 
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                myConnection.Close();
            }
        }

        public static void closeConnection()
        {
            try
            {
                myConnection.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
