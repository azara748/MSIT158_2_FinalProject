namespace MSIT158_2_API.Models.DTO.Back
{
    public class ProductSearchDTO
    {
        public string? searchword { get; set; } = string.Empty;
        public int? proId { get; set; } = 0;
        public int page { get; set; } = 1;
        public int pagesSize { get; set; } = 12;
        public string? sortBy { get; set; }
        public string? sortType { get; set; }
        public int? labelID { get; set; } = 0;
        public int? catID { get; set; } = 0;
        public int? status { get; set; } = 0;
    }
}
