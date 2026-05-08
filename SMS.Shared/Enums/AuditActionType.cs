namespace SMS.Shared.Enums
{
    public enum AuditActionType
    {
        Insert = 1,
        Read = 2,
        Update = 3,
        Delete = 4,

        Login = 5,
        Logout = 6,
        Register = 7,

        FailedLogin = 8,
        TokenRefresh = 9,
        TokenExpired = 10,
        AccessDenied = 11,

        Unknown = 99
    }
}
