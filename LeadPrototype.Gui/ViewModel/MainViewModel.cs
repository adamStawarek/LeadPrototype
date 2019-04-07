using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using LeadPrototype.Libs.Models;
using LeadPrototype.Libs.Readers;
using LeadPrototype.Libs.Readers.Settings;
using ReportGenerator.Models;

namespace ReportGenerator.ViewModel
{
    public class MainViewModel : ViewModelBase
    {     
        public RelayCommand OpenFileCommand { get; }
        public RelayCommand<object> DropFileCommand { get; }
        public List<Product> _products { get;set;}=new List<Product>();
        public List<Category> Categories { get; set; }=new List<Category>();
        private Dictionary<int, int[]> _table;        
        
        public MainViewModel()
        {
            OpenFileCommand = new RelayCommand(OpenFile);
            DropFileCommand = new RelayCommand<object>(DropFile);
            FetchProductsAndCategories();
        }
        
        private void FetchProductsAndCategories()
        {
            var settings = new CsvSettings(@"../../../Tmp/products.csv","");
            var reader = ReaderFactory.CreateReader(settings);
            _products = reader.ReadObject().ToList();
            Categories = _products.GroupBy(x => new {x.CategoryId, x.CategoryName}).Select(p => new Category()
            {
                CategoryId = p.Key.CategoryId,
                CategoryName = p.Key.CategoryName
            }).ToList();
        }

        private void DropFile(object p)
        {
            var e = p as DragEventArgs;
            if (e != null && !e.Data.GetDataPresent(DataFormats.FileDrop)) return;
            var files = (string[]) e.Data.GetData(DataFormats.FileDrop);
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
            var settings = new CsvSettings("",file);
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