namespace MSIT158_2_API.Models.DTO.Back
{
    public class ProductToBackDTO
    {
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
        public List<ProductShowDTO>? Productshow { get; set; }
    }
}
