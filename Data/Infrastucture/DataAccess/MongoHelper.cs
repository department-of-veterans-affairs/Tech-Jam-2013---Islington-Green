using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Driver;

namespace IG.Data
{
    public interface IMongoHelper
    {
        MongoClient Client { get; }
        MongoServer Server { get; }
        MongoDatabase Database { get; }
    }

    public class MongoHelper : IMongoHelper
    {
        public static IMongoHelper Current
        {
            get
            {
                return new MongoHelper();
            }
        }

        private MongoHelper() { }

        public MongoClient Client
        {
            get { return new MongoClient(Settings.Current.ConnectionString); }
        }

        public MongoServer Server
        {
            get { return Client.GetServer(); }
        }

        public MongoDatabase Database
        {
            get { return Server.GetDatabase(Settings.Current.DatabaseName); }
        }
    }
}

