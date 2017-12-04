using System;
using System.Linq;

namespace WidgetScmDataAccess
{
  public class Inventory
  {
    private ScmContext _context;
    public Inventory(ScmContext context)
    {
      _context = context;
    }

    public void UpdateInventory() 
    {
      foreach (var cmd in _context.GetPartCommands())
      {
        var item = _context.Inventory.Single(i => i.PartTypeId == cmd.PartTypeId);
        if (cmd.Command == PartCountOperation.Add)
          item.Count += cmd.PartCount;
        else
          item.Count -= cmd.PartCount;
        
        var transaction = _context.BeginTransaction();
        try {
          _context.UpdateInventoryItem(item.PartTypeId, item.Count, transaction);
          _context.DeletePartCommand(cmd.Id, transaction);
          transaction.Commit();
        }
        catch {
          transaction.Rollback();
          throw;
        }
      }

      var orders = _context.GetOrders();

      foreach (var item in _context.Inventory)
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
      order.Part = _context.Parts.Single(p => p.Id == order.PartTypeId);
      order.Supplier = _context.Suppliers.First(s => s.PartTypeId == part.Id);
      order.SupplierId = order.Supplier.Id;
      _context.CreateOrder(order);
    }
  }
}
