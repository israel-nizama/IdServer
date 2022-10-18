namespace IdServer.Infraestructure.Services
{
    public class SecurityOptions
    {
        public static string SectionName = "Security";
        public string SecurityTokenKeySecret { get; set; }
        public string AccessTokenKeySecret { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
}
