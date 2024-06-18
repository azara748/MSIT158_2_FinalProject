namespace MSIT158_2_FinalProject.DTO
{
    public class PackageDto
    {
        public string PackName { get; set; }
        public decimal Price { get; set; }
        public IFormFile Picture { get; set; }
        public int PackageStyleId { get; set; }
        public int MaterialId { get; set; }
        public string Description { get; set; }
    }
}
