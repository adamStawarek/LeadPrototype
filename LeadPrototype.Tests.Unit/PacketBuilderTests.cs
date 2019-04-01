using System.Collections.Generic;
using System.Linq;
using LeadPrototype.Models;
using LeadPrototype.Readers;
using LeadPrototype.Readers.Settings;
using NUnit.Framework;

namespace LeadPrototype.Tests.Unit
{
    public class PacketBuilderTests
    {
        private List<Product> _products;
        private Dictionary<int, int[]> _table;

        [SetUp]
        public void SetUp()
        {
            var settings = new CsvSettings(@"../../../Tmp/products.csv") { IsHeader = true };
            var reader1 = ReaderFactory.CreateReader(settings);
            _products = reader1.ReadObject().ToList();

            settings = new CsvSettings(@"../../../Tmp/products_corr_no_header.csv") { IsHeader = false };
            var reader2 = ReaderFactory.CreateReader(settings);
            _table = reader2.ReadTable();
        }

        [Test]
        public void When_Products_And_Table_Are_Provided_CreatePackets_Returns_Best_Match_For_Each_Product()
        {
            var packetFactory=new PacketBuilder()
                .AddProducts(_products)
                .AddCorrelationTable(_table);
            var packets=packetFactory.CreatePackets();
            Assert.Multiple(() =>
            {
                Assert.AreEqual(3,packets.Count);
                Assert.AreEqual(1, packets[0].prod1);
                Assert.AreEqual(3,packets[0].prod2);
                Assert.AreEqual(2, packets[0].val);

                Assert.AreEqual(2, packets[1].prod1);
                Assert.AreEqual(4, packets[1].prod2);
                Assert.AreEqual(4, packets[1].val);

                Assert.AreEqual(3, packets[2].prod1);
                Assert.AreEqual(4, packets[2].prod2);
                Assert.AreEqual(3, packets[2].val);
            });
        }
    }
}