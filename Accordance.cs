using System;

namespace MongoDBApp
{
    class Accordance    //соответствие интегрального критерия вакансии, рассчитывается для каждого кандидата
    {
        private string vacancyID;       
        private float integratedCriterion;
        public Accordance(string vacancy, float integrCrit)
        {
            vacancyID=vacancy;
            integrCrit=integratedCriterion;
        }
    }
}