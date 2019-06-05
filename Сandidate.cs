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

namespace MongoDBApp
{
    class Requirement   //: Participants
    {
        public int id;
        public string nameAttr;
        public string valueAttr;
        public float importance;

        public Requirement(int id, string nameAttr, string valueAttr, float importance)
        {
            this.id = id;
            this.nameAttr = nameAttr;
            this.valueAttr = valueAttr;
            this.importance = importance;
        }        
    }

    class Candidate // : Participants
    {
        public List <Candidate> listCandidate;
        public int id;
        public string fio;
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

        public async Task GetAllCandidates(IMongoDatabase database)
        {
            var candidateCollection = database.GetCollection<BsonDocument>("candidate");
            var filter = new BsonDocument();
            //this.participantsList = new List<Participants>();
            this.listCandidate = new List<Candidate>();
            int countCandidates = 0;
            using (var cursor = await candidateCollection.FindAsync(filter))
            {
                while (await cursor.MoveNextAsync())
                {                    
                    var currentCandidate = cursor.Current;
                    foreach (var doc in currentCandidate)
                    {
                        var elements = doc.Elements.ToArray();
                        Candidate tempCand = new Candidate();  
                        foreach (var el in elements)
                        {
                            if(el.Name == "_id")
                                tempCand.id=el.Value.ToInt32();
                            if (el.Name == "fio")
                                tempCand.fio = el.Value.ToString();
                            if (el.Name == "requirement")
                            {
                                tempCand.currentRequirements = new List<Requirement>();
                                JArray jsonArray = JArray.Parse(el.Value.ToString()); 
                                JToken jsonArray_Item = jsonArray.First;
                                while (jsonArray_Item != null) 
                                {
                                    int itemID = jsonArray_Item.Value<int>("id");
                                    string itemName = jsonArray_Item.Value<string>("name"); 
                                    string itemValue = jsonArray_Item.Value<string>("value");
                                    float itemImportance = jsonArray_Item.Value<float>("importance");
                                    Requirement item = new Requirement(itemID, itemName, itemValue, itemImportance);
                                    tempCand.currentRequirements.Add(item);
                                    jsonArray_Item = jsonArray_Item.Next; 
                                } 
                            }
                        }
                        this.listCandidate.Add(tempCand);
                        countCandidates++; 
                    }
                }
            }
        }
        public void ShowCandidate()
        {
            Console.WriteLine(this.ToJson());
        }
    
        public async Task SaveDocs(IMongoDatabase database)
        {
            var collection = database.GetCollection<BsonDocument>("candidate");
            await collection.InsertOneAsync(this.ToBsonDocument());
        }
    
    
    }

}