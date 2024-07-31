using Microsoft.AspNetCore.Identity;

namespace MSIT158_2_FinalProject.Models
{
    public class CApplicationUser: IdentityUser
    {
        public string PasswordResetToken { get; set; }
        public DateTime? PasswordResetTokenExpires { get; set; }
    }
}
