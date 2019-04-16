using System;
using System.Collections.Generic;
using System.Text;
using LeadPrototype.Libs.Helpers;
using NUnit.Framework;

namespace LeadPrototype.Tests.Unit
{
    public class ListExtensionsTests
    {
        [Test]
        public void GetNIndexesOfBiggestValues_Returns_Indexes_Of_N_Biggest_Values_From_A_List()
        {
            var list = new List<float> {1, 2, 3, 4, 5, 2, 3, 4, 5};
            var indexes = list.GetNIndexesOfBiggestValues(4);
            CollectionAssert.AreEquivalent(new int[]{3,4,7,8},indexes);
        }
    }
}
