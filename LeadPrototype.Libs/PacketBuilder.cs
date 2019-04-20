using LeadPrototype.Libs.Helpers;
using LeadPrototype.Libs.Models;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.InMemory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LeadPrototype.Libs
{
    public class PacketBuilder
    {
        private List<Product> _products;
        private CorrelationTable _correlationTable;
        private SubstitutesTable _substitutesTable;
        private int _countOfSubstitutesPerPacket = 5;
        private readonly ILogger _logger;
        private float? _minCorrelation;
        private float? _maxCorrelation;

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

        public PacketBuilder AddCorrelationTable(CorrelationTable table)
        {
            _correlationTable = table;
            return this;
        }

        public PacketBuilder AddSubstitutesTable(SubstitutesTable table)
        {
            _substitutesTable = table;
            return this;
        }

        public PacketBuilder SetNumberOfSubstitutes(int n)
        {
            _countOfSubstitutesPerPacket = n;
            return this;
        }

        public PacketBuilder AddPacketConstraint(Func<Product, bool> constraint)
        {
            _products = _products.Where(constraint).ToList();
            return this;
        }

        public PacketBuilder SetCorrelationConstraint(float? min, float? max)
        {
            _minCorrelation = min??float.MinValue;
            _maxCorrelation = max??float.MaxValue;
            return this;
        }

        public List<Packet> CreatePackets()
        {
            var packets = new List<Packet>();

            if (!CheckIfProductsAndTableAreProvided(TableType.Correlation))
            {
                _logger.Write(LogEventLevel.Warning, "no correlation table provided");
                return packets;
            }

            FetchFromCorrelationTable(packets);

            if (!CheckIfProductsAndTableAreProvided(TableType.Substitutes))
            {
                _logger.Write(LogEventLevel.Warning, "no substitutes table provided");
                return packets;
            }

            FetchFromSubstitutesTable(packets);


            return packets;
        }

        private void FetchFromSubstitutesTable(IEnumerable<Packet> packets)
        {
            try
            {
                foreach (var packet in packets)
                {
                    foreach (var packetProduct in packet.PacketProducts)
                    {
                        packetProduct.Substitutes = new Dictionary<Product, float>();
                        var values = _substitutesTable.Content.FirstOrDefault(t => t.Key == packetProduct.Product.Id).Value.ToList();
                        var productIndex = Mapper.MapToIndex(packetProduct.Product);
                        values[productIndex] = -1;

                        var indexesWithMaxValues =
                            values.ToList().GetNIndexesOfBiggestValues(_countOfSubstitutesPerPacket);
                        foreach (var index in indexesWithMaxValues)
                        {
                            var correlation = values[index];
                            var substitute = Mapper.MapToProduct(index);
                            packetProduct.Substitutes.Add(substitute, correlation);
                        }

                        packetProduct.Substitutes = packetProduct.Substitutes.OrderByDescending(p => p.Value).ToDictionary(p => p.Key, p => p.Value);
                    }

                }
            }
            catch (Exception e)
            {
                _logger.Write(LogEventLevel.Error, $"cannot create substitutes: exception: {e.InnerException}");

            }
        }

        private void FetchFromCorrelationTable(ICollection<Packet> packets)
        {
            try
            {
                foreach (var product in _products)
                {
                    var values = _correlationTable.Content.FirstOrDefault(t => t.Key == product.Id).Value.ToList();
                    var productIndex = Mapper.MapToIndex(product);
                    values[productIndex] = -1;
                    var maxCorrelation = values.Max();
                    if(maxCorrelation<_minCorrelation||maxCorrelation>_maxCorrelation) continue;
                    var index = values.IndexOf(maxCorrelation);
                    var product2 = Mapper.MapToProduct(index);
                    packets.Add(new Packet()
                    {
                        Correlation = maxCorrelation,
                        PacketProducts = new PacketProduct[]
                        {
                            new PacketProduct()
                            {
                                Product = product
                            },
                            new PacketProduct()
                            {
                                Product = product2
                            },
                        }
                    });
                }
            }
            catch (Exception e)
            {
                _logger.Write(LogEventLevel.Error, $"cannot create packets for correlation table: exception: {e.InnerException}");
            }
        }

        private bool CheckIfProductsAndTableAreProvided(TableType type)
        {
            if (_products == null || _products.Count < 1)
            {
                _logger.Write(LogEventLevel.Error, "no products provided");
                return false;
            }
            switch (type)
            {
                case TableType.Correlation when (_correlationTable == null || _correlationTable.Content.Count < 1):
                    return false;
                case TableType.Substitutes when (_substitutesTable == null || _substitutesTable.Content.Count < 1):
                    return false;
                default:
                    return true;
            }
        }

        public List<Product> GetProducts()
        {
            return _products;

        }

        public int? GetProductsCount()
        {
            return _products.Count;
        }
    }
}
