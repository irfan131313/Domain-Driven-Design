using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Movie.Domain;

namespace Movie.Application
{
    public interface IMovieRepository
    {

        List<TMovie> GetAllMovies();
        TMovie CreateMovie(TMovie movie);

    }
}
