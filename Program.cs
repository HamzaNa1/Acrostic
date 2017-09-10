// \/\/\/\__Acrostic__/\/\/\/
// Made by Hamza
// Credit to https://github.com/BenVlodgi for the Console (its useful if u want a colorful console, renamed it to ExtraConsole)
// Credit to http://developer.wordnik.com/ for the API

using System;
using System.Net; // To get the JSON API
using System.IO; // To get the JSON API
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq; // To pick the words out of the JSON API

namespace Acrostic
{
    class Program
    {
        static void Main(string[] args)
        {
            ExtraConsole.WriteLine("<f=darkgreen>Welcome to my Acrostic poem generator (its not really a poem) program. #TODO make it looks more like a poem.");
            ExtraConsole.WriteLine("<f=darkgreen>Write a word and click <f=red>ENTER<f=darkgreen> to start.");
            while (true)
            {
                Console.WriteLine();
                var Words = Console.ReadLine();
                Console.WriteLine();

                foreach (char c in Words)
                {
                    string word = null;
                    if (c == ' ') word = " ";
                    else if (c == '.') word = ".";

                    if (word == null) word = GetWords("http://api.wordnik.com:80/v4/words.json/search/" + c + "?caseSensitive=true&minCorpusCount=0&maxCorpusCount=-1&minDictionaryCount=1&maxDictionaryCount=-1&minLength=4&maxLength=-1&skip=1&limit=-1&api_key=a2a73e7b926c924fad7001ca3111acd55af2ffabf50eb4ae5");

                    if (word != null)
                    {
                        ExtraConsole.Write($"<f=red>{word.Substring(0, 1)}");
                        ExtraConsole.WriteLine($"<f=darkgreen>{word.Substring(1, word.Length - 1)}");
                    } else
                    {
                        ExtraConsole.WriteLine($"<f=red>an ERROR has occure, please make sure you are using ENGLISH.");
                        goto G;
                    }
                }
                
                G:
                Console.WriteLine();
                ExtraConsole.WriteLine("<f=darkgreen>If u want to try again, write the word and click <f=red>ENTER<f=darkgreen> to start.");
            }
        }

        public static string GetWords(string url)
        {
            string JSON = null;
            List<string> words = new List<string>();

            try
            {
                while (JSON == null) JSON = GET(url);
                dynamic data = JObject.Parse(JSON);
                JArray array = new JArray(data.searchResults);
                foreach (JObject o in array.Children<JObject>())
                {
                    foreach (JProperty p in o.Properties())
                    {
                        if (p.Name == "word")
                        {
                            words.Add(p.Value.ToString());
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ExtraConsole.WriteLine($"<f=red>{e}");
            }

            try
            {
                return words[new Random().Next(words.Count)];
            }
            catch (Exception)
            {
                return null;
            }
        }


        static string GET(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            try
            {
                WebResponse response = request.GetResponse();
                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    return reader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                WebResponse errorResponse = ex.Response;
                using (Stream responseStream = errorResponse.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
                    String errorText = reader.ReadToEnd();
                    // log errorText
                }
                throw;
            }
        }
    }
}