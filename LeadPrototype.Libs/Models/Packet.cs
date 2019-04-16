using System.Linq;

namespace LeadPrototype.Libs.Models
{
    public class Packet
    { 
        public decimal TotalPrice
        {
            get { return PacketProducts.Sum(p => p.Product.AveragePrice); }
        }
        public PacketProduct[] PacketProducts { get; set; }
        public float Correlation { get; set; }
    }
}
