using System;
using System.IO;
using System.Linq;
using LeadPrototype.Libs.Readers;
using LeadPrototype.Libs.Readers.Settings;

namespace LeadPrototype.Libs
{
    class Program
    {
        static void Main()
        {
            string path = null;
            bool isHeader = false,useSavedFile = false;
            var pathToProductsTxt = @"C:/Lead/Temp/path_to_products.txt";
            if (File.Exists(pathToProductsTxt))
            {
                string[] text = File.ReadAllText(pathToProductsTxt).Split(';');
                path = text[0];
                isHeader = bool.Parse(text[1]);
                Console.Write($"Use {path} (y/n)?");
                useSavedFile = Console.ReadKey().KeyChar == 'y';
            }               
            if (!File.Exists(pathToProductsTxt)||!useSavedFile)
            {
                Console.WriteLine("Give path to product file: ");
                path = Console.ReadLine();
                Console.Write("Is header included(y/n)? ");
                isHeader = Console.ReadKey().KeyChar == 'y';
                Directory.CreateDirectory(@"C:/Lead/Temp");
                File.WriteAllText(pathToProductsTxt,path+"; "+isHeader);
            }

            string pathToCorr = null;
            bool isHeaderInTable = false;
            var pathToTableTxt =  @"C:/Lead/Temp/path_to_table.txt";
            if (File.Exists(pathToTableTxt))
            {
                string[] text = File.ReadAllText(pathToTableTxt).Split(';');
                pathToCorr = text[0];
                isHeaderInTable = bool.Parse(text[1]);
                Console.Write($"\nUse {pathToCorr} (y/n)?");
                useSavedFile = Console.ReadKey().KeyChar == 'y';
            }
            if (!File.Exists(pathToTableTxt) || !useSavedFile)
            {
                Console.WriteLine("\nGive path to correlation table file: ");
                pathToCorr = Console.ReadLine();
                Console.Write("Is header included(y/n)? ");
                isHeaderInTable = Console.ReadKey().KeyChar == 'y';
                Directory.CreateDirectory(@"C:/Lead/Temp");
                File.WriteAllText(pathToTableTxt, pathToCorr + ";" + isHeaderInTable);
            }        

            var settings = new CsvSettings(path) { IsHeader = isHeader };
            var reader1 = ReaderFactory.CreateReader(settings);
            var _products = reader1.ReadObject().ToList();

            settings = new CsvSettings(pathToCorr) { IsHeader = isHeaderInTable };
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