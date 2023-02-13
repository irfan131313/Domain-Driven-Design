using Movie.Application;
using Movie.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movie.Infrastructure
{
    public class MovieRepository : IMovieRepository
    {


        private readonly MovieDbContext _movieDbContext;

        public MovieRepository(MovieDbContext movieDbContext)
        {
            _movieDbContext = movieDbContext;
        }
        public TMovie CreateMovie(TMovie movie)
        {
            _movieDbContext.Movies.Add(movie);
            _movieDbContext.SaveChanges();

            return movie;
        }

        public List<TMovie> GetAllMovies()
        {
            return _movieDbContext.Movies.ToList();
        }

    }
}
