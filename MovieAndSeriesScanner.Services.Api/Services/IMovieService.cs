using MovieAndSeriesScanner.Common.Api.Dto;

namespace MovieAndSeriesScanner.Services.Api.Services
{
	public interface IMovieService
	{
		Task<DiscoverMoviesResponse> GetMoviesAsync(DiscoverMoviesParametersRequest request);
	}
}
