namespace EduApoyosDomain.Dtos
{
    public class JwtResultDto
    {
        public string Token { get; set; } = string.Empty;
        public string JwtId { get; set; } = string.Empty;
        public DateTime Expiration { get; set; }
    }
}
