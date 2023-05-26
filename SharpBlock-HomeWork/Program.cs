using System.Reflection;

namespace SharpBlock_HomeWork
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TextProcessor textProcessor = new();

            textProcessor.CountUniqueWords("tolstoy"); //tolstoy или dostoewskij - имена файлов для проверки
            textProcessor.ParallelCountUniqueWords("tolstoy"); //tolstoy или dostoewskij - имена файлов для проверки
        }
    }
}