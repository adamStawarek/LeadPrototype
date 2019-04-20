using System.Linq;
using LeadPrototype.Libs.Models;
using LeadPrototype.Libs.Readers;
using LeadPrototype.Libs.Readers.Settings;

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

            var settings = new CsvSettings(@"C:\Windows\LeadPrototype\products.csv", "");
            var reader = ReaderFactory.CreateReader(settings);
            var products = reader.ReadProducts().ToArray();            
            return products;
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