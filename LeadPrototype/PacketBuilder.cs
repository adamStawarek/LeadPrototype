using Serilog;
using Serilog.Events;
using Serilog.Sinks.InMemory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LeadPrototype
{
    public class PacketBuilder
    {
        private List<Product> _products;
        private Dictionary<(int key1, int key2), int> _table;
        private readonly ILogger _logger;

        public PacketBuilder()
        {
            _logger = new LoggerConfiguration()
                .WriteTo.InMemory()
                .WriteTo.ColoredConsole()
                .WriteTo.Debug()
                .CreateLogger();
        }

        public PacketBuilder AddProducts(IEnumerable<Product> products)
        {
            _products = new List<Product>(products);
            return this;
        }

        public PacketBuilder AddCorrelationTable(Dictionary<(int key1, int key2), int> table)
        {
            _table = table;
            return this;
        }

        public PacketBuilder AddPacketConstraint(Func<Product,bool> constraint)
        {
            _products = _products.Where(constraint).ToList();
            return this;
        }


        public List<(int prod1, int prod2)> CreatePackets()
        {
            if (!CheckIfProductsAndTableAreProvided()) return null;
            try
            {
                var packets = new List<(int prod1, int prod2)>();
                foreach (var product in _products)
                {
                    var substitute = _table.Where(t => t.Key.key1 == product.Id && t.Value != 0)
                        .OrderByDescending(t => t.Value).Select(t => t.Key.key2).FirstOrDefault();
                    packets.Add((product.Id,substitute));//if substitute equals 0 then there is no substite for given product
                }

                return packets;
            }
            catch (Exception e)
            {
                _logger.Write(LogEventLevel.Error,$"cannot create packets: exception occured: {e.InnerException}");
                return null;
            }
        }

        private bool CheckIfProductsAndTableAreProvided()
        {
            if (_table == null || _table.Count < 1)
            {
                _logger.Write(LogEventLevel.Error, "no correlation table provided");
                return false;
            }

            if (_products == null || _products.Count < 1)
            {
                _logger.Write(LogEventLevel.Error, "no products provided");
                return false;
            }

            return true;
        }
    }
}
