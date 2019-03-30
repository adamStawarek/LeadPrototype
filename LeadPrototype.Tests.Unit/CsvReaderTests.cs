using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace LeadPrototype.Tests.Unit
{
    public class CsvReaderTests
    {
        [Test]
        public void ReadObject_Returns_Collection_Of_3_Products()
        {
            var settings = new CsvSettings(@"../../../products.csv") { IsHeader = true };
            var reader = ReaderFactory.CreateReader(settings);
            var expected = new List<Product>
            {
                new Product(){Id = 1},
                new Product(){Id=2},
                new Product(){Id=3}
            };
            Assert.Multiple(() =>
            {
                var actual = reader.ReadObject().ToList();
                Assert.AreEqual(3, actual.Count());
                CollectionAssert.AreEquivalent(expected, actual);
            });
        }
    }
}