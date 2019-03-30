using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace LeadPrototype.Tests.Unit
{
    public class PacketBuilderTests
    {
        private List<Product> _products;
        private Dictionary<(int key1, int key2), int> _table;

        [SetUp]
        public void SetUp()
        {
            var settings = new CsvSettings(@"../../../products.csv") { IsHeader = true };
            var reader1 = ReaderFactory.CreateReader(settings);
            _products = reader1.ReadObject().ToList();

            settings = new CsvSettings(@"../../../products_corr.csv") { IsHeader = true };
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
            Assert.AreEqual(3,packets.Count);
        }
    }
}