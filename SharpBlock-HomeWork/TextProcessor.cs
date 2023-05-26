using StopWord;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

namespace SharpBlock_HomeWork
{
    public class TextProcessor
    {
        public Dictionary<string, int> CountUniqueWords(string inputFile)
        {
            if (inputFile != null)
            {
                var stopWords = StopWords.GetStopWords("ru");
                Stopwatch stopWatch = new();
                stopWatch.Start();

                Type classType = typeof(UniqueWordsCounterLibrary.Counter);

                MethodInfo? method = classType.GetMethod("ProcessFile", BindingFlags.NonPublic | BindingFlags.Static);

                Dictionary<string, int> uniqueWords = (Dictionary<string, int>)method.Invoke(null, new object[] { inputFile, stopWords });

                stopWatch.Stop();
                GetElapsedTime(stopWatch, "CountUniqueWords");

                return uniqueWords;
            }
            else
            {
                throw new ArgumentException("Invalid text file", nameof(inputFile));
            }
        }

        public Dictionary<string, int> ParallelCountUniqueWords(string inputFile)
        {
            if (inputFile != null)
            {
                var stopWords = StopWords.GetStopWords("ru");
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();

                Dictionary<string, int> uniqueWords = UniqueWordsCounterLibrary.Counter.ParallelProcessFile(inputFile, stopWords);

                stopWatch.Stop();
                GetElapsedTime(stopWatch, "ParallelCountUniqueWords");

                return uniqueWords;
            }
            else
            {
                throw new ArgumentException("Invalid text file", nameof(inputFile));
            }
        }
        private void GetElapsedTime(Stopwatch stopWatch, string methodName)
        {
            TimeSpan ts = stopWatch.Elapsed;

            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);
            Console.WriteLine($"RunTime of {methodName} is " + elapsedTime);
        }

        public async Task CountUniqueWordsViaAPI(string inputFile)
        {
            using (var client = new HttpClient())
            {
                string apiUrl = "https://localhost:7003/UniqueWordsCounter/Counter";

                var formData = new MultipartFormDataContent
                {
                    { new StringContent(inputFile), "text" }
                };

                try
                {
                    HttpResponseMessage response = await client.PostAsync(apiUrl, formData);

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var dictionary = JsonSerializer.Deserialize<Dictionary<string, int>>(content);

                        foreach (var item in dictionary)
                        {
                            Console.WriteLine($"{item.Key,-18}\t{item.Value}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Ошибка вызова API: " + response.StatusCode);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка вызова API: " + ex.Message);
                }
            }

            Console.ReadLine();
        }
    }
}

