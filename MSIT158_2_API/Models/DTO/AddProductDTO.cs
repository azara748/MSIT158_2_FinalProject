namespace MSIT158_2_API.Models.DTO
{
	//後臺用
	public class AddProductDTO
	{
		public string? Productphoto { get; set; }
		public string? ProductName { get; set; }
		public int? Stocks { get; set; }
		public int? LabelId { get; set; }
		public int? SubCategoryId { get; set; }
		public decimal? UnitPrice { get; set; }
		public decimal? Cost { get; set; }
		public string? Description { get; set; }
		public DateOnly? LanchTime { get; set; }
		public int? status { get; set; }
	}
}
