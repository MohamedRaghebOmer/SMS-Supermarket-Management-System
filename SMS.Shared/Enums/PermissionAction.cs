namespace SMS.Shared.Enums
{
    [Flags]
    public enum PermissionAction
    {
        Create = 1,
        Read = 2,
        Update = 4,
        Delete = 8,
    }
}
