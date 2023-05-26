using StopWord;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;

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
    }
}
