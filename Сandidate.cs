using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Configuration;
using MongoDB.Bson.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
//protected override IList<JToken> ChildrenTokens { get; }

namespace MongoDBApp
{
    class Requirement : List<object>
    {
        public int id { get; set; }
        public string nameAttr { get; set; }
        public string valueAttr { get; set; }
        public float importance { get; set; }

        public Requirement(int id, string nameAttr, string valueAttr, float importance)
        {
            this.id = id;
            this.nameAttr = nameAttr;
            this.valueAttr = valueAttr;
            this.importance = importance;
        }
    }

    class Candidate
    {
        public int id { get; set; }
        public string fio { get; set; }
        public List<Requirement> currentRequirements;
        public async Task GetCandidate(IMongoDatabase database, int candidateID)
        {
            this.id = candidateID;
            var candidateCollection = database.GetCollection<BsonDocument>("candidate");
            var filter = new BsonDocument("_id", this.id);
            using (var cursor = await candidateCollection.FindAsync(filter))
            {
                while (await cursor.MoveNextAsync())
                {
                    var currentCandidate = cursor.Current;
                    foreach (var doc in currentCandidate)
                    {
                        var elements = doc.Elements.ToArray();
                        foreach (var el in elements)
                        {
                            if (el.Name == "fio")
                                this.fio = el.Value.ToString();
                            if (el.Name == "requirement")
                            {
                                currentRequirements = new List<Requirement>();
                                JArray jsonArray = JArray.Parse(el.Value.ToString()); 
                                JToken jsonArray_Item = jsonArray.First;
                                while (jsonArray_Item != null) 
                                {
                                    int itemID = jsonArray_Item.Value<int>("id");
                                    string itemName = jsonArray_Item.Value<string>("name"); 
                                    string itemValue = jsonArray_Item.Value<string>("value");
                                    float itemImportance = jsonArray_Item.Value<float>("importance");
                                    Requirement item = new Requirement(itemID, itemName, itemValue, itemImportance);
                                    this.currentRequirements.Add(item);
                                    jsonArray_Item = jsonArray_Item.Next; 
                                } 
                            }
                        }
                    }
                }
            }
            this.ShowCandidate();
        }

        public static async Task GetAllCandidates(IMongoDatabase database)
        {

        }
        public void ShowCandidate()
        {
            Console.WriteLine(this.ToJson());
        }
    }

}