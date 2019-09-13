namespace WebSpeakAngular.Models
{
    public static class TokenOptions
    {
        public static string ValidIssuer => "http://localhost:50342";
        public static string ValidAudience => "http://localhost:50342";
        public static string SecretKey => "You`ll never know this key@777";
        public static int MinutesExpire => 3;
    }
}