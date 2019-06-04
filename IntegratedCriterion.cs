using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Configuration;
using MongoDB.Bson.Serialization;

namespace MongoDBApp
{
    class IntegratedCriterion
    {
        public List <Accordance> accordanceList;
        public Candidate currentCandidate;

        public static async Task FindIntegratedCriterion(IMongoDatabase database, int candID)
        {
            var candidateCollection = database.GetCollection<BsonDocument>("candidate");
            var filter = new BsonDocument{{"_id", candID},{"requirement.id",1}};
            using (var cursor = await candidateCollection.FindAsync(filter))
            {
                while (await cursor.MoveNextAsync())
                {
                    var currentCandidate = cursor.Current;
                    foreach (var doc in currentCandidate)
                    {                           
                        var elements = doc.Elements.ToArray();
                        var ell = elements[2].ToBsonDocument();
                        var elll=ell[1].ToJson();

                        foreach (var el in elements)
                        {
                            Console.WriteLine(el);
                        }
                        
                        

                    }
                }
            }
        }
    }
}