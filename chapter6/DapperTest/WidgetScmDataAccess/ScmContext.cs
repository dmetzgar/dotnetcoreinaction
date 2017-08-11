using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using Dapper;

namespace WidgetScmDataAccess
{
  public class ScmContext
  {
    private DbConnection connection;
    public IEnumerable<PartType> Parts { get; private set; }
    public IEnumerable<InventoryItem> Inventory { get; private set; }
    public IEnumerable<Supplier> Suppliers { get; private set; }
    
    public ScmContext(DbConnection conn)
    {
      connection = conn;
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
      var command = connection.CreateCommand();
      if (transaction != null)
        command.Transaction = transaction;
      command.CommandText = @"DELETE FROM PartCommands
        WHERE Id=@id";
      AddParameter(command, "@id", id);
      command.ExecuteNonQuery();
    }

    public void UpdateInventoryItem(int partTypeId, int count, 
      DbTransaction transaction)
    {
      var command = connection.CreateCommand();
      if (transaction != null)
        command.Transaction = transaction;
      command.CommandText = @"UPDATE InventoryItem 
        SET Count=@count
        WHERE PartTypeId=@partTypeId";
      AddParameter(command, "@count", count);
      AddParameter(command, "@partTypeId", partTypeId);
      command.ExecuteNonQuery();
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
      catch {
        transaction.Rollback();
        throw;
      }
    }

    public DbTransaction BeginTransaction() 
    {
      return connection.BeginTransaction();
    }

    public IEnumerable<Order> GetOrders()
    {
      var command = connection.CreateCommand();
      command.CommandText = @"SELECT 
          Id, SupplierId, PartTypeId, PartCount, PlacedDate, FulfilledDate 
        FROM Order";
      var reader = command.ExecuteReader();
      var orders = new List<Order>();
      while (reader.Read())
      {
        var order = new Order() {
          Id = reader.GetInt32(0),
          SupplierId = reader.GetInt32(1),
          PartTypeId = reader.GetInt32(2),
          PartCount = reader.GetInt32(3),
          PlacedDate = reader.GetDateTime(4),
          FulfilledDate = reader.IsDBNull(5) ? 
            default(DateTime?) : reader.GetDateTime(5)
        };
        order.Part = Parts.Single(p => p.Id == order.PartTypeId);
        order.Supplier = Suppliers.First(s => s.Id == order.SupplierId);
        orders.Add(order);
      }  

      return orders;
    }

    private void AddParameter(DbCommand cmd, string name, object value)
    {
      var p = cmd.CreateParameter();
      if (value == null)
        throw new ArgumentNullException("value");
      Type t = value.GetType();
      if (t == typeof(int))
        p.DbType = DbType.Int32;
      else if (t == typeof(string))
        p.DbType = DbType.String;
      else if (t == typeof(DateTime))
        p.DbType = DbType.DateTime;
      p.Direction = ParameterDirection.Input;
      p.ParameterName = name;
      p.Value = value;
      cmd.Parameters.Add(p);
    }
  }
}
