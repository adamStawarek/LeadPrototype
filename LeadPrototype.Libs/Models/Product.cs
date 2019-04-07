namespace LeadPrototype.Libs.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public decimal AveragePrice { get; set; }

        public override string ToString()
        {
            return $"Id: {Id}, Name: {ProductName}, CategoryId: {CategoryId}, CategoryName: {CategoryName}, AvgPrice: {AveragePrice}";
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Product other))
            {
                return false;
            }

            return Id == other.Id && string.Equals(ProductName, other.ProductName) 
                                  && CategoryId == other.CategoryId
                                  && string.Equals(CategoryName, other.CategoryName) 
                                  && decimal.Compare((decimal)other.AveragePrice, (decimal)this.AveragePrice) == 0;
        }       
    }
}