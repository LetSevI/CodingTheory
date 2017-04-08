using System;
using System.IO;

namespace CodingTheory
{
    internal class Program
    {
        private static IAlgorithm _algorithm;
        private static IAlgorithm[] _algorithmsList = { new HuffmanCoding(), new ShannonFanoCoding() };

        private static void Main(string[] args)
        {
            string input = "";
            do
            {
                PrintMenu();
                input = Console.ReadLine();
                string source = ReadTextFromFile();
                if (source == "")
                {
                    Console.WriteLine("Ошибка! Введите кодируемую строку в файл input.txt");
                    Console.ReadKey();
                    continue;
                }
                if (input == "10")
                {
                    Console.WriteLine("Сравнение коэффицентов сжатия:");
                    foreach (var alg in _algorithmsList)
                    {
                        alg.Encode(source);
                        Console.WriteLine("{0}: {1:0.000}", alg.GetName(), alg.GetCompressionRatio());
                    }
                    Console.WriteLine();
                    continue;
                }
                _algorithm = ParseAlgorithm(input);
                if (_algorithm == null)
                {
                    continue;
                }

                string encodeString = _algorithm.Encode(source);
                Console.WriteLine("Исходная строка:");
                Console.WriteLine(source);
                Console.WriteLine("Закодированная строка:");
                Console.WriteLine(encodeString);
                Console.WriteLine("Полученный коэффицент сжатия: {0:0.000}", _algorithm.GetCompressionRatio());
                Console.WriteLine("Раскодированная строка:");
                Console.WriteLine(_algorithm.Decode(encodeString));
                Console.WriteLine();
            } while (input != "q");
        }

        private static void PrintMenu()
        {
            Console.WriteLine("Выберите способ кодирования:");
            for (int i = 0; i < _algorithmsList.Length; i ++)
            {
                Console.WriteLine("{0}. {1}", i+1, _algorithmsList[i].GetName());
            }
            Console.WriteLine("10. Сравнение коэффицентов сжатия");
        }

        private static IAlgorithm ParseAlgorithm(string input)
        {
            int id;
            if (!int.TryParse(input, out id) || (id < 1 || id > _algorithmsList.Length))
            {
                return null;
            }
            return _algorithmsList[id - 1];
        }

        private static string ReadTextFromFile(string fileName = "input.txt")
        {
            using (StreamReader streamReader = new StreamReader(fileName))
            {
                return streamReader.ReadToEnd();
            }
        }
    }
}