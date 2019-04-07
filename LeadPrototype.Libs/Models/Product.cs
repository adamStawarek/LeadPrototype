namespace LeadPrototype.Libs.Models
{
    public class Product
    {
        public int Id { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Product product &&
                   Id == product.Id;
        }
    }
}