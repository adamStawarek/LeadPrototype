using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using LeadPrototype.Libs.Models;
using LeadPrototype.Libs.Readers;
using LeadPrototype.Libs.Readers.Settings;

namespace ReportGenerator.ViewModel
{
    public class MainViewModel : ViewModelBase
    {     
        public RelayCommand OpenFileCommand { get; }
        public RelayCommand<object> DropFileCommand { get; }
        public RelayCommand LoadedCommand { get; set; }
        private List<Product> _products=new List<Product>();
        
        public MainViewModel()
        {
            LoadedCommand=new RelayCommand(OnLoad);
            OpenFileCommand = new RelayCommand(OpenFile);
            DropFileCommand = new RelayCommand<object>(DropFile);
            FetchProducts();
        }

        private void OnLoad()
        {
            FetchProducts();
        }

        private void FetchProducts()//load products from csv
        {
//            var settings=new CsvSettings("")
//            var reader=ReaderFactory.CreateReader()
        }

        private void DropFile(object p)
        {
            var e = p as DragEventArgs;
            if (e != null && !e.Data.GetDataPresent(DataFormats.FileDrop)) return;
            var file = (string) e?.Data.GetData(DataFormats.FileDrop);
            if (file == null) return;
            if (Path.GetExtension(file) != ".csv")
            {
                Debug.WriteLine("Not supported file type");
                return;
            }
            FetchCorrelationTable(file);
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

        private void FetchCorrelationTable(string files)
        {
            throw new NotImplementedException();
        }       
          
    }
}