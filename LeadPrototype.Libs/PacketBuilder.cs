using System;
using System.Collections.Generic;
using System.Linq;
using LeadPrototype.Libs.Models;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.InMemory;

namespace LeadPrototype.Libs
{
    public class PacketBuilder
    {
        private List<Product> _products;
        private Dictionary<int, int[]> _table;
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

        public PacketBuilder AddCorrelationTable(Dictionary<int, int[]> table)
        {
            _table = table;
            return this;
        }

        public PacketBuilder AddPacketConstraint(Func<Product,bool> constraint)
        {
            _products = _products.Where(constraint).ToList();
            return this;
        }


        public List<(int prod1, int prod2,int val)> CreatePackets()
        {
            if (!CheckIfProductsAndTableAreProvided()) return null;
            var packets = new List<(int prod1, int prod2, int val)>();
            try
            {               
                foreach (var product in _products)
                {
                    var values = _table.FirstOrDefault(t => t.Key == product.Id).Value.ToList();
                    var productIndex = Mapper.MapToIndex(product);
                    values[productIndex] = -1;
                    var max = values.Max();
                    var index = values.IndexOf(max);
                    var prod2Id = Mapper.MapToProduct(index).Id;
                    packets.Add((product.Id,prod2Id,max));//if substitute equals 0 then there is no substitute for given product
                }

                return packets.OrderByDescending(p=>p.val).ToList();
            }
            catch (Exception e)
            {
                _logger.Write(LogEventLevel.Error,$"cannot create packets: exception occured: {e.InnerException}");
               
            } return packets.OrderByDescending(p => p.val).ToList();
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
