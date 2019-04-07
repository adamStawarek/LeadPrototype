using System.Collections.Generic;
using LeadPrototype.Libs.Models;
using LeadPrototype.Libs.Readers.Settings;
using Serilog;

namespace LeadPrototype.Libs.Readers
{
    public abstract class BaseReader : IReader
    {
        protected readonly ILogger Logger;
        public IReaderSettings Settings { get; set; }

        protected BaseReader(ILogger logger, IReaderSettings settings)
        {
            Logger = logger;
            Settings = settings;
        }     
        public abstract IEnumerable<Product> ReadObject();
        public abstract Dictionary<int, int[]> ReadTable();
    }
}