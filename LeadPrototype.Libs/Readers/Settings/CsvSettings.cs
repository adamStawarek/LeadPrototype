namespace LeadPrototype.Libs.Readers.Settings
{
    public class CsvSettings : IReaderSettings
    {
        public CsvSettings(string pathToProducts,string pathToTable)
        {
            PathToProducts = pathToProducts;
            PathToTable = pathToTable;
        }

        public string PathToProducts { get; private set; }
        public string PathToTable { get; set; }
    }
}