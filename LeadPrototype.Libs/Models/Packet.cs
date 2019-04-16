using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using LeadPrototype.Libs.Annotations;

namespace LeadPrototype.Libs.Models
{
    public class Packet:INotifyPropertyChanged
    { 
        public decimal TotalPrice
        {
            get { return PacketProducts.Sum(p => p.Product.AveragePrice); }
        }
        public PacketProduct[] PacketProducts { get; set; }
        public float Correlation { get; set; }

        public void ChangeProduct(Product original,Product newProduct)
        {
            try
            {
                PacketProducts.First(p => p.Product.Id == original.Id).Product = newProduct;
                OnPropertyChanged(nameof(TotalPrice));
            }
            catch 
            {
                throw new Exception($"There is no product {original} in the packet");
            }

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
