using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Dapper;
using Microsoft.Data.Sqlite;
using ScmDataAccess;

namespace SqliteDal
{
  public class SqliteScmContext : IScmContext
  {
    private SqliteConnection connection;
    public IEnumerable<PartType> Parts { get; private set; }
    public IEnumerable<InventoryItem> Inventory { get; private set; }
    public IEnumerable<Supplier> Suppliers { get; private set; }
    
    public SqliteScmContext(SqliteConnection conn)
    {
      connection = conn;
      conn.Open();
      Parts = conn.Query<PartType>("SELECT * FROM PartType");
      Inventory = conn.Query<InventoryItem>("SELECT * FROM InventoryItem");
      foreach (var item in Inventory)
        item.Part = Parts.Single(p => p.Id == item.PartTypeId);
      Suppliers = conn.Query<Supplier>("SELECT * FROM Supplier");
      foreach (var supplier in Suppliers)
        supplier.Part = Parts.Single(p => p.Id == supplier.PartTypeId);
    }

    public PartCommand[] GetPartCommands()
    {
      return connection.Query<PartCommand>("SELECT * FROM PartCommand").ToArray();
    }

    public void DeletePartCommand(int id, DbTransaction transaction)
    {
      connection.Execute(@"DELETE FROM PartCommands
        WHERE Id=@Id", new { Id = id }, transaction);
    }

    public void UpdateInventoryItem(int partTypeId, int count, 
      DbTransaction transaction)
    {
      connection.Execute(@"UPDATE InventoryItem 
        SET Count=@Count
        WHERE PartTypeId=@PartTypeId",
        new { Count = count, PartTypeId = partTypeId},
        transaction);
    }

    public void CreateOrder(Order order)
    {
      var transaction = connection.BeginTransaction();
      try {
        order.Id = connection.Query<int>(
          @"INSERT INTO [Order]
          (SupplierId, PartTypeId, PartCount, 
          PlacedDate) VALUES (@SupplierId, 
          @PartTypeId, @PartCount, @PlacedDate);
          SELECT last_insert_rowid();", order,
          transaction).First();

        connection.Execute(@"INSERT INTO SendEmailCommand
          ([To], Subject, Body) VALUES
          (@To, @Subject, @Body)", new {
            To = order.Supplier.Email,
            Subject = $"Order #{order.Id} for {order.Part.Name}",
            Body = $"Please send {order.PartCount}" + 
              $" items of {order.Part.Name} to Widget Corp"
          }, transaction);

        transaction.Commit();
      }
      catch (Exception exc) {
        transaction.Rollback();
        throw new AggregateException(exc);
      }
    }

    public DbTransaction BeginTransaction() 
    {
      return connection.BeginTransaction();
    }

    public IEnumerable<Order> GetOrders()
    {
      var orders = connection.Query<Order>(
        "SELECT * FROM [Order]");
      foreach (var order in orders)
      {
        order.Part = Parts.Single(p => p.Id == order.PartTypeId);
        order.Supplier = Suppliers.Single(s => s.Id == order.SupplierId);
      }

      return orders;
    }
  }
}
