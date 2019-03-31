using System.Collections.Generic;

namespace LeadPrototype
{
    public interface IReader
    {
        IReaderSettings Settings { get; set; }
        IEnumerable<Product> ReadObject();
        Dictionary<int, int[]> ReadTable();
    }
}