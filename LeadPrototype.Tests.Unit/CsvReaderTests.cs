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
            var settings = new CsvSettings(@"../../../Tmp/products.csv","");
            var reader = ReaderFactory.CreateReader(settings);
            var expected = new List<Product>
            {
                new Product(){Id=1, ProductName = "Piwo specjalne butelka 149",CategoryId = 13,CategoryName = "Piwo",AveragePrice = 3.46m},
                new Product(){Id=3, ProductName = "Piwo jasne premium butelka 89",CategoryId = 13,CategoryName = "Piwo",AveragePrice = 2.55m},
                new Product(){Id=4, ProductName = "Fast-food pozostałe 39",CategoryId = 6,CategoryName = "Żabka Cafe",AveragePrice = 4.97m},
                new Product(){Id=5, ProductName = "Warzywa, owoce, rośliny REG 258",CategoryId = 18,CategoryName = "Warzywa & Owoce & Rośliny",AveragePrice = 7.26m}
            };
            
             var actual = reader.ReadObject().ToList();
             
            Assert.Multiple(() =>
            {              
                Assert.AreEqual(4, actual.Count());
                CollectionAssert.AreEquivalent(expected, actual);
            });
        }

        [Test]
        public void ReadTable_Returns_Collection_Of_8_Dictionaries_In_Which_Keys_Are_Product_Indexes()
        {
            var settings = new CsvSettings("",@"../../../Tmp/products_corr_no_header.csv");
            var expected = new List<int> {1, 3, 4, 5, 6, 8, 9, 10};
            var reader = ReaderFactory.CreateReader(settings);
            var table = reader.ReadTable(TableType.Correlation);
            Assert.Multiple(() =>
            {
                Assert.AreEqual(8,table.Content.Count);
                CollectionAssert.AreEquivalent(expected,table.Content.Keys);
            });                       
        }  
        
        [Test]
        public void ReadTable_No_Exception_Is_Thrown_When_Read_File_With_All_Products()
        {
            var settings = new CsvSettings("",@"../../../../Tmp/corelation_table.csv");
            var reader = ReaderFactory.CreateReader(settings);
            Assert.DoesNotThrow(()=>reader.ReadTable(TableType.Correlation));                     
        }  
    }
}