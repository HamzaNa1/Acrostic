// \/\/\/\__Acrostic__/\/\/\/
// Made by Hamza
// Credit to https://github.com/BenVlodgi for the Console (its useful if u want a colorful console, renamed it to ExtraConsole)
// Credit to http://developer.wordnik.com/ for the API (OLD API)
// Credit to http://poetrydb.org/ for the poetry API

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
            List<string[]> Authors = new List<string[]>();
            Console.Clear();
            ExtraConsole.WriteLine("<f=red>LOADING....");
            ExtraConsole.WriteLine("<f=red>0/8");
            Authors.Add(GetWords("http://poetrydb.org/author/Shakespeare/lines"));
            Console.Clear();
            ExtraConsole.WriteLine("<f=red>LOADING....");
            ExtraConsole.WriteLine("<f=red>1/8");
            Authors.Add(GetWords("http://poetrydb.org/author/Emily%20Dickinson/lines"));
            Console.Clear();
            ExtraConsole.WriteLine("<f=red>LOADING....");
            ExtraConsole.WriteLine("<f=red>2/8");
            Authors.Add(GetWords("http://poetrydb.org/author/Alan%20Seeger/lines"));
            Console.Clear();
            ExtraConsole.WriteLine("<f=red>LOADING....");
            ExtraConsole.WriteLine("<f=red>3/8");
            Authors.Add(GetWords("http://poetrydb.org/author/Adam%20Lindsay%20Gordon/lines"));
            Console.Clear();
            ExtraConsole.WriteLine("<f=red>LOADING....");
            ExtraConsole.WriteLine("<f=red>4/8");
            Authors.Add(GetWords("http://poetrydb.org/author/Alexander%20Pope/lines"));
            Console.Clear();
            ExtraConsole.WriteLine("<f=red>LOADING....");
            ExtraConsole.WriteLine("<f=red>5/8");
            Authors.Add(GetWords("http://poetrydb.org/author/William%20Vaughn%20Moody/lines"));
            Console.Clear();
            ExtraConsole.WriteLine("<f=red>LOADING....");
            ExtraConsole.WriteLine("<f=red>6/8");
            Authors.Add(GetWords("http://poetrydb.org/author/McGonagall/lines"));
            Console.Clear();
            ExtraConsole.WriteLine("<f=red>LOADING....");
            ExtraConsole.WriteLine("<f=red>7/8");
            Authors.Add(GetWords("http://poetrydb.org/author/William%20Wordsworth/lines"));
            Console.Clear();
            ExtraConsole.WriteLine("<f=red>LOADING....");
            ExtraConsole.WriteLine("<f=red>8/8");

            Console.Clear();
            ExtraConsole.WriteLine("<f=darkgreen>DONE!");

            Console.WriteLine();
            ExtraConsole.WriteLine("<f=darkgreen>Welcome to my Acrostic poem generator.");
            ExtraConsole.WriteLine("<f=darkgreen>Write a word and click <f=red>ENTER<f=darkgreen> to start.");


            while (true)
            {
                B:
                Console.WriteLine();
                string Text = Console.ReadLine();
                Console.WriteLine();

                int Trys = 0;
                int index = -1;
                int tottalspacing = 0;

                List<string> wordsList = new List<string>();
                List<string> unspacedWordsList = new List<string>();

                foreach (char c in Text)
                {
                    G:
                    if (Trys == 100000)
                    {
                        ExtraConsole.WriteLine("<f=red> a letter is not found.");
                        goto B;
                    }
                    Trys++;
                    string[] Words;
                    PickRandomAuthor(out Words, Authors);

                    Random r = new Random(DateTime.Now.Millisecond);
                    string word = Words[r.Next(Words.Length)];
                    if (c == ' ') word = " ";
                    else if (c == '.') word = ".";
                    else
                    {

                        if (word != null)
                        {
                            word = word.Replace("[", "");
                            word = word.Replace("]", "");
                            word = word.Replace("\"", "");
                            word = word.Replace(",", "");


                            List<string> poetry = word.Split(new string[] { "\r\n", "\n", Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();

                            int changeAbleIndex = -1;

                            for(int trys = 0; trys < poetry.Count; trys++)
                            {
                                if (poetry[trys].Length < 40 && !Contains(unspacedWordsList, poetry[trys]))
                                {
                                    bool isDone = false;
                                    for (int i = 0; i < poetry[trys].Length; i++)
                                    {
                                        if (poetry[trys][i] == c)
                                        {
                                            changeAbleIndex = i;
                                            unspacedWordsList.Add(poetry[trys]);
                                            word = poetry[trys];
                                            if (index == -1)
                                            {
                                                index = i + "                                ".Length;
                                                word = word + "                                ";
                                            }
                                            isDone = true;
                                            Trys = 0;
                                            break;
                                        }
                                    }
                                    if (isDone)
                                    {
                                        break;
                                    }
                                }
                                else
                                {
                                    goto G;
                                }
                                poetry.Remove(poetry[trys]);
                            }

                            if (index == -1 || changeAbleIndex == -1)
                            {
                                goto G;
                            }

                            if (changeAbleIndex < index)
                            {
                                word = Spacing(index - changeAbleIndex) + word;
                            }
                            else if (changeAbleIndex > index)
                            {
                                for (int i = 0; i < wordsList.Count; i++)
                                {
                                    var spacing = Spacing(changeAbleIndex - index);
                                    wordsList[i] = spacing + wordsList[i];
                                    tottalspacing += changeAbleIndex - index;
                                }
                            }

                            wordsList.Add(word);
                        }
                        else
                        {
                            ExtraConsole.WriteLine($"<f=red>an ERROR has occure, please make sure you are using ENGLISH.");
                            break;
                        }
                    }
                }
                index += tottalspacing;
                foreach (string word in wordsList)
                {
                    if (word != " " || word != ".")
                    {
                        try
                        {
                            ExtraConsole.Write($"<f=darkgreen>{word.Substring(0, index)}");
                            ExtraConsole.Write($"<f=red>{word.Substring(index, 1)}");
                            ExtraConsole.WriteLine($"<f=darkgreen>{word.Substring(index + 1, word.Length - (index + 1))}");
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("");
                        }
                    }
                    else if (word == ".")
                    {
                        ExtraConsole.WriteLine("<f=darkgreen>.");
                    }
                    else
                    {
                        ExtraConsole.WriteLine("");
                    }
                }

                Console.WriteLine();
                ExtraConsole.WriteLine("<f=darkgreen>If u want to try again, write the word and click <f=red>ENTER<f=darkgreen> to start.");
            }
        }

        public static bool Contains(List<string> list, string word)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] == word) return true;
            }
            return false;
        }

        public static string[] GetWords(string url)
        {
            retry:
            string JSON = null;
            List<string> lines = new List<string>();

            try
            {
                while (JSON == null) JSON = GET(url);
                JSON = "{\"searchResults\":" + JSON + "}";
                dynamic data = JObject.Parse(JSON);
                JArray array = new JArray(data.searchResults);

                foreach (JObject o in array.Children<JObject>())
                {
                    dynamic data1 = o;
                    JArray array1 = new JArray(data1.lines);
                    foreach (JObject o1 in array.Children<JObject>())
                    {
                        foreach (JProperty p in o1.Properties())
                        {
                            lines.Add(p.Value.ToString());
                            p.Value = p.Value.ToString().Replace("[", "");
                            p.Value = p.Value.ToString().Replace("]", "");
                            p.Value = p.Value.ToString().Replace("\"", "");
                            p.Value = p.Value.ToString().Replace(",", "");
                        }
                    }
                }
            }
            catch (Exception)
            {
                ExtraConsole.WriteLine($"<f=red>LOADING FAILED PLEASE MAKE SURE YOUR INTERNET CONNECTION IS OK");
                ExtraConsole.WriteLine($"<f=red>Retrying...");
                Console.WriteLine();
                goto retry;
            }

            try
            {
                return lines.ToArray();
            }
            catch (Exception)
            {
                return null;
            }
        }


        static string Spacing(int amount)
        {
            string spaces = "";

            for (int i = 0; i < amount; i++)
            {
                spaces += " ";
            }

            return spaces;
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

        public static void PickRandomAuthor(out string[] author, List<string[]> Authors)
        {
            author = Authors[new Random().Next(0, Authors.Count)];
        }
    }
}