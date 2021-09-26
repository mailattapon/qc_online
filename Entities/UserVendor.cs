namespace QcSupplier.Entities {
  public class UserVendor {
    public int UserId { get; set; }
    public int VendorId { get; set; }
    public virtual User User { get; set; }
    public virtual Vendor Vendor { get; set; }
  }
}