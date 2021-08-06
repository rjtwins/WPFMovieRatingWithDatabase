using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace MovieRatingWithDatabase
{
    public class DataController : IDataController
    {
        private HttpClient Client = new HttpClient();

        public DataController() : base()
        {
            this.LoadPasswordFile();
            base.DatabaseInterface = new SQLDatabaseInterface();
        }

        public override void LoadPasswordFile()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            try
            {
                UTILS.Passwords = JsonConvert.DeserializeObject<Passwords>(File.ReadAllText(baseDir + @"Passwords.end"));
            }
            catch (Exception e)
            {
                throw new Exception("Password File could not be found or could not be loaded at: " +
                    baseDir + @"Passwords.end" +
                    "\n" + e.Message +
                    "\n" + e.StackTrace);
            }
        }

        public override Result SearchWebByID(string id)
        {
            return new Result();
        }

        public override List<Result> SearchWebByTitle(string title)
        {
            if (StopSearchFlag)
            {
                return null;
            }

            title = Uri.EscapeDataString(title);
            List<Result> Results = null;
            try
            {
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri("https://imdb8.p.rapidapi.com/title/find?q=" + title),
                    Headers =
                {
                    //Ofcourse you would read your key for somewhere and keep it secret bu this is just for testing.
                    { "x-rapidapi-key", UTILS.Passwords.xrapidapikey },
                    { "x-rapidapi-host", "imdb8.p.rapidapi.com" },
                },
                };
                Root root = MakeCallAsync(request).Result;
                Results = root.results;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                Debug.WriteLine(e.StackTrace);
            }

            if (Results == null)
                return null;
            SanitizeResults(Results);
            AddBitmapsToResults(Results);

            ////testing sql stuff
            //ResultDataSet.resultDataTable table = new ResultDataSet.resultDataTable();
            //foreach(Result r in Results)
            //{
            //    PutDataInRow(r);
            //}
            //DatabaseInterface.FillDataBase(this.DataSet.result);

            return Results;
        }

        private static void ProcessIDString(List<Result> input)
        {
            foreach (Result r in input)
            {
                string id = r.id;
                string[] splitId = r.id.Split('/');
                r.id = splitId[splitId.Length - 2];
            }
        }

        private static void RemoveNoMovieOrTv(List<Result> input)
        {
            input.RemoveAll(r => (r.titleType != "tvSeries" && r.titleType != "movie"));
        }

        private void AddBitmapsToResults(List<Result> results)
        {
            if (StopSearchFlag)
            {
                return;
            }

            foreach (Result r in results)
            {
                if (StopSearchFlag)
                {
                    return;
                }

                if (r.image == null)
                {
                    continue;
                }

                if (string.IsNullOrEmpty(r.image.url))
                {
                    continue;
                }
                r.bitMap = GetBitMapFromURL(r.image.url).Result;
                r.imageUrl = r.image.url;
            }
        }

        private async Task<Root> MakeCallAsync(HttpRequestMessage request)
        {
            //We use task.wait() here to prevent a deadlock where both methods are waiting for each other to finish.
            string body;
            var task = Client.SendAsync(request);
            task.Wait();
            var response = task.Result;

            response.EnsureSuccessStatusCode();

            body = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrEmpty(body))
            {
                return null;
            }

            return JsonConvert.DeserializeObject<Root>(body);
        }

        private void SanitizeResults(List<Result> input)
        {
            if (StopSearchFlag)
            {
                return;
            }
            RemoveNoMovieOrTv(input);
            ProcessIDString(input);
        }

    }
}