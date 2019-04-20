using LeadPrototype.Libs;
using LeadPrototype.Libs.Models;
using LeadPrototype.Libs.Readers;
using LeadPrototype.Libs.Readers.Settings;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace LeadPrototype.Tests.Unit
{
    public class PacketBuilderTests
    {
        private List<Product> _products;
        private CorrelationTable _correlationTable;
        private SubstitutesTable _substitutesTable;

        [SetUp]
        public void SetUp()
        {
            var settings = new CsvSettings(@"../../../Tmp/products.csv", @"../../../Tmp/products_corr_no_header.csv");
            var reader = ReaderFactory.CreateReader(settings);
            _products = reader.ReadProducts().ToList();          
            _correlationTable = (CorrelationTable)reader.ReadTable(TableType.Correlation);
            ((CsvSettings) reader.Settings).PathToTable = @"../../../Tmp/substitutes.csv";
            _substitutesTable = (SubstitutesTable)reader.ReadTable(TableType.Substitutes);
        }

        [Test]
        public void When_Add_Constraint_Number_Of_Products_Is_Appropriate_Less()
        {
            var packetFactory = new PacketBuilder()
                .AddProducts(_products);
            var oldProductCount = packetFactory.GetProductsCount();
            packetFactory.AddPacketConstraint(p => p.Id <= 1);
            var newProductCount = packetFactory.GetProductsCount();
            var differenceCount = oldProductCount - newProductCount;
            Assert.AreEqual(2,differenceCount);
        }

        [Test]
        public void When_Only_Correlation_Table_Is_Provided_CreatePackets_Returns_Packets_With_Correct_Product_But_Without_Substitutes()
        {
            var packetFactory=new PacketBuilder()
                .AddProducts(_products)
                .AddCorrelationTable(_correlationTable);

            var packets = packetFactory.CreatePackets().OrderBy(p => p.PacketProducts[0].Product.Id).ToList();
            {
                Assert.AreEqual(3,packets.Count);
                Assert.AreEqual(1, packets[0].PacketProducts[0].Product.Id);
                Assert.AreEqual(3,packets[0].PacketProducts[1].Product.Id);
                Assert.AreEqual(2, packets[0].Correlation);

                Assert.AreEqual(2, packets[1].PacketProducts[0].Product.Id);
                Assert.AreEqual(4, packets[1].PacketProducts[1].Product.Id);
                Assert.AreEqual(4, packets[1].Correlation);

                Assert.AreEqual(3, packets[2].PacketProducts[0].Product.Id);
                Assert.AreEqual(4, packets[2].PacketProducts[1].Product.Id);
                Assert.AreEqual(3, packets[2].Correlation);
            };
        }

        [Test]
        public void When_Both_Tables_Are_Provided_CreatePackets_Returns_Packets_With_Correct_Product_And_Its_Substitutes()
        {
            var packetFactory = new PacketBuilder()              
                .AddProducts(_products)
                .AddPacketConstraint(p=>p.Id==3)
                .AddCorrelationTable(_correlationTable)
                .AddSubstitutesTable(_substitutesTable)
                .SetNumberOfSubstitutes(3);


            var packets = packetFactory.CreatePackets().OrderBy(p => p.PacketProducts[0].Product.Id).ToList();
            {
                Assert.AreEqual(1, packets.Count);
                Assert.AreEqual(3, packets[0].PacketProducts[0].Product.Id);
                Assert.AreEqual(4, packets[0].PacketProducts[1].Product.Id);
                Assert.AreEqual(3, packets[0].PacketProducts[0].Substitutes.Count);     
                CollectionAssert.AreEquivalent(new List<int> { 4, 6, 9 }, packets[0].PacketProducts[0].Substitutes.Select(s=>s.Key.Id));
            };
        }
    }
}