using System.Collections.Generic;

namespace LeadPrototype.Libs.Models
{
    public abstract class Table
    {
        public Dictionary<int, float[]> Content { get; set; }
        public abstract TableType Type { get; }
    }

    public class CorrelationTable:Table
    {
        public override TableType Type => TableType.Correlation;
    }

    public class SubstitutesTable : Table
    {
        public override TableType Type => TableType.Substitutes;
    }
}
