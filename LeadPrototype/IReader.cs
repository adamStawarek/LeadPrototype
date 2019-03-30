using System.Collections.Generic;

namespace LeadPrototype
{
    public interface IReader
    {
        IReaderSettings Settings { get; set; }
        IEnumerable<Product> ReadObject();
        Dictionary<(int key1, int key2), int> ReadTable();
    }
}