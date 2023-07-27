namespace WDA.Shared;

public sealed class AppSettings
{
    public static AppSettings Instance { get; set; }
    public ConnectionStrings ConnectionStrings { get; set; }
    public Jwt Jwt { get; set; }
    public Cors Cors { get; set; }
    public AzureStorage AzureStorage { get; set; }
    public Smtp Smtp { get; set; }
    public ClientConfiguration ClientConfiguration { get; set; }
}
public sealed class ConnectionStrings
{
    public string SqlServer { get; set; }
}
public sealed class Jwt
{
    public string ValidIssuer { get; set; }
    public string ValidAudience { get; set; }
    public string Secret { get; set; }
}

public sealed class Cors
{
    public string Origins { get; set; }
}

public sealed class AzureStorage
{
    public string ConnectionString { get; set; }
}
public sealed class Smtp
{
    public string Server { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string From { get; set; }
    public string DisplayName { get; set; }
}

public sealed class ClientConfiguration
{
    public string CreateTicketBaseUrl { get; set; }
}