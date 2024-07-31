namespace MSIT158_2_API.Models
{
    public class CApplicationUser
    {
        public string PasswordResetToken { get; set; }
        public DateTime? PasswordResetTokenExpires { get; set; }
    }
}
