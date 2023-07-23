namespace WDA.Service.User;

public class TokenResult
{
    public TokenResult(string token, DateTime validTo)
    {
        Token = token;
        ValidTo = validTo;
    }
    public string Token { get; set; }
    public DateTime ValidTo { get; set; }
}