namespace LeadPrototype
{
    public class CsvSettings : IReaderSettings
    {
        public CsvSettings(string filePath)
        {
            FilePath = filePath;
        }

        public bool IsHeader { get; set; }
        public string FilePath { get; set; }
    }
}