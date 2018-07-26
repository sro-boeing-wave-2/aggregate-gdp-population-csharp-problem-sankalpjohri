using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace AggregateGDPPopulation.Classes
{
    public class AggregatedData
    {
        private Dictionary<string, ContinentData> ContinentAggregateData;

        public AggregatedData()
        {
            ContinentAggregateData = new Dictionary<string, ContinentData>();
        }

        public void AddOrUpdateData(string continentName, float gdp, float population)
        {
            if (!ContinentAggregateData.ContainsKey(continentName))
            {
                ContinentAggregateData.Add(continentName, new ContinentData());
            }

            ContinentAggregateData[continentName].GDP_2012 += gdp;
            ContinentAggregateData[continentName].POPULATION_2012 += population;
        }

        public string SerializeData()
        {
            return JsonConvert.SerializeObject(ContinentAggregateData);
        }

        public void DeserializeData(string jsonData)
        {
            ContinentAggregateData = JsonConvert.DeserializeObject<Dictionary<string, ContinentData>>(jsonData);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != this.GetType())
            {
                return false;
            }

            if (ContinentAggregateData.Count != ((AggregatedData) obj).ContinentAggregateData.Count)
            {
                return false;
            }
            
            return ContinentAggregateData.Keys.All(key => ((AggregatedData) obj).ContinentAggregateData.ContainsKey(key) && ContinentAggregateData[key].Equals(((AggregatedData)obj).ContinentAggregateData[key]));
        }
    }
}