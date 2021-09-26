namespace QcSupplier.Settings {
  public class PasswordPolicySetting {
    public int RequiredLength { get; set; }
    public bool RequireUppercase { get; set; }
    public bool RequireDigit { get; set; }
    public bool RequireNonAlphanumeric { get; set; }
    public string PasswordRegex { get; set; }
  }
}