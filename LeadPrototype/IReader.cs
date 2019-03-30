using System.Collections.Generic;

namespace LeadPrototype
{
    public interface IReader
    {
        IReaderSettings Settings { get; set; }
        IEnumerable<Product> ReadObject();
        IEnumerable<Product> ReadTable();
    }
}