using System.Collections.Generic;

namespace LeadPrototype.Libs.Models
{
    public class PacketProduct
    {
        public Product Product { get; set; }
        public Dictionary<Product,float> Substitutes { get; set; } //value represents correlation with substitute
    }
}
