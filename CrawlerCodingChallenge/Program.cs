using HtmlAgilityPack;
using System;
using System.Linq;
using System.Net.Http;

namespace CrawlerCodingChallenge
{
    class Program
    {
        private static async void GetHistoryHtmlAsync(string excludedWords, int maxWords)
        {
            var url = "https://en.wikipedia.org/wiki/Microsoft";

            var httpClient = new HttpClient();
            var html = await httpClient.GetStringAsync(url);

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            var historyDescription = "";

            int count = 0;
            int maxWordsDefault = 10;

            //Checks if user int input > 0
            if (maxWordsDefault != maxWords && maxWords > 0)
            {
                maxWordsDefault = maxWords;
            }

            string[] excludedWordsList = excludedWords.Split(' ');

            //For loop using i to parse Xpath paragraphs from 7-31 which belong to the history section
            for (int i = 7; i < 32; i++)
            {
                var paragraph = htmlDocument.DocumentNode.SelectNodes("//*[@id=\"mw-content-text\"]/div[1]/p[" + i + "]").First().InnerText;
                historyDescription += paragraph;
            }

            var results = historyDescription.Split(' ').GroupBy(x => x).Select(x => new { Count = x.Count(), Word = x.Key }).OrderByDescending(x => x.Count);

            //Checks if each word found within results is in the excluded word list; excludes word if true
            //Returns the max number of words specified and number of occurrences
            foreach (var item in results)
                if ((!excludedWordsList.Contains(item.Word.ToLower())) && count < maxWordsDefault)
                {
                    Console.WriteLine(String.Format("{0} occured {1} times", item.Word, item.Count));
                    count++;
                }
        }
        static void Main(string[] args)
        {

            Console.WriteLine("Please enter the words to be excluded: ");
            string excludedWords = Console.ReadLine().ToLower();

            Console.WriteLine("Please enter max number of words to be returned: ");
            int maxWords = Convert.ToInt32(Console.ReadLine());

            GetHistoryHtmlAsync(excludedWords, maxWords);
            Console.ReadLine();
        }
    }
}
