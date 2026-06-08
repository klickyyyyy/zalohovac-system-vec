namespace ZalohovacServer.Entities.API
{
    public class Credentials
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class Token
    {
        public string TokenString { get; set; } = string.Empty;

        public Token(string tokenString)
        {
            TokenString = tokenString;
        }
    }
}
