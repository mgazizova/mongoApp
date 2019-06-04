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
    class Employer
    {
        public int id {get; set;} 
        public string specialty {get; set;} 
        public string workType {get; set;} 
        public string wageForm {get; set;} 
        public string schedule {get; set;} 
        public bool housing {get; set;} 
        public bool socialPackage {get; set;} 
        public bool kindergarten {get; set;}
        public bool dispensary {get; set;} 
        public bool insalubrity {get; set;} 
        public string natureOfWork {get; set;} 
        public string location {get; set;} 
        public bool careerProspects {get; set;} 
        public int wage {get; set;} 
        public DateTime placementDate {get; set;} 

        private void MapEmployerAttributes(BsonElement element)
        {
            if (element.Name == "specialty")
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

        public static async Task GetAllEmployers(IMongoDatabase database)
            {
                var employerCollection = database.GetCollection<BsonDocument>("employer");
                var filter = new BsonDocument();
                int countEmployers = 0;
                List <Employer> employerList = new List<Employer>();
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
                            employerList.Add(tempEmpl);
                            countEmployers++;                   
                        }
                        
                    }
                }
            }

        public void ShowEmployer()
        {
            Console.WriteLine(this.ToJson());
        }
    }
}