using System;
using System.Linq;
using ScmDataAccess;
using Xunit;

namespace ScmDalTest
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            using (var ctxt = new ScmContext())
            {
                var partName = "Sample" + DateTime.Now.ToString("HHmmss");
                var part = new PartType() {
                    Name = partName
                };
                ctxt.Parts.Add(part);
                ctxt.SaveChanges();

                var getPart = ctxt.Parts.Single(
                    p => p.Name == partName);
                Assert.Equal(getPart.Name, part.Name);

                ctxt.Parts.Remove(getPart);
                ctxt.SaveChanges();

                getPart = ctxt.Parts.FirstOrDefault(
                    p => p.Name == partName);
                Assert.Null(getPart);
            }
        }
    }
}
