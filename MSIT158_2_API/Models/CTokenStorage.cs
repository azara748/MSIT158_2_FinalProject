namespace MSIT158_2_API.Models
{
    public static class CTokenStorage
    {
        public static Dictionary<string, CPasswordResetToken> Tokens = new Dictionary<string, CPasswordResetToken>();
    }
}
