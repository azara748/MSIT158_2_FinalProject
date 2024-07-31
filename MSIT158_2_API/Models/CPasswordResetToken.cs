namespace MSIT158_2_API.Models
{
    public class CPasswordResetToken
    {
        public string Token { get; set; }
        public int UserId { get; set; }
        public DateTime Expiration { get; set; }
    }
}