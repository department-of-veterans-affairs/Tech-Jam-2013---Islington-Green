using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using MongoDB.Driver;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;

namespace IG.Data
{
    using System.Reflection;

    public class TextToMongo
    {
        private static MongoServer MServer;
        private static MongoDatabase MDatabase;
        private static MongoClient MClient;

        //private static string ConnectionString = @"mongodb://localhost";
        private static string ConnectionString = @"mongodb://islingtongreen2.cloudapp.net";

        private static string DatabaseName = @"va";
        private static string CollectionName = @"customers";

        private static string FILEPATH = @"C:\Github\TechJam\Customer.txt";
        private static string line;
        private static int count;
        private static char delim = ':';
        private static string block = string.Empty;
        private static string appts = string.Empty;
        private static BsonDocument root = new BsonDocument();

         
        public static int Parse()
        {
            return Parse(FILEPATH);
        }

        public static int Parse(string fileName)
        {
            InitDatabase();
            try
            {
                // Read the file and display it line by line.
                using (System.IO.StreamReader stream = new System.IO.StreamReader(fileName))
                {
                    while ((line = stream.ReadLine()) != null)
                    {
                        switch (line)
                        {
                            case Constants.DEMOGRAPHICS:
                                //demographics = ReadBlock(stream, HEALTHCAREPROVIDERS);
                                block = ReadBlock(stream);

                                var fragment = AddToMongo_Demographics();
                                root.Add("Demographics", fragment);
                                break;

                            case Constants.VAAPPOINTMENTS:
                                //appts = ReadBlock(stream, VAMEDICATIONHISTORY);
                                block = ReadBlock(stream);
                                var fragment2 = AddToMongo_VaAppointments();
                                root.Add("VaAppointments", fragment2);
                                break;
                        }
                        count++;
                    }
                    
                }
            }
            catch (Exception ex)
            {
                throw new FileLoadException("Could not parse or find the file. " + ex.Message);
            }

            var collection = MDatabase.GetCollection(CollectionName);
            collection.Insert(root);
            return count;
        }

        private static string ReadBlock(System.IO.StreamReader stream, string end)
        {
            var sb = new StringBuilder();
            do
            {
                    line = stream.ReadLine();
                    sb.Append(line + System.Environment.NewLine);                    

            } while (line.Contains(end) == false);

            return sb.ToString();
        }

        private static string ReadBlock(System.IO.StreamReader stream)
        {
            var sb = new StringBuilder();
            string previous = string.Empty;           
            do
            {
                line = stream.ReadLine();
                if (String.IsNullOrEmpty(previous) && !String.IsNullOrEmpty(line) && line.Contains("--------------------"))
                {
                    break;
                }
                sb.Append(line + System.Environment.NewLine);
                previous = line;

            } while (true);

            return sb.ToString();
        }



        #region  Demographics

        private static BsonDocument AddToMongo_Demographics()
        {
            string[] lines = Regex.Split(block, System.Environment.NewLine);
           
            BsonDocument doc = new BsonDocument();
            foreach (string line in lines)
            {
                string[] kvp = line.Split(new char[] { delim });

                if (kvp[0].Contains("EMERGENCY"))
                {
                    // assign to doc goes here
                    var array = AddEmergencyContacts(lines);
                    doc.Add("EmergencyContacts", array);
                    break;
                }
                else if (!String.IsNullOrEmpty(line) && kvp != null && kvp.Length == 2)
                {
                    //insert to Mongo
                    BsonElement element = new BsonElement(kvp[0].Clean(), kvp[1].Trim());
                    doc.Add(element);
                    Console.WriteLine("{0} |||  {1}", kvp[0].Clean(), kvp[1].Trim());
                }
            }

            return doc;            
        }

        private static BsonArray AddEmergencyContacts(string[] lines)
        {           
            bool emergency = false;
            var subDoc = new BsonDocument();
            BsonArray array = new BsonArray();

            try
            {
                foreach (string tmp in lines)
                {
                    if (tmp == Constants.HEALTHCAREPROVIDERS) break;
                    string[] kvp = tmp.Split(new char[] { delim });
                    var docs = new List<BsonDocument>();                    
                    if (kvp[0].Contains("EMERGENCY CONTACTS"))
                    {
                        emergency = true;
                    }

                    if (emergency == true && kvp[0].Clean() == "ContactFirstName")
                    {
                        subDoc = new BsonDocument();
                    }

                    if (emergency == true && kvp[0].Clean() == "EmailAddress")
                    {
                        array.Add(subDoc);
                        subDoc = new BsonDocument();
                    }
                    else if (emergency == true && !String.IsNullOrEmpty(line) && kvp != null && kvp.Length == 2)
                    {
                        BsonElement e = new BsonElement(kvp[0].Clean(), kvp[1].Trim());
                        subDoc.Add(e);
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return array;
        }

        #endregion


        #region VA Appointments

        private static BsonDocument AddToMongo_VaAppointments()
        {
            string[] lines = Regex.Split(block, Environment.NewLine);
            BsonDocument doc = new BsonDocument();
            foreach (string line in lines)
            {
                string[] kvp = line.Split(new char[] { delim });

                if (kvp[0].Contains("FUTURE APPOINTMENTS"))
                {
                    var array = AddFutureAppts(lines);
                    doc.Add("FutureAppointments", array); // assign to doc goes here
                }
                else if (kvp[0].Contains("PAST APPOINTMENTS"))
                {
                    var array = AddPastAppts(lines);
                    doc.Add("PastAppointments", array); // assign to doc goes here
                    break;
                }
                else if (!String.IsNullOrEmpty(line) && kvp.Length == 2)
                {
                    BsonElement e = new BsonElement(kvp[0].Clean(), kvp[1].Trim());
                    doc.Add(e);   //insert to Mongo
                    Console.WriteLine("key:{0}        value:{1}", kvp[0].Clean(), kvp[1].Trim());
                }
            }

            return doc;
        }

        private static BsonArray AddFutureAppts(string[] lines)
        {
            bool futureAppts = false;
            var subDoc = new BsonDocument();
            BsonArray array = new BsonArray();
            foreach (string tmp in lines)
            {
                if (tmp.Contains("PAST APPOINTMENTS")) break;
                
                string[] kvp = tmp.Split(new char[] { delim });
                var docs = new List<BsonDocument>();
                if (kvp[0].Contains("FUTURE APPOINTMENTS"))
                {
                    futureAppts = true;
                }
                else if (futureAppts == true && kvp[0].Clean() == "DateTime")
                {
                    subDoc = new BsonDocument();
                }
                else if (futureAppts == true && kvp[0].Clean() == "Type")
                {
                    array.Add(subDoc);
                    subDoc = new BsonDocument();
                }
                else if (futureAppts == true && !String.IsNullOrEmpty(line) && kvp.Length == 2)
                {
                    BsonElement e = new BsonElement(kvp[0].Clean(), kvp[1].Trim());
                    subDoc.Add(e);
                }
            }

            return array;
        }

        private static BsonArray AddPastAppts(string[] lines)
        {
            bool pastAppts = false;
            var subDoc = new BsonDocument();
            BsonArray array = new BsonArray();
            foreach (string tmp in lines)
            {
                string[] kvp = tmp.Split(new char[] { delim });
                var docs = new List<BsonDocument>();
                if (kvp[0].Contains("PAST APPOINTMENTS"))
                {
                    pastAppts = true;
                }

                if (pastAppts == true && kvp[0].Clean() == "DateTime")
                {
                    subDoc = new BsonDocument();
                }

                if (pastAppts == true && kvp[0].Clean() == "")
                {
                    if (subDoc.Count() > 0)
                    {
                        array.Add(subDoc);
                    }
                    subDoc = new BsonDocument();
                }
                else if (pastAppts == true && !String.IsNullOrEmpty(line) && kvp.Length == 2)
                {
                        BsonElement e = new BsonElement(kvp[0].Clean(), kvp[1].Trim());
                        subDoc.Add(e);
                }
            }

            return array;
        }

        #endregion

        private static void InitDatabase()
        {
            MClient = new MongoClient(ConnectionString);
            MServer = MClient.GetServer();
            MDatabase = MServer.GetDatabase(DatabaseName);
        }

    }
}
