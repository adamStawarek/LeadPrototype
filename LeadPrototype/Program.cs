using System;
using System.Linq;

namespace LeadPrototype
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Give path to product file: ");
            var path=Console.ReadLine();
            Console.Write("Is header included(y/n)? ");
            var isHeader = Console.ReadKey().KeyChar=='y';
            Console.WriteLine("\nGive path to correlation table file: ");
            var path_to_corr = Console.ReadLine();
            Console.WriteLine("Is header included(y/n)? ");
            var isHeaderInCorr = Console.ReadKey().KeyChar=='y';


            var settings = new CsvSettings(path) { IsHeader = isHeader };
            var reader1 = ReaderFactory.CreateReader(settings);
            var _products = reader1.ReadObject().ToList();

            settings = new CsvSettings(path_to_corr) { IsHeader = isHeaderInCorr };
            var reader2 = ReaderFactory.CreateReader(settings);
            var _table = reader2.ReadTable();

            Console.WriteLine("\nPreparing packets....");
            var packetFactory = new PacketBuilder()
                .AddProducts(_products)
                .AddCorrelationTable(_table);
            var packets = packetFactory.CreatePackets();

            foreach (var packet in packets)
            {
                Console.WriteLine($"[{packet.prod1}, {packet.prod2}, {packet.val}]");
            }
        }
    }
}