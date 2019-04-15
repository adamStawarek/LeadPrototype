using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using ReportGenerator.Annotations;

namespace ReportGenerator.Models
{
    public class FileViewModel : INotifyPropertyChanged
    {
        private bool _isCorrelationTable;
        private bool _isSubstitutesTable;
        private string _filePath;
        private string _fileName;

        public string FilePath
        {
            get => _filePath;
            set
            {
                _filePath = value;
                _fileName = _filePath.Split('\\').Last();
                OnPropertyChanged("FileName");
            }
        }

        public string FileName
        {
            get => _fileName;
            set => _fileName = value;
        }

        public bool IsCorrelationTable
        {
            get => _isCorrelationTable;
            set
            {
                _isCorrelationTable = value;
                _isSubstitutesTable = !_isCorrelationTable && _isSubstitutesTable;
                OnPropertyChanged("IsCorrelationTable");
                OnPropertyChanged("IsSubstitutesTable");
            }
        }

        public bool IsSubstitutesTable
        {
            get => _isSubstitutesTable;
            set
            {
                _isSubstitutesTable = value;
                _isCorrelationTable = !_isSubstitutesTable && _isCorrelationTable;
                OnPropertyChanged("IsCorrelationTable");
                OnPropertyChanged("IsSubstitutesTable");
            }
        }

        public FileViewModel(string filePath)
        {
            FilePath = filePath;
        }

        #region property changed
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
