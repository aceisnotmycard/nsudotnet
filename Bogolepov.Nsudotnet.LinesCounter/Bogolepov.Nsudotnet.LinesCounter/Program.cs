using System;
using System.IO;
using System.Linq;

namespace Bogolepov.Nsudotnet.LinesCounter
{
    class Program
    {
        private static int CountLinesCS(string file)
        {

            bool multiline = false;
            int count = 0;
            using (var reader = new StreamReader(file))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    line = line.Trim();
                    var containsCode = false;
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    for (int i = 0; i < line.Length - 1; i++)
                    {
                        if (line[i] == '/' && line[i+1] == '/')
                        {
                            if (!multiline)
                            {
                                break;
                            }
                        }
                        else if (line[i] == '/' && line[i+1] == '*')
                        {
                            multiline = true;
                            i++;
                        }
                        else if (line[i] == '*' && line[i + 1] == '/')
                        {
                            multiline = false;
                            i++;
                        }
                        else if (!multiline)
                        {
                            containsCode = true;
                        }
                    }
                    if (containsCode || (!multiline && line.Length == 1))
                    {
                        count++;
                    }

                }
            }
            return count;
        }

        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("usage: counter.exe {extension}");
                return;
            }
            string[] files = Directory.GetFiles(Directory.GetCurrentDirectory(), "*." + args[0],
                SearchOption.AllDirectories);

            int lines = files.Sum(file => CountLinesCS(file));
            Console.WriteLine(lines);
        }
    }
}
