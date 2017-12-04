using System;
using System.Data;
using System.Data.Common;
using System.Linq;
using Microsoft.Data.Sqlite;
using WidgetScmDataAccess;
using Xunit;

namespace SqliteScmTest
{
  public class UnitTest1 : IClassFixture<SampleScmDataFixture>
  {
    private SampleScmDataFixture fixture;
    private ScmContext context;
    
    public UnitTest1(SampleScmDataFixture fixture)
    {
      this.fixture = fixture;
      this.context = new ScmContext(fixture.Connection);
    }
    
    [Fact]
    public void Test1()
    {
      var parts = context.Parts;
      Assert.Equal(1, parts.Count());
      var part = parts.First();
      Assert.Equal("8289 L-shaped plate", part.Name);
      var inventory = context.Inventory;
      Assert.Equal(1, inventory.Count());
      var item = inventory.First();
      Assert.Equal(part.Id, item.PartTypeId);
      Assert.Equal(100, item.Count);
      Assert.Equal(10, item.OrderThreshold);
      Assert.Equal(item.Part.Id, item.PartTypeId);
    }

    [Fact]
    public void TestPartCommands()
    {
      var item = context.Inventory.First();
      var startCount = item.Count;
      context.CreatePartCommand(new PartCommand() {
        PartTypeId = item.PartTypeId,
        PartCount = 10,
        Command = PartCountOperation.Add
      });
      context.CreatePartCommand(new PartCommand() {
        PartTypeId = item.PartTypeId,
        PartCount = 5,
        Command = PartCountOperation.Remove
      });
      var inventory = new Inventory(context);
      inventory.UpdateInventory();
      Assert.Equal(startCount + 5, item.Count);
    }

    [Fact]
    public void TestUpdateInventory()
    {
      var item = context.Inventory.First();
      var totalCount = item.Count;
      context.CreatePartCommand(new PartCommand() {
        PartTypeId = item.PartTypeId,
        PartCount = totalCount,
        Command = PartCountOperation.Remove
      });

      var inventory = new Inventory(context);
      inventory.UpdateInventory();
      var order = context.GetOrders().FirstOrDefault(
        o => o.PartTypeId == item.PartTypeId &&
        !o.FulfilledDate.HasValue);
      Assert.NotNull(order);

      context.CreatePartCommand(new PartCommand() {
        PartTypeId = item.PartTypeId,
        PartCount = totalCount,
        Command = PartCountOperation.Add
      });

      inventory.UpdateInventory();
      Assert.Equal(totalCount, item.Count);
    }

    [Fact]
    public void TestCreateOrder()
    {
      var placedDate = DateTime.Now;
      var supplier = context.Suppliers.First();
      var order = new Order()
      {
        PartTypeId = supplier.PartTypeId,
        Part = context.Parts.Single(p => p.Id == supplier.PartTypeId),
        SupplierId = supplier.Id,
        Supplier = supplier,
        PartCount = 10,
        PlacedDate = placedDate
      };
      context.CreateOrder(order);

      var command = new SqliteCommand(
        @"SELECT Id FROM [Order] WHERE 
          SupplierId=@supplierId AND 
          PartTypeId=@partTypeId AND
          PlacedDate=@placedDate AND
          PartCount=10 AND
          FulfilledDate IS NULL",
        fixture.Connection);
      AddParameter(command, "@supplierId", supplier.Id);
      AddParameter(command, "@partTypeId", supplier.PartTypeId);
      AddParameter(command, "@placedDate", placedDate);
      var scalar = command.ExecuteScalar();
      Assert.NotNull(scalar);
      Assert.IsType<long>(scalar);
      long orderId = (long)scalar;
      Assert.Equal(order.Id, orderId);

      command = new SqliteCommand(
        $@"SELECT Count(*) FROM SendEmailCommand 
          WHERE [To]=@email AND 
          Subject LIKE @subject",
        fixture.Connection);
      AddParameter(command, "@email", supplier.Email);
      AddParameter(command, "@subject", $"Order #{order.Id}%");
      Assert.Equal(1, (long)command.ExecuteScalar());
    }

    [Fact]
    public void TestCreateOrderTransaction()
    {
      var placedDate = DateTime.Now;
      var supplier = context.Suppliers.First();
      var order = new Order()
      {
        PartTypeId = supplier.PartTypeId,
        SupplierId = supplier.Id,
        PartCount = 10,
        PlacedDate = placedDate
      };
      Assert.Throws<NullReferenceException>(() => context.CreateOrder(order));

      var command = new SqliteCommand(
        @"SELECT Count(*) FROM [Order] WHERE 
          SupplierId=@supplierId AND 
          PartTypeId=@partTypeId AND
          PlacedDate=@placedDate AND
          PartCount=10 AND
          FulfilledDate IS NULL",
        fixture.Connection);
      AddParameter(command, "@supplierId", supplier.Id);
      AddParameter(command, "@partTypeId", supplier.PartTypeId);
      AddParameter(command, "@placedDate", placedDate);
      Assert.Equal(0, (long)command.ExecuteScalar());
    }

    [Fact]
    public void TestCreateOrderSysTx()
    {
      var placedDate = DateTime.Now;
      var supplier = context.Suppliers.First();
      var order = new Order()
      {
        PartTypeId = supplier.PartTypeId,
        SupplierId = supplier.Id,
        PartCount = 10,
        PlacedDate = placedDate
      };
      Assert.Throws<NullReferenceException>(() => context.CreateOrderSysTx(order));

      var command = new SqliteCommand(
        @"SELECT Count(*) FROM [Order] WHERE 
          SupplierId=@supplierId AND 
          PartTypeId=@partTypeId AND
          PlacedDate=@placedDate AND
          PartCount=10 AND
          FulfilledDate IS NULL",
        fixture.Connection);
      AddParameter(command, "@supplierId", supplier.Id);
      AddParameter(command, "@partTypeId", supplier.PartTypeId);
      AddParameter(command, "@placedDate", placedDate);
      Assert.Equal(0, (long)command.ExecuteScalar());
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
