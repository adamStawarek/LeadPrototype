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
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace ReportGenerator.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        #region fields
        private int substitutesCount = 5;
        #endregion

        #region properties        
        public CorrelationTable CorrelationTable { get; set; }
        public SubstitutesTable SubstituesTable { get; set; }
        public List<Product> Products { get; set; }
        public ObservableCollection<FileViewModel> Files { get; set; }
        public ObservableCollection<Packet> Packets { get; set; }
        #endregion

        #region computed properties
        private Packet _selectedPacket;
        public Packet SelectedPacket
        {
            get => _selectedPacket;
            set
            {
                _selectedPacket = value;
                RaisePropertyChanged(nameof(SelectedPacket));
            }
        }
        private Visibility _spinnerVisibility;
        public Visibility SpinnerVisibility
        {
            get => _spinnerVisibility;
            set
            {
                _spinnerVisibility = value;
                RaisePropertyChanged(nameof(SpinnerVisibility));
            }
        }
        #endregion

        #region constraints 
        public List<CategoryViewModel> Categories { get; set; }
        private bool _categoryConstraint;
        public bool CategoryConstraint
        {
            get => _categoryConstraint;
            set
            {
                _categoryConstraint = value;
                RaisePropertyChanged(nameof(CategoryConstraint));
            }
        }

        public PriceRange PriceRange { get; set; }
        private bool _priceConstraint;  
        public bool PriceConstraint
        {
            get => _priceConstraint;
            set
            {
                _priceConstraint = value;
                RaisePropertyChanged(nameof(PriceConstraint));
            }
        }

        public string ProductName { get; set; }
        private bool _productNameConstraint;
        public bool ProductNameConstraint {
            get => _productNameConstraint;
            set
            {
                _productNameConstraint = value; 
                RaisePropertyChanged(nameof(ProductNameConstraint));
            }
        }
        #endregion

        #region commands
        public RelayCommand OpenFileCommand { get; }
        public RelayCommand<object> DropFileCommand { get; }
        public RelayCommand GeneratePacketsCommand { get; set; }
        public RelayCommand ReadFilesCommand { get; set; }
        public RelayCommand<object> SwapProductsCommand { get; set; }
        #endregion

        public MainViewModel()
        {
            OpenFileCommand = new RelayCommand(OpenFiles);
            DropFileCommand = new RelayCommand<object>(DropFiles);
            GeneratePacketsCommand = new RelayCommand(GeneratePackets);
            ReadFilesCommand = new RelayCommand(ReadFiles);
            SwapProductsCommand = new RelayCommand<object>(SwapProducts);

            Files = new ObservableCollection<FileViewModel>();
            Packets = new ObservableCollection<Packet>();
            PriceRange = new PriceRange();
            SpinnerVisibility = Visibility.Hidden;

            FetchProductsAndCategories();
        }

        private void SwapProducts(object obj)
        {
            var values = (object[])obj;
            var newProductId = (int)values[0];
            var orginal = (PacketProduct)values[1];
            var newProduct = Products.First(p => p.Id == newProductId);
            SelectedPacket.ChangeProduct(orginal.Product, newProduct);
        }

        private void GeneratePackets()
        {
            var packetFactory = new PacketBuilder()
                .AddProducts(Products)
                .AddCorrelationTable(CorrelationTable)
                .AddSubstitutesTable(SubstituesTable)
                .SetNumberOfSubstitutes(substitutesCount);

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

            if (ProductNameConstraint)
            {
                packetFactory.AddPacketConstraint(p => p.ProductName.ToLower().Contains(ProductName.ToLower()));
            }

            try
            {
                var packets = packetFactory.CreatePackets();
                Packets.Clear();
                packets.ForEach(p => Packets.Add(p));
            }
            catch (Exception e)
            {
                Debug.WriteLine($"cannot create packets: {e.InnerException}");
            }
        }

        private void FetchProductsAndCategories()
        {
            var settings = new CsvSettings(@"../../../Tmp/products.csv", "");
            var reader = ReaderFactory.CreateReader(settings);
            Products = reader.ReadProducts().ToList();
            Categories = Products.GroupBy(x => new { x.CategoryId, x.CategoryName }).Select(p => new CategoryViewModel()
            {
                CategoryId = p.Key.CategoryId,
                CategoryName = p.Key.CategoryName
            }).ToList();
        }

        private void DropFiles(object eventArgs)
        {
            var e = eventArgs as DragEventArgs;
            if (e != null && !e.Data.GetDataPresent(DataFormats.FileDrop)) return;
            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files == null) return;
            files.ToList().ForEach(f => Files.Add(new FileViewModel(f)));
        }

        private void OpenFiles()
        {
            var openFileDialog = new OpenFileDialog
            {
                Multiselect = true,
                Filter = "Csv file|*.csv;"
            };
            if (openFileDialog.ShowDialog() != true) return;

            var files = openFileDialog.FileNames;
            files.ToList().ForEach(f => Files.Add(new FileViewModel(f)));

        }

        private async void ReadFiles()
        {
            CorrelationTable = null;
            SubstituesTable = null;
            var correlationFile = Files.First(f => f.IsCorrelationTable).FilePath;
            var substituesFile = Files.First(f => f.IsSubstitutesTable).FilePath;
            SpinnerVisibility = Visibility.Visible;
            CorrelationTable = (CorrelationTable)await FetchTable(correlationFile, TableType.Correlation);
            SubstituesTable = (SubstitutesTable)await FetchTable(substituesFile, TableType.Substitutes);
            SpinnerVisibility = Visibility.Hidden;
        }

        private async Task<Table> FetchTable(string file, TableType type)
        {
            Table tmpTable = null;
            await Task.Run(delegate
            {
                try
                {
                    var settings = new CsvSettings("", file);
                    var reader = ReaderFactory.CreateReader(settings);

                    tmpTable = reader.ReadTable(type);
                    Dispatcher.CurrentDispatcher.Invoke(() =>
                    {
                        MessageBox.Show($"Sucesfully loadded table from {file.Split('\\').Last()}, number of rows: {tmpTable.Content.Count}");
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