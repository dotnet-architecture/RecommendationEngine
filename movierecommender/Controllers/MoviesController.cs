using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using movierecommender.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace movierecommender.Controllers
{
    public class StringTable
    {
        public string[] ColumnNames { get; set; }
        public string[,] Values { get; set; }
    }

    public class MoviesController : Controller
    {
        private readonly MovieService _movieService;
        private readonly AppSettings _appSettings;
        //private static HttpClient Client = new HttpClient();
        private readonly ILogger<MoviesController> _logger;

        public MoviesController(MovieService movieService, ILogger<MoviesController> logger, IOptions<AppSettings> appSettings)
        {
            _movieService = movieService;
            _logger = logger;
            _appSettings = appSettings.Value;
        }

        public ActionResult Choose()
        {
            return View(_movieService.GetSomeSuggestions());
        }

        static async Task<string> InvokeRequestResponseService(int id, ILogger logger, AppSettings appSettings)
        {
            using (var client = new HttpClient())
            {
                var scoreRequest = new
                {

                    Inputs = new Dictionary<string, StringTable>() {
                        {
                            "input1",
                            new StringTable()
                            {
                                ColumnNames = new string[] {"UserID", "MovieID", "Rating"},
                                Values = new string[,] {  { "0", id.ToString(), "5" } }
                            }
                        },
                    },
                    GlobalParameters = new Dictionary<string, string>()
                    {
                    }
                };

                string apiKey = appSettings.apikey;
                string uri = appSettings.uri; 
                if (apiKey != string.Empty && uri != string.Empty)
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
                    client.BaseAddress = new Uri(uri);

                    HttpResponseMessage response = await client.PostAsync(appSettings.uri, new JsonContent(scoreRequest));

                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();
                        return result;
                    }
                    else
                    {
                        logger.LogError(string.Format("The request failed with status code: {0}", response.StatusCode));
                        // Print the headers - they include the requert ID and the timestamp,
                        // which are useful for debugging the failure
                        logger.LogDebug(response.Headers.ToString());

                        string responseContent = await response.Content.ReadAsStringAsync();
                        logger.LogDebug(responseContent);
                        return null;
                    }
                } else
                {
                    logger.LogError("Please provide apiKey and URI to the Machine Learning WebService");
                }

                return null; 
        }
}

        public async Task<ActionResult> Recommend(int id)
        {
            try
            {
                var result = await InvokeRequestResponseService(id, _logger, _appSettings);
                var obj = JObject.Parse(result);
                var suggestedMovies = obj["Results"]["output1"]["value"]["Values"][0]
                        .Select(i => i.Value<string>())
                        .Select(i => int.Parse(i))
                        .Select(i => _movieService.Get(i));
                return View(suggestedMovies);
            }
            catch (Exception e)
            {
                _logger.LogError(string.Format("An error occured:'{0}'", e));
                return View(); 
            }
         }

        public ActionResult Watch()
        {
            return View();
        }

        public class JsonContent : StringContent
        {
            public JsonContent(object obj) :
                base(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json")
            { }
        }
    }
}