using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace ReportGenerator.ViewModel
{
    public class MainViewModel : ViewModelBase
    {     
        public RelayCommand OpenFileCommand { get; }
        public RelayCommand<object> DropFileCommand { get; }

        public MainViewModel()
        {
            OpenFileCommand = new RelayCommand(OpenFile);
            DropFileCommand = new RelayCommand<object>(DropFile);
            FetchProducts();
        }

        private void FetchProducts()//load products from csv
        {
            throw new NotImplementedException();
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