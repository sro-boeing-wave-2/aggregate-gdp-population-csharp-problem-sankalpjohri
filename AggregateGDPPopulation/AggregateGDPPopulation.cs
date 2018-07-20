using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AggregateGDPPopulation
{
    public class AggregateGDP
    {
        String mapperFilePath;
        String outputFilePath;
        String fieldCountryName;
        String fieldGDP2012;
        String fieldPopulation2012;
        Dictionary<String, String> countryContinentMapper;

        public AggregateGDP()
        {
            mapperFilePath = Environment.CurrentDirectory + @"data/country-list.json";
            outputFilePath = Environment.CurrentDirectory + @"/output/output.json";
            mapperFilePath = @"data/country-list.json";
            fieldCountryName = "Country Name";
            fieldGDP2012 = "GDP Billions (US Dollar) - 2012";
            fieldPopulation2012 = "Population (Millions) - 2012";
            countryContinentMapper = new Dictionary<String, String>();
        }

        public async Task AggregatePopulationAndGDPData(string filePath)
        {
           countryContinentMapper = await ReadFile(mapperFilePath, ParseMapper);
           Dictionary<string, ContinentData> continentAggregateData = await ReadFile(filePath, AggregateContinentData);
           await WriteFile(outputFilePath, continentAggregateData);
        }

        public async Task<Tresult> ReadFile<Tresult> (string filePath, Func<string, Tresult> processingCallback)
        {
            string content = "";
            using (StreamReader reader = new StreamReader(filePath))
            {
                content = await reader.ReadToEndAsync();
            }
            return processingCallback(content);
        }

        public async Task WriteFile(string filePath, Dictionary<String, ContinentData> continentAggregateData)
        {
            if (!Directory.Exists(Environment.CurrentDirectory + @"/output")) { 
                Directory.CreateDirectory(Environment.CurrentDirectory + @"/output");
            }   
            string content = JsonConvert.SerializeObject(continentAggregateData);
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    await writer.WriteAsync(content);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception while generating the output file: {0}", e.Message);
            }
        }

        public Dictionary<string, string> ParseMapper(string countryContinentJson)
        {
            Dictionary<string, string> mapper = new Dictionary<string, string>();
            try
            {
                mapper = JsonConvert.DeserializeObject<Dictionary<string, string>>(countryContinentJson);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error in parsing the mapper file: {}", e.Message);
            }
            return mapper;
        }

        public Dictionary<string, ContinentData> AggregateContinentData(string csvString)
        {
            Dictionary<string, ContinentData> continentData = new Dictionary<string, ContinentData>();
            string[] csvRows = csvString.Replace("\"", String.Empty).Trim().Split('\n');
            string[] headers = csvRows[0].Split(',');
            int indexCountryName = Array.IndexOf(headers, fieldCountryName);
            int indexGDP2012 = Array.IndexOf(headers, fieldGDP2012);
            int indexPopulation2012 = Array.IndexOf(headers, fieldPopulation2012);
            for (int i = 1; i < csvRows.Length; i++)
            {
                string[] rowData = csvRows[i].Split(',');
                String countryName = " ";
                if (countryContinentMapper.ContainsKey(rowData[indexCountryName]))
                {
                    countryName = countryContinentMapper[rowData[indexCountryName]];
                }
                if (!String.IsNullOrWhiteSpace(countryName))
                {
                    string continentName = countryContinentMapper[rowData[indexCountryName]];
                    if (!continentData.ContainsKey(continentName))
                    {
                        continentData.Add(continentName, new ContinentData());
                    }
                    continentData[continentName].GDP_2012 += float.Parse(rowData[indexGDP2012]);
                    continentData[continentName].POPULATION_2012 += float.Parse(rowData[indexPopulation2012]);
                }
            }
            return continentData;
        } 
    }
}
