using System.Collections.Generic;
using LeadPrototype.Libs.Models;
using LeadPrototype.Libs.Readers.Settings;

namespace LeadPrototype.Libs.Readers
{
    public interface IReader
    {
        IReaderSettings Settings { get; set; }
        IEnumerable<Product> ReadProducts();
        Table ReadTable(TableType type);
    }
}