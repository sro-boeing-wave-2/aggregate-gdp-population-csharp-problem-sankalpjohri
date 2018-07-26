using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using AggregateGDPPopulation.Classes;
using Newtonsoft.Json;

namespace AggregateGDPPopulation
{
    public class AggregateGDP
    {
        // Stores the path to the mapper file.
        private readonly string mapperFilePath;

        // Stores the path to the output file.
        private readonly String outputFilePath;

        // Stores the header value for country name.
        private readonly String fieldCountryName;

        // Stores the header value for GDP of 2012.
        private readonly String fieldGDP2012;

        // Stores the header value for Population of 2012.
        private readonly String fieldPopulation2012;
        private Dictionary<String, String> countryContinentMapper;

        private readonly  FileUtils _fileUtils;

        public AggregateGDP()
        {
            mapperFilePath = Environment.CurrentDirectory + @"data/country-list.json";
            outputFilePath = Environment.CurrentDirectory + @"/output/output.json";
            mapperFilePath = @"data/country-list.json";
            fieldCountryName = "Country Name";
            fieldGDP2012 = "GDP Billions (US Dollar) - 2012";
            fieldPopulation2012 = "Population (Millions) - 2012";
            countryContinentMapper = new Dictionary<String, String>();
            _fileUtils = new FileUtils();
        }

        public async Task AggregatePopulationAndGDPData(string filePath)
        {
            Task<string> mapperTask = _fileUtils.ReadFile(mapperFilePath);
            Task<string> csvParserTask = _fileUtils.ReadFile(filePath);
            await mapperTask;
            countryContinentMapper = ParseMapper(mapperTask.Result);
            await csvParserTask;
            AggregatedData aggregatedData = new AggregatedData();
            AggregateContinentData(csvParserTask.Result, aggregatedData);
            await _fileUtils.WriteFile(outputFilePath, aggregatedData.SerializeData());
        }

        /**
         * Method to parse the mapper data.
         * */
        private Dictionary<string, string> ParseMapper(string countryContinentJson)
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

        
        private void AggregateContinentData(string csvString, AggregatedData aggregatedData)
        {
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
                    aggregatedData.AddOrUpdateData(continentName, float.Parse(rowData[indexGDP2012]),
                        float.Parse(rowData[indexPopulation2012]));
                }
            }
        }
    }
}