using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using ScmDataAccess;
using SqliteScmTest;

namespace ScmConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var fixture = new SampleScmDataFixture();
            var context = fixture.Services.
              GetRequiredService<IScmContext>();
            var supplier = context.Suppliers.First();
            var part = context.Parts.First();
            var order = new Order() {
                SupplierId = supplier.Id,
                Supplier = supplier,
                PartTypeId = part.Id,
                //Part = part,
                PartCount = 10,
                PlacedDate = DateTime.Now
            };
            context.CreateOrder(order);
        }
    }
}
