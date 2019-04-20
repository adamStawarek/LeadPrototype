using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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
        public CsvReader(ILogger logger, CsvSettings settings) : base(logger, settings){}

        public override IEnumerable<Product> ReadProducts()
        {
            var filePath = ((CsvSettings) Settings).PathToProducts;
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

                        if (isFirstLine)
                        {
                            isFirstLine = false;
                            continue;
                        }

                        var values = line?.Split(';');
                        if (values == null) continue;
                        requests.Add(new Product()
                        {
                            Id = int.Parse(values[0]),
                            ProductName = values[1],
                            CategoryId = int.Parse(values[2]),
                            CategoryName = values[3],
                            AveragePrice = decimal.Parse(values[4], CultureInfo.InvariantCulture)
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

        public override Table ReadTable(TableType type)
        {
            var filePath = ((CsvSettings)Settings).PathToTable;
            if (!CheckFilePath(filePath)) return null;

            try
            {
                var lines = File.ReadLines(filePath);

                var separator = new[] { ',' };

                var result = lines.AsParallel().AsOrdered().Select((line, index) =>
                {
                    var values = line?.Split(separator, StringSplitOptions.RemoveEmptyEntries)
                        .Select(f => float.Parse(f, CultureInfo.InvariantCulture)).ToArray();
                    return (Mapper.MapToProduct(index).Id, values);
                }).ToDictionary(d => d.Item1, d => d.Item2);
            
                switch (type)
                {
                    case TableType.Correlation:
                        return new CorrelationTable() { Content = result };
                    case TableType.Substitutes:
                        return new SubstitutesTable(){Content = result};
                    default:
                        throw new InvalidEnumArgumentException();
                }
               
            }
            catch (Exception e)
            {
                Logger.Write(LogEventLevel.Warning,
                    $"Exception occured when reading {filePath.Split('\\').Last()}, exception: {e.InnerException}");
                return null;
            }
        }
    }
}