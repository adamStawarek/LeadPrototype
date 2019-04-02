using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using LeadPrototype.Models;

namespace LeadPrototype
{
    public static class Mapper
    {
        private static readonly Product[] Products;
        static Mapper()
        {
            Products = FetchProducts();
        }

        private static Product[] FetchProducts()
        {
            var products=new List<Product>();
            using (var reader =
                new StreamReader("../../../../corr_map.csv", Encoding.Default, true))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();                    
                    var values = line?.Split(',');
                    if (values == null) continue;
                    products.Add(new Product()
                    {
                        Id = int.Parse(values[0])
                    });

                }
            }

            return products.ToArray();
        }
        public static Product MapToProduct(int index)//index comes from correlation table
        {
            return Products?.ElementAt(index);
        }
        public static Product[] GetProducts()
        {
            return Products;
        }
    }
}
