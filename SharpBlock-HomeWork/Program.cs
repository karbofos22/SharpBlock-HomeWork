using System.Reflection;

namespace SharpBlock_HomeWork
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TextProcessor textProcessor = new();

            textProcessor.ProcessFile("tolstoy"); //tolstoy или dostoewskij - имена файлов для проверки
        }
    }
}