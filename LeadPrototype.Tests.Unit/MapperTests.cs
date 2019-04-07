using System;
using System.Collections.Generic;
using System.Text;
using LeadPrototype.Libs;
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
    }
}
