using FrequentWordConsole;
using System.Collections.Generic;
using System.Web.Http;

namespace FrequentWordFinderAPI.Controllers
{
    public class WordRankerController : ApiController
    {
        public string Get(string url)
        {
            if (url.Length > 0)
            {
                string result = DynamoDbActions.RetrieveItem(url);

                if (result.Length == 0)
                {
                    result = Rank(url);
                    bool success = DynamoDbActions.AddItem(url, result);

                    if (!success) result += "\r\n" + "Error saving result to table";
                }

                return result;

            }
            return "Invalid URL";
        }

        public string Rank(string url)
        {
            if (url.Length > 0)
            {
                URLFetcher uf = new URLFetcher();
                string urlText = uf.GetURLText(url);

                if (urlText.Length > 0)
                {
                    string returnString;
                    WordCollector wc = new WordCollector();
                    char[] delimeters = { ' ' };
                    Dictionary<string, long> words = wc.GetWords(urlText, delimeters);

                    if (words.Keys.Count > 0)
                    {
                        WordRanker wr = new WordRanker();
                        List<KeyValuePair<string, long>> rankedWords = wr.RankWords(words, 10);

                        returnString = "Top 10 Words:" + "\r\n";

                        foreach (KeyValuePair<string, long> word in rankedWords)
                        {
                            returnString += $"Word: {word.Key}  Count: {word.Value}" + "\r\n";
                        }

                        return returnString;
                    }
                    else
                    {
                        return "No words retrieved from URL: " + url;
                    }
                }
                else
                {
                    return "No text retrieved from URL: " + url;
                }


            }
            else
            {
                return "Invalid URL";
            }
        }
    }
    
}
