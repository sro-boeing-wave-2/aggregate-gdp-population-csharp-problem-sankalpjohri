using System;
using System.IO;
using System.Threading.Tasks;

namespace AggregateGDPPopulation.Classes
{
    public class FileUtils
    {
        /**
         * Method to read a file and call the calllback function with the data.
         * */
        public async Task<string> ReadFile(string filePath)
        {
            string content = "";
            using (StreamReader reader = new StreamReader(filePath))
            {
                content = await reader.ReadToEndAsync();
            }
            return content;
        }

        /**
         * Method to write a dictionary to a given output file.
         * */
        public async Task WriteFile(string filePath, string data)
        {
            if (!Directory.Exists(Environment.CurrentDirectory + @"/output")) { 
                Directory.CreateDirectory(Environment.CurrentDirectory + @"/output");
            }   
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    await writer.WriteAsync(data);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception while generating the output file: {0}", e.Message);
            }
        }
    }
}