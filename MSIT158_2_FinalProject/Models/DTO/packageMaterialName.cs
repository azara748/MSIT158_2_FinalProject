namespace MSIT158_2_FinalProject.Models.DTO
{
    public class packageMaterialName
    {
        public int PackageId { get; set; }
        public int? MaterialId { get; set; }
        public string? MaterialName { get; set; }
        public int? MaterialColorId { get; set; }
        public string? ColorName { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public string? Picture { get; set; }
        public int? PackageStyleId { get; set; }
        public string? PackName { get; set; }

    }
}
