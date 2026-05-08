using SMS.Shared.Enums;

namespace SMS.API.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public sealed class AuditActionTypeAttribute : Attribute
    {
        public AuditActionType ActionType { get; }

        public AuditActionTypeAttribute(AuditActionType actionType)
        {
            ActionType = actionType;
        }
    }
}
