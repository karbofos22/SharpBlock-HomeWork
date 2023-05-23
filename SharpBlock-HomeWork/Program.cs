using System.Reflection;

namespace SharpBlock_HomeWork
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TextProcessor textProcessor = new();

            textProcessor.ProcessFile("dostoewskij"); //tolstoy - доп. файл для проверки
        }
    }
}