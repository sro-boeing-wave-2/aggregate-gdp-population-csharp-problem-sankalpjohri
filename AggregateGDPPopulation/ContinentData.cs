using System;
using System.Collections.Generic;
using System.Text;

namespace AggregateGDPPopulation
{
    public class ContinentData
    {

        public float GDP_2012 { get; set; }
        public float POPULATION_2012 { get; set; }

        public ContinentData()
        {
            GDP_2012 = 0.0f;
            POPULATION_2012 = 0.0f;
        }

        // override object.Equals
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            if (GDP_2012 != ((ContinentData)obj).GDP_2012 || POPULATION_2012 != ((ContinentData)obj).POPULATION_2012)
            {
                return false;
            }
            return true;
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }   
    }
}
