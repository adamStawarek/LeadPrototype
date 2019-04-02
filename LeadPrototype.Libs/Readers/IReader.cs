using System.Collections.Generic;
using LeadPrototype.Models;
using LeadPrototype.Readers.Settings;

namespace LeadPrototype.Readers
{
    public interface IReader
    {
        IReaderSettings Settings { get; set; }
        IEnumerable<Product> ReadObject();
        Dictionary<int, int[]> ReadTable();
    }
}