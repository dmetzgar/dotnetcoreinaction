using System.Collections.Generic;
using System.Data.Common;

namespace ScmDataAccess
{
  public interface IScmContext
  {
    IEnumerable<PartType> Parts { get; }
    IEnumerable<InventoryItem> Inventory { get; }
    IEnumerable<Supplier> Suppliers { get; }

    PartCommand[] GetPartCommands();
    void DeletePartCommand(int id, DbTransaction transaction);
    void UpdateInventoryItem(int partTypeId, int count, 
      DbTransaction transaction);
    void CreateOrder(Order order);
    DbTransaction BeginTransaction();
    IEnumerable<Order> GetOrders();
  }
}
