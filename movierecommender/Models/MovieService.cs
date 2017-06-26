using CsvHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;

namespace movierecommender.Models
{
    public partial class MovieService
    {
        public readonly static int _moviesToRecommend = 6;
        public Lazy<List<Movie>> _movies = new Lazy<List<Movie>>(() => LoadMovieData());

        public IEnumerable<Movie> GetSomeSuggestions()
        {
            var movies = GetRecentMovies().ToArray();

            Random rnd = new Random();
            int[] movieselector = new int[_moviesToRecommend];
            for (int i = 0; i < _moviesToRecommend; i++)
            {
                movieselector[i] = rnd.Next(movies.Length);
            }

            return movieselector.Select(s => movies[s]);
        }

        public IEnumerable<Movie> GetRecentMovies()
        {
            return GetAllMovies()
                .Where(m => m.MovieName.Contains("20")
                            || m.MovieName.Contains("198")
                            || m.MovieName.Contains("199"));
        }

        public Movie Get(int id)
        {
            return _movies.Value.Single(m => m.MovieID == id);
        }


        public IEnumerable<Movie> GetAllMovies()
        {
            return _movies.Value;
        }

        private static List<Movie> LoadMovieData()
        {
            var result = new List<Movie>();

            // Use this for the 20M dataset. 
            //Stream fileReader = File.OpenRead("Content/MovieLens 20M dataset _movies.csv");

            Stream fileReader = File.OpenRead("Content/MovieLens 1M dataset _movies.csv");
            StreamReader reader = new StreamReader(fileReader);
            try
            {
                bool header = true;
                int index = 0;
                var line = "";
                while (!reader.EndOfStream)
                {
                    if (header)
                    {
                        line = reader.ReadLine();
                        header = false;
                    }
                    line = reader.ReadLine();
                    string[] fields = line.Split(',');
                    int MovieID = Int32.Parse(fields[0].ToString().TrimStart(new char[] { '0' }));
                    string MovieName = fields[1].ToString();
                    result.Add(new Movie() { MovieID = MovieID, MovieName = MovieName });
                    index++;
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Dispose(); 
                }
            }

            return result;
        }
    }
}