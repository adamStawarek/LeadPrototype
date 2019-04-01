using LeadPrototype.Readers;
using LeadPrototype.Readers.Settings;
using NUnit.Framework;

namespace LeadPrototype.Tests.Unit
{
    public class ReaderFactoryTests
    {
        [Test]
        public void CreateReader_Returns_CsvReader_When_CsvSettings_Are_Passed_As_Parameter()
        {
            var settings = new CsvSettings("");
            var reader = ReaderFactory.CreateReader(settings);
            Assert.IsInstanceOf<CsvReader>(reader);
        }
    }
}
