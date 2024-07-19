namespace MSIT158_2_FinalProject.Models.DTO
{
    public class CESearchDTO
    {
        public int employeeId { get; set; } = 0;
        public string? keyword { get; set; }
        public int page { get; set; } = 1;
        public int pageSize { get; set; } = 10;
        public string? sortType { get; set; }
        public string? sortBy { get; set; }
    }
}
