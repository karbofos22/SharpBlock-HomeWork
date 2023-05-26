using System.Reflection;
using System.Text.Json;

namespace SharpBlock_HomeWork
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            string text = "tolstoy"; // dostoewskij или просто текст

            TextProcessor textProcessor = new();

            textProcessor.CountUniqueWords(text); 
            textProcessor.ParallelCountUniqueWords(text);

            await textProcessor.CountUniqueWordsViaAPI(text);
        }
    }
}