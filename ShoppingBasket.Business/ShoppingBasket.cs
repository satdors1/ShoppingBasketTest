using ShoppingBasket.Business.Interfaces;
using ShoppingBasket.Business.Models;
using System.Collections.Generic;
namespace ShoppingBasket.Business
{
    public interface IShoppingBasket
    {
        FinalBillObject GetFinalBill();
    }
    public class ShoppingBasket
    {
        public List<ShoppingItem> ShoppingList { get; set; }
        public List<Voucher> Vouchers;

        public IStrategy CurrentStrategy;

        public ShoppingBasket(IStrategy NewStrategy)
        {
            CurrentStrategy = NewStrategy;
        }
        public FinalBillObject GetFinalBill()
        {
            return CurrentStrategy.GetFinalBill(this.ShoppingList, this.Vouchers);
        }
    }
}
