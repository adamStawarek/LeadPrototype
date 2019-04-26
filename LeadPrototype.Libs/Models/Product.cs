using System.ComponentModel;
using System.Runtime.CompilerServices;
using LeadPrototype.Libs.Annotations;

namespace LeadPrototype.Libs.Models
{
    public class Product:INotifyPropertyChanged
    {       
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public decimal AveragePrice { get; set; }    

        private string _productName;    
        public string ProductName
        {
            get => _productName;
            set
            {
                _productName = value; 
                OnPropertyChanged(nameof(ProductName));
            }
        }
        
        public override string ToString()
        {
            return $"Id: {Id}, Name: {ProductName}, CategoryId: {CategoryId}, CategoryName: {CategoryName}, AvgPrice: {AveragePrice}";
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Product other))
            {
                return false;
            }

            return Id == other.Id && string.Equals(ProductName, other.ProductName) 
                                  && CategoryId == other.CategoryId
                                  && string.Equals(CategoryName, other.CategoryName) 
                                  && decimal.Compare((decimal)other.AveragePrice, (decimal)this.AveragePrice) == 0;
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