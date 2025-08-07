using MovieAndSeriesScanner.Common.Api.Dto;

namespace MovieAndSeriesScanner.Services.Api.Services
{
	public interface ITmdbHttpService
	{
		Task<TmdbMoviesApiResponse> GetMoviesAsync(DiscoverMoviesParametersRequest request);
		Task<TmdbMovieApiResponse> GetMovieDetailsAndVideoAsync(int movieId);
	}
}
