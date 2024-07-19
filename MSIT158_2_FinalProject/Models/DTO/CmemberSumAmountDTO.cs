namespace MSIT158_2_FinalProject.Models.DTO
{
    public class CmemberSumAmountDTO
    {
        public int MemberId { get; set; }

        public string? MemberName { get; set; }

        public decimal? TotalConsumption { get; set; }

        public decimal? Amount { get; set; }

        public decimal? Price { get; set; }
    }
}
