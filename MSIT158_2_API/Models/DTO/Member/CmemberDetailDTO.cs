namespace MSIT158_2_API.Models.DTO.Member
{
    public class CmemberDetailDTO
    {
        public int Id { get; set; }

        public string? MemberName { get; set; }

        public string? Cellphone { get; set; }

        public string? Address { get; set; }

        public DateOnly? Birthday { get; set; }

        public string? Sex { get; set; }

        public string? Password { get; set; }

        public string? Salt { get; set; }

        public string? EMail { get; set; }

        public int? Points { get; set; }

        public string? Vip { get; set; }

        public byte[]? Vipphoto { get; set; }

        public string? MemberPhoto { get; set; }

        public decimal? Wallet { get; set; }
    }
}
