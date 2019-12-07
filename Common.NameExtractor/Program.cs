using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace Common.NameExtractor
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var maleNameStream = File.Create(@"c:\temp\male-names.txt");
            var femaleNameStream = File.Create(@"c:\temp\female-names.txt");

            HashSet<string> maleNames = new HashSet<string>();
            HashSet<string> femaleNames = new HashSet<string>();

            string[] lines = File.ReadAllLines("data\\yob2018.txt");
            foreach (string line in lines)
            {
                string[] split = line.Split(',');

                byte[] buffer = Encoding.UTF8.GetBytes(split[0] + Environment.NewLine);
                if (split[1] == "M")
                {
                    await maleNameStream.WriteAsync(buffer, 0, buffer.Length);
                }
                else if (split[1] == "F")
                {
                    await femaleNameStream.WriteAsync(buffer, 0, buffer.Length);
                }
                else
                {
                    Console.WriteLine($"Other gender: {split[1]}");
                }
            }

            await maleNameStream.FlushAsync();
            await femaleNameStream.FlushAsync();

            maleNameStream.Close();
            femaleNameStream.Close();
        }
    }
}
