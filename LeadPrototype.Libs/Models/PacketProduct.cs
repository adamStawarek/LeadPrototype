using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using LeadPrototype.Libs.Annotations;

namespace LeadPrototype.Libs.Models
{
    public class PacketProduct:INotifyPropertyChanged
    {
        private Product _product;
        public Product Product
        {
            get => _product;
            set
            {
                if (Equals(value, _product)) return;
                _product = value;
                OnPropertyChanged(nameof(Product));
            }
        }

        public Dictionary<Product,float> Substitutes { get; set; } //value represents correlation with substitute

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
