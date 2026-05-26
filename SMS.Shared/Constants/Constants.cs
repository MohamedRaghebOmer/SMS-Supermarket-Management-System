namespace SMS.Shared.Constants
{
    public static class Constants
    {
        public const string PaginationResponseTotalCountParamName = "TotalCount";
        public const string SlidingIp = "sliding-ip";
        public const string ApiVersion = "v1";
        public const string ApiTitle = "SMS API";
        public const int AccessTokenPeriodInMinutes = 15;
        public const int RefreshTokenPeriodInDays = 7;
        public const long MaxImageSizeInBytes = 5 * 1024 * 1024; // 5 MB
        public const string CorePolicyName = "SMSApiCorsPolicy";
    }
}
