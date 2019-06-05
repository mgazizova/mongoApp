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

namespace MongoDBApp
{
    class Employer : Participants
    {
        public List<Employer> listEmployer;
        public int id;
        public string specialty;
        public string workType;
        public string wageForm;
        public string schedule;
        public bool housing;
        public bool socialPackage; 
        public bool kindergarten;
        public bool dispensary; 
        public bool insalubrity; 
        public string natureOfWork;
        public string location;
        public bool careerProspects;
        public int wage; 
        public DateTime placementDate; 

        private void MapEmployerAttributes(BsonElement element)
        {
            if (element.Name == "_id")
                this.id = element.Value.ToInt32();

            else if (element.Name == "specialty")
                this.specialty = element.Value.ToString();

            else if (element.Name == "work_type")
                this.workType = element.Value.ToString();

            else if (element.Name == "wage_form")
                this.wageForm = element.Value.ToString();

            else if (element.Name == "schedule")
                this.schedule = element.Value.ToString();

            else if (element.Name == "housing")
            {
                if (element.Value == "есть")
                    this.housing = true;
                else
                    this.housing = false;
            }

            else if (element.Name == "social_package")
            {
                if (element.Value == "есть")
                    this.socialPackage = true;
                else
                    this.socialPackage = false;
            }

            else if (element.Name == "kindergarten")
            {
                if (element.Value == "есть")
                    this.kindergarten = true;
                else
                    this.kindergarten = false;
            }

            else if (element.Name == "dispensary")
            {
                if (element.Value == "есть")
                    this.dispensary = true;
                else
                    this.dispensary = false;
            }

            else if (element.Name == "insalubrity")
            {
                if (element.Value == "есть")
                    this.insalubrity = true;
                else
                    this.insalubrity = false;
            }     

            else if (element.Name == "nature_of_work")
                this.natureOfWork = element.Value.ToString();

            else if (element.Name == "location")
                this.location = element.Value.ToString();

            else if (element.Name == "career_prospects")
            {
                if (element.Value == "есть")
                    this.careerProspects = true;
                else
                    this.careerProspects = false;
            }

            else if (element.Name == "wage")
                this.wage = element.Value.ToInt32();
        }

        public async Task GetEmployer(IMongoDatabase database, int employerID)
        {
            this.id = employerID;
            var employerCollection = database.GetCollection<BsonDocument>("employer");
            var filter = new BsonDocument("_id", this.id);
            using (var cursor = await employerCollection.FindAsync(filter))
            {
                while (await cursor.MoveNextAsync())
                {
                    var currentEmployer = cursor.Current;
                    foreach (var doc in currentEmployer)
                    {
                        var elements = doc.Elements.ToArray();
                        foreach (var el in elements)
                        {
                            this.MapEmployerAttributes(el);
                        }                    
                    }
                }
            }
            this.ShowEmployer();
        }

        public async Task GetAllEmployers(IMongoDatabase database)
            {
                var employerCollection = database.GetCollection<BsonDocument>("employer");
                var filter = new BsonDocument();
                int countEmployers = 0;
                this.listEmployer = new List<Employer>();
                using (var cursor = await employerCollection.FindAsync(filter))
                {
                    while (await cursor.MoveNextAsync())
                    {
                        var currentEmployer = cursor.Current;                            
                        foreach (var doc in currentEmployer)
                        {
                            Employer tempEmpl = new Employer();  
                            var elements = doc.Elements.ToArray();
                            foreach (var el in elements)
                            {
                                tempEmpl.MapEmployerAttributes(el);
                            } 
                            this.listEmployer.Add(tempEmpl);
                            countEmployers++;                   
                        }
                        
                    }
                }
            }
        public void ShowEmployer()
        {
            Console.WriteLine(this.ToJson());
        }

        public async Task SaveDocs(IMongoDatabase database)
        {
            var collection = database.GetCollection<BsonDocument>("employer");
            await collection.InsertOneAsync(this.ToBsonDocument());
        }
    }
}