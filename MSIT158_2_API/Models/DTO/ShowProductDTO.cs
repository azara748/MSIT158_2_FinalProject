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
		public DateOnly? LanchTime { get; set; }
		public int? ProductId { get; set; }
		public double? Score { get; set; }
		public bool? isnew { get; set; }
        //後臺用
		public decimal? cost { get; set; }
		public string? Description { get; set; }
        
    }
}
