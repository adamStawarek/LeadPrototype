using System.Collections.Generic;
using System.Diagnostics;
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
        public void ReadProducts_Returns_Collection_Of_3_Products()
        {
            var settings = new CsvSettings(@"../../../Tmp/products.csv","");
            var reader = ReaderFactory.CreateReader(settings);
            var expected = new List<Product>
            {
                new Product(){Id=1, ProductName = "Rogale impulsowe 18",CategoryId = 15,CategoryName = "Słodycze",AveragePrice = 2.69m},
                new Product(){Id=2, ProductName = "Znicze, wkłady 219",CategoryId = 5,CategoryName = "Niespożywcze",AveragePrice = 4.14m},
                new Product(){Id=3, ProductName = "Nabiał REG 161",CategoryId = 17,CategoryName = "Spożywka świeża",AveragePrice = 1.98m}
            };
            
             var actual = reader.ReadProducts().ToList();
             
            Assert.Multiple(() =>
            {              
                Assert.AreEqual(3, actual.Count());
                CollectionAssert.AreEquivalent(expected, actual);
            });
        }

        [Test]
        public void ReadTable_Returns_Collection_Of_8_Dictionaries_In_Which_Keys_Are_Product_Indexes()
        {
            var settings = new CsvSettings("",@"../../../Tmp/products_corr_no_header.csv");
            var expected = new List<int> {1,2, 3, 4, 5, 6, 8, 9};
            var reader = ReaderFactory.CreateReader(settings);
            var table = reader.ReadTable(TableType.Correlation);
            Assert.Multiple(() =>
            {
                Assert.AreEqual(8,table.Content.Count);
                CollectionAssert.AreEquivalent(expected,table.Content.Keys);
            });                       
        }  
        
        [Test]
        [Ignore("Big file, takes too much time to complete")]
        public void ReadTable_No_Exception_Is_Thrown_When_Read_File_With_All_Products()
        {
            var settings = new CsvSettings("",@"../../../../Tmp/corelation_table.csv");
            var reader = ReaderFactory.CreateReader(settings);
            Assert.DoesNotThrow(()=>reader.ReadTable(TableType.Correlation));                     
        }      
    }

    [TestFixture]
    public class ConvertersTests
    {
        public void PartialAnonymizer_Converts_Last_Number_Of_String_To_Hash()
        {

        }
    }
}