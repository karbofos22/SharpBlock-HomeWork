using StopWord;
using System;
using System.Reflection;
using System.Text.RegularExpressions;

namespace SharpBlock_HomeWork
{
    public class TextProcessor
    {
        private readonly string FilesDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\Files");
        private string fileName = "";

        public void ProcessFile(string inputFileName)
        {
            fileName = inputFileName;

            string text = ReadTextFromFile(inputFileName);

            if (text != null)
            {
                var textToProcess = PrepareText(text);

                var processedText = CountUniqueWords(textToProcess);

                WriteResultToFile(processedText);
            }
        }

        private string ReadTextFromFile(string inputFileName)
        {
            if (string.IsNullOrWhiteSpace(inputFileName))
            {
                throw new ArgumentException("Invalid input file path.", nameof(inputFileName));
            }

            try
            {
                string inputFile = Path.Combine(FilesDirectory, inputFileName + ".fb2");
                return File.ReadAllText(inputFile);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while reading the file: {ex.Message}");
            }
        }

        private string[] PrepareText(string text)
        {
            text = Regex.Replace(text, "<.*?>", string.Empty);
            text = Regex.Replace(text, @"[^\p{L}\p{N}\s]", string.Empty);

            string[] separators = { " ", ",", ".", ";", ":", "-", "!", "?", "\r", "\n", "\t" };
            var stopWords = StopWords.GetStopWords("ru");

            List<string> filteredWords = new();

            string[] words = text.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            Regex russianRegex = new Regex(@"^[\p{IsCyrillic}\s]+$");

            foreach (string word in words)
            {
                string cleanedWord = word.ToLowerInvariant().Trim();

                if (russianRegex.IsMatch(cleanedWord) && !stopWords.Contains(cleanedWord))
                {
                    filteredWords.Add(cleanedWord);
                }
            }

            return filteredWords.ToArray();
        }

        private Dictionary<string, int> CountUniqueWords(string[] words)
        {
            if (words != null)
            {
                Type classType = typeof(UniqueWordsCounterLibrary.UniqueWordsCounter);

                MethodInfo? method = classType.GetMethod("ProcessFile", BindingFlags.NonPublic | BindingFlags.Static);

                Dictionary<string, int> uniqueWords = (Dictionary<string, int>)method.Invoke(null, new object[] { words });

                return uniqueWords;
            }
            else
            {
                throw new ArgumentException("Invalid text file", nameof(words));
            }
        }

        private void WriteResultToFile(Dictionary<string, int> processedFiled)
        {
            var sortedWords = processedFiled.OrderByDescending(pair => pair.Value);

            string outputPath = Path.Combine(FilesDirectory, $"{fileName}_unique_words.txt");

            using (StreamWriter writer = new(outputPath))
            {
                foreach (var pair in sortedWords)
                {
                    writer.WriteLine($"{pair.Key,-18}\t{pair.Value}");
                }
            }
        }
    }
}
