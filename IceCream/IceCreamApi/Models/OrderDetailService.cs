namespace IceCreamApi.Models
{
    public class OrderDetailService
    {
        public int Id { get; set; }
        public int? BookOrderId { get; set; }
        public int? BookId { get; set; }
        public int? Price { get; set; }
        public int? Quantity { get; set; }

        public BookIceCream BookIceCream { get; set; }
    }
}
