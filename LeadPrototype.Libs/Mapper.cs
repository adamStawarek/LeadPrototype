using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using LeadPrototype.Libs.Models;

namespace LeadPrototype.Libs
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
            var products = new List<Product>();
            using (var reader =
                new StreamReader("../../../../Tmp/corr_map.csv", Encoding.Default, true))
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

        public static Product MapToProduct(int index) 
        {
            return Products?.ElementAt(index);
        }
        
        public static int MapToIndex(Product product)
        {
            var _product = Products.FirstOrDefault(p => p.Id == product.Id);
            return Products.ToList().IndexOf(_product);
        }

        public static Product[] GetProducts()
        {
            return Products;
        }
    }
}