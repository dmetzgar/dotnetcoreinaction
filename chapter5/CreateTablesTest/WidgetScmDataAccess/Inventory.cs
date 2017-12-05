using System;
using System.Linq;

namespace WidgetScmDataAccess
{
  public class Inventory
  {
    private ScmContext context;
    public Inventory(ScmContext context)
    {
      this.context = context;
    }

    public void UpdateInventory() 
    {
      foreach (var cmd in context.GetPartCommands())
      {
        var item = context.Inventory.Single(i => i.PartTypeId == cmd.PartTypeId);
        if (cmd.Command == PartCountOperation.Add)
          item.Count += cmd.PartCount;
        else
          item.Count -= cmd.PartCount;
        
        var transaction = context.BeginTransaction();
        try {
          context.UpdateInventoryItem(item.PartTypeId, item.Count, transaction);
          context.DeletePartCommand(cmd.Id, transaction);
          transaction.Commit();
        }
        catch {
          transaction.Rollback();
          throw;
        }
      }

      var orders = context.GetOrders();

      foreach (var item in context.Inventory)
      {
        if (item.Count < item.OrderThreshold &&
          orders.FirstOrDefault(o => 
          o.PartTypeId == item.PartTypeId && 
          !o.FulfilledDate.HasValue) == null)
        {
          OrderPart(item.Part, item.OrderThreshold);
        }
      }
    }

    public void OrderPart(PartType part, int count)
    {
      var order = new Order() {
        PartTypeId = part.Id,
        PartCount = count,
        PlacedDate = DateTime.Now
      };
      order.Part = context.Parts.Single(p => p.Id == order.PartTypeId);
      order.Supplier = context.Suppliers.First(s => s.PartTypeId == part.Id);
      order.SupplierId = order.Supplier.Id;
      context.CreateOrder(order);
    }
  }
}
