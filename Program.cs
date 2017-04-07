using System;
using System.IO;

namespace CodingTheory
{
    internal class Program
    {
        private static IAlgorithm _algorithm;

        private static void Main(string[] args)
        {
            string input = "";
            do
            {
                PrintMenu();
                input = Console.ReadLine();
                _algorithm = ParseAlgorithm(input);
                if (_algorithm == null)
                {
                    continue;
                }

                string source = ReadTextFromFile();
                if (source == "")
                {
                    Console.WriteLine("Ошибка! Введите кодируемую строку в файл input.txt");
                    Console.ReadKey();
                    return;
                }
                string encodeString = _algorithm.Encode(source);
                Console.WriteLine("Исходная строка:");
                Console.WriteLine(source);
                Console.WriteLine("Закодированная строка:");
                Console.WriteLine(encodeString);
                Console.WriteLine("Полученный коэффицент сжатия: {0:0.000}", _algorithm.GetCompressionRatio());
                Console.WriteLine("Раскодированная строка:");
                Console.WriteLine(_algorithm.Decode(encodeString));
                Console.ReadKey();
            } while (input != "q");
        }

        private static void PrintMenu()
        {
            Console.WriteLine("Выберите способ кодирования:");
            Console.WriteLine("1. Кодирование Хаффмана");
            Console.WriteLine("2. Кодирование Шеннона-Фано");
        }

        private static IAlgorithm ParseAlgorithm(string input)
        {
            int id;
            if (!int.TryParse(input, out id))
            {
                return null;
            }

            switch (id)
            {
                case 1:
                    return new HuffmanCoding();
                case 2:
                    return new ShannonFanoCoding();
            }

            return null;
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
