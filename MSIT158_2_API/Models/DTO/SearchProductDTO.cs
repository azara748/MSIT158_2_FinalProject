namespace MSIT158_2_API.Models.DTO
{
	public class SearchProductDTO
	{
		public int subcatId { get; set; } = 0;
		public string? searchword { get; set; }
		public int page { get; set; } = 1;
		public int pagesSize { get; set; } = 12;
		public string? sortBy { get; set; }
		public string? sortType { get; set; }
		public int? lowPrice { get; set; }
		public int? highPrice { get; set; }
		public bool stock { get; set; } = false;
		public bool newlan { get; set; } = false;
		public bool rankfour { get; set; } = false;
		public bool rankthree { get; set; } = false;
		//public int score { get; set; } 
	}
}
