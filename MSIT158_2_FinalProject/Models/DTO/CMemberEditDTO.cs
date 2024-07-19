namespace MSIT158_2_FinalProject.Models.DTO
{
    public class CMemberEditDTO
    {
        public int MemberId { get; set; }

        public string? MemberName { get; set; }

        public string? Cellphone { get; set; }

        public string? Address { get; set; }

        public DateOnly? Birthday { get; set; }

        public string? Sex { get; set; }

        public string? Password { get; set; }

        public string? Salt { get; set; }

        public string? EMail { get; set; }

        public int? Points { get; set; }

        public int? Vipid { get; set; }

        public string? MemberPhoto { get; set; }

        public decimal? Wallet { get; set; }

        //public string? repassword { get; set; }

        //public IFormFile avatar { get; set; }
    }
}
