namespace Markets.DataManipulation
{
    using System.Collections.Generic;
    using MongoDB.Driver;

    public class MongoDb
    {
        private static MongoServer server;

        static MongoDb()
        {
            const string connectionString = "mongodb://localhost";
            var client = new MongoClient(connectionString);
            server = client.GetServer();
        }

        public MongoDb(string databaseName)
        {
            this.Database = server.GetDatabase(databaseName);
            this.Collections = new Dictionary<string, MongoCollection>();
        }

        private MongoDatabase Database { get; set; }
        public Dictionary<string, MongoCollection> Collections { get; private set; }

        public void CreateCollection<T>(string collectionName)
        {
            var collection = this.Database.GetCollection<T>(collectionName);
            this.Collections.Add(collectionName, collection);
        }
    }
}
