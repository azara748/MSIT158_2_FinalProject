namespace MSIT158_2_API.Models.DTO
{
	public class SearchProductDTO
	{
		public int subcatId { get; set; } = 0;
		public string? searchword { get; set; }
		public int page { get; set; } = 1;
		public int pagesSize { get; set; } = 9;
		public string? sortBy { get; set; }
		public string? sortType { get; set; }
	}
}
