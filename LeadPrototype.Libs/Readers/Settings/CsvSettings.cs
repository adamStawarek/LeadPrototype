namespace LeadPrototype.Libs.Readers.Settings
{
    public class CsvSettings : IReaderSettings
    {
        public CsvSettings(string pathToProducts,string pathToProductsTable)
        {
            PathToProducts = pathToProducts;
            PathToProductsTable = pathToProductsTable;
        }

        public string PathToProducts { get; private set; }
        public string PathToProductsTable { get; set; }
    }
}