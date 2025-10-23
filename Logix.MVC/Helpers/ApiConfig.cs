namespace Logix.MVC.Helpers
{
    public static class ApiConfig
    {
        public const string ApiVersion = "v1";
        public const string ApiLoginUrl = $"/api/{ApiConfig.ApiVersion}/Auth/login";
        public const string ApiAuthUrl = $"/api/{ApiConfig.ApiVersion}/Auth";
        public const string MvcLoginUrl = "/Account/Login";
    }
}
