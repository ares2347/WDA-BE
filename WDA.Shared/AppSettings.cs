namespace WDA.Shared;

public sealed class AppSettings
{
    public static AppSettings Instance { get; set; }
    public ConnectionStrings ConnectionStrings { get; set; }
    public Jwt Jwt { get; set; }
    public Cors Cors { get; set; }
    public AzureStorage AzureStorage { get; set; }
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
