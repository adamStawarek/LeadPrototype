using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using LeadPrototype.Libs.Models;
using LeadPrototype.Libs.Readers.Settings;
using Serilog;
using Serilog.Events;

namespace LeadPrototype.Libs.Readers
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

        public override Dictionary<int, int[]> ReadTable()
        {

            var filePath = ((CsvSettings)Settings).FilePath;
            var isHeader = ((CsvSettings)Settings).IsHeader;
            if (!CheckFilePath(filePath)) return null;

            try
            {
                if (!isHeader)
                {
                    var lines = File.ReadAllLines(filePath).ToList();
                    var result = lines.AsParallel().AsOrdered().Select((line, index) =>
                    {
                        Logger.Write(LogEventLevel.Information,$"parse {index} table row");
                        var values = line?.Split(',').Where(v => !string.IsNullOrEmpty(v)).Select(int.Parse).ToArray();
                        return (index+1, values);
                    }).ToDictionary(d => d.Item1, d => d.Item2);
                    return result;
                }
                else
                {
                    var lines = File.ReadAllLines(filePath).ToList();
                    lines.RemoveAt(0);
                    var result = lines.AsParallel().AsOrdered().Select((line,index) =>
                    {
                        Logger.Write(LogEventLevel.Information, $"parse {index} table row");
                        var values = line?.Split(',').Where(v => !string.IsNullOrEmpty(v)).Select(int.Parse).ToList();
                        var idx = values[0];
                        values.RemoveAt(0);
                        return (idx, values.ToArray());
                    }).ToDictionary(d => d.Item1, d => d.Item2);
                    return result;
                }

            }
            catch
            {
                Logger.Write(LogEventLevel.Warning,
                    $"Exception occured when reading {filePath.Split('\\').Last()}");
                return null;
            }
        }

    }
}