using System.Collections.Generic;
using System.Linq;
using LeadPrototype.Libs.Models;
using LeadPrototype.Libs.Readers;
using LeadPrototype.Libs.Readers.Settings;
using NUnit.Framework;

namespace LeadPrototype.Tests.Unit
{
    public class CsvReaderTests
    {
        [Test]
        public void ReadObject_Returns_Collection_Of_3_Products()
        {
            var settings = new CsvSettings(@"../../../Tmp/products.csv") { IsHeader = true };
            var reader = ReaderFactory.CreateReader(settings);
            var expected = new List<Product>
            {
                new Product(){Id=1},
                new Product(){Id=2},
                new Product(){Id=3},
                new Product(){Id=8}
            };
            Assert.Multiple(() =>
            {
                var actual = reader.ReadObject().ToList();
                Assert.AreEqual(4, actual.Count());
                CollectionAssert.AreEquivalent(expected, actual);
            });
        }

        [Test]
        public void ReadTable_Returns__Collections_With_8_Items()
        {
            var settings = new CsvSettings(@"../../../Tmp/products_corr_no_header.csv") { IsHeader = false };
            var reader = ReaderFactory.CreateReader(settings);
            Assert.AreEqual(8, reader.ReadTable().Count());
        }       
    }
}