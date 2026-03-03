
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

    // ═══════════════════════════════════════════════════════════
    //  EMAIL SUBJECTS
    // ═══════════════════════════════════════════════════════════

    public const string EmailSubjectConfirm = "✨ Welcome to BelieveIn — Confirm Your Email";
    public const string EmailSubjectReset = "🔐 BelieveIn — Reset Your Password";

    // ═══════════════════════════════════════════════════════════
    //  EMAIL: CONFIRM EMAIL
    //  {0} = confirmation link
    // ═══════════════════════════════════════════════════════════

    public const string EmailBodyConfirmTemplate = @"
<!DOCTYPE html>
<html>
<head>
  <meta charset='utf-8'>
  <meta name='viewport' content='width=device-width, initial-scale=1.0'>
</head>
<body style='margin:0;padding:0;background-color:#FFF8F4;font-family:Arial,Helvetica,sans-serif;'>
  <table role='presentation' width='100%' cellpadding='0' cellspacing='0' style='background-color:#FFF8F4;padding:40px 20px;'>
    <tr>
      <td align='center'>
        <table role='presentation' width='520' cellpadding='0' cellspacing='0' style='background-color:#FFFFFF;border-radius:24px;overflow:hidden;box-shadow:0 4px 24px rgba(0,0,0,0.06);'>

          <!-- Header -->
          <tr>
            <td style='background:linear-gradient(135deg,#FF6B35,#FF8F5E);padding:40px 40px 30px;text-align:center;'>
              <div style='font-size:36px;margin-bottom:8px;'>✨</div>
              <h1 style='margin:0;color:#FFFFFF;font-size:26px;font-weight:800;letter-spacing:-0.5px;'>
                Welcome to BelieveIn
              </h1>
              <p style='margin:8px 0 0;color:rgba(255,255,255,0.85);font-size:14px;font-weight:500;'>
                Your journey to a positive mindset starts here
              </p>
            </td>
          </tr>

          <!-- Body -->
          <tr>
            <td style='padding:36px 40px 20px;'>
              <p style='margin:0 0 16px;color:#1A1A2E;font-size:16px;line-height:1.6;'>
                Hi there! 👋
              </p>
              <p style='margin:0 0 16px;color:#555555;font-size:15px;line-height:1.7;'>
                Thank you for joining <strong style='color:#1A1A2E;'>BelieveIn</strong>. You're one step away from receiving daily affirmations that will inspire and uplift you every single day.
              </p>
              <p style='margin:0 0 28px;color:#555555;font-size:15px;line-height:1.7;'>
                Please confirm your email address to activate your account:
              </p>

              <!-- CTA Button -->
              <table role='presentation' width='100%' cellpadding='0' cellspacing='0'>
                <tr>
                  <td align='center'>
                    <a href='{0}' target='_blank' style='display:inline-block;background:linear-gradient(135deg,#FF6B35,#FF8F5E);color:#FFFFFF;text-decoration:none;font-size:16px;font-weight:700;padding:16px 48px;border-radius:50px;letter-spacing:0.3px;box-shadow:0 6px 20px rgba(255,107,53,0.35);'>
                      Confirm My Email
                    </a>
                  </td>
                </tr>
              </table>

              <p style='margin:24px 0 0;color:#999999;font-size:12px;line-height:1.6;text-align:center;'>
                Button not working? Copy and paste this link:<br/>
                <a href='{0}' style='color:#FF6B35;word-break:break-all;font-size:11px;'>{0}</a>
              </p>
            </td>
          </tr>

          <!-- Divider -->
          <tr>
            <td style='padding:0 40px;'>
              <hr style='border:none;border-top:1px solid #F0F0F0;margin:0;'/>
            </td>
          </tr>

          <!-- What's Next -->
          <tr>
            <td style='padding:24px 40px 12px;'>
              <p style='margin:0 0 16px;color:#1A1A2E;font-size:15px;font-weight:700;'>
                What's next? 🚀
              </p>
              <table role='presentation' width='100%' cellpadding='0' cellspacing='0'>
                <tr>
                  <td style='padding:8px 0;'>
                    <table role='presentation' cellpadding='0' cellspacing='0'>
                      <tr>
                        <td style='vertical-align:top;padding-right:12px;'>
                          <div style='width:28px;height:28px;background:#FFF0E8;border-radius:50%;text-align:center;line-height:28px;font-size:13px;'>🌅</div>
                        </td>
                        <td style='color:#555555;font-size:14px;line-height:1.5;'>
                          <strong style='color:#1A1A2E;'>Set your schedule</strong> — Choose when affirmations appear on your screen
                        </td>
                      </tr>
                    </table>
                  </td>
                </tr>
                <tr>
                  <td style='padding:8px 0;'>
                    <table role='presentation' cellpadding='0' cellspacing='0'>
                      <tr>
                        <td style='vertical-align:top;padding-right:12px;'>
                          <div style='width:28px;height:28px;background:#FFF0E8;border-radius:50%;text-align:center;line-height:28px;font-size:13px;'>✍️</div>
                        </td>
                        <td style='color:#555555;font-size:14px;line-height:1.5;'>
                          <strong style='color:#1A1A2E;'>Add your own sparks</strong> — Write personal affirmations that resonate with you
                        </td>
                      </tr>
                    </table>
                  </td>
                </tr>
                <tr>
                  <td style='padding:8px 0;'>
                    <table role='presentation' cellpadding='0' cellspacing='0'>
                      <tr>
                        <td style='vertical-align:top;padding-right:12px;'>
                          <div style='width:28px;height:28px;background:#FFF0E8;border-radius:50%;text-align:center;line-height:28px;font-size:13px;'>🎨</div>
                        </td>
                        <td style='color:#555555;font-size:14px;line-height:1.5;'>
                          <strong style='color:#1A1A2E;'>Customize your overlay</strong> — Pick fonts, animations, and styles you love
                        </td>
                      </tr>
                    </table>
                  </td>
                </tr>
              </table>
            </td>
          </tr>

          <!-- Quote -->
          <tr>
            <td style='padding:16px 40px 28px;'>
              <div style='background:#FFF8F4;border-radius:16px;padding:20px 24px;border-left:4px solid #FF6B35;'>
                <p style='margin:0;color:#1A1A2E;font-size:15px;font-style:italic;line-height:1.6;'>
                  &ldquo;Believe you can and you're halfway there.&rdquo;
                </p>
                <p style='margin:8px 0 0;color:#FF6B35;font-size:12px;font-weight:700;'>
                  — Theodore Roosevelt
                </p>
              </div>
            </td>
          </tr>

          <!-- Footer -->
          <tr>
            <td style='background:#FAFAFA;padding:24px 40px;border-top:1px solid #F0F0F0;text-align:center;'>
              <p style='margin:0 0 4px;color:#1A1A2E;font-size:16px;font-weight:800;'>BelieveIn</p>
              <p style='margin:0 0 12px;color:#999999;font-size:12px;'>Believe in Yourself ✨</p>
              <p style='margin:0;color:#BBBBBB;font-size:11px;line-height:1.5;'>
                This link expires in 24 hours.<br/>
                If you didn't create an account, you can safely ignore this email.
              </p>
            </td>
          </tr>

        </table>
      </td>
    </tr>
  </table>
</body>
</html>";

    // ═══════════════════════════════════════════════════════════
    //  EMAIL: RESET PASSWORD
    //  {0} = reset link
    // ═══════════════════════════════════════════════════════════

    public const string EmailBodyResetTemplate = @"
<!DOCTYPE html>
<html>
<head>
  <meta charset='utf-8'>
  <meta name='viewport' content='width=device-width, initial-scale=1.0'>
</head>
<body style='margin:0;padding:0;background-color:#FFF8F4;font-family:Arial,Helvetica,sans-serif;'>
  <table role='presentation' width='100%' cellpadding='0' cellspacing='0' style='background-color:#FFF8F4;padding:40px 20px;'>
    <tr>
      <td align='center'>
        <table role='presentation' width='520' cellpadding='0' cellspacing='0' style='background-color:#FFFFFF;border-radius:24px;overflow:hidden;box-shadow:0 4px 24px rgba(0,0,0,0.06);'>

          <!-- Header -->
          <tr>
            <td style='background:linear-gradient(135deg,#1A1A2E,#2D2D4E);padding:40px 40px 30px;text-align:center;'>
              <div style='font-size:36px;margin-bottom:8px;'>🔐</div>
              <h1 style='margin:0;color:#FFFFFF;font-size:26px;font-weight:800;letter-spacing:-0.5px;'>
                Reset Your Password
              </h1>
              <p style='margin:8px 0 0;color:rgba(255,255,255,0.7);font-size:14px;font-weight:500;'>
                No worries — it happens to the best of us
              </p>
            </td>
          </tr>

          <!-- Body -->
          <tr>
            <td style='padding:36px 40px 20px;'>
              <p style='margin:0 0 16px;color:#1A1A2E;font-size:16px;line-height:1.6;'>
                Hi there 👋
              </p>
              <p style='margin:0 0 16px;color:#555555;font-size:15px;line-height:1.7;'>
                We received a request to reset the password for your <strong style='color:#1A1A2E;'>BelieveIn</strong> account. Click the button below to choose a new password:
              </p>

              <!-- CTA Button -->
              <table role='presentation' width='100%' cellpadding='0' cellspacing='0' style='margin:28px 0;'>
                <tr>
                  <td align='center'>
                    <a href='{0}' target='_blank' style='display:inline-block;background:linear-gradient(135deg,#FF6B35,#FF8F5E);color:#FFFFFF;text-decoration:none;font-size:16px;font-weight:700;padding:16px 48px;border-radius:50px;letter-spacing:0.3px;box-shadow:0 6px 20px rgba(255,107,53,0.35);'>
                      Reset My Password
                    </a>
                  </td>
                </tr>
              </table>

              <p style='margin:0 0 0;color:#999999;font-size:12px;line-height:1.6;text-align:center;'>
                Button not working? Copy and paste this link:<br/>
                <a href='{0}' style='color:#FF6B35;word-break:break-all;font-size:11px;'>{0}</a>
              </p>
            </td>
          </tr>

          <!-- Divider -->
          <tr>
            <td style='padding:0 40px;'>
              <hr style='border:none;border-top:1px solid #F0F0F0;margin:0;'/>
            </td>
          </tr>

          <!-- Security Tips -->
          <tr>
            <td style='padding:24px 40px 12px;'>
              <p style='margin:0 0 16px;color:#1A1A2E;font-size:15px;font-weight:700;'>
                Security tips 🛡️
              </p>
              <table role='presentation' width='100%' cellpadding='0' cellspacing='0'>
                <tr>
                  <td style='padding:8px 0;'>
                    <table role='presentation' cellpadding='0' cellspacing='0'>
                      <tr>
                        <td style='vertical-align:top;padding-right:12px;'>
                          <div style='width:28px;height:28px;background:#FFF0E8;border-radius:50%;text-align:center;line-height:28px;font-size:13px;'>🔑</div>
                        </td>
                        <td style='color:#555555;font-size:14px;line-height:1.5;'>
                          Use a <strong style='color:#1A1A2E;'>strong, unique password</strong> with at least 8 characters
                        </td>
                      </tr>
                    </table>
                  </td>
                </tr>
                <tr>
                  <td style='padding:8px 0;'>
                    <table role='presentation' cellpadding='0' cellspacing='0'>
                      <tr>
                        <td style='vertical-align:top;padding-right:12px;'>
                          <div style='width:28px;height:28px;background:#FFF0E8;border-radius:50%;text-align:center;line-height:28px;font-size:13px;'>🔒</div>
                        </td>
                        <td style='color:#555555;font-size:14px;line-height:1.5;'>
                          Mix <strong style='color:#1A1A2E;'>letters, numbers, and symbols</strong> for extra security
                        </td>
                      </tr>
                    </table>
                  </td>
                </tr>
                <tr>
                  <td style='padding:8px 0;'>
                    <table role='presentation' cellpadding='0' cellspacing='0'>
                      <tr>
                        <td style='vertical-align:top;padding-right:12px;'>
                          <div style='width:28px;height:28px;background:#FFF0E8;border-radius:50%;text-align:center;line-height:28px;font-size:13px;'>🚫</div>
                        </td>
                        <td style='color:#555555;font-size:14px;line-height:1.5;'>
                          <strong style='color:#1A1A2E;'>Never share</strong> your password with anyone
                        </td>
                      </tr>
                    </table>
                  </td>
                </tr>
              </table>
            </td>
          </tr>

          <!-- Quote -->
          <tr>
            <td style='padding:16px 40px 28px;'>
              <div style='background:#FFF8F4;border-radius:16px;padding:20px 24px;border-left:4px solid #FF6B35;'>
                <p style='margin:0;color:#1A1A2E;font-size:15px;font-style:italic;line-height:1.6;'>
                  &ldquo;Every day is a new beginning. Take a deep breath, smile, and start again.&rdquo;
                </p>
                <p style='margin:8px 0 0;color:#FF6B35;font-size:12px;font-weight:700;'>
                  — BelieveIn
                </p>
              </div>
            </td>
          </tr>

          <!-- Footer -->
          <tr>
            <td style='background:#FAFAFA;padding:24px 40px;border-top:1px solid #F0F0F0;text-align:center;'>
              <p style='margin:0 0 4px;color:#1A1A2E;font-size:16px;font-weight:800;'>BelieveIn</p>
              <p style='margin:0 0 12px;color:#999999;font-size:12px;'>Believe in Yourself ✨</p>
              <p style='margin:0;color:#BBBBBB;font-size:11px;line-height:1.5;'>
                This link expires in 1 hour.<br/>
                If you didn't request a password reset, you can safely ignore this email.<br/>
                Your password will remain unchanged.
              </p>
            </td>
          </tr>

        </table>
      </td>
    </tr>
  </table>
</body>
</html>";

    // ═══════════════════════════════════════════════════════════
    //  EMAIL: OTP CODE
    //  {0} = OTP code (6 digits)
    // ═══════════════════════════════════════════════════════════

    public const string EmailSubjectOtp = "🔑 BelieveIn — Your Verification Code";

    public const string EmailBodyOtpTemplate = @"
<!DOCTYPE html>
<html>
<head>
  <meta charset='utf-8'>
  <meta name='viewport' content='width=device-width, initial-scale=1.0'>
</head>
<body style='margin:0;padding:0;background-color:#FFF8F4;font-family:Arial,Helvetica,sans-serif;'>
  <table role='presentation' width='100%' cellpadding='0' cellspacing='0' style='background-color:#FFF8F4;padding:40px 20px;'>
    <tr>
      <td align='center'>
        <table role='presentation' width='520' cellpadding='0' cellspacing='0' style='background-color:#FFFFFF;border-radius:24px;overflow:hidden;box-shadow:0 4px 24px rgba(0,0,0,0.06);'>

          <!-- Header -->
          <tr>
            <td style='background:linear-gradient(135deg,#6A1B9A,#9C27B0);padding:40px 40px 30px;text-align:center;'>
              <div style='font-size:36px;margin-bottom:8px;'>🔑</div>
              <h1 style='margin:0;color:#FFFFFF;font-size:26px;font-weight:800;letter-spacing:-0.5px;'>
                Verification Code
              </h1>
              <p style='margin:8px 0 0;color:rgba(255,255,255,0.8);font-size:14px;font-weight:500;'>
                Use this code to reset your password
              </p>
            </td>
          </tr>

          <!-- Body -->
          <tr>
            <td style='padding:36px 40px 20px;'>
              <p style='margin:0 0 16px;color:#1A1A2E;font-size:16px;line-height:1.6;'>
                Hi there 👋
              </p>
              <p style='margin:0 0 24px;color:#555555;font-size:15px;line-height:1.7;'>
                We received a request to reset your <strong style='color:#1A1A2E;'>BelieveIn</strong> password. Enter the following code to continue:
              </p>

              <!-- OTP Code Box -->
              <table role='presentation' width='100%' cellpadding='0' cellspacing='0'>
                <tr>
                  <td align='center'>
                    <div style='background:linear-gradient(135deg,#FFF8F4,#FFF0E8);border:2px dashed #FF6B35;border-radius:20px;padding:28px 40px;display:inline-block;'>
                      <p style='margin:0 0 6px;color:#999999;font-size:12px;font-weight:600;letter-spacing:1px;text-transform:uppercase;'>
                        Your verification code
                      </p>
                      <p style='margin:0;color:#1A1A2E;font-size:42px;font-weight:900;letter-spacing:12px;font-family:monospace,Arial;'>
                        {0}
                      </p>
                    </div>
                  </td>
                </tr>
              </table>

              <!-- Timer Warning -->
              <table role='presentation' width='100%' cellpadding='0' cellspacing='0' style='margin-top:24px;'>
                <tr>
                  <td align='center'>
                    <div style='background:#FFF8F4;border-radius:12px;padding:12px 20px;display:inline-block;'>
                      <p style='margin:0;color:#FF6B35;font-size:13px;font-weight:700;'>
                        ⏱️ This code expires in 10 minutes
                      </p>
                    </div>
                  </td>
                </tr>
              </table>
            </td>
          </tr>

          <!-- Divider -->
          <tr>
            <td style='padding:8px 40px 0;'>
              <hr style='border:none;border-top:1px solid #F0F0F0;margin:0;'/>
            </td>
          </tr>

          <!-- Security Notice -->
          <tr>
            <td style='padding:24px 40px 12px;'>
              <p style='margin:0 0 16px;color:#1A1A2E;font-size:15px;font-weight:700;'>
                Keep it safe 🛡️
              </p>
              <table role='presentation' width='100%' cellpadding='0' cellspacing='0'>
                <tr>
                  <td style='padding:6px 0;'>
                    <table role='presentation' cellpadding='0' cellspacing='0'>
                      <tr>
                        <td style='vertical-align:top;padding-right:12px;'>
                          <div style='width:28px;height:28px;background:#F3E5F5;border-radius:50%;text-align:center;line-height:28px;font-size:13px;'>🚫</div>
                        </td>
                        <td style='color:#555555;font-size:14px;line-height:1.5;'>
                          <strong style='color:#1A1A2E;'>Never share this code</strong> — BelieveIn will never ask you for it
                        </td>
                      </tr>
                    </table>
                  </td>
                </tr>
                <tr>
                  <td style='padding:6px 0;'>
                    <table role='presentation' cellpadding='0' cellspacing='0'>
                      <tr>
                        <td style='vertical-align:top;padding-right:12px;'>
                          <div style='width:28px;height:28px;background:#F3E5F5;border-radius:50%;text-align:center;line-height:28px;font-size:13px;'>📧</div>
                        </td>
                        <td style='color:#555555;font-size:14px;line-height:1.5;'>
                          Only enter this code in the <strong style='color:#1A1A2E;'>BelieveIn app</strong>
                        </td>
                      </tr>
                    </table>
                  </td>
                </tr>
                <tr>
                  <td style='padding:6px 0;'>
                    <table role='presentation' cellpadding='0' cellspacing='0'>
                      <tr>
                        <td style='vertical-align:top;padding-right:12px;'>
                          <div style='width:28px;height:28px;background:#F3E5F5;border-radius:50%;text-align:center;line-height:28px;font-size:13px;'>⚠️</div>
                        </td>
                        <td style='color:#555555;font-size:14px;line-height:1.5;'>
                          If you didn't request this, <strong style='color:#1A1A2E;'>change your password</strong> immediately
                        </td>
                      </tr>
                    </table>
                  </td>
                </tr>
              </table>
            </td>
          </tr>

          <!-- Quote -->
          <tr>
            <td style='padding:16px 40px 28px;'>
              <div style='background:#FFF8F4;border-radius:16px;padding:20px 24px;border-left:4px solid #9C27B0;'>
                <p style='margin:0;color:#1A1A2E;font-size:15px;font-style:italic;line-height:1.6;'>
                  &ldquo;The secret of getting ahead is getting started.&rdquo;
                </p>
                <p style='margin:8px 0 0;color:#9C27B0;font-size:12px;font-weight:700;'>
                  — Mark Twain
                </p>
              </div>
            </td>
          </tr>

          <!-- Footer -->
          <tr>
            <td style='background:#FAFAFA;padding:24px 40px;border-top:1px solid #F0F0F0;text-align:center;'>
              <p style='margin:0 0 4px;color:#1A1A2E;font-size:16px;font-weight:800;'>BelieveIn</p>
              <p style='margin:0 0 12px;color:#999999;font-size:12px;'>Believe in Yourself ✨</p>
              <p style='margin:0;color:#BBBBBB;font-size:11px;line-height:1.5;'>
                This code is valid for 10 minutes only.<br/>
                If you didn't request a password reset, you can safely ignore this email.
              </p>
            </td>
          </tr>

        </table>
      </td>
    </tr>
  </table>
</body>
</html>";
}

