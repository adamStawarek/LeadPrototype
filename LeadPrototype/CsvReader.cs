using Serilog;
using Serilog.Events;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LeadPrototype
{
    public class CsvReader : BaseReader
    {
        public CsvReader(ILogger logger, CsvSettings settings) : base(logger, settings) { }

        public override IEnumerable<Product> ReadObject()
        {
            var filePath = ((CsvSettings)Settings).FilePath;
            var isHeader = ((CsvSettings)Settings).IsHeader;
            if (!CheckFilePath(filePath)) return null;

            try
            {
                var requests = new List<Product>();
                var isFirstLine = true;
                using (var reader =
                    new StreamReader(filePath, Encoding.Default, true))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();

                        if (isFirstLine && isHeader)
                        {
                            isFirstLine = false;
                            continue;
                        }
                        var values = line?.Split(',');
                        if (values == null) continue;
                        requests.Add(new Product()
                        {
                            Id = int.Parse(values[0])
                        });

                    }
                }
                return requests;
            }
            catch
            {
                Logger.Write(LogEventLevel.Warning, $"Exception occured when reading {filePath.Split('\\').Last()}");
                return null;
            }
        }

        private bool CheckFilePath(string filePath)
        {
            if (filePath.EndsWith(".csv") && File.Exists(filePath)) return true;
            Logger.Write(LogEventLevel.Warning, $"Not supported extension");
            return false;
        }

        public override IEnumerable<Product> ReadTable()
        {
            throw new System.NotImplementedException();
        }
    }
}