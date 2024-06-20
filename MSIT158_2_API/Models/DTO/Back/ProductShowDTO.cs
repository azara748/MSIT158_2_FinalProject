namespace MSIT158_2_API.Models.DTO.Back
{
    public class ProductShowDTO
    {
        public int? ProductId { get; set; }
        public byte[]? Productphoto { get; set; }
        public string? ProductName { get; set; }
        public int? Stocks { get; set; }
        public decimal? UnitPrice { get; set; }
        public int? SubCategoryId { get; set; }
        public string? SubCatName { get; set; }
        public int? LabelId { get; set; }
        public string? LabelName { get; set; }
        public DateOnly? LanchTime { get; set; }
        public decimal? cost { get; set; }
        public string? Description { get; set; }
        public int? Status { get; set; }
    }
}
