using System;

namespace MongoDBApp
{
    class Accordance    //соответствие интегрального критерия вакансии, рассчитывается для каждого кандидата
    {
        private int vacancyID;
        private int candidateID;
        private float integratedCriterion;
        public Accordance(int vacancy, int candidate, float integrCrit)
        {
            vacancyID=vacancy;
            candidateID=candidate;
            integratedCriterion=integrCrit;
        }
    }
}