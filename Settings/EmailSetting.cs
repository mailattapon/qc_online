namespace QcSupplier.Settings {
  public class EmailSetting {
    public string Host { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public bool Enabled { get; set; }
    public int Port { get; set; }
    public bool EnableSsl { get; set; }
    public string From { get; set; }
  }
}