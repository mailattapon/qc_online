namespace QcSupplier.Entities {
  public class UserProgram {
    public int UserId { get; set; }
    public int ProgramId { get; set; }
    public virtual User User { get; set; }
    public virtual Program Program { get; set; }
  }
}