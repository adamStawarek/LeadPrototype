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
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace ReportGenerator.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private Dictionary<int, int[]> _table;

        private Visibility _spinnerVisibility = Visibility.Hidden;
        public Visibility SpinnerVisibility
        {
            get => _spinnerVisibility;
            set
            {
                _spinnerVisibility = value;
                RaisePropertyChanged("SpinnerVisibility");
            }
        }
        public List<Product> Products { get; set; }
        public List<Category> Categories { get; set; }
        public ObservableCollection<Packet> Packets { get; set; }
        public PriceRange PriceRange { get; set; } = new PriceRange();

        #region constraints
        private bool _categoryConstraint;
        public bool CategoryConstraint
        {
            get => _categoryConstraint;
            set
            {
                _categoryConstraint = value;
                RaisePropertyChanged("CategoryConstraint");
            }
        }

        private bool _priceConstraint;
        public bool PriceConstraint
        {
            get => _priceConstraint;
            set
            {
                _priceConstraint = value;
                RaisePropertyChanged("PriceConstraint");
            }
        }
        #endregion

        public RelayCommand OpenFileCommand { get; }
        public RelayCommand<object> DropFileCommand { get; }
        public RelayCommand GeneratePacketsCommand { get; set; }

        public MainViewModel()
        {
            OpenFileCommand = new RelayCommand(OpenFile);
            DropFileCommand = new RelayCommand<object>(DropFile);
            Packets = new ObservableCollection<Packet>();
            GeneratePacketsCommand = new RelayCommand(GeneratePackets);
            FetchProductsAndCategories();
        }

        private void GeneratePackets()
        {
            var packetFactory = new PacketBuilder()
                .AddProducts(Products)
                .AddCorrelationTable(_table);

            if (CategoryConstraint)
            {
                var selectedCategories = Categories.Where(c => c.IsSelected);
                packetFactory.AddPacketConstraint(p => selectedCategories.Any(c => c.CategoryId == p.CategoryId));
            }

            if (PriceConstraint)
            {
                packetFactory.AddPacketConstraint(p =>
                    p.AveragePrice >= PriceRange.From && p.AveragePrice <= PriceRange.To);
            }

            var packets = packetFactory.CreatePackets().OrderBy(p => p.prod1).ToList();
            Packets.Clear();
            foreach (var packet in packets)
            {
                var prod1 = Products.FirstOrDefault(p => p.Id == packet.prod1);
                var prod2 = Products.FirstOrDefault(p => p.Id == packet.prod2);
                Packets.Add(new Packet()
                {
                    Products = new[] { prod1, prod2 },
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

        private async void DropFile(object p)
        {
            var e = p as DragEventArgs;
            if (e != null && !e.Data.GetDataPresent(DataFormats.FileDrop)) return;
            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files == null) return;
            _table = await FetchCorrelationTable(files.FirstOrDefault());
        }

        private async void OpenFile()
        {
            var openFileDialog = new OpenFileDialog
            {
                Multiselect = false,
                Filter = "Csv file|*.csv;"
            };
            if (openFileDialog.ShowDialog() != true) return;

            _table = await FetchCorrelationTable(openFileDialog.FileName);
        }

        private async Task<Dictionary<int, int[]>> FetchCorrelationTable(string file)
        {
            SpinnerVisibility = Visibility.Visible;
            var tmpTable = new Dictionary<int, int[]>();
            await Task.Run(delegate
            {
                try
                {
                    var settings = new CsvSettings("", file);
                    var reader = ReaderFactory.CreateReader(settings);

                    tmpTable = reader.ReadTable();
                    Dispatcher.CurrentDispatcher.Invoke(() =>
                    {
                        SpinnerVisibility = Visibility.Hidden;
                        MessageBox.Show($"Sucesfully loadded table from {file.Split('\\').Last()}, number of rows: {tmpTable?.Count}");
                    });
                }
                catch (Exception e)
                {
                    Dispatcher.CurrentDispatcher.Invoke(() =>
                    {
                        SpinnerVisibility = Visibility.Hidden;
                        MessageBox.Show($"Exception occured when reading {file.Split('\\').Last()}, exception: {e.InnerException}");
                    });
                }
            });
           
            return tmpTable;

        }

    }
}