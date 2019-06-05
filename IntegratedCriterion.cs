using System;
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
using System.Reflection;

namespace MongoDBApp
{
    class IntegratedCriterion
    {
        public List <Accordance> accordanceList;
        public IntegratedCriterion()
        {
            
        }
        
        public void MapCurrentRequirenments(Requirement requirement, List <Employer> employerList)
        {
      //      foreach 
        }

        public void FindIntegratedCriterion(List<Candidate> candidates, List<Employer> employers)
        {
            accordanceList = new List<Accordance>();
            float integratedCriterion = 0;
            foreach (var candidate in candidates)   //цикл по кандидатам
            {
                foreach (var employer in employers) //цикл по вакансиям
                {
                    foreach (var requirement in candidate.currentRequirements)  //цикл по требованиям кандидатов
                    {                  
                        //ищем атрибут в классе Employer с названием требования из кандидата
                        var employerAttr = employer.GetType().GetField(requirement.nameAttr);  
                        if (employerAttr==null)
                            break;
                        //проверяем соответствие значений атрибутов из Candidate и Employer
                        var employerAttrValue = employerAttr.GetValue(employer);
                        if (requirement.valueAttr.ToString() == employerAttrValue.ToString())
                            integratedCriterion = integratedCriterion + requirement.importance;
                    }
                    Accordance tempAccordance = new Accordance(employer.id, candidate.id, integratedCriterion);
                    accordanceList.Add(tempAccordance);
                    integratedCriterion=0;
                }
            }
        }
    }
}