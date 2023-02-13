using Movie.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movie.Application
{
    public class MovieService : IMovieService
    {

        private readonly IMovieRepository _movieService;

        public MovieService(IMovieRepository movieRepository)
        {
            _movieService = movieRepository;
        }

        public TMovie CreateMovie(TMovie movie)
        {
            return _movieService.CreateMovie(movie);
        }


        public List<TMovie> GetAllMovies()
        {
            var movies = _movieService.GetAllMovies();
            
            return movies;
        }
    }
}
