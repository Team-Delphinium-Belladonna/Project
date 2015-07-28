//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
////using ExtensionsMethods;
//using MongoDB.Bson;
//using MongoDB.Bson.Serialization.Attributes;
//using MongoDB.Driver;
//using MongoDB.Driver.Builders;

//namespace Markets.DataManipulation
//{
//    public class MongoDbDatabase
//    {
//        const string DatabaseHost = "mongodb://127.0.0.1";
//        const string DatabaseName = "test";

//        static MongoDatabase GetDatabase(string name, string fromHost)
//        {
//            var mongoClient = new MongoClient(fromHost);
//            var server = mongoClient.GetServer();
//            return server.GetDatabase(name);
//        }

//        /*private MongoServer server;
//        private string connectionString = "mongodb://localhost";
//        private MongoClient client;

//        public MongoDbDatabase(databaseName)
//        {
//            client = new MongoClient(connectionString);
//            server = client.GetDatabase();
//            Database = server.GetDatabase(databaseName);
//            Collections = new Dictionary<string, MongoCollection>();
//        }

//        public MongoServer Server
//        {
//            get
//            {
//                return server;
//            }

//            set
//            {
//                server = value;
//            }
//        }



//        /*MongoClient cl = new MongoClient();
//        MongoServerAddress server = MongoServer("mongodb://localhost");
//        MongoDatabase db = cl.GetServer().GetDatabase("proba");

//        /*var messagesCollection = db.GetCollection<Message>("messages");

//        var message = new Message { Date = DateTime.Now, Username = username, Text = text };
//        private static object MongoServer;

//        messagesCollection.Insert(message);
        


//        var server = MongoServer.Create(ConnectionString);
//            MongoDatabase myCompany = server.GetDatabase("MyCompany");

//            MongoCollection<BsonDocument> employees =
//                myCompany.GetCollection<BsonDocument>("Employees");
//            BsonDocument employee = new BsonDocument {
//                { "FirstName", firstName },
//                { "LastName", lastName },
//                { "Address", address },
//                { "City", city },
//                { "DepartmentId", departmentId }
//                };

//            employees.Insert(employee); */


//    }
//}
