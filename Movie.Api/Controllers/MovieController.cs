using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movie.Application;
using Movie.Domain;

namespace Movie.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MovieController : ControllerBase
    {

        private readonly IMovieService _movieService;
        public MovieController(IMovieService service)
        {
            _movieService = service;
        }


        [AllowAnonymous]
        [HttpGet("GetMovies")]
        public IActionResult Movies()
        {
            var movies = _movieService.GetAllMovies();
            return Ok(movies);

        }
        [AllowAnonymous]
        [HttpPost("AddMovie")]
        public IActionResult AddMovie(TMovie movie)
        {
            var movies = _movieService.CreateMovie(movie);

            return Ok();
        }




    }
}
