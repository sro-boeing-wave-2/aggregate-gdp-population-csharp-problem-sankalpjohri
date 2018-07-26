using System;
using System.IO;
using System.Threading.Tasks;
using AggregateGDPPopulation.Classes;
using Xunit;

namespace AggregateGDPPopulation.Tests
{
    public class AggregateDataTest
    {
        [Fact]
        public async Task ShouldBeSameAsExpectedOutput()
        {
            var aggregateData = new AggregateGDP();
            var actualData = new AggregatedData();
            var expectedData = new AggregatedData();
            
            await aggregateData.AggregatePopulationAndGDPData(@"data/datafile.csv");
            string actualContent;
            using (var reader = new StreamReader(Environment.CurrentDirectory + @"/output/output.json"))
            {
                actualContent = await reader.ReadToEndAsync();
            }
            actualData.DeserializeData(actualContent);
            string expectedContent;
            using (var reader = new StreamReader(Environment.CurrentDirectory + @"../../../../expected-output.json"))
            {
                expectedContent = await reader.ReadToEndAsync();
            }
            expectedData.DeserializeData(expectedContent);
            
            Assert.Equal(actualData, expectedData);
        }
    }
}
