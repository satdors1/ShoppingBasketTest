namespace ShoppingBasket.Business.Models
{
    public class FinalBillObject
    {
        public bool IsError { get; set; }
        public string ErrorMessage { get; set; }
        public decimal FinalBillAmount { get; set; }
        public decimal AppliedDiscount { get; set; }
    }
}
