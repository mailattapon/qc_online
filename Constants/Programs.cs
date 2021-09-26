using System;
using QcSupplier.Extensions;

namespace QcSupplier.Constants {
  public static class Programs {
    public const string SUPPLIER_EVALUATION = "SupplierEvaluation";
    public const string DOWNLOAD = "Download";
    public const string TNS_FORM = "TNSForm";
    public const string SUPPLIER_INFORMATION = "SupplierInformation";
    public const string OUT_GOING_DATA = "OutGoingData";
    public const string SELF_CONTROLLED_IPP = "SelfControlledIPP";

    public static Entities.Program SupplierEvaluation { get; private set; } = new Entities.Program {
      Id = 1,
      Name = "Supplier Evaluation",
      PolicyName = SUPPLIER_EVALUATION,
      Enabled = true,
      CreatedAt = DateTime.UtcNow.ToAppDateTime(),
      UpdatedAt = DateTime.UtcNow.ToAppDateTime()
    };
    public static Entities.Program Download { get; private set; } = new Entities.Program {
      Id = 2,
      Name = "Download",
      PolicyName = DOWNLOAD,
      Enabled = true,
      CreatedAt = DateTime.UtcNow.ToAppDateTime(),
      UpdatedAt = DateTime.UtcNow.ToAppDateTime()
    };
    public static Entities.Program TnsForm { get; private set; } = new Entities.Program {
      Id = 3,
      Name = "TNS Form",
      PolicyName = TNS_FORM,
      Enabled = true,
      CreatedAt = DateTime.UtcNow.ToAppDateTime(),
      UpdatedAt = DateTime.UtcNow.ToAppDateTime()
    };
    public static Entities.Program SupplierInformation { get; private set; } = new Entities.Program {
      Id = 4,
      Name = "Supplier Information",
      PolicyName = SUPPLIER_INFORMATION,
      Enabled = true,
      CreatedAt = DateTime.UtcNow.ToAppDateTime(),
      UpdatedAt = DateTime.UtcNow.ToAppDateTime()
    };
    public static Entities.Program OutGoingData { get; private set; } = new Entities.Program {
      Id = 5,
      Name = "Out Going Data",
      PolicyName = OUT_GOING_DATA,
      Enabled = true,
      CreatedAt = DateTime.UtcNow.ToAppDateTime(),
      UpdatedAt = DateTime.UtcNow.ToAppDateTime()
    };
    public static Entities.Program SelfControlledIpp { get; private set; } = new Entities.Program {
      Id = 6,
      Name = "Self Controlled IPP",
      PolicyName = SELF_CONTROLLED_IPP,
      Enabled = true,
      CreatedAt = DateTime.UtcNow.ToAppDateTime(),
      UpdatedAt = DateTime.UtcNow.ToAppDateTime()
    };
  }
}