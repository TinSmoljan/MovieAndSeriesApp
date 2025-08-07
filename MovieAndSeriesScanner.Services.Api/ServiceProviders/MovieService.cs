using MovieAndSeriesScanner.Common.Api.Dto;
using MovieAndSeriesScanner.Services.Api.Services;

namespace MovieAndSeriesScanner.Services.Api.ServiceProviders
{
	public class MovieService : IMovieService
	{
		private readonly ITmdbHttpService _tmdbHttpService;
		private readonly ICacheService _cacheService;

		public MovieService(ITmdbHttpService tmdbHttpService, ICacheService cacheService)
		{
			_tmdbHttpService = tmdbHttpService;
			_cacheService = cacheService;
		}

		public async Task<DiscoverMoviesResponse> GetMoviesAsync(DiscoverMoviesParametersRequest request)
		{
			string cacheKey = string.IsNullOrWhiteSpace(request.query)
				? $"discover_movies:{request.page}:{request.sort_by}:{request.language}:{request.include_adult}:{request.include_video}"
				: $"search_movies:{request.query}:{request.page}:{request.sort_by}:{request.language}:{request.include_adult}:{request.include_video}";

			var cached = await _cacheService.GetItem<DiscoverMoviesResponse>(cacheKey);
			if (cached != null)
			{
				return cached;
			}

			var tmdbResult = await _tmdbHttpService.GetMoviesAsync(request);
			var discoverResult = new DiscoverMoviesResponse
			{
				page = tmdbResult.page,
				total_pages = tmdbResult.total_pages,
				total_results = tmdbResult.total_results,
				results = new List<MovieWithVideoResult>()
			};

			if (tmdbResult.results == null || !tmdbResult.results.Any()) 
			{
				return discoverResult;
			}

			var movieTasks = tmdbResult.results.Select(async movie =>
			{
				var details = await _tmdbHttpService.GetMovieDetailsAndVideoAsync(movie.id);
				if (details?.videos != null)
				{
					var trailer = details.videos.results
						.FirstOrDefault(v => v.type == "Trailer" && v.site == "YouTube");
					if (trailer != null && !string.IsNullOrEmpty(trailer.key))
					{
						return new MovieWithVideoResult(details) { trailerUrl = $"https://www.youtube.com/watch?v={trailer.key}" };
					}
				}
				return new MovieWithVideoResult(details);
			});

			var discoverMovies = await Task.WhenAll(movieTasks);
			discoverResult.results.AddRange(discoverMovies);

			if (string.IsNullOrWhiteSpace(request.query))
			{
				await _cacheService.SetItem(cacheKey, discoverResult, null, DateTimeOffset.UtcNow.AddDays(1));
			}
			else
			{
				await _cacheService.SetItem(cacheKey, discoverResult, TimeSpan.FromMinutes(20), null);
			}

			return discoverResult;
		}
	}
}
