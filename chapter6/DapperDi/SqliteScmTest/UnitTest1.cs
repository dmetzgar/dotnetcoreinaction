using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using ScmDataAccess;
using Xunit;

namespace SqliteScmTest
{
    public class UnitTest1 : IClassFixture<SampleScmDataFixture>
    {
        private SampleScmDataFixture fixture;
        private IScmContext context;

        public UnitTest1(SampleScmDataFixture fixture)
        {
            this.fixture = fixture;
            this.context = fixture.Services.
              GetRequiredService<IScmContext>();
        }

        [Fact]
        public void Test1()
        {
            var orders = context.GetOrders();
            Assert.Equal(0, orders.Count());
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
            Assert.NotEqual(0, order.Id);
            orders = context.GetOrders();
            Assert.Equal(1, orders.Count());
        }
    }
}
