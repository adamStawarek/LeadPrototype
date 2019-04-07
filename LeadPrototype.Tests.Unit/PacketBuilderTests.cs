using System.Collections.Generic;
using System.Linq;
using LeadPrototype.Libs;
using LeadPrototype.Libs.Models;
using LeadPrototype.Libs.Readers;
using LeadPrototype.Libs.Readers.Settings;
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
            var settings = new CsvSettings(@"../../../Tmp/products.csv", @"../../../Tmp/products_corr_no_header.csv");
            var reader = ReaderFactory.CreateReader(settings);
            _products = reader.ReadObject().ToList();          
            _table = reader.ReadTable();
        }

        [Test]
        public void When_Products_And_Table_Are_Provided_CreatePackets_Returns_Best_Match_For_Each_Product()
        {
            var packetFactory=new PacketBuilder()
                .AddProducts(_products)
                .AddCorrelationTable(_table);
            var packets=packetFactory.CreatePackets().OrderBy(p=>p.prod1).ToList();
            Assert.Multiple(() =>
            {
                Assert.AreEqual(4,packets.Count);
                Assert.AreEqual(1, packets[0].prod1);
                Assert.AreEqual(4,packets[0].prod2);
                Assert.AreEqual(2, packets[0].val);

                Assert.AreEqual(3, packets[1].prod1);
                Assert.AreEqual(5, packets[1].prod2);
                Assert.AreEqual(4, packets[1].val);

                Assert.AreEqual(4, packets[2].prod1);
                Assert.AreEqual(5, packets[2].prod2);
                Assert.AreEqual(3, packets[2].val);

                Assert.AreEqual(5, packets[3].prod1);
                Assert.AreEqual(6, packets[3].prod2);
                Assert.AreEqual(2, packets[3].val);
            });
        }
    }
}