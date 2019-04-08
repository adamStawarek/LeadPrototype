using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using LeadPrototype.Libs;
using LeadPrototype.Libs.Models;
using LeadPrototype.Libs.Readers;
using LeadPrototype.Libs.Readers.Settings;
using Microsoft.Win32;
using ReportGenerator.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace ReportGenerator.ViewModel
{
    public class MainViewModel : ViewModelBase
    {

        private Dictionary<int, int[]> _table;    

        public List<Product> Products { get; set; }
        public List<Category> Categories { get; set; }
        public ObservableCollection<Packet> Packets { get; set; }

        public RelayCommand OpenFileCommand { get; }
        public RelayCommand<object> DropFileCommand { get; }
        public RelayCommand GeneratePacketsCommand { get; set; }

        public MainViewModel()
        {
            OpenFileCommand = new RelayCommand(OpenFile);
            DropFileCommand = new RelayCommand<object>(DropFile);
            Packets=new ObservableCollection<Packet>();
            GeneratePacketsCommand = new RelayCommand(GeneratePackets);
            FetchProductsAndCategories();
        }

        private void GeneratePackets()
        {
            var packetFactory = new PacketBuilder()
                .AddProducts(Products)
                .AddCorrelationTable(_table);
            var packets = packetFactory.CreatePackets().OrderBy(p => p.prod1).ToList();
            Packets.Clear();
            foreach (var packet in packets)
            {
                var prod1 = Products.FirstOrDefault(p => p.Id == packet.prod1);
                var prod2 = Products.FirstOrDefault(p => p.Id == packet.prod2);
                Packets.Add(new Packet()
                {
                   Products = new []{prod1,prod2},
                    Value = packet.val
                });
            }
        }

        private void FetchProductsAndCategories()
        {
            var settings = new CsvSettings(@"../../../Tmp/products.csv", "");
            var reader = ReaderFactory.CreateReader(settings);
            Products = reader.ReadObject().ToList();
            Categories = Products.GroupBy(x => new { x.CategoryId, x.CategoryName }).Select(p => new Category()
            {
                CategoryId = p.Key.CategoryId,
                CategoryName = p.Key.CategoryName
            }).ToList();
        }

        private void DropFile(object p)
        {
            var e = p as DragEventArgs;
            if (e != null && !e.Data.GetDataPresent(DataFormats.FileDrop)) return;
            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files == null) return;
            FetchCorrelationTable(files.FirstOrDefault());
        }

        private void OpenFile()
        {
            var openFileDialog = new OpenFileDialog
            {
                Multiselect = false,
                Filter = "Csv file|*.csv;"
            };
            if (openFileDialog.ShowDialog() != true) return;

            FetchCorrelationTable(openFileDialog.FileName);
        }

        private void FetchCorrelationTable(string file)
        {
            var settings = new CsvSettings("", file);
            var reader = ReaderFactory.CreateReader(settings);
            try
            {
                _table = reader.ReadTable();
                MessageBox.Show(
                    $"Sucesfully loadded table from {file.Split('\\').Last()}, number of rows: {_table.Count}");
            }
            catch (Exception e)
            {
                MessageBox.Show($"Exception occured when reading {file.Split('\\').Last()}, exception: {e.InnerException}");
            }

        }

    }
}