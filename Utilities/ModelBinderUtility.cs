namespace QcSupplier.Utilities {
  public static class ModelBinderUtility {
    public static int? GetVendorId(string vendor) {
      int? vendorId = null;
      if (!string.IsNullOrEmpty(vendor)) {
        vendorId = int.Parse(vendor);
      }
      return vendorId;
    }
  }
}