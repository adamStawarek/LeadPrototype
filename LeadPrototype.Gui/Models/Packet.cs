using System.Linq;
using LeadPrototype.Libs.Models;

namespace ReportGenerator.Models
{
    public class Packet
    { 
        public decimal TotalPrice
        {
            get { return Products.Sum(p => p.AveragePrice); }
        }
        public Product[] Products { get; set; }
        public float Value { get; set; }
    }
}
