public class Config
{
    private readonly IConfiguration _configuration;

    public Config(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GetConnectionString() => _configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
    public string GetJwtKey() => _configuration["Jwt:Key"] ?? string.Empty;
    public string GetJwtIssuer() => _configuration["Jwt:Issuer"] ?? string.Empty;
    public string GetJwtAudience() => _configuration["Jwt:Audience"] ?? string.Empty;
}