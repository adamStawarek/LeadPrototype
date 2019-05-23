using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using LeadPrototype.Libs.Annotations;
using LeadPrototype.Libs.Helpers;

namespace LeadPrototype.Libs.Models
{
    public class Product:INotifyPropertyChanged
    {
        private readonly Random _rand;
        private string _productName;
        private string _encryptedProductName;

        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public decimal AveragePrice { get; set; }
        public string ProductName
        {
            get => _productName;
            set
            {
                _productName = value;                    
                OnPropertyChanged(nameof(ProductName));
            }
        }

        public string EncryptedId { get; set; }
        public string EncryptedCategoryId { get; set; }
        public string EncryptedCategoryName { get; set; }
        public decimal EncryptedAveragePrice { get; set; }
        public string EncryptedProductName
        {
            get => _encryptedProductName;
            set
            {
                _encryptedProductName = value;
                OnPropertyChanged(nameof(EncryptedProductName));
            }
        }
           
        public Product()
        {
            _rand = new Random();
        }

        public void EncryptProperties()
        {
            //EncryptedId = EncryptionHelper.Encrypt(Id.ToString());
            //EncryptedCategoryId = EncryptionHelper.Encrypt(CategoryId.ToString());
            //EncryptedCategoryName = EncryptionHelper.Encrypt(CategoryName);
            //EncryptedAveragePrice = AveragePrice + AveragePrice * (decimal)_rand.NextDouble();
            //var singleWords = _productName.Split();
            //if (singleWords.Length == 1)
            //{
            //    EncryptedProductName = EncryptionHelper.Encrypt(_productName);
            //}
            //else
            //{
            //    EncryptedProductName = string.Join(" ", singleWords.Take(singleWords.Length - 1)) + EncryptionHelper.Encrypt(singleWords.Last());
            //}
            EncryptedId = Id.ToString().GetHashCode().ToString().Substring(0,5);
            EncryptedCategoryId = CategoryId.ToString().GetHashCode().ToString().Substring(0, 5);
            EncryptedCategoryName = CategoryName.GetHashCode().ToString().Substring(0, 5);
            EncryptedAveragePrice = decimal.Round(AveragePrice + AveragePrice * (decimal)_rand.NextDouble(),2);
            var singleWords = _productName.Split();
            if (singleWords.Length == 1)
            {
                EncryptedProductName = ProductName.GetHashCode().ToString().Substring(0, 5);
            }
            else
            {
                EncryptedProductName = string.Join(" ", singleWords.Take(singleWords.Length - 1)) + singleWords.Last().GetHashCode().ToString().Substring(0, 5);
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