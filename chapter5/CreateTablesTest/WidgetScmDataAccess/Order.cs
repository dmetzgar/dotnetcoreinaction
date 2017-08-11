using System;

namespace WidgetScmDataAccess
{
  public class Order
  {
    public int Id { get; set; }
    public int SupplierId { get; set; }
    public Supplier Supplier { get; set; }
    public int PartTypeId { get; set; }
    public PartType Part { get; set; }
    public int PartCount { get; set; }
    public DateTime PlacedDate { get; set; }
    public DateTime? FulfilledDate { get; set; }
  }
}
