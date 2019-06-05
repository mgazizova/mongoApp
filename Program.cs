using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Configuration;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MongoDBApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "mongodb://localhost:27017";
            MongoClient client = new MongoClient(connectionString);
            var database = client.GetDatabase("sdb_findJob");

            Employer myEmployer = new Employer();
            myEmployer.GetAllEmployers(database).GetAwaiter();

            Candidate myCandidate = new Candidate();
            myCandidate.GetAllCandidates(database).GetAwaiter();
            
            Thread.Sleep(1000);
            IntegratedCriterion ic = new IntegratedCriterion();
            ic.FindIntegratedCriterion(myCandidate.listCandidate, myEmployer.listEmployer);

            Console.ReadLine();
        }

        private static async Task GetDatabaseNames(MongoClient client)
        {
            using (var cursor = await client.ListDatabasesAsync())
            {
                var databaseDocuments = await cursor.ToListAsync();
                foreach (var databaseDocument in databaseDocuments)
                {
                    Console.WriteLine(databaseDocument["name"]);
                }
            }
        }

        private static async Task GetCollectionNames(IMongoDatabase database)
        {
            Console.WriteLine("В базе данных {0} имеются следующие коллекции:", database.DatabaseNamespace.DatabaseName);
            using (var cursor = await database.ListCollectionsAsync())
            {
                var databaseCollection = await cursor.ToListAsync();
                foreach (var dbCollection in databaseCollection)
                {
                    Console.WriteLine(dbCollection["name"]);
                }
            }
        }

        public static async Task FindDocs(IMongoDatabase database, IMongoCollection<BsonDocument> collection)
        {
            var filter = new BsonDocument();
            using (var cursor = await collection.FindAsync(filter))
            {
                while (await cursor.MoveNextAsync())
                {
                    var people = cursor.Current;
                    foreach (var doc in people)
                    {
                        Console.WriteLine(doc);
                    }
                }
            }
        }

        


    }
}