using System;
using System.Data.Common;
using Microsoft.Data.Sqlite;

namespace SqliteScmTest
{
  public class SampleScmDataFixture : IDisposable
  {
    private const string PartTypeTable = 
        @"CREATE TABLE PartType(
            Id INTEGER PRIMARY KEY,
            Name VARCHAR(255) NOT NULL
          );";
    private const string InventoryItemTable =
        @"CREATE TABLE InventoryItem(
            PartTypeId INTEGER PRIMARY KEY,
            Count INTEGER NOT NULL,
            OrderThreshold INTEGER,
            FOREIGN KEY(PartTypeId) REFERENCES PartType(Id)
          );";
    private const string PartCommandTable =
        @"CREATE TABLE PartCommand(
            Id INTEGER PRIMARY KEY,
            Count INTEGER NOT NULL,
            PartTypeId INTEGER NOT NULL,
            Command VARCHAR(50) NOT NULL,
            FOREIGN KEY(PartTypeId) REFERENCES PartType(Id)
          );";
    private const string SupplierTable =
        @"CREATE TABLE Supplier(
            Id INTEGER PRIMARY KEY,
            Name VARCHAR(255) NOT NULL,
            Email VARCHAR(255) NOT NULL,
            PartTypeId INTEGER NOT NULL,
            FOREIGN KEY(PartTypeId) REFERENCES PartType(Id)
          );";
    private const string OrderTable =
        @"CREATE TABLE [Order](
            Id INTEGER PRIMARY KEY,
            SupplierId INTEGER NOT NULL,
            PartTypeId INTEGER NOT NULL,
            PartCount INTEGER NOT NULL,
            PlacedDate DATETIME NOT NULL,
            FulfilledDate DATETIME,
            FOREIGN KEY(SupplierId) REFERENCES Supplier(Id),
            FOREIGN KEY(PartTypeId) REFERENCES PartType(Id)
          );";
    private const string SendEmailCommandTable =
        @"CREATE TABLE SendEmailCommand(
            Id INTEGER PRIMARY KEY,
            [To] VARCHAR(255) NOT NULL,
            Subject VARCHAR(255) NOT NULL,
            Body BLOB
          );";
    public SqliteConnection Connection { get; private set; }
    
    public SampleScmDataFixture()
    {
      var conn = new SqliteConnection("Data Source=:memory:");
      Connection = conn;
      conn.Open();
      
      (new SqliteCommand(PartTypeTable, conn)).ExecuteNonQuery();
      (new SqliteCommand(InventoryItemTable, conn)).ExecuteNonQuery();
      (new SqliteCommand(PartCommandTable, conn)).ExecuteNonQuery();
      (new SqliteCommand(SupplierTable, conn)).ExecuteNonQuery();
      (new SqliteCommand(OrderTable, conn)).ExecuteNonQuery();
      (new SqliteCommand(SendEmailCommandTable, conn)).ExecuteNonQuery();
      (new SqliteCommand(
        @"INSERT INTO PartType
            (Id, Name)
            VALUES
            (0, '8289 L-shaped plate')",
        conn)).ExecuteNonQuery();
      (new SqliteCommand(
        @"INSERT INTO InventoryItem
            (PartTypeId, Count, OrderThreshold)
            VALUES
            (0, 100, 10)",
        conn)).ExecuteNonQuery();
      (new SqliteCommand(
        @"INSERT INTO Supplier
            (Name, Email, PartTypeId)
            VALUES
            ('Joe Supplier', 'joe@joesupplier.com', 0)",
        conn)).ExecuteNonQuery();
    }
    
    public void Dispose()
    {
      if (Connection != null)
        Connection.Dispose();
    }
  }
}
