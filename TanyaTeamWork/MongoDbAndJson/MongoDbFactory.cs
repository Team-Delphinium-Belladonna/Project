using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBandJSON
{
    class MongoDbFactory
    {
        private const string connectionString = "mongodb://127.0.0.1";

        public static MongoClient client = new MongoClient(connectionString);
        public static MongoServer server = client.GetServer();
        public MongoDatabase database = server.GetDatabase("jsonToMongo");

        //var periodReports = database.GetCollection<JsonReport>("periodReports");


    }
}
