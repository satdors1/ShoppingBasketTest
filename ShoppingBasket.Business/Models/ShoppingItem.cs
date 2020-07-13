namespace ShoppingBasket.Business.Models
{
    public class ShoppingItem
    {
        public string Name { get; set; }
        public int Qty { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
        public bool DiscountQualified { get; set; }
    }
   
}
