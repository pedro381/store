namespace Store.Domain.Configurations
{
    public class TokenSettings
    {
        public string SecretKey { get; set; } = string.Empty;
        public int ExpirationHours { get; set; }
    }
}
