namespace MSIT158_2_API.Models.DTO
{
	public class ShowProductDTO
	{
		public byte[] Productphoto { get; set; }
		public string? ProductName { get; set; }
		public int? Stocks { get; set; }
		public decimal? UnitPrice { get; set; }
		public int? SubCategoryId { get; set; }
		public string? SubCatName { get; set; }
		public decimal? Discount { get; set; }
		public string? LabelName { get; set; }
		public string? LanchTime { get; set; }
		public int TotalPages { get; set; }
		//public int SubCatId { get; set; }
		//public string? SubcatName { get; set; }
		//public int LabelId { get; set; }
		//public string? LabelName { get; set; }
		//public int CatId { get; set; }
		//public string? CatName { get; set; }
		//public int? ActiveId { get; set; }
		//public string? ActiveName { get; set; }
		//public string? Description { get; set; }
		////跨資料表:


		//labelid
		//subcat
		//Activeid
		//LaunchTime-新上市TBC

	}
}
