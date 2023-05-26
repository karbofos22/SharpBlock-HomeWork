using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace UniqueWordsCounterLibrary
{
    public class Counter
    {
        private static readonly string FilesDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\Files");
        private static string fileName = "";

        private static string ReadTextFromFile(string inputFileName)
        {
            if (string.IsNullOrWhiteSpace(inputFileName))
            {
                throw new ArgumentException("Invalid input file path.", nameof(inputFileName));
            }

            try
            {
                string inputFile = Path.Combine(FilesDirectory, inputFileName + ".fb2");

                if (File.Exists(inputFile))
                {
                    fileName = inputFileName;
                    return File.ReadAllText(inputFile);
                }
                else
                {
                    return inputFileName;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while reading the file: {ex.Message}");
            }
        }

        private static void WriteResultToFile(Dictionary<string, int> processedFiled)
        {
            string outputPath = Path.Combine(FilesDirectory, $"{fileName}_unique_words.txt");

            using (StreamWriter writer = new(outputPath))
            {
                foreach (var pair in processedFiled)
                {
                    writer.WriteLine($"{pair.Key,-18}\t{pair.Value}");
                }
            }
        }

        private static string[] PrepareText(string text, string[] stopWords)
        {
            text = Regex.Replace(text, "<.*?>", string.Empty);
            text = Regex.Replace(text, @"[^\p{L}\p{N}\s]", string.Empty);

            string[] separators = { " ", ",", ".", ";", ":", "-", "!", "?", "\r", "\n", "\t" };

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

        private static Dictionary<string, int> ProcessFile(string inputFile, string[] stopWords)
        {
            string text = ReadTextFromFile(inputFile);

            var textToProcess = PrepareText(text, stopWords);

            if (textToProcess == null)
            {
                throw new ArgumentNullException(nameof(textToProcess));
            }

            Dictionary<string, int> uniqueWords = new();

            foreach (string word in textToProcess)
            {
                if (uniqueWords.ContainsKey(word))
                {
                    uniqueWords[word]++;
                }
                else
                {
                    uniqueWords[word] = 1;
                }
            }

            var sortedWords = uniqueWords.OrderByDescending(pair => pair.Value)
                .ToDictionary(pair => pair.Key, pair => pair.Value);

            if (fileName != "")
            {
                WriteResultToFile(sortedWords);
            }

            return sortedWords;
        }

        public static Dictionary<string, int> ParallelProcessFile(string inputFileName, string[] stopWords)
        {
            string text = ReadTextFromFile(inputFileName);

            var textToProcess = PrepareText(text, stopWords);

            if (textToProcess == null)
            {
                throw new ArgumentNullException(nameof(textToProcess));
            }

            ConcurrentDictionary<string, int> uniqueWords = new ConcurrentDictionary<string, int>();

            Parallel.ForEach(textToProcess, word =>
            {
                uniqueWords.AddOrUpdate(word, 1, (_, count) => count + 1);
            });

            var sortedWords = uniqueWords.OrderByDescending(pair => pair.Value)
                .ToDictionary(pair => pair.Key, pair => pair.Value);

            if (fileName != "")
            {
                WriteResultToFile(sortedWords);
            }

            return sortedWords;
        }
    }
}
