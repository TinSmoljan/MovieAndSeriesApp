using Microsoft.AspNetCore.Mvc;
using MovieAndSeriesScanner.Common.Api.Dto;
using MovieAndSeriesScanner.Services.Api.Services;

namespace MovieAndSeriesScanner.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class MovieController : ControllerBase
	{
		private readonly IMovieService _movieService;

		public MovieController(IMovieService movieService)
		{
			_movieService = movieService;
		}

		[HttpGet("Movies")]
		public async Task<IActionResult> GetMovies([FromQuery] DiscoverMoviesParametersRequest parameters)
		{
			var movies = await _movieService.GetMoviesAsync(parameters);
			return Ok(movies);
		}
	}
}
