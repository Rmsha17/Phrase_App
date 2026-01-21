
// Static container for all constant messages, roles, and email templates.
public static class StaticDetails
{
    // Roles
    public const string RoleUser = "User";

    // General messages
    public const string MsgEmailAlreadyRegistered = "Email already registered";
    public const string MsgRegistrationFailed = "Registration failed";
    public const string MsgAccountCreated = "Account Created Successfully";
    public const string MsgInvalidCredentials = "Invalid credentials";
    public const string MsgEmailNotConfirmed = "Email not confirmed";
    public const string MsgInvalidEmail = "Invalid email";
    public const string MsgEmailConfirmationFailed = "Email confirmation failed";
    public const string MsgConfirmationEmailSentIfExists = "If the email exists, a confirmation link has been sent.";
    public const string MsgEmailAlreadyConfirmed = "Email is already confirmed.";
    public const string MsgConfirmationEmailResent = "Confirmation email resent successfully.";
    public const string MsgPasswordResetFailed = "Password reset failed";
    public const string userLoginRequired = "User login required.";

    // Email subjects
    public const string EmailSubjectConfirm = "Confirm your email";
    public const string EmailSubjectReset = "Reset Password";

    // Email body templates
    // {0} for link, {1} for optional second parameter (e.g., user full name for richer templates)
    public const string EmailBodyConfirmSimpleTemplate = "Click here to confirm: <a href='{0}'>Confirm Email</a>";
    public const string EmailBodyConfirmTemplate =
        "<p>Hello {0},</p>" +
        "<p>Please confirm your email by clicking the link below:</p>" +
        "<p><a href='{1}'>Confirm Email</a></p>" +
        "<p>If you did not create this account, please ignore this email.</p>";
    public const string EmailBodyResetTemplate = "Click here to reset password: <a href='{0}'>Reset Password</a>";
}
