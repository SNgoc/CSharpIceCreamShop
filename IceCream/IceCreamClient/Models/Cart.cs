namespace IceCreamClient.Models
{
    public class Cart
    {
        public BookIceCream BookIceCream { get; set; }
        public int Quantity { get; set; }
        public double? Price { get; set; }
    }
}
