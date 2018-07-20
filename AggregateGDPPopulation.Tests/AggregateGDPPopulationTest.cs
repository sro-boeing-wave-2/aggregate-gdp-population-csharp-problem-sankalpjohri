using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace AggregateGDPPopulation.Tests
{
    public class AggregateDataTest
    {
        [Fact]
        public async Task ShouldBeSameAsExpectedOutput()
        {
            AggregateGDP aggregateData = new AggregateGDP();
            await aggregateData.AggregatePopulationAndGDPData(@"data/datafile.csv");
            string actualContent = "";
            using (StreamReader reader = new StreamReader(Environment.CurrentDirectory + @"/output/output.json"))
            {
                actualContent = await reader.ReadToEndAsync();
            }
            Dictionary<string, ContinentData> actual = JsonConvert.DeserializeObject<Dictionary<string, ContinentData>>(actualContent); 
            string expectedContent = "";
            using (StreamReader reader = new StreamReader(Environment.CurrentDirectory + @"../../../../expected-output.json"))
            {
                expectedContent = await reader.ReadToEndAsync();
            }
            Dictionary<string, ContinentData> expected = JsonConvert.DeserializeObject<Dictionary<string, ContinentData>>(expectedContent);

            foreach (var key in actual.Keys)
            {
                if (expected.ContainsKey(key))
                {
                    Assert.Equal(actual[key], expected[key]);
                } else
                {
                    Assert.True(false);
                }
            }
            
        }
    }
}
