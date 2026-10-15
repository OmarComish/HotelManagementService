namespace HotelManagementService.Core.Constants;
public static class RbacPermissions
{
    public const string UsersView = "Users.View";
    public const string UsersCreate = "Users.Create";
    public const string UsersEdit = "Users.Edit";
    public const string UsersDelete = "Users.Delete";
    public const string UsersActivate = "Users.Activate";
    public const string UsersDeactivate = "Users.Deactivate";
    public const string UsersResetPassword = "Users.ResetPassword";
    public const string UsersChangeRole = "Users.ChangeRole";

    public const string RolesView = "Roles.View";
    public const string RolesCreate = "Roles.Create";
    public const string RolesEdit = "Roles.Edit";
    public const string RolesDelete = "Roles.Delete";
    public const string RolesAssignPermissions = "Roles.AssignPermissions";

    public const string SettingsView = "Settings.View";
    public const string SettingsEdit = "Settings.Edit";

    public const string AuditLogsView = "AuditLogs.View";
}