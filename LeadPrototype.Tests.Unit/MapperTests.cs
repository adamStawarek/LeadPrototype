using System;
using System.Collections.Generic;
using System.Text;
using LeadPrototype.Libs;
using LeadPrototype.Libs.Models;
using NUnit.Framework;

namespace LeadPrototype.Tests.Unit
{
    public class MapperTests
    {
        [Test]
        public void After_Initialization_GetProducts_Returns_Array_Of_10337_Products()
        {
            var productCount = Mapper.GetProducts().Length;
            Assert.AreEqual(10337,productCount);
        }
        
        [Test]
        public void Index_Is_Match_To_Appropriate_Id_From_CorrMap_File()
        {
            Assert.Multiple(() =>
            {
                Assert.AreEqual(3,Mapper.MapToProduct(1).Id);
                Assert.AreEqual(43,Mapper.MapToProduct(35).Id);
                Assert.AreEqual(11982,Mapper.MapToProduct(10332).Id);
            });           
        }
        
        [Test]
        public void MapToIndex_Given_Product_Id_Returns_Appropriate_Index_From_CorrMap_File()
        {
            Assert.Multiple(() =>
            {
                Assert.AreEqual(1,Mapper.MapToIndex(new Product(){Id = 3}));
                Assert.AreEqual(35,Mapper.MapToIndex(new Product(){Id = 43}));
                Assert.AreEqual(10332,Mapper.MapToIndex(new Product(){Id = 11982}));
            });           
        }
    }
}
