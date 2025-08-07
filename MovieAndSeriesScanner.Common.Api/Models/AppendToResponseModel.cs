namespace MovieAndSeriesScanner.Common.Api.Models
{
	public class AppendToResponseModel
	{
		public string? append_to_response{ get; set; }

		public AppendToResponseModel(string? append_to_response = null)
		{
			this.append_to_response = append_to_response;
		}
	}
}
